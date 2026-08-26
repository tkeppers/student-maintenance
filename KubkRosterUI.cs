using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// The day-to-day KUBK screen: per-dojo student roster showing rank, rank-verification
    /// status, and annual dues status for a selected year, with dues and verification actions.
    /// </summary>
    public partial class KubkRosterUI : Form
    {
        /// <summary>Sentinel club id meaning "every member dojo except Windsong" in the selector.</summary>
        private const string AllDojosClubId = "";

        private static readonly Color UnverifiedRowColor = Color.FromArgb(255, 243, 205);

        /// <summary>
        /// One shared style instance reused by every unverified row. Assigning a fresh
        /// DataGridViewCellStyle per row would allocate thousands of objects on a full roster.
        /// </summary>
        private readonly DataGridViewCellStyle unverifiedRowStyle =
            new DataGridViewCellStyle { BackColor = UnverifiedRowColor };

        private readonly IDataRepository dataRepository;
        private readonly KubkManagementFunctions kubkFunctions;

        private List<KubkRosterEntry> loadedEntries = new List<KubkRosterEntry>();
        private List<KubkRosterEntry> displayedEntries = new List<KubkRosterEntry>();

        /// <summary>Set while controls are being populated from code so handlers do not re-enter.</summary>
        private bool suppressCriteriaEvents;

        /// <summary>
        /// Set while the grid is being rebuilt, so writing cell values back into the Dues Paid
        /// column cannot be mistaken for the user ticking it and recurse into another save.
        /// </summary>
        private bool suppressDuesCellEvents;

        public KubkRosterUI(IDataRepository dataRepository)
        {
            InitializeComponent();

            this.dataRepository = dataRepository;
            kubkFunctions = new KubkManagementFunctions(dataRepository);
        }

        private void KubkRosterUI_Load(object sender, EventArgs e)
        {
            suppressCriteriaEvents = true;

            numDuesYear.Value = DateTime.Today.Year;
            PopulateDojoSelector();

            suppressCriteriaEvents = false;

            RefreshRoster();
        }

        private void PopulateDojoSelector()
        {
            List<Dojo> selectableDojos = KubkManagementFunctions.GetSelectableRosterDojos(kubkFunctions.GetDojos());

            var options = new List<DojoOption>
            {
                new DojoOption { ClubID = AllDojosClubId, Display = "All member dojos" }
            };

            options.AddRange(selectableDojos.Select(d => new DojoOption
            {
                ClubID = d.ClubID,
                Display = string.IsNullOrWhiteSpace(d.Name) ? d.ClubID : d.Name
            }));

            cmbDojo.DisplayMember = nameof(DojoOption.Display);
            cmbDojo.ValueMember = nameof(DojoOption.ClubID);
            cmbDojo.DataSource = options;
            cmbDojo.SelectedIndex = 0;
        }

        private string SelectedClubId => (cmbDojo.SelectedItem as DojoOption)?.ClubID ?? AllDojosClubId;

        private int SelectedDuesYear => (int)numDuesYear.Value;

        /// <summary>
        /// Re-queries the database. Used when the dojo or dues year changes, and after any
        /// action that writes to the database.
        /// </summary>
        private void RefreshRoster()
        {
            if (suppressCriteriaEvents)
                return;

            loadedEntries = kubkFunctions.GetRosterEntries(SelectedClubId, SelectedDuesYear);
            ApplyFiltersAndDisplay();
        }

        /// <summary>
        /// Re-filters the already-loaded rows. Used when only a display filter changes, so
        /// toggling a checkbox does not hit the database again.
        /// </summary>
        private void ApplyFiltersAndDisplay()
        {
            displayedEntries = KubkManagementFunctions.ApplyRosterFilters(
                loadedEntries, cbActiveOnly.Checked, cbUnpaidOnly.Checked);

            PopulateRosterGrid();
            UpdateStatusLine();
            UpdateActionButtonState();
        }

        /// <summary>
        /// Builds every row up front and adds them in a single AddRange call. "All dojos" spans
        /// roughly 2,400 student/art rows, and adding those one at a time makes the grid
        /// re-layout on each insert, which is slow enough to look like the screen has hung.
        /// </summary>
        private void PopulateRosterGrid()
        {
            dgvRoster.SuspendLayout();
            suppressDuesCellEvents = true;

            try
            {
                dgvRoster.Rows.Clear();

                if (displayedEntries.Count == 0)
                    return;

                var rows = new DataGridViewRow[displayedEntries.Count];

                for (int i = 0; i < displayedEntries.Count; i++)
                {
                    KubkRosterEntry entry = displayedEntries[i];

                    var row = new DataGridViewRow();
                    row.CreateCells(dgvRoster,
                        entry.FullName,
                        entry.Dojo,
                        entry.Art,
                        entry.Rank,
                        entry.RankVerifiedDisplay,
                        FormatDate(entry.LastPromotionDate),
                        entry.LastPromotionDate.HasValue ? entry.YearsAtRank().ToString("0.0") : string.Empty,
                        entry.DuesArePaid);

                    // Flag unverified ranks so they stand out at a glance.
                    if (!entry.RankIsVerified)
                        row.DefaultCellStyle = unverifiedRowStyle;

                    // The entry travels with its row. These columns are sortable, so a row's
                    // position stops matching its position in displayedEntries as soon as the
                    // user sorts, and every action here writes against a specific student.
                    row.Tag = entry;

                    rows[i] = row;
                }

                dgvRoster.Rows.AddRange(rows);
            }
            finally
            {
                suppressDuesCellEvents = false;
                dgvRoster.ResumeLayout();
            }

            dgvRoster.ClearSelection();
        }

        private string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        private void UpdateStatusLine()
        {
            // Counts about what is on screen come from the filtered rows, but re-verification
            // progress comes from everything loaded for this dojo. Otherwise ticking "Unpaid
            // dues only" would shrink the denominator and make a dojo look better verified than
            // it is.
            KubkRosterSummary shown = KubkManagementFunctions.SummarizeRoster(displayedEntries);
            KubkRosterSummary wholeRoster = KubkManagementFunctions.SummarizeRoster(loadedEntries);

            string dojoLabel = string.IsNullOrEmpty(SelectedClubId) ? "All member dojos" : SelectedClubId;

            lblStatus.Text = $"Students shown: {shown.StudentsShown}     " +
                $"{KubkManagementFunctions.BuildVerificationProgressText(dojoLabel, wholeRoster)}     " +
                $"Unpaid for {SelectedDuesYear}: {shown.StudentsUnpaid}";
        }

        /// <summary>
        /// Read from the row's Tag rather than by index: the grid is sortable, so a row index is
        /// not a position in displayedEntries once the user has clicked a column header.
        /// </summary>
        private KubkRosterEntry SelectedEntry
        {
            get
            {
                if (dgvRoster.SelectedRows.Count == 0)
                    return null;

                return dgvRoster.SelectedRows[0].Tag as KubkRosterEntry;
            }
        }

        private void UpdateActionButtonState()
        {
            KubkRosterEntry entry = SelectedEntry;
            bool hasSelection = entry != null;

            btnVerifyRank.Enabled = hasSelection;
            btnCorrectRank.Enabled = hasSelection;
            btnPromoteStudent.Enabled = hasSelection;
            btnToggleActive.Enabled = hasSelection;

            // The button offers the opposite of the student's current state.
            btnToggleActive.Text = hasSelection && !entry.IsActive ? "Mark Active" : "Mark Inactive";
        }

        /// <summary>
        /// Administrative correction of a rank the dojo has re-reported differently. Distinct
        /// from Promote Student: this writes no promotion history.
        /// </summary>
        private void btnCorrectRank_Click(object sender, EventArgs e)
        {
            KubkRosterEntry entry = SelectedEntry;

            if (entry == null)
                return;

            using (var correctionDialog = new RankCorrectionUI(entry.FullName, entry.Art, entry.Rank, kubkFunctions.GetRankLadder()))
            {
                if (correctionDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                if (kubkFunctions.CorrectStudentRank(entry.StudentID, entry.Art, entry.Rank, correctionDialog.CorrectedRank, out string error))
                {
                    Log.Information($"Corrected {entry.Art} rank for student {entry.StudentID} to {correctionDialog.CorrectedRank}");
                    RefreshRoster();
                }
                else
                {
                    MessageService.ShowErrorMessage(error, "Error Correcting Rank");
                }
            }
        }

        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            KubkRosterEntry entry = SelectedEntry;

            if (entry == null)
                return;

            bool makeActive = !entry.IsActive;
            string action = makeActive ? "active" : "inactive";

            DialogResult confirmation = MessageService.ShowAreYouSureMessage(
                $"Mark {entry.FullName} as {action}?" +
                (makeActive ? string.Empty : $"{Environment.NewLine}{Environment.NewLine}They will drop off this roster while \"Active students only\" is checked."),
                $"Mark Student {char.ToUpper(action[0]) + action.Substring(1)}?");

            if (confirmation != DialogResult.Yes)
                return;

            if (kubkFunctions.SetStudentActiveStatus(entry.StudentID, makeActive))
            {
                Log.Information($"Marked student {entry.StudentID} {action}");
                RefreshRoster();
            }
            else
            {
                MessageService.ShowErrorMessage(
                    $"Error updating the status for {entry.FullName}. The change may not have been saved.",
                    "Error Updating Student");
            }
        }

        private void btnPromoteStudent_Click(object sender, EventArgs e)
        {
            KubkRosterEntry entry = SelectedEntry;

            if (entry == null)
                return;

            using (var promoteDialog = new KubkPromoteStudentUI(dataRepository, entry))
            {
                promoteDialog.StudentPromoted += (s, args) => RefreshRoster();
                promoteDialog.ShowDialog(this);
            }
        }

        private void dgvRoster_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtonState();
        }

        private void RosterCriteria_Changed(object sender, EventArgs e)
        {
            RefreshRoster();
        }

        private void RosterFilter_Changed(object sender, EventArgs e)
        {
            if (suppressCriteriaEvents)
                return;

            ApplyFiltersAndDisplay();
        }

        /// <summary>
        /// A checkbox cell does not normally commit until focus leaves it, so its change would
        /// not be seen here. Committing as soon as it goes dirty makes CellValueChanged fire for
        /// both a mouse click and a space-bar toggle.
        /// </summary>
        private void dgvRoster_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (suppressDuesCellEvents || !dgvRoster.IsCurrentCellDirty)
                return;

            if (dgvRoster.CurrentCell?.OwningColumn == colDuesPaid)
                dgvRoster.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>
        /// Ticking the Dues Paid box marks that year's dues confirmed for the student; clearing
        /// it removes the marker. The dojo tracks the money in its own accounting system, so no
        /// amount is collected here.
        /// </summary>
        private void dgvRoster_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (suppressDuesCellEvents || e.RowIndex < 0 || e.ColumnIndex != colDuesPaid.Index)
                return;

            // Taken from the row itself, not from displayedEntries by index, because sorting
            // reorders the rows and this writes dues against a specific student.
            KubkRosterEntry entry = dgvRoster.Rows[e.RowIndex].Tag as KubkRosterEntry;

            if (entry == null)
                return;

            // Follow the state the user actually set rather than inverting what was loaded, so a
            // keyboard toggle and a mouse click both do what the checkbox now shows.
            bool markAsPaid = Convert.ToBoolean(dgvRoster.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            // Leave edit mode before the refresh below clears the rows out from under it.
            dgvRoster.EndEdit();

            if (kubkFunctions.SetDuesPaid(entry.StudentID, SelectedDuesYear, markAsPaid))
            {
                Log.Information($"Marked {SelectedDuesYear} dues {(markAsPaid ? "paid" : "unpaid")} for student {entry.StudentID}");
            }
            else
            {
                MessageService.ShowErrorMessage(
                    $"Error updating the dues status for {entry.FullName}. The change may not have been saved.",
                    "Error Updating Dues");
            }

            // Either way, re-read so the checkbox cannot be left showing a state the database
            // does not hold.
            RefreshRoster();
        }

        private void btnVerifyRank_Click(object sender, EventArgs e)
        {
            KubkRosterEntry entry = SelectedEntry;

            if (entry == null)
                return;

            DialogResult confirmation = MessageService.ShowAreYouSureMessage(
                $"Confirm that {entry.FullName} currently holds the rank of {entry.Rank} in {entry.Art}?",
                "Verify Rank?");

            if (confirmation != DialogResult.Yes)
                return;

            if (kubkFunctions.VerifyStudentRank(entry.StudentID, entry.Art, DateTime.Today))
            {
                Log.Information($"Verified {entry.Art} rank for student {entry.StudentID}");
                RefreshRoster();
            }
            else
            {
                MessageService.ShowErrorMessage(
                    $"Error verifying the rank for {entry.FullName}. The change may not have been saved.",
                    "Error Verifying Rank");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Backing item for the dojo selector, so the combo can show a friendly name while
        /// carrying the club id the repository actually filters on.
        /// </summary>
        private class DojoOption
        {
            public string ClubID { get; set; }
            public string Display { get; set; }
        }
    }
}
