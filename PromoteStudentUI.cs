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

        public PromoteStudentUI()
        {
            InitializeComponent();
        }

        //TODO: Do we really need to depend on 3 other classes? or can we decouple this a bit?
        public PromoteStudentUI(Student student, StudentArtsAndRank art, DataTable promotionRequirements)
        {
            InitializeComponent();

            currentStudent = student;
            currentArt = art;

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
            cmbNextRank.SelectedItem = art.NextRank;

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

        private bool ConfirmStudentPromotion()
        {
            bool confirmPromotion = false;

            if (cmbNextRank.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid rank for promotion.", "Promotion Rank Not Set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            string message = "Promote " + currentStudent.FirstName + " " + currentStudent.LastName + " from " +
                currentArt.Rank + " to " + cmbNextRank.Text + " in " + currentArt.StudentArt + "?";
            MessageBox.Show(message, "Promote Student?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return confirmPromotion;
        }

        private void btnPromoteStudent_Click(object sender, EventArgs e)
        {
            ConfirmStudentPromotion();
        }
    }
}
