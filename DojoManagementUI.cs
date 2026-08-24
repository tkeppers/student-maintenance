using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// Maintains the KUBK member-dojo registry (the Club_Parameters table).
    ///
    /// Change tracking here is explicit - the edit panel writes into a Dojo object only when the
    /// user saves, and a dirty flag drives the Save button. This deliberately avoids the
    /// DataRow.RowState approach used by PromotionCriteriaUI, which CLAUDE.md documents as
    /// fragile because a stray AcceptChanges() silently discards pending edits.
    /// </summary>
    public partial class DojoManagementUI : Form
    {
        private readonly KubkManagementFunctions kubkFunctions;

        private List<Dojo> allDojos = new List<Dojo>();
        private List<Dojo> displayedDojos = new List<Dojo>();
        private Dictionary<string, int> studentCountsByDojo = new Dictionary<string, int>();
        private Dojo selectedDojo;

        private bool isDirty;
        private bool isAddingNew;

        /// <summary>
        /// Set while the edit panel is being populated from code so the field-changed handlers
        /// do not mistake programmatic updates for user edits.
        /// </summary>
        private bool suppressFieldChangeEvents;

        public DojoManagementUI(IDataRepository dataRepository)
        {
            InitializeComponent();

            kubkFunctions = new KubkManagementFunctions(dataRepository);
        }

        private void DojoManagementUI_Load(object sender, EventArgs e)
        {
            LoadDojos();
        }

        private void LoadDojos(string clubIdToReselect = null)
        {
            allDojos = kubkFunctions.GetDojos();

            // Loaded once per refresh rather than per selection, so the Delete button can be
            // enabled or disabled without a database round trip every time the grid moves.
            studentCountsByDojo = kubkFunctions.GetStudentCountsByDojo();

            RefreshDojoGrid(clubIdToReselect);
        }

        private void RefreshDojoGrid(string clubIdToReselect = null)
        {
            displayedDojos = allDojos
                .Where(d => !cbHideInactive.Checked || d.Active)
                .OrderBy(d => d.Name)
                .ToList();

            suppressFieldChangeEvents = true;
            dgvDojos.Rows.Clear();

            foreach (Dojo dojo in displayedDojos)
            {
                dgvDojos.Rows.Add(dojo.ClubID, dojo.Name, dojo.Instructor, dojo.Active ? "Yes" : "No");
            }

            suppressFieldChangeEvents = false;

            if (dgvDojos.Rows.Count == 0)
            {
                selectedDojo = null;
                ClearDojoDetails();
                return;
            }

            int indexToSelect = 0;

            if (!string.IsNullOrEmpty(clubIdToReselect))
            {
                int foundIndex = displayedDojos.FindIndex(d =>
                    string.Equals(d.ClubID, clubIdToReselect, StringComparison.OrdinalIgnoreCase));

                if (foundIndex >= 0)
                    indexToSelect = foundIndex;
            }

            dgvDojos.ClearSelection();
            dgvDojos.Rows[indexToSelect].Selected = true;
            dgvDojos.FirstDisplayedScrollingRowIndex = indexToSelect;

            // Selecting a row while the grid was being rebuilt may not have raised
            // SelectionChanged, so populate the panel explicitly.
            LoadSelectedDojoIntoDetails();
        }

        private void dgvDojos_SelectionChanged(object sender, EventArgs e)
        {
            if (suppressFieldChangeEvents)
                return;

            PromptToSaveOutstandingChanges();
            LoadSelectedDojoIntoDetails();
        }

        /// <summary>
        /// If the user has pending edits, offer to save them before moving on. Answering No
        /// discards the edits - the selection change itself is never blocked, which keeps the
        /// grid's behavior predictable.
        /// </summary>
        private void PromptToSaveOutstandingChanges()
        {
            if (!isDirty)
                return;

            string name = isAddingNew ? "the new dojo" : selectedDojo?.Name ?? "this dojo";
            DialogResult result = MessageService.ShowAreYouSureMessage(
                $"You have unsaved changes to {name}. Save them now?", "Unsaved Changes");

            if (result == DialogResult.Yes)
                SaveCurrentDojo();
            else
                ClearDirtyState();
        }

        private void LoadSelectedDojoIntoDetails()
        {
            isAddingNew = false;

            if (dgvDojos.SelectedRows.Count == 0)
            {
                selectedDojo = null;
                ClearDojoDetails();
                UpdateDeleteButtonState();
                return;
            }

            int index = dgvDojos.SelectedRows[0].Index;

            if (index < 0 || index >= displayedDojos.Count)
                return;

            selectedDojo = displayedDojos[index];
            PopulateDojoDetails(selectedDojo);
            UpdateDeleteButtonState();
        }

        /// <summary>
        /// Delete is offered only for a dojo no student points at. Anything with students can
        /// only be deactivated, so no student can ever be left referencing a dojo that is gone.
        /// </summary>
        private void UpdateDeleteButtonState()
        {
            if (isAddingNew || selectedDojo == null)
            {
                btnDeleteDojo.Enabled = false;
                return;
            }

            int studentCount = KubkManagementFunctions.GetStudentCountForDojo(studentCountsByDojo, selectedDojo.ClubID);
            btnDeleteDojo.Enabled = studentCount == 0;
        }

        private void PopulateDojoDetails(Dojo dojo)
        {
            suppressFieldChangeEvents = true;

            txtClubID.Text = dojo.ClubID;
            txtName.Text = dojo.Name;
            txtInstructor.Text = dojo.Instructor;
            txtInstructorEmail.Text = dojo.InstructorEmail;
            txtPhone.Text = dojo.Phone;
            txtAddress1.Text = dojo.Address1;
            txtAddress2.Text = dojo.Address2;
            txtAddress3.Text = dojo.Address3;
            numAnnualDues.Value = ClampToDuesRange(dojo.AnnualDues);
            cbActive.Checked = dojo.Active;
            txtNotes.Text = dojo.Notes;

            // Club ID is the key Students.stud_club points at; renaming it would orphan every
            // student at this dojo, so it is only editable while adding a brand-new dojo.
            txtClubID.ReadOnly = true;

            suppressFieldChangeEvents = false;
            ClearDirtyState();
        }

        private void ClearDojoDetails()
        {
            suppressFieldChangeEvents = true;

            txtClubID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtInstructor.Text = string.Empty;
            txtInstructorEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            numAnnualDues.Value = 0;
            cbActive.Checked = true;
            txtNotes.Text = string.Empty;

            suppressFieldChangeEvents = false;
            ClearDirtyState();
        }

        /// <summary>
        /// NumericUpDown throws if assigned a value outside its configured range, and legacy
        /// annual_dues values are not guaranteed to be inside it.
        /// </summary>
        private decimal ClampToDuesRange(decimal value)
        {
            if (value < numAnnualDues.Minimum)
                return numAnnualDues.Minimum;

            if (value > numAnnualDues.Maximum)
                return numAnnualDues.Maximum;

            return value;
        }

        private void DojoField_Changed(object sender, EventArgs e)
        {
            if (suppressFieldChangeEvents)
                return;

            isDirty = true;
            btnSave.Enabled = true;
        }

        private void ClearDirtyState()
        {
            isDirty = false;
            btnSave.Enabled = false;
        }

        private void cbHideInactive_CheckedChanged(object sender, EventArgs e)
        {
            PromptToSaveOutstandingChanges();
            RefreshDojoGrid(selectedDojo?.ClubID);
        }

        private void btnAddDojo_Click(object sender, EventArgs e)
        {
            PromptToSaveOutstandingChanges();

            selectedDojo = null;
            ClearDojoDetails();

            suppressFieldChangeEvents = true;
            dgvDojos.ClearSelection();
            suppressFieldChangeEvents = false;

            isAddingNew = true;
            txtClubID.ReadOnly = false;
            txtClubID.Focus();

            UpdateDeleteButtonState();
        }

        private void btnDeleteDojo_Click(object sender, EventArgs e)
        {
            if (selectedDojo == null)
                return;

            string dojoName = string.IsNullOrWhiteSpace(selectedDojo.Name) ? selectedDojo.ClubID : selectedDojo.Name;

            DialogResult confirmation = MessageService.ShowAreYouSureMessage(
                $"Permanently delete the dojo {dojoName} ({selectedDojo.ClubID})?\n\nThis cannot be undone.",
                "Delete Dojo?");

            if (confirmation != DialogResult.Yes)
                return;

            if (kubkFunctions.DeleteDojo(selectedDojo, out string error))
            {
                string message = $"Dojo {selectedDojo.ClubID} deleted.";
                Log.Information(message);

                selectedDojo = null;
                ClearDirtyState();
                LoadDojos();

                MessageService.ShowInformationMessage(message, "Dojo Deleted");
            }
            else
            {
                MessageService.ShowErrorMessage(error, "Unable to Delete Dojo");

                // The refusal may be because the dojo gained students since this screen loaded,
                // so reload to bring the counts (and the button state) back in step.
                LoadDojos(selectedDojo?.ClubID);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentDojo();
        }

        private void SaveCurrentDojo()
        {
            if (!isAddingNew && selectedDojo == null)
                return;

            Dojo dojoToSave = isAddingNew ? new Dojo() : selectedDojo;
            ApplyDetailsToDojo(dojoToSave);

            if (!kubkFunctions.SaveDojo(dojoToSave, isAddingNew, out string validationError))
            {
                MessageService.ShowErrorMessage(validationError, "Unable to Save Dojo");

                // Re-load so an in-memory object mutated by a failed save cannot drift out of
                // sync with what is actually stored.
                LoadDojos(isAddingNew ? null : dojoToSave.ClubID);
                return;
            }

            string savedClubId = dojoToSave.ClubID;
            bool wasAdding = isAddingNew;

            ClearDirtyState();
            isAddingNew = false;

            LoadDojos(savedClubId);

            string message = wasAdding
                ? $"Dojo {savedClubId} added successfully."
                : $"Dojo {savedClubId} saved successfully.";

            MessageService.ShowInformationMessage(message, "Success");
            Log.Information(message);
        }

        private void ApplyDetailsToDojo(Dojo dojo)
        {
            dojo.ClubID = txtClubID.Text.Trim();
            dojo.Name = txtName.Text.Trim();
            dojo.Instructor = txtInstructor.Text.Trim();
            dojo.InstructorEmail = txtInstructorEmail.Text.Trim();
            dojo.Phone = txtPhone.Text.Trim();
            dojo.Address1 = txtAddress1.Text.Trim();
            dojo.Address2 = txtAddress2.Text.Trim();
            dojo.Address3 = txtAddress3.Text.Trim();
            dojo.AnnualDues = numAnnualDues.Value;
            dojo.Active = cbActive.Checked;
            dojo.Notes = txtNotes.Text.Trim();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DojoManagementUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            PromptToSaveOutstandingChanges();
        }
    }
}
