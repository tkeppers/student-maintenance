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

        public StudentAddModifyArtUI(int studentID, string studentName)
        {
            modifyExistingArt = false;
            currentArt = new StudentArtsAndRank();
            this.studentName = studentName;
       
            InitializeComponent();

            PopulateFormForAdd();
            GeneralFormSetup();
        }

        public StudentAddModifyArtUI(int studentID, string studentName, StudentArtsAndRank artInfo)
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
            //dtPromotionDate.Value = currentArt.DatePromoted.Value;
            cmbArtType.Text = currentArt.StudentArt;
        }

        private bool SaveUpdatesToArt()
        {
            return false;
        }

        private bool AddNewArt()
        {

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (modifyExistingArt)
                SaveUpdatesToArt();
            else
                AddNewArt();
        }
    }
}
