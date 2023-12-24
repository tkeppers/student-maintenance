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

        public StudentAddModifyArtUI(int studentID, string studentName)
        {
            modifyExistingArt = false;
            currentArt = new StudentArtsAndRank();
       
            InitializeComponent();

            btnAddStudent.Visible = true;
            btnPromoteStudent.Visible = false;
            btnSave.Visible = false;

            //TODO: Default rank to "White Belt" for new art. Should we let them override it?

            PopulateComboBox();
            this.Text = "Add New Art for " + studentName;
        }

        public StudentAddModifyArtUI(int studentID, string studentName, StudentArtsAndRank artInfo)
        {
            modifyExistingArt = true;
            currentArt = artInfo;

            InitializeComponent();

            btnAddStudent.Visible = false;
            btnPromoteStudent.Visible = true;
            btnSave.Visible = true;
            PopulateComboBox();
            PopulateFormForModify();

            this.Text = "Modifying " + artInfo.StudentArt + " for " + studentName;
        }

        private void PopulateComboBox()
        {
            //TODO: Remove hard-coded testing items and populate this from the database.
            cmbArtType.Items.Add("Aikido");
            cmbArtType.Items.Add("Judo");
            cmbArtType.Items.Add("Jyodo");
            cmbArtType.Items.Add("Iaido");
        }

        private void PopulateFormForModify()
        {
            txtCumulativeHours.Text = currentArt.HoursInArt.ToString();
            txtPromotionHours.Text = currentArt.PromotionHours.ToString();
            txtCurrentRank.Text = currentArt.Rank;
            dtBeginDate.Value = currentArt.DateStarted.Value;
            dtLastSignin.Value = currentArt.DateOfLatestSignIn.Value;
            dtPromotionDate.Value = currentArt.DatePromoted.Value;
            cmbArtType.Text = currentArt.StudentArt;
        }
    }
}
