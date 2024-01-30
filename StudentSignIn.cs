using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public partial class StudentSignIn : Form
    {
        IDataAccess dataAccess;

        public StudentSignIn(IDataAccess dataAccess)
        {
            InitializeComponent();
            this.dataAccess = dataAccess;

            //Subscribe to the DrawItem event for the listbox so we can change the color of certain messages
            listboxSignInList.DrawMode = DrawMode.OwnerDrawFixed;
            listboxSignInList.DrawItem += new DrawItemEventHandler(listboxSignInList_DrawItem);

            SetupListAOfAvailiableArts();
            RefreshStudentList();
        }

        private void SetupListAOfAvailiableArts()
        {
            DataTable studentArtsTable = dataAccess.GetListOfArts();

            listboxSelectArt.DataSource = studentArtsTable;
            listboxSelectArt.DisplayMember = "art_id";
        }

        //TODO: Move RefreshStudentList to a class where it can be accessed by both StudentSignIn and StudentMaintenance
        private void RefreshStudentList()
        {
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

        private void FilterStudentList()
        {
            string filterExpression = $"stud_id is not null and stud_status = 'A' and stud_club = 'Windsong'";  

            if (!string.IsNullOrWhiteSpace(txtFirstNameFilter.Text))
                filterExpression += " and stud_firstname LIKE '" + txtFirstNameFilter.Text + "%'";

            if (!string.IsNullOrWhiteSpace(txtLastNameFilter.Text))
                filterExpression += " and stud_lastname LIKE '" + txtLastNameFilter.Text + "%'";

            DataView dv = (DataView)dgvStudentList.DataSource;
            dv.RowFilter = filterExpression;

            dgvStudentList.DataSource = dv;
        }

        private void ProcessStudentSignIn(int selectedIndex)
        {   
            if (!ValidArtIsSelected())
                return;

            int studentID = Convert.ToInt32(dgvStudentList.Rows[selectedIndex].Cells["StudentID"].Value);
            string studentName = $"{dgvStudentList.Rows[selectedIndex].Cells["FirstName"].Value}  {dgvStudentList.Rows[selectedIndex].Cells["LastName"].Value}";
            string signInArt = listboxSelectArt.Text;
            string signInMessage = string.Empty; 

            StudentSignInFunctions signInFunctions = new StudentSignInFunctions();
            bool success = signInFunctions.SignInStudent(dataAccess, studentID, listboxSelectArt.Text, out signInMessage);


            if (success)
            {
                listboxSignInList.Items.Add($"{studentName} [{studentID}] signed in for {signInArt} on {DateTime.Now}");

                bool.TryParse(ConfigurationManager.AppSettings["DatabasePassword"], out bool checkPromotionEligibility);
                if (checkPromotionEligibility)
                {
                    string promotionMessage;
                    if (signInFunctions.IsEligibleForPromotion(dataAccess, studentID, signInArt, out promotionMessage))
                        listboxSignInList.Items.Add(promotionMessage);
                }
            }
            else
            {
                listboxSignInList.Items.Add($"Error signing in {studentName} [{studentID}] for {signInArt} on {DateTime.Now}");
            }

            if (signInMessage != string.Empty)
                listboxSignInList.Items.Add($"    {signInMessage}");
        }

        private bool ValidArtIsSelected()
        {
            if (listboxSelectArt.SelectedItem == null)
            {
                MessageService.ShowErrorMessage("Please select a valid art", "Art Not Selected");
                return false;
            }

            return true;
        }

        private void txtFirstNameFilter_TextChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void txtLastNameFilter_TextChanged(object sender, EventArgs e)
        {
            FilterStudentList();
        }

        private void dgvStudentList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selectedIndex = e.RowIndex;

            if (selectedIndex < 0)
                return;

            ProcessStudentSignIn(selectedIndex);
        }

        private void listboxSignInList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return; // Avoid drawing when the list is empty

            string text = listboxSignInList.Items[e.Index].ToString();

            Brush textBrush = Brushes.Black;

            // Change the color to red for error messages
            if (text.StartsWith("Error"))
            {
                textBrush = Brushes.Red;
            }

            // Change the color to green for promotion messages
            else if (text.Contains("promotion"))
            {
                textBrush = Brushes.Green;
            }

            e.DrawBackground();
            e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds);
            e.DrawFocusRectangle();
        }
    }
}
