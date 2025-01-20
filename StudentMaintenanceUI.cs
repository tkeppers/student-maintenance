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
        private readonly IDataRepository dataRepository;
        private readonly StudentMaintenanceFunctions studentMaintenanceFunctions;
        private Student currentStudent;
        private StudentArtsAndRank selectedArt;
        private DataTable promotionRequirements;

        public StudentMaintenanceUI(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
            studentMaintenanceFunctions = new StudentMaintenanceFunctions();
            InitializeComponent();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"Windsong Dojo Student Maintenance Application - Version {version.Major}.{version.Minor}";

            //Read this lookup table into memory up front because it will be accessed for 
            //most operations involving students and martial arts
            promotionRequirements = dataRepository.GetStudentPromotionRequirements();
        }

        private void LoadFormInformation()
        {
            richTextPromotionEligibility.Clear();

            currentStudent = studentMaintenanceFunctions.PopulateStudentData(dataRepository, currentStudentID);

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

            //Fix null reference exception if there is not actually a valid date of birth for the
            //student that is automatically selected (it happens on a few legacy students)
            if (currentStudent.DateOfBirth > DateTimePicker.MinimumDateTime && currentStudent.DateOfBirth < DateTimePicker.MaximumDateTime)
                dtBirthdate.Value = currentStudent.DateOfBirth;
            else
                dtBirthdate.Value = DateTimePicker.MinimumDateTime; // or some default value
           
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

            foreach (StudentArtsAndRank studentArtRank in currentStudent.StudentArtsAndRanks)
            {
                ListViewItem item = CreateListViewItemForArt(studentArtRank);
                lvwArtsAndRanks.Items.Add(item);
            }
        }

        private ListViewItem CreateListViewItemForArt(StudentArtsAndRank studentArtRank)
        {
            string[] subitems = new string[10];
            subitems[0] = studentArtRank.StudentArt;
            subitems[1] = studentArtRank.Rank;
            subitems[2] = FormatDate(studentArtRank.DateStarted);
            subitems[3] = studentArtRank.HoursInArt.ToString();
            subitems[4] = FormatDate(studentArtRank.DatePromoted);
            subitems[5] = studentArtRank.PromotionHours.ToString();
            subitems[6] = studentArtRank.StudentArtID.ToString();
            subitems[7] = studentArtRank.DateOfLatestSignIn.ToString();
            subitems[8] = studentArtRank.EligibleForPromotion ? "Y" : string.Empty;
            subitems[9] = studentArtRank.EligibleForPromotion ? studentArtRank.NextRank : string.Empty;

            ListViewItem item = new ListViewItem(subitems);

            NotifyIfEligibleForPromotion(studentArtRank);

            return item;
        }

        private string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        private void NotifyIfEligibleForPromotion(StudentArtsAndRank currentArtRank)
        {
            //PromotionCriteria eligibility = new PromotionCriteria(promotionRequirements);
            
            //eligibility.GetNextPromotionCriteria(currentArtRank);

            PromotionCriteria eligibility = GetPromotionEligibilityRequirements(currentArtRank);
            eligibility.GetNextPromotionCriteria(currentArtRank);
            
            if (currentArtRank != null)
                UpdateEligibilityDisplay(currentArtRank, eligibility);
            //currentArtRank.EligibleForPromotion = currentStudent.IsEligibleForPromotion(currentArtRank, eligibility);

            //if (currentArtRank.EligibleForPromotion)
            //    txtMessages.Text += "Eligible for promotion to " + eligibility.NextRank + " in " + eligibility.CurrentArt + "\n";
        }

        private PromotionCriteria GetPromotionEligibilityRequirements(StudentArtsAndRank currentArtRank)
        {
            PromotionCriteria eligibility = new PromotionCriteria(promotionRequirements);
            eligibility.GetNextPromotionCriteria(currentArtRank);

            return eligibility;
        }

        private void UpdateEligibilityDisplay(StudentArtsAndRank currentArtRank, PromotionCriteria eligibility)
        {
            richTextPromotionEligibility.Clear();
            bool isEligible = false;

            //Edge case: we don't have eligibility data for the next rank (usually because the 
            //student is so highly ranked that there are no further promotions available, or promotions
            //are awarded based on other criteria)
            if (eligibility.NextRank == null)
            {
                richTextPromotionEligibility.Text = $"No promotion requirements available for {currentArtRank.StudentArt} - {currentArtRank.Rank}";
                return;
            }

            GenerateHeaderText(currentArtRank);
            //richTextPromotionEligibility.Text = $"Promotion Eligibility for {currentStudent.FullName}: {currentArtRank.StudentArt} - {currentArtRank.NextRank}\n";

            // Check and store eligibility for minimum age
            isEligible = currentStudent.StudentHasReachedMinimumAgeForPromotion(eligibility);
            string ageEligibilityText = isEligible
                ? "Minimum age: ELIGIBLE\n"
                : "Minimum age: NOT ELIGIBLE\n" +
                  " Student Age: " + currentStudent.StudentAgeInYears + ". Min Age: " + eligibility.MinimumAge + "\n";
            AppendColoredText(richTextPromotionEligibility, ageEligibilityText, isEligible ? Color.Green : Color.Red);

            // Check and store eligibility for training hours
            isEligible = currentStudent.StudentHasSufficientTrainingHoursForPromotion(currentArtRank.HoursInArt, eligibility);
            string trainingHoursText = isEligible
                ? "Total Training Hours: ELIGIBLE\n"
                : "Total Training Hours: NOT ELIGIBLE\n" +
                  "     Earned: " + currentArtRank.HoursInArt + " hours. Required: " + eligibility.MinimumTrainingHours + " hours\n";
            AppendColoredText(richTextPromotionEligibility, trainingHoursText, isEligible ? Color.Green : Color.Red);

            // Check and store eligibility for time in art
            isEligible = currentStudent.StudentHasSufficientTimeInArtForPromotion(currentArtRank.TotalYearsInArt(), eligibility);
            string timeInArtText = isEligible
                ? "Time in Art: ELIGIBLE\n"
                : "Time in Art: NOT ELIGIBLE\n" +
                  "     Earned: " + currentArtRank.TotalYearsInArt() + " years. Required: " + eligibility.YearsInArt + " years\n";
            AppendColoredText(richTextPromotionEligibility, timeInArtText, isEligible ? Color.Green : Color.Red);

            //Display eligibility based on student's time at current rank
            isEligible = currentStudent.StudentHasSufficientTimeAtCurrentRank(currentArtRank.YearsAtCurrentLevel(), eligibility);

            string eligibilityText = isEligible
                ? "Time at current rank: ELIGIBLE\n"
                : "Time at current rank: NOT ELIGIBLE\n" +
                  "     Earned: " + currentArtRank.YearsAtCurrentLevel() + " years. Required: " + eligibility.YearsAtCurrentRank + " years\n";

            AppendColoredText(richTextPromotionEligibility, eligibilityText, isEligible ? Color.Green : Color.Red);
        }

        private void GenerateHeaderText(StudentArtsAndRank currentArtRank)
        {
            // Clear the RichTextBox
            richTextPromotionEligibility.Clear();

            // Set the text with bold and underlined formatting
            richTextPromotionEligibility.SelectionStart = richTextPromotionEligibility.TextLength;
            richTextPromotionEligibility.SelectionLength = 0;
            richTextPromotionEligibility.SelectionFont = new Font(
                richTextPromotionEligibility.Font,
                FontStyle.Bold | FontStyle.Underline
            );
            richTextPromotionEligibility.AppendText($"{currentStudent.FullName}: {currentArtRank.StudentArt} - {currentArtRank.NextRank}\n");

            // Reset the font to avoid applying bold/underline to subsequent text
            richTextPromotionEligibility.SelectionFont = new Font(
                richTextPromotionEligibility.Font,
                FontStyle.Regular
            );
        }

        private void AppendColoredText(RichTextBox richTextBox, string text, Color color)
        {
            // Set the selection to the end of the current text
            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;

            // Set the color for the appended text
            richTextBox.SelectionColor = color;

            // Append the text
            richTextBox.AppendText(text);

            // Reset the selection color to the default
            richTextBox.SelectionColor = richTextBox.ForeColor;
        }


        private void FilterStudentList()
        {
            string filterExpression = $"StudentID is not null ";  //Generic statement that will always be true

            if (!cbNonWindsongStudents.Checked)
                filterExpression += "and StudentDojo = 'Windsong' ";

            if (!cbShowInactiveStudents.Checked)
                filterExpression += "and StudentStatus = 'A' ";

            if (!string.IsNullOrWhiteSpace(txtFirstNameFilter.Text))
                filterExpression += " and StudentFirstName LIKE '" + txtFirstNameFilter.Text + "%'";

            if (!string.IsNullOrWhiteSpace(txtLastNameFilter.Text))
                filterExpression += " and StudentLastName LIKE '" + txtLastNameFilter.Text + "%'";


            DataView dv = (DataView)dgvStudentList.DataSource;
            dv.RowFilter = filterExpression;

            dgvStudentList.DataSource = dv;
        }

        private void RefreshStudentList()
        {
            //dgvStudentList.Rows.Clear();

            DataTable studentList = dataRepository.GetStudentTable();

            //If the database isn't accessible, an empty table will be returned. Handle this 
            //gracefully to avoid exceptions being thrown.
            if (studentList.Columns.Count == 0)
                return;

            DataView studentDataView = new DataView(studentList);

            dgvStudentList.AutoGenerateColumns = false;

            dgvStudentList.Columns.Add("StudentID", "StudentID");
            dgvStudentList.Columns.Add("FirstName", "FirstName");
            dgvStudentList.Columns.Add("LastName", "LastName");

            dgvStudentList.Columns["StudentID"].DataPropertyName = "StudentID";
            dgvStudentList.Columns["FirstName"].DataPropertyName = "StudentFirstName";
            dgvStudentList.Columns["LastName"].DataPropertyName = "StudentLastName";

            dgvStudentList.Columns["StudentID"].Width = 100;
            dgvStudentList.Columns["FirstName"].Width = 150; 
            dgvStudentList.Columns["LastName"].Width = 150;

            dgvStudentList.DataSource = studentDataView;

            //Display only the active students initially
            studentDataView.RowFilter = "StudentStatus = 'A'";
        }

        private bool IsSelectedStudentValid()
        {
            if (!studentMaintenanceFunctions.IsValidStudent(currentStudentID))
            {
                MessageService.ShowErrorMessage("Please select a student before performing this action.", "Student Not Selected");
                return false;
            }

            return true;
        }

        private void LoadInformationForSelectedArt()
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

                PromotionCriteria eligibility = GetPromotionEligibilityRequirements(selectedArt);
                UpdateEligibilityDisplay(selectedArt, eligibility);
            }
        }

        private void UpdateStudentInformation()
        {
            //TODO: Refactor this method and give it a better name
            if (!studentMaintenanceFunctions.IsValidStudent(currentStudentID))
                return;

            DialogResult result = MessageService.ShowAreYouSureMessage($"Update database for student {currentStudent.FullName}", "Save Student?");

            if (result == DialogResult.No)
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

            if (dataRepository.UpdateStudent(currentStudent))
            {
                string successMessage = $"Student {currentStudent.FullName} saved successfully.";
                MessageService.ShowInformationMessage(successMessage, "Success");
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

            DialogResult result = MessageService.ShowAreYouSureMessage($"Are you sure you want to delete student {currentStudent.FullName}", "Delete Student?");

            if (result == DialogResult.No)
                return;

            if (dataRepository.DeleteStudent(currentStudentID))
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
            using (StudentAddUI addStudent = new StudentAddUI(dataRepository))
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
            if (IsSelectedStudentValid())
            {
                StudentAddModifyArtUI addArt = new StudentAddModifyArtUI(currentStudentID, currentStudent.FullName, dataRepository);
                DialogResult result = addArt.ShowDialog();

                if (result == DialogResult.OK)
                    LoadFormInformation();
            }
        }

        private void ModifySelectedArt()
        {
            if (IsValidArtSelected() == false)
                return;

            StudentAddModifyArtUI modifyArt = new StudentAddModifyArtUI(currentStudentID, currentStudent.FullName, dataRepository, selectedArt);
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

            DialogResult result = MessageService.ShowAreYouSureMessage(dialogText, "Are You Sure?");
            if (result == DialogResult.Yes)
            {
                dataRepository.DeleteStudentArt(selectedArt.StudentArtID, selectedArt.StudentArt);
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
            StudentPromotionHistoryUI promotions = new StudentPromotionHistoryUI(dataRepository, currentStudentID, currentStudent.FullName);
            promotions.ShowDialog();
        }

        private void btnSignInHistory_Click(object sender, EventArgs e)
        {
            StudentSignInHistoryUI signins = new StudentSignInHistoryUI(dataRepository, currentStudentID, currentStudent.FullName);
            signins.ShowDialog();
        }

        private void dgvStudentList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudentList.SelectedRows.Count == 0)
                return;

            currentStudentID = Convert.ToInt32(dgvStudentList.SelectedRows[0].Cells[0].Value.ToString());
            LoadFormInformation();
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
            ApplicationSettingsUI config = new ApplicationSettingsUI();
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
            UpdateStudentInformation();
        }

        private void txtEmailAddress_MouseLeave(object sender, EventArgs e)
        {
            btnSaveChanges.Enabled = true;
        }

        private void lvwArtsAndRanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadInformationForSelectedArt();
        }

        private void btnModifyArt_Click(object sender, EventArgs e)
        {
            ModifySelectedArt();
        }

        private void tsbPromotionSettings_Click(object sender, EventArgs e)
        {
            DataTable listOfArts = dataRepository.GetListOfArts();
            PromotionCriteriaUI promotionSettings = new PromotionCriteriaUI(dataRepository);
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

            PromoteStudentUI promote = new PromoteStudentUI(currentStudent, selectedArt, promotionRequirements, dataRepository);
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
