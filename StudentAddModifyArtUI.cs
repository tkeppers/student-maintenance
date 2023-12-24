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
        StudentArtsAndRank currentArt;
        string studentName;

        //TODO: We can combine this into a single constructor
        public StudentAddModifyArtUI(int studentID, string studentName)
        {
            modifyExistingArt = false;

            currentArt = new StudentArtsAndRank();
            currentArt.StudentArtID = studentID;

            this.studentName = studentName;
       
            InitializeComponent();

            PopulateFormForAdd();
            GeneralFormSetup();
        }

        public StudentAddModifyArtUI(string studentName, StudentArtsAndRank artInfo)
        {
            modifyExistingArt = true;
            currentArt = artInfo;
            this.studentName = studentName;

            InitializeComponent();

            PopulateFormForModify();
            GeneralFormSetup();
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
            //TODO: Remove hard-coded testing items and populate this from the database.
            cmbArtType.Items.Add("Aikido");
            cmbArtType.Items.Add("Judo");
            cmbArtType.Items.Add("Jyodo");
            cmbArtType.Items.Add("Iaido");
        }

        private void PopulateFormForModify()
        {
            this.Text = $"Modifying {currentArt.StudentArt} for " + studentName;

            txtCumulativeHours.Text = currentArt.HoursInArt.ToString();
            txtPromotionHours.Text = currentArt.PromotionHours.ToString();
            txtCurrentRank.Text = currentArt.Rank;
            dtBeginDate.Value = currentArt.DateStarted.Value;
            dtLastSignin.Value = currentArt.DateOfLatestSignIn.Value;
            dtPromotionDate.Value = currentArt.DatePromoted.Value;
            cmbArtType.Text = currentArt.StudentArt;
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
            //TODO: Do we want to handle promotion date/promotion hours, even though they are read-only?
        }

        private bool SaveUpdatesToArt()
        {
            UpdateCurrentArtWithFormData();
            //Save data to database
            return false;
        }

        private bool AddNewArt()
        {
            UpdateCurrentArtWithFormData();
            //Save data to database
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (modifyExistingArt)
                SaveUpdatesToArt();
            else
                AddNewArt();
        }

        private void StudentAddModifyArtUI_Load(object sender, EventArgs e)
        {

        }
    }
}
