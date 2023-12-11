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
    public partial class PromoteStudentUI : Form
    {
        Student currentStudent;
        StudentArtsAndRank currentArt;
        IDataAccess dataAccess;

        public PromoteStudentUI()
        {
            InitializeComponent();
        }

        //TODO: Now that we are passing in IDataAccess, it might be better to re-read the promotion requirements instead of passing them
        public PromoteStudentUI(Student student, StudentArtsAndRank art, DataTable promotionRequirements, IDataAccess dataAccess)
        {
            InitializeComponent();

            currentStudent = student;
            currentArt = art;
            this.dataAccess = dataAccess;

            LoadListOfRanks(promotionRequirements, art.StudentArt);
            PopulatePromotionData(student, art);
        }

        private void LoadListOfRanks(DataTable promotionRequirements, string art)
        {
            string filter = $"Art = '{art}'";

            cmbNextRank.DisplayMember = "NextRank";
            cmbNextRank.ValueMember = "NextRank"; // Optional, set if you need to get the selected value
            cmbNextRank.DataSource = promotionRequirements.Select(filter).CopyToDataTable();
        }

        private void PopulatePromotionData(Student student, StudentArtsAndRank art)
        {
            txtStudentName.Text = student.FirstName + " " + student.LastName;
            
            txtArt.Text = art.StudentArt;
            chkEligibleForPromotion.Checked = art.EligibleForPromotion;
            
            txtCurrentRank.Text = art.Rank;
            cmbNextRank.Text = art.NextRank;

            dtPromotionDate.Value = DateTime.Now;
        }

        private void PromoteStudentUI_Load(object sender, EventArgs e)
        {
        }

        private void ValidatePromotion()
        {
            //TODO: Add logic to make sure there is nothing "weird" about this promotion. Possible things to 
            //consider:
            // - Student is being promoted multiple levels. Allow user to override, but give them a warning.
            // - Student is being demoted (promotion to a lower level than current rank). Allow them to override, because it could 
            //         just be a matter of correcting a mistake. 
            // - Student is being promoted to same level. Tell the user they're being silly and don't update anything
            // - Promoting a student in an art they are not currently taking. This might be an issue if we allow the user to select 
            //      a different student after the screen loads. Not sure if allowing that is a good idea.
            //
            // Move this to a business logic class, like PromotionCriteria or Student
        }

        private bool ValidateFormData()
        {
            string errorText = string.Empty;

            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
                errorText = "Invalid student name.\n";

            if (string.IsNullOrWhiteSpace(txtCurrentRank.Text))
                errorText = "Invalid value for current rank.\n";

            if (string.IsNullOrWhiteSpace(txtArt.Text))
                errorText = "Invalid value for student art.\n";

            if (cmbNextRank.SelectedItem == null)
                errorText = "Please select a valid rank for promotion.\n";

            if (string.IsNullOrWhiteSpace(errorText) == false)
            {
                MessageBox.Show(errorText, "Form Data Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private bool ConfirmStudentPromotion()
        {
            if (!ValidateFormData())
                return false;

            currentArt.NextRank = cmbNextRank.Text;
            currentArt.DatePromoted = dtPromotionDate.Value;

            string message = $"Promote {currentStudent.FirstName} {currentStudent.LastName} from {currentArt.Rank} to {cmbNextRank.Text} in {currentArt.StudentArt}?";

            DialogResult result = MessageBox.Show(message, "Promote Student?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return (result == DialogResult.Yes);
        }

        private void UpdateStudentPromotion()
        {
            if (dataAccess.UpdateStudentPromotion(currentStudent.StudentID, currentArt))
            {
                currentArt.PromoteStudentToNewLevel(cmbNextRank.Text);
                MessageBox.Show($"Student {currentStudent.FirstName} {currentStudent.LastName} successfully promoted to {currentArt.Rank}",
                    "Promotion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error updating database with student promotion.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPromoteStudent_Click(object sender, EventArgs e)
        {
            if (ConfirmStudentPromotion())
                UpdateStudentPromotion();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
