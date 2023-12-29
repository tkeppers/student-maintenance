using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public partial class StudentMaintenanceUI : Form
    {
        int currentStudentID;
        private readonly IDataAccess dataAccess;
        private readonly StudentMaintenanceFunctions studentMaintenanceFunctions;
        private Student currentStudent;
        private StudentArtsAndRank selectedArt;
        private DataTable promotionRequirements;

        public StudentMaintenanceUI(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            studentMaintenanceFunctions = new StudentMaintenanceFunctions();
            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"Windsong Dojo Student Maintenance Application - Version {version.Major}.{version.Minor}";

            if (dataAccess.DatabaseExistsAndIsValid() == false)
                return;

            //Read this lookup table into memory up front because it will be accessed for 
            //most operations involving students and martial arts
            promotionRequirements = dataAccess.GetStudentPromotionRequirements();
        }

        private void LoadFormInformation()
        {
            txtMessages.Clear();

            currentStudent = studentMaintenanceFunctions.PopulateStudentData(dataAccess, currentStudentID);

            PopulateStudentInformation();

            PopulateArtsAndRanks();

            selectedArt = new StudentArtsAndRank();
        }

        private void PopulateStudentInformation()
        {
            txtFirstName.Text = currentStudent.FirstName;
            txtLastName.Text = currentStudent.LastName;
            txtAddress1.Text = currentStudent.Address1;
            txtAddress2.Text = currentStudent.Address2;
            txtCity.Text = currentStudent.AddressCity;
            txtState.Text = currentStudent.AddressState;
            txtZipCode.Text = currentStudent.AddressZip;
            txtPrimaryPhone.Text = currentStudent.PrimaryPhoneNumber;
            txtSecondaryPhone.Text = currentStudent.SecondaryPhoneNumber;
            txtEmailAddress.Text = currentStudent.EmailAddress;
            cbActiveStudent.Checked = currentStudent.ActiveMember;
            txtHomeDojo.Text = currentStudent.HomeDojo;
            dtBirthdate.Value = currentStudent.DateOfBirth;
            dtBirthdate.CustomFormat = "MM/dd/yyyy";

            if (currentStudent.StudentGender == Gender.MALE)
                rbMale.Checked = true;
            else if (currentStudent.StudentGender == Gender.FEMALE)
                rbFemale.Checked = true;
            else
                rbUnknown.Checked = true;
        }

        private void PopulateArtsAndRanks()
        {
            lvwArtsAndRanks.Items.Clear();

            foreach (StudentArtsAndRank artRank in currentStudent.StudentArtsAndRanks)
            {
                string[] subitems = { "Art", "Rank", "Start Date", "Hours", "Promotion Date", "Promotion Hours", "ID", 
                    "Last Sign-In Date", "Eligible for Promotion", "Next Rank" };
                ListViewItem item = new ListViewItem(subitems);
                item.SubItems[0].Text = artRank.StudentArt;
                item.SubItems[1].Text = artRank.Rank;
                item.SubItems[2].Text = artRank.DateStarted.HasValue ?
                    artRank.DateStarted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[3].Text = artRank.HoursInArt.ToString();
                item.SubItems[4].Text = artRank.DatePromoted.HasValue ? 
                    artRank.DatePromoted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[5].Text = artRank.PromotionHours.ToString();
                item.SubItems[6].Text = artRank.StudentArtID.ToString();
                item.SubItems[7].Text = artRank.DateOfLatestSignIn.ToString();

                NotifyIfEligibleForPromotion(artRank);

                if (artRank.EligibleForPromotion)
                {
                    item.SubItems[8].Text = "Y";
                    item.SubItems[9].Text = artRank.NextRank;
                }

                lvwArtsAndRanks.Items.Add(item);
            }
        }

        private void NotifyIfEligibleForPromotion(StudentArtsAndRank currentArtRank)
        {
            PromotionCriteria eligibility = new PromotionCriteria(promotionRequirements);
            
            eligibility.GetNextPromotionCriteria(currentArtRank);
            currentArtRank.EligibleForPromotion = currentStudent.IsEligibleForPromotion(currentArtRank, eligibility);

            if (currentArtRank.EligibleForPromotion)
                txtMessages.Text += "Eligible for promotion to " + eligibility.NextRank + " in " + eligibility.CurrentArt + "\n";
        }


        private void FilterStudentList()
        {
            string filterExpression = $"stud_id is not null ";  //Generic statement that will always be true

            if (!cbNonWindsongStudents.Checked)
                filterExpression += "and stud_club = 'Windsong' ";

            if (!cbShowInactiveStudents.Checked)
                filterExpression += "and stud_status = 'A' ";

            if (!string.IsNullOrWhiteSpace(txtFirstNameFilter.Text))
                filterExpression += " and stud_firstname LIKE '" + txtFirstNameFilter.Text + "%'";

            if (!string.IsNullOrWhiteSpace(txtLastNameFilter.Text))
                filterExpression += " and stud_lastname LIKE '" + txtLastNameFilter.Text + "%'";


            DataView dv = (DataView)dgvStudentList.DataSource;
            dv.RowFilter = filterExpression;

            dgvStudentList.DataSource = dv;
        }

        private void RefreshStudentList()
        {
            //dgvStudentList.Rows.Clear();

            DataTable studentList = dataAccess.GetStudentTable();

            //If the database isn't accessible, an empty table will be returned. Handle this 
            //gracefully to avoid exceptions being thrown.
            if (studentList.Columns.Count == 0)
                return;

            DataView studentDataView = new DataView(studentList);

            dgvStudentList.AutoGenerateColumns = false;

            dgvStudentList.Columns.Add("StudentID", "StudentID");
            dgvStudentList.Columns.Add("FirstName", "FirstName");
            dgvStudentList.Columns.Add("LastName", "LastName");

            dgvStudentList.Columns["StudentID"].DataPropertyName = "stud_id";
            dgvStudentList.Columns["FirstName"].DataPropertyName = "stud_firstname";
            dgvStudentList.Columns["LastName"].DataPropertyName = "stud_lastname";

            dgvStudentList.DataSource = studentDataView;

            //Display only the active students initially
            studentDataView.RowFilter = "stud_status = 'A'";
        }

        private bool ValidStudentIsSelected()
        {
            if (!studentMaintenanceFunctions.IsValidStudent(currentStudentID))
            {
                MessageService.ShowErrorMessage("Please select a student before performing this action.", "Student Not Selected");
                return false;
            }

            return true;
        }

        private void SaveChanges()
        {
            //TODO: Refactor this method and give it a better name
            if (!studentMaintenanceFunctions.IsValidStudent(currentStudentID))
                return;

            DialogResult result = MessageBox.Show($"Update database for student {currentStudent.FullName}", "Save Student?", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel || result == DialogResult.No)
                return;

            currentStudent.FirstName = txtFirstName.Text.Trim();
            currentStudent.LastName = txtLastName.Text.Trim();
            currentStudent.Address1 = txtAddress1.Text.Trim();
            currentStudent.Address2 = txtAddress2.Text.Trim();
            currentStudent.AddressCity = txtCity.Text.Trim();
            currentStudent.AddressState = txtState.Text.Trim();
            currentStudent.AddressZip = txtZipCode.Text.Trim();
            currentStudent.PrimaryPhoneNumber = txtPrimaryPhone.Text.Trim();
            currentStudent.SecondaryPhoneNumber = txtSecondaryPhone.Text.Trim();
            currentStudent.EmailAddress = txtEmailAddress.Text.Trim();
            currentStudent.ActiveMember = cbActiveStudent.Checked;
            currentStudent.HomeDojo = txtHomeDojo.Text.Trim();
            currentStudent.DateOfBirth = dtBirthdate.Value;

            if (rbMale.Checked)
                currentStudent.StudentGender = Gender.MALE;
            else if (rbFemale.Checked)
                currentStudent.StudentGender = Gender.FEMALE;
            else
                currentStudent.StudentGender = Gender.UNKNOWN;

            if (dataAccess.UpdateStudent(currentStudent))
            {
                string successMessage = $"Student {currentStudent.FullName} saved successfully.";
                MessageService.ShowErrorMessage(successMessage, "Success");
                Log.Information(successMessage);
                btnSaveChanges.Enabled = false;
            }
            else
            {
                string failureMessage = $"Error updating student {currentStudent.FullName}. Changes may not have been saved to the database.";
                MessageService.ShowErrorMessage(failureMessage, "Error Saving Student Infomation");
                Log.Information(failureMessage);
            }
        }

        private void DeleteSelectedStudent()
        {
            if (!studentMaintenanceFunctions.IsValidStudent(currentStudentID))
                return;

            DialogResult result = MessageBox.Show($"Are you sure you want to delete student {currentStudent.FullName}", "Delete Student?", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            if (dataAccess.DeleteStudent(currentStudentID))
            {
                string successMessage = $"Student {currentStudent.FullName} deleted successfully.";
                MessageService.ShowErrorMessage(successMessage, "Success");
                Log.Information(successMessage);
                RefreshStudentList();
            }
            else
            {
                string failureMessage = $"Error deleting student {currentStudent.FullName}. Changes may not have been saved to the database.";
                MessageService.ShowErrorMessage(failureMessage, "Error Deleting Student");
                Log.Information(failureMessage);
            }

        }

        private void AddNewStudent()
        {
            using (StudentAddUI addStudent = new StudentAddUI(dataAccess))
            {
                DialogResult result = addStudent.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RefreshStudentList();
                    SelectLastAddedStudent();
                }
            }
        }

        private void SelectLastAddedStudent()
        {
            if (dgvStudentList.Rows.Count > 0)
            {
                int lastIndex = dgvStudentList.Rows.Count - 1;
                dgvStudentList.Rows[lastIndex].Selected = true;

                dgvStudentList.FirstDisplayedScrollingRowIndex = lastIndex;
            }
        }

        private void AddNewArtForStudent()
        {
            if (ValidStudentIsSelected())
            {
                StudentAddModifyArtUI addArt = new StudentAddModifyArtUI(currentStudentID, currentStudent.FullName, dataAccess);
                DialogResult result = addArt.ShowDialog();

                if (result == DialogResult.OK)
                    LoadFormInformation();
            }
        }

        private void ModifySelectedArt()
        {
            if (IsValidArtSelected() == false)
                return;

            StudentAddModifyArtUI modifyArt = new StudentAddModifyArtUI(currentStudentID, currentStudent.FullName, dataAccess, selectedArt);
            DialogResult result = modifyArt.ShowDialog();

            if (result == DialogResult.OK)
                LoadFormInformation();
        }

        private void DeleteSelectedArt()
        {
            if (IsValidArtSelected() == false)
                return;

            string dialogText = $"Are you sure you want to remove {selectedArt.StudentArt} from student {currentStudent.FullName}?" +
                $"\n\nThis action cannot be undone.";

            DialogResult result = MessageBox.Show(dialogText, "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result.Equals(DialogResult.Yes))
            {
                dataAccess.DeleteStudentArt(selectedArt.StudentArtID, selectedArt.StudentArt);
                LoadFormInformation();
            }
        }

        private bool IsValidArtSelected()
        {
            if (lvwArtsAndRanks.SelectedItems.Count == 0)
            {
                MessageService.ShowErrorMessage("No art is selected. Please select a valid art before performing this action.", "No Art Selected");
                return false;
            }

            return true;
        }

        private void StudentMaintenance_Load(object sender, EventArgs e)
        {
            RefreshStudentList();
        }

        private void btnPromotionHistory_Click(object sender, EventArgs e)
        {
            StudentPromotionHistoryUI promotions = new StudentPromotionHistoryUI(dataAccess, currentStudentID, currentStudent.FullName);
            promotions.ShowDialog();
        }

        private void btnSignInHistory_Click(object sender, EventArgs e)
        {
            StudentSignInHistoryUI signins = new StudentSignInHistoryUI(dataAccess, currentStudentID, currentStudent.FullName);
            signins.ShowDialog();
        }

        private void dgvStudentList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudentList.SelectedRows.Count == 0)
                return;

            currentStudentID = Convert.ToInt32(dgvStudentList.SelectedRows[0].Cells[0].Value.ToString());
            LoadFormInformation();
            Log.Information($"Loaded information for student {currentStudent.FullName}");
        }

        private void cbShowInactiveStudents_CheckedChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void cbNonWindsongStudents_CheckedChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void txtFirstNameFilter_TextChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void txtLastNameFilter_TextChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void databasePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseConfigUI config = new DatabaseConfigUI();
            config.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutProgramUI about = new AboutProgramUI();
            about.ShowDialog();
        }

        private void tsbAddNewStudent_Click(object sender, EventArgs e)
        {
            AddNewStudent();
        }

        private void btnAddArt_Click(object sender, EventArgs e)
        {
            AddNewArtForStudent();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void txtEmailAddress_MouseLeave(object sender, EventArgs e)
        {
            btnSaveChanges.Enabled = true;
        }

        private void lvwArtsAndRanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwArtsAndRanks.SelectedItems.Count > 0)
            {
                ListViewItem selecteditem = lvwArtsAndRanks.SelectedItems[0];
                selectedArt.StudentArt = selecteditem.SubItems[0].Text;
                selectedArt.Rank = selecteditem.SubItems[1].Text;
                selectedArt.DateStarted = DateTime.TryParse(selecteditem.SubItems[2].Text, out DateTime dateStarted) ? dateStarted : (DateTime?)null;
                selectedArt.HoursInArt = double.TryParse(selecteditem.SubItems[3].Text, out double hoursInArt) ? hoursInArt : 0;
                selectedArt.DatePromoted = DateTime.TryParse(selecteditem.SubItems[4].Text, out DateTime datePromoted) ? datePromoted : (DateTime?)null;
                selectedArt.PromotionHours = double.TryParse(selecteditem.SubItems[5].Text, out double promotionHours) ? hoursInArt : 0;
                selectedArt.StudentArtID = int.TryParse(selecteditem.SubItems[6].Text, out int studentArtID) ? studentArtID : 0;
                selectedArt.DateOfLatestSignIn = DateTime.TryParse(selecteditem.SubItems[7].Text, out DateTime dateOfLatestSignin) ? dateOfLatestSignin : (DateTime?)null;
                selectedArt.EligibleForPromotion = selecteditem.SubItems[8].Text.Contains("Y");
                selectedArt.NextRank = selecteditem.SubItems[9].Text;
            }
        }

        private void btnModifyArt_Click(object sender, EventArgs e)
        {
            ModifySelectedArt();
        }

        private void tsbPromotionSettings_Click(object sender, EventArgs e)
        {
            PromotionCriteriaUI promotionSettings = new PromotionCriteriaUI(promotionRequirements);
            promotionSettings.ShowDialog();
        }

        private void btnPromoteStudent_Click(object sender, EventArgs e)
        {
            //TODO: If there isn't a student art selected AND the student is only in one art, automatically select that one 
            //so that the user doesn't have to. 
            //TODO: If there isn't a student art selected AND the student is in multiple arts, but is only eligible for 
            //promotion in one of them, automatically select that one.
            //Otherwise, display a message to the user advising them to choose an art to promote the student in
            if (IsValidArtSelected() == false)
                return;

            PromoteStudentUI promote = new PromoteStudentUI(currentStudent, selectedArt, promotionRequirements, dataAccess);
            promote.ShowDialog();
        }

        private void btnRemoveArt_Click(object sender, EventArgs e)
        {
            DeleteSelectedArt();
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            DeleteSelectedStudent();
        }
    }
}
