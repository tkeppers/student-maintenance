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

        public StudentMaintenanceUI(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            InitializeComponent();
        }

        private void PopulateStudentInformation()
        {
            StudentMaintenanceFunctions s = new StudentMaintenanceFunctions();
            Student currentStudent = s.PopulateStudentData(dataAccess, currentStudentID);

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

            PopulateArtsAndRanks(currentStudent);
        }

        private void PopulateArtsAndRanks(Student currentStudent)
        {
            lvwArtsAndRanks.Items.Clear();

            foreach (StudentArtsAndRank artRank in currentStudent.StudentArtsAndRanks)
            {
                string[] subitems = { "Art", "Rank", "Start Date", "Promotion Date", "Promotion Hours" };
                ListViewItem item = new ListViewItem(subitems);
                item.SubItems[0].Text = artRank.Art;
                item.SubItems[1].Text = artRank.Rank;
                item.SubItems[2].Text = artRank.DateStarted.HasValue ?
                    artRank.DateStarted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[3].Text = artRank.DatePromoted.HasValue ? 
                    artRank.DatePromoted.Value.ToString("MM/dd/yyyy") : string.Empty;
                item.SubItems[4].Text = artRank.PromotionHours.ToString();

                lvwArtsAndRanks.Items.Add(item);
            }

        }

        private void StudentMaintenance_Load(object sender, EventArgs e)
        {
            DataTable studentList = dataAccess.GetStudentTable();

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

        private void btnPromotionHistory_Click(object sender, EventArgs e)
        {
            StudentPromotionHistoryUI promotions = new StudentPromotionHistoryUI(dataAccess, currentStudentID);
            promotions.ShowDialog();

        }

        private void dgvStudentList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudentList.SelectedRows.Count == 0)
                return;

            currentStudentID = Convert.ToInt32(dgvStudentList.SelectedRows[0].Cells[0].Value.ToString());
            PopulateStudentInformation();
        }
    }
}
