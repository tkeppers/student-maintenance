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
    public partial class StudentAddModifyArtUI : Form
    {
        public bool modifyExistingArt;
        readonly StudentArtsAndRank currentArt;
        readonly string studentName;
        readonly IDataRepository dataRepository;

        public StudentAddModifyArtUI(int studentID, string studentName, IDataRepository dataRepository, StudentArtsAndRank artInfo = null)
        {
            this.studentName = studentName;
            this.dataRepository = dataRepository;

            InitializeComponent();
            GeneralFormSetup();

            if (artInfo != null)
            {
                modifyExistingArt = true;
                currentArt = artInfo;
                PopulateFormForModify();
            }
            else
            {
                modifyExistingArt = false;
                currentArt = new StudentArtsAndRank { StudentArtID = studentID };
                PopulateFormForAdd();
            }
        }

        private void GeneralFormSetup()
        {
            PopulateListOfArts();

            toolTip.SetToolTip(lblPromotionHours, "Training hours at student's last promotion");
            toolTip.SetToolTip(txtPromotionHours, "Training hours at student's last promotion");
            toolTip.SetToolTip(lblCumulativeHours, $"Total training hours since beginning {currentArt}");
            toolTip.SetToolTip(txtCumulativeHours, $"Total training hours since beginning {currentArt}");
            toolTip.SetToolTip(lblLastPromotionDate, "To change promotion date or hours, please click Promote Student option on main screen.");
            toolTip.SetToolTip(dtPromotionDate, "To change promotion date or hours, please click Promote Student option on main screen.");
        }

        private void PopulateListOfArts()
        {
            cmbArtType.DataSource = dataRepository.GetListOfArts();
            cmbArtType.DisplayMember = "art_id"; 
            cmbArtType.ValueMember = "art_id";
        }

        private void PopulateFormForModify()
        {
            this.Text = $"Modifying {currentArt.StudentArt} for " + studentName;

            txtCumulativeHours.Text = currentArt.HoursInArt.ToString();
            txtPromotionHours.Text = currentArt.PromotionHours.ToString();
            txtCurrentRank.Text = currentArt.Rank;
            dtBeginDate.Value = currentArt.DateStarted.Value;
            dtLastSignin.Value = currentArt.DateOfLatestSignIn ?? DateTime.Today;
            dtPromotionDate.Value = currentArt.DatePromoted.Value;
            cmbArtType.Text = currentArt.StudentArt;

            cmbArtType.Enabled = false;
        }

        private void PopulateFormForAdd()
        {
            this.Text = "Add New Art for " + studentName;

            txtCumulativeHours.Text = "0";
            txtPromotionHours.Text = "0";
            txtCurrentRank.Text = "WHITE";
            dtBeginDate.Value = DateTime.Today;
            dtLastSignin.Value = DateTime.Today;
            dtPromotionDate.Value = DateTime.Today;
            cmbArtType.Text = currentArt.StudentArt;
        }

        private void UpdateCurrentArtWithFormData()
        {
            currentArt.Rank = txtCurrentRank.Text.Trim();
            currentArt.StudentArt = cmbArtType.Text.Trim();
            currentArt.HoursInArt = double.TryParse(txtCumulativeHours.Text, out double hours) ? hours : 0.0;
            currentArt.DateStarted = dtBeginDate.Value;
            currentArt.PromotionHours = double.TryParse(txtPromotionHours.Text, out double promotionHours) ? promotionHours : 0.0;
            currentArt.DatePromoted = dtPromotionDate.Value;
        }

        private void UpdateStudentArtRecord()
        {
            string message;

            UpdateCurrentArtWithFormData();
            if (dataRepository.UpdateStudentArt(currentArt))
            {
                message = $"Updated {currentArt.StudentArt} data for {studentName}";
                MessageBox.Show(message, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Information($"UpdateStudentArtRecord: {message}");
                return;
            }

            message = $"Error updating {currentArt.StudentArt} data for {studentName}";
            MessageBox.Show(message, "Error Updating Art", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error($"UpdateStudentArtRecord: {message}");
        }

        private void CreateNewStudentArtRecord()
        {
            UpdateCurrentArtWithFormData();
            if (dataRepository.AddNewStudentArt(currentArt))
            {
                MessageBox.Show($"Added {currentArt.StudentArt} as a new art for {studentName}", "Add New Art Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show($"Error adding {currentArt.StudentArt} for {studentName}", "Error AddingInformation", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSave_Click(object sender, EventArgs e)
        { 
            if (modifyExistingArt)
                UpdateStudentArtRecord();
            else
                CreateNewStudentArtRecord();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Dispose();
            Close();
        }
    }
}
