using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// Registers a new student at a KUBK member dojo.
    ///
    /// Deliberately separate from StudentAddUI, which is the Windsong screen. Two differences
    /// matter enough to justify a second form: the dojo is chosen from the list of member dojos
    /// rather than typed, because Students.stud_club has to match Club_Parameters.club_id exactly
    /// or the student silently falls off every per-dojo view; and an art and rank are required,
    /// because the roster inner-joins StudArts and a student with no enrollment would never appear
    /// on it, making a successful add look like a failure.
    ///
    /// The address and birthdate the Windsong form collects are not asked for. Hombu tracks who a
    /// member dojo's students are and what rank they hold; the rest is the home dojo's business.
    /// </summary>
    public partial class KubkAddStudentUI : Form
    {
        /// <summary>How many same-name matches to list before summarising the rest.</summary>
        private const int MaxDuplicatesListed = 10;

        private readonly KubkManagementFunctions kubkFunctions;
        private readonly IDataRepository dataRepository;

        /// <summary>Dojo the roster is currently filtered to, pre-selected when it names one.</summary>
        private readonly string initialClubID;

        /// <summary>The student created by this dialog. Valid once it returns OK.</summary>
        public int NewStudentID { get; private set; }

        /// <summary>The dojo that student was added to, so the caller can go and show them.</summary>
        public string NewStudentClubID { get; private set; }

        public KubkAddStudentUI(IDataRepository dataRepository, string initialClubID)
        {
            InitializeComponent();

            this.dataRepository = dataRepository;
            this.initialClubID = initialClubID;

            kubkFunctions = new KubkManagementFunctions(dataRepository);
        }

        private void KubkAddStudentUI_Load(object sender, EventArgs e)
        {
            dtRankHeldSince.Value = DateTime.Today;

            PopulateDojoSelector();
            PopulateArtCombo();
            PopulateRankCombo();
        }

        /// <summary>
        /// Offers the active member dojos, matching the roster's own selector. Windsong is not
        /// among them: its students are added on the maintenance screen.
        /// </summary>
        private void PopulateDojoSelector()
        {
            List<Dojo> selectableDojos = KubkManagementFunctions.GetSelectableRosterDojos(kubkFunctions.GetDojos());

            foreach (Dojo dojo in selectableDojos)
            {
                cmbDojo.Items.Add(new DojoOption
                {
                    ClubID = dojo.ClubID,
                    Display = string.IsNullOrWhiteSpace(dojo.Name) ? dojo.ClubID : dojo.Name
                });
            }

            cmbDojo.DisplayMember = nameof(DojoOption.Display);

            // Nothing is pre-selected unless the roster was already showing one dojo. Defaulting
            // to whichever dojo sorts first would make the most consequential field on the form
            // the easiest one to get wrong without noticing.
            cmbDojo.SelectedIndex = IndexOfDojo(initialClubID);
        }

        private int IndexOfDojo(string clubID)
        {
            if (string.IsNullOrWhiteSpace(clubID))
                return -1;

            for (int i = 0; i < cmbDojo.Items.Count; i++)
            {
                if (cmbDojo.Items[i] is DojoOption option &&
                    string.Equals(option.ClubID?.Trim(), clubID.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private void PopulateArtCombo()
        {
            DataTable arts = dataRepository.GetListOfArts();

            foreach (DataRow row in arts.Rows)
                cmbArt.Items.Add(row["art_id"].ToString());
        }

        /// <summary>
        /// Every rank is offered with none pre-selected. A transferring student arrives at
        /// whatever rank their home dojo awarded them, so there is no sensible default - and
        /// WHITE would be both wrong and plausible enough to be missed.
        /// </summary>
        private void PopulateRankCombo()
        {
            foreach (Rank rank in kubkFunctions.GetRankLadder())
                cmbRank.Items.Add(rank.RankID);
        }

        private string SelectedClubID => (cmbDojo.SelectedItem as DojoOption)?.ClubID;

        private string SelectedDojoDisplay => (cmbDojo.SelectedItem as DojoOption)?.Display ?? "the selected dojo";

        private MemberDojoStudentRegistration BuildRegistration()
        {
            return new MemberDojoStudentRegistration
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                ClubID = SelectedClubID,
                EmailAddress = txtEmailAddress.Text.Trim(),
                PhoneNumber = txtPhoneNumber.Text.Trim(),
                IsActive = cbActiveStudent.Checked,
                Art = cmbArt.SelectedItem?.ToString(),
                Rank = cmbRank.SelectedItem?.ToString(),
                RankHeldSince = dtRankHeldSince.Value.Date
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MemberDojoStudentRegistration registration = BuildRegistration();

            if (!kubkFunctions.ValidateMemberDojoStudent(registration, out string validationError))
            {
                MessageService.ShowErrorMessage(validationError, "Incomplete Student");
                return;
            }

            // Confirm before anything is written, so backing out here leaves nothing behind.
            if (!ConfirmAdd(registration))
                return;

            if (!kubkFunctions.AddMemberDojoStudent(registration, out int newStudentID, out string error))
            {
                MessageService.ShowErrorMessage(error, "Error Adding Student");
                return;
            }

            NewStudentID = newStudentID;
            NewStudentClubID = registration.ClubID;

            MessageService.ShowInformationMessage(
                $"{registration.FullName} was added to {SelectedDojoDisplay}." + Environment.NewLine + Environment.NewLine +
                $"Their {registration.Rank} rank in {registration.Art} is recorded as unverified. " +
                "Use Verify Rank on the roster once it has been confirmed.",
                "Student Added");

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Final confirmation before the write. When someone of the same name is already on file
        /// the prompt says so instead, because that is the thing worth stopping to look at: a
        /// duplicate record splits one person's rank history across two student ids, and there is
        /// no tool to merge them afterwards. It stays a warning rather than a block - genuine
        /// namesakes and re-enrolments both exist in this data.
        /// </summary>
        private bool ConfirmAdd(MemberDojoStudentRegistration registration)
        {
            List<KubkRosterEntry> duplicates =
                kubkFunctions.GetStudentsWithSameName(registration.FirstName, registration.LastName);

            string prompt = duplicates.Count == 0
                ? $"Add {registration.FullName} to {SelectedDojoDisplay} as {registration.Rank} in {registration.Art}?"
                : BuildDuplicateWarning(registration, duplicates);

            string title = duplicates.Count == 0 ? "Add Student?" : "Student May Already Exist";

            return MessageService.ShowAreYouSureMessage(prompt, title) == DialogResult.Yes;
        }

        private string BuildDuplicateWarning(MemberDojoStudentRegistration registration, List<KubkRosterEntry> duplicates)
        {
            var message = new StringBuilder();

            message.AppendLine(duplicates.Count == 1
                ? $"A student named {registration.FullName} is already on file:"
                : $"{duplicates.Count} students named {registration.FullName} are already on file:");

            message.AppendLine();

            int listed = Math.Min(duplicates.Count, MaxDuplicatesListed);

            for (int i = 0; i < listed; i++)
            {
                KubkRosterEntry duplicate = duplicates[i];

                message.AppendLine($"     id {duplicate.StudentID}  -  {duplicate.Dojo}  -  " +
                    (duplicate.IsActive ? "active" : "inactive"));
            }

            if (duplicates.Count > listed)
                message.AppendLine($"     ...and {duplicates.Count - listed} more");

            message.AppendLine();
            message.Append($"Add another record for this name at {SelectedDojoDisplay}?");

            return message.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
