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
        IDataRepository dataRepository;

        public PromoteStudentUI()
        {
            InitializeComponent();
        }

        public PromoteStudentUI(Student student, StudentArtsAndRank art, DataTable promotionRequirements, IDataRepository dataRepository)
        {
            InitializeComponent();

            currentStudent = student;
            currentArt = art;
            this.dataRepository = dataRepository;

            LoadListOfRanks(promotionRequirements, art.StudentArt);
            PopulatePromotionData(student, art);
        }

        private void LoadListOfRanks(DataTable promotionRequirements, string art)
        {
            // Filter and order the data
            var filteredRows = promotionRequirements.AsEnumerable()
                .Where(row => row.Field<string>("Art") == art)
                .OrderBy(row => row.Field<int>("MinimumTrainingHours"))
                .CopyToDataTable();

            cmbNextRank.DisplayMember = "NextRank";
            cmbNextRank.ValueMember = "NextRank"; 
            cmbNextRank.DataSource = filteredRows;

            //Set the default value to the next rank for this student
            PromotionCriteria promotionCriteria = new PromotionCriteria(promotionRequirements);
            cmbNextRank.Text = promotionCriteria.GetNextPromotionCriteria(currentArt).NextRank;
        }

        private void PopulatePromotionData(Student student, StudentArtsAndRank art)
        {
            txtStudentName.Text = student.FullName;
            
            txtArt.Text = art.StudentArt;
            chkEligibleForPromotion.Checked = art.EligibleForPromotion;
            
            txtCurrentRank.Text = art.Rank;
            cmbNextRank.Text = art.NextRank;

            dtPromotionDate.Value = DateTime.Now;
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

            string message = $"Promote {currentStudent.FullName} from {currentArt.Rank} to {cmbNextRank.Text} in {currentArt.StudentArt}?";

            DialogResult result = MessageBox.Show(message, "Promote Student?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return (result == DialogResult.Yes);
        }

        private void UpdateStudentPromotion()
        {
            if (dataRepository.UpdateStudentPromotion(currentStudent.StudentID, currentArt))
            {
                currentArt.PromoteStudentToNewLevel(cmbNextRank.Text);
                MessageBox.Show($"Student {currentStudent.FullName} successfully promoted to {currentArt.Rank}",
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
