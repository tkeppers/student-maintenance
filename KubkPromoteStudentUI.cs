using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// Records a promotion for a student at a KUBK member dojo.
    ///
    /// Unlike the Windsong flow in PromoteStudentUI, there are no eligibility gates: KUBK
    /// promotions are granted on instructor recommendation, so hours, age and time in grade are
    /// shown for reference only and never block. Any rank may be selected, because transfers and
    /// corrections legitimately skip rungs of the ladder.
    /// </summary>
    public partial class KubkPromoteStudentUI : Form
    {
        private readonly IDataRepository dataRepository;
        private readonly KubkManagementFunctions kubkFunctions;
        private readonly DataTable promotionRequirements;

        private readonly int studentID;
        private readonly string studentName;
        private readonly string studentDojo;
        private readonly string initialArt;

        private List<Rank> rankLadder = new List<Rank>();

        /// <summary>The student's enrollment in the selected art, or null when not enrolled.</summary>
        private StudentArtsAndRank selectedEnrollment;

        private bool suppressArtChangeEvents;

        /// <summary>Raised after a promotion is saved so the parent roster can refresh.</summary>
        public event EventHandler StudentPromoted;

        public KubkPromoteStudentUI(IDataRepository dataRepository, KubkRosterEntry rosterEntry)
        {
            InitializeComponent();

            this.dataRepository = dataRepository;
            kubkFunctions = new KubkManagementFunctions(dataRepository);
            promotionRequirements = dataRepository.GetStudentPromotionRequirements();

            studentID = rosterEntry.StudentID;
            studentName = rosterEntry.FullName;
            studentDojo = rosterEntry.Dojo;
            initialArt = rosterEntry.Art;
        }

        private void KubkPromoteStudentUI_Load(object sender, EventArgs e)
        {
            lblStudentName.Text = studentName;
            lblDojo.Text = studentDojo;
            dtPromotionDate.Value = DateTime.Today;

            rankLadder = kubkFunctions.GetRankLadder();
            PopulateRankCombo();
            PopulateArtCombo();
            PrefillRecommendedBy();

            LoadSelectedArtContext();
        }

        private void PopulateRankCombo()
        {
            cmbNewRank.Items.Clear();

            foreach (Rank rank in rankLadder)
                cmbNewRank.Items.Add(rank.RankID);
        }

        private void PopulateArtCombo()
        {
            suppressArtChangeEvents = true;
            cmbArt.Items.Clear();

            DataTable arts = dataRepository.GetListOfArts();

            foreach (DataRow row in arts.Rows)
                cmbArt.Items.Add(row["art_id"].ToString());

            int index = cmbArt.Items.IndexOf(initialArt);
            cmbArt.SelectedIndex = index >= 0 ? index : (cmbArt.Items.Count > 0 ? 0 : -1);

            suppressArtChangeEvents = false;
        }

        /// <summary>
        /// Pre-fills the recommender with the home dojo's instructor, which is who normally puts
        /// a student forward. Always editable - a promotion may be recommended by someone else.
        /// </summary>
        private void PrefillRecommendedBy()
        {
            Dojo homeDojo = kubkFunctions.GetDojos().FirstOrDefault(d =>
                string.Equals(d.ClubID?.Trim(), studentDojo?.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(homeDojo?.Instructor))
                txtRecommendedBy.Text = homeDojo.Instructor.Trim();
        }

        private void cmbArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressArtChangeEvents)
                return;

            LoadSelectedArtContext();
        }

        private string SelectedArt => cmbArt.SelectedItem?.ToString();

        /// <summary>
        /// Reloads the read-only context for whichever art is selected, and re-defaults the new
        /// rank. Handles the not-enrolled case, which is reachable by choosing an art the student
        /// has no StudArts row for.
        /// </summary>
        private void LoadSelectedArtContext()
        {
            selectedEnrollment = kubkFunctions.GetStudentArtEnrollment(studentID, SelectedArt);

            if (selectedEnrollment == null)
            {
                lblCurrentRank.Text = "(not enrolled in this art)";
                lblYearsAtRank.Text = string.Empty;
                lblVerified.Text = string.Empty;
                lblReferenceCriteria.Text = string.Empty;
                cmbNewRank.SelectedIndex = -1;
                return;
            }

            lblCurrentRank.Text = selectedEnrollment.Rank;
            lblYearsAtRank.Text = selectedEnrollment.YearsAtCurrentLevel().ToString("0.0") + " years";
            lblVerified.Text = selectedEnrollment.RankIsVerified
                ? selectedEnrollment.RankVerifiedDate.Value.ToString("MM/dd/yyyy")
                : "Unverified";

            SelectDefaultNextRank();
            ShowReferenceCriteria();
        }

        private void SelectDefaultNextRank()
        {
            string defaultNextRank = KubkManagementFunctions.GetDefaultNextRank(rankLadder, selectedEnrollment.Rank);

            // No pre-selection at the top of the ladder or for a rank the ladder does not list;
            // the user can still choose any rank manually.
            cmbNewRank.SelectedIndex = string.IsNullOrEmpty(defaultNextRank)
                ? -1
                : cmbNewRank.Items.IndexOf(defaultNextRank);
        }

        /// <summary>
        /// Shows the Windsong promotion requirements for this art and rank purely as context.
        /// These are never enforced here.
        /// </summary>
        private void ShowReferenceCriteria()
        {
            if (promotionRequirements == null || promotionRequirements.Columns.Count == 0)
            {
                lblReferenceCriteria.Text = "No reference criteria available.";
                return;
            }

            var criteria = new PromotionCriteria(promotionRequirements);
            criteria.GetNextPromotionCriteria(selectedEnrollment);

            if (string.IsNullOrWhiteSpace(criteria.NextRank))
            {
                lblReferenceCriteria.Text =
                    $"No Windsong criteria on record for {selectedEnrollment.StudentArt} - {selectedEnrollment.Rank}.";
                return;
            }

            lblReferenceCriteria.Text =
                $"For {criteria.CurrentRank} to {criteria.NextRank}:  " +
                $"minimum {criteria.MinimumTrainingHours:0} training hours, " +
                $"minimum age {criteria.MinimumAge:0}, " +
                $"{criteria.YearsInArt:0.##} years in art, " +
                $"{criteria.YearsAtCurrentRank:0.##} years at rank." + Environment.NewLine + Environment.NewLine +
                $"This student has {selectedEnrollment.HoursInArt:0} hours and " +
                $"{selectedEnrollment.YearsAtCurrentLevel():0.0} years at their current rank.";
        }

        private void btnPromote_Click(object sender, EventArgs e)
        {
            string newRank = cmbNewRank.SelectedItem?.ToString();
            DateTime promotionDate = dtPromotionDate.Value.Date;
            string recommendedBy = txtRecommendedBy.Text.Trim();

            if (!KubkManagementFunctions.ValidateKubkPromotion(newRank, promotionDate, recommendedBy, out string validationError))
            {
                MessageService.ShowErrorMessage(validationError, "Incomplete Promotion");
                return;
            }

            // Confirm before anything is written. Enrolling first and asking afterwards would
            // leave a brand-new enrollment behind when the user backs out here.
            DialogResult confirmation = MessageService.ShowAreYouSureMessage(
                $"Promote {studentName} to {newRank} in {SelectedArt}?", "Promote Student?");

            if (confirmation != DialogResult.Yes)
                return;

            bool enrollmentWasCreated = false;

            if (selectedEnrollment == null)
            {
                if (!EnrollStudentInSelectedArt(newRank, promotionDate))
                    return;

                enrollmentWasCreated = true;
            }

            if (!kubkFunctions.RecordKubkPromotion(selectedEnrollment, newRank, promotionDate, recommendedBy, out string error))
            {
                // Enrollment and promotion are separate transactions, so undo an enrollment we
                // created a moment ago rather than leaving the student enrolled in an art they
                // were never actually promoted in.
                if (enrollmentWasCreated && !UndoEnrollmentCreatedForThisPromotion())
                {
                    error += $"{Environment.NewLine}{Environment.NewLine}" +
                        $"{studentName} was also left enrolled in {SelectedArt} and may need to be removed manually.";
                }

                MessageService.ShowErrorMessage(error, "Error Recording Promotion");
                return;
            }

            MessageService.ShowInformationMessage(
                $"{studentName} promoted to {newRank} in {SelectedArt}.", "Promotion Recorded");

            StudentPromoted?.Invoke(this, EventArgs.Empty);

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Offers to create the missing StudArts row so a student can be promoted in an art they
        /// were never enrolled in. The enrollment starts at the promotion date holding the rank
        /// they are being promoted from.
        /// </summary>
        private bool EnrollStudentInSelectedArt(string newRank, DateTime promotionDate)
        {
            DialogResult response = MessageService.ShowAreYouSureMessage(
                $"{studentName} is not enrolled in {SelectedArt}. Enroll them now so the promotion can be recorded?",
                "Student Not Enrolled");

            if (response != DialogResult.Yes)
                return false;

            // Start them one rung below the rank being awarded where the ladder allows it, so the
            // promotion reads as a change of rank rather than appearing to start at the new rank.
            string startingRank = GetRankBelow(newRank);

            if (!kubkFunctions.EnrollStudentInArt(studentID, SelectedArt, startingRank, promotionDate))
            {
                MessageService.ShowErrorMessage(
                    $"Error enrolling {studentName} in {SelectedArt}. The promotion was not recorded.",
                    "Error Enrolling Student");
                return false;
            }

            selectedEnrollment = kubkFunctions.GetStudentArtEnrollment(studentID, SelectedArt);

            if (selectedEnrollment == null)
            {
                MessageService.ShowErrorMessage(
                    $"{studentName} could not be loaded after enrolling in {SelectedArt}. The promotion was not recorded.",
                    "Error Enrolling Student");
                return false;
            }

            Log.Information($"Enrolled student {studentID} in {SelectedArt} ahead of a KUBK promotion");
            return true;
        }

        /// <summary>
        /// Compensating delete for an enrollment this dialog created immediately before a
        /// promotion that then failed. Returns false if the enrollment could not be removed, in
        /// which case the caller tells the user it was left behind.
        /// </summary>
        private bool UndoEnrollmentCreatedForThisPromotion()
        {
            bool removed = kubkFunctions.RemoveStudentArtEnrollment(studentID, SelectedArt);

            if (removed)
            {
                selectedEnrollment = null;
                Log.Information($"Removed the {SelectedArt} enrollment for student {studentID} after the promotion failed");
            }
            else
            {
                Log.Error($"Could not remove the {SelectedArt} enrollment for student {studentID} after the promotion failed");
            }

            return removed;
        }

        private string GetRankBelow(string rank)
        {
            Rank previous = rankLadder.FirstOrDefault(r =>
                string.Equals(r.RankNext?.Trim(), rank?.Trim(), StringComparison.OrdinalIgnoreCase));

            return previous?.RankID ?? rank;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
