using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// Administrative correction of a recorded rank, used while re-verifying a member dojo's
    /// roster. Deliberately distinct from the promotion dialog: this fixes what the database
    /// says a student holds, and writes no promotion history.
    /// </summary>
    public partial class RankCorrectionUI : Form
    {
        private readonly string currentRank;

        /// <summary>The rank chosen by the reviewer, valid once the dialog returns OK.</summary>
        public string CorrectedRank { get; private set; }

        public RankCorrectionUI(string studentName, string artName, string currentRank, IEnumerable<Rank> rankLadder)
        {
            InitializeComponent();

            this.currentRank = currentRank;

            lblStudent.Text = studentName;
            lblArt.Text = artName;
            lblCurrentRank.Text = string.IsNullOrWhiteSpace(currentRank) ? "(none recorded)" : currentRank;

            foreach (Rank rank in rankLadder)
                cmbNewRank.Items.Add(rank.RankID);

            // Start on the recorded rank so the reviewer changes it deliberately.
            int index = cmbNewRank.Items.IndexOf(currentRank);

            if (index >= 0)
                cmbNewRank.SelectedIndex = index;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newRank = cmbNewRank.SelectedItem?.ToString();

            if (!KubkManagementFunctions.ValidateRankCorrection(currentRank, newRank, out string error))
            {
                MessageService.ShowErrorMessage(error, "Rank Not Corrected");
                return;
            }

            CorrectedRank = newRank;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
