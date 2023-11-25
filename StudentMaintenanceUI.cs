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
        private IDataAccess dataAccess;
        string currentStudentName;

        public StudentMaintenanceUI(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            InitializeComponent();
        }

        private void PopulateStudentInformation()
        {
            StudentMaintenanceFunctions s = new StudentMaintenanceFunctions();
            Student currentStudent = s.PopulateStudentData(dataAccess, currentStudentID);

            this.currentStudentName = currentStudent.FirstName + " " + currentStudent.LastName;

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

            PopulateArtsAndRanks(currentStudent);
        }

        private void PopulateArtsAndRanks(Student currentStudent)
        {
            lvwArtsAndRanks.Items.Clear();

            foreach (StudentArtsAndRank artRank in currentStudent.StudentArtsAndRanks)
            {
                string[] subitems = { "Art", "Rank", "Start Date", "Hours", "Promotion Date", "Promotion Hours" };
                ListViewItem item = new ListViewItem(subitems);
                item.SubItems[0].Text = artRank.Art;
                item.SubItems[1].Text = artRank.Rank;
                item.SubItems[2].Text = artRank.DateStarted.HasValue ?
                    artRank.DateStarted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[3].Text = artRank.HoursInArt.ToString();
                item.SubItems[4].Text = artRank.DatePromoted.HasValue ? 
                    artRank.DatePromoted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[5].Text = artRank.PromotionHours.ToString();

                lvwArtsAndRanks.Items.Add(item);
            }

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

        private void PopulateStudentList()
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

        private void AddStudent_StudentAdded(object sender, EventArgs e)
        {
            PopulateStudentList();

            //Automatically select the most recently added student and scroll
            //to the new student's row to make it visible
            if (dgvStudentList.Rows.Count > 0)
            {
                int lastIndex = dgvStudentList.Rows.Count - 1;
                dgvStudentList.Rows[lastIndex].Selected = true;

                dgvStudentList.FirstDisplayedScrollingRowIndex = lastIndex;
            }
        }

        private void StudentMaintenance_Load(object sender, EventArgs e)
        {
            PopulateStudentList();
        }

        private void btnPromotionHistory_Click(object sender, EventArgs e)
        {
            StudentPromotionHistoryUI promotions = new StudentPromotionHistoryUI(dataAccess, currentStudentID, currentStudentName);
            promotions.ShowDialog();

        }

        private void btnSignInHistory_Click(object sender, EventArgs e)
        {
            StudentSignInHistoryUI signins = new StudentSignInHistoryUI(dataAccess, currentStudentID, currentStudentName);
            signins.ShowDialog();
        }

        private void dgvStudentList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudentList.SelectedRows.Count == 0)
                return;

            currentStudentID = Convert.ToInt32(dgvStudentList.SelectedRows[0].Cells[0].Value.ToString());
            PopulateStudentInformation();
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

        private void btnAddNewStudent_Click(object sender, EventArgs e)
        {
            StudentAddUI addStudent = new StudentAddUI(dataAccess);
            addStudent.StudentAdded += AddStudent_StudentAdded;

            addStudent.ShowDialog();
        }

        private void databaseConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseConfigUI updateDatabasePath = new DatabaseConfigUI();
            updateDatabasePath.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutProgramUI about = new AboutProgramUI();
            about.ShowDialog();
        }
    }
}
