using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace DojoStudentManagement
{
    public partial class PromotionCriteriaUI : Form
    {
        DataTable promotionRequirementsTable;
        DataTable artsTable;
        IDataAccess dataAccess;
        PromotionCriteria selectedPromotionCriteria;

        public PromotionCriteriaUI(IDataAccess dataAccess)
        {
            InitializeComponent();

            this.dataAccess = dataAccess;
            selectedPromotionCriteria = new PromotionCriteria();

            LoadPromotionCriteriaData(dataAccess);
            SetUpFilterByArt(dataAccess);
        }

        private void LoadPromotionCriteriaData(IDataAccess dataAccess)
        {
            promotionRequirementsTable = dataAccess.GetStudentPromotionRequirements();
            promotionRequirementsTable.DefaultView.Sort = "Art ASC, YearsInArt ASC";

            dgvPromotionSettings.DataSource = promotionRequirementsTable.DefaultView;
        }

        private void SetUpFilterByArt(IDataAccess dataAccess)
        {
            //Arts table is just used for filtering via the on-screen combo box. Loading 
            //just the values (instead of using data binding) so we can also add
            //an option for "All"
            artsTable = dataAccess.GetListOfArts();

            cmbFilterByArt.Items.Add("All");
            foreach (DataRow row in artsTable.Rows)
                cmbFilterByArt.Items.Add(row[0].ToString());

            cmbFilterByArt.SelectedItem = "All";
        }

        private void FilterDataGridView()
        {
            if (cmbFilterByArt.SelectedIndex <= 0) // 'All' is selected or no valid selection
            {
                promotionRequirementsTable.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                string filterValue = cmbFilterByArt.SelectedItem.ToString();
                promotionRequirementsTable.DefaultView.RowFilter = $"Art = '{filterValue}'";
            }
        }

        private void LoadInformationForSelectedPromotionCriteria()
        {
            if (dgvPromotionSettings.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvPromotionSettings.SelectedRows[0];
                selectedPromotionCriteria.CurrentArt = selectedRow.Cells["Art"].ToString();
                selectedPromotionCriteria.CurrentRank = selectedRow.Cells["CurrentRank"].ToString();
                selectedPromotionCriteria.NextRank = selectedRow.Cells["NextRank"].ToString();
                selectedPromotionCriteria.MinimumTrainingHours = double.TryParse(selectedRow.Cells["MinimumTrainingHours"].ToString(), 
                    out double minHours) ? minHours : 0;
                selectedPromotionCriteria.MinimumAge = double.TryParse(selectedRow.Cells["MinimumAge"].ToString(),
                    out double minAge) ? minAge : 0;
                selectedPromotionCriteria.YearsInArt = double.TryParse(selectedRow.Cells["YearsInArt"].ToString(),
                    out double yearsInArt) ? yearsInArt : 0;
                selectedPromotionCriteria.YearsAtCurrentRank = double.TryParse(selectedRow.Cells["YearsAtCurrentRank"].ToString(),
                    out double yearsAtCurrentRank) ? yearsAtCurrentRank : 0;
            }
        }

        private void DeletePromotionCriteria()
        {
            if (IsValidPromotionCriteriaRowSelected() == false)
                return;

            if (ConfirmDeletion() == false)
                return;

            bool deletionSuccess = dataAccess.DeletePromotionCriteria(
                selectedPromotionCriteria.CurrentArt,
                selectedPromotionCriteria.CurrentRank);

            if (deletionSuccess)
            {
                Log.Information($"Successfully deleted promotion criteria for {selectedPromotionCriteria.CurrentArt} - {selectedPromotionCriteria.CurrentRank}");
            }
            else
            {
                Log.Error($"Failed to delete promotion criteria for {selectedPromotionCriteria.CurrentArt} - {selectedPromotionCriteria.CurrentRank}");
            }
        }

        private bool ConfirmDeletion()
        {
            return MessageService.ShowAreYouSureMessage($"Delete promotion criteria for {selectedPromotionCriteria.CurrentArt} - {selectedPromotionCriteria.CurrentRank}?")
                == DialogResult.Yes;
        }

        private void AddModifyPromotionCriteria(bool addNew)
        {
            if (addNew == false && IsValidPromotionCriteriaRowSelected() == false)
                return;

            //Open a new form dialog (blank for add, pre-filled for modify) to allow the user 
            //to make changes as needed.
        }

        private bool IsValidPromotionCriteriaRowSelected()
        {
            if (dgvPromotionSettings.SelectedRows.Count == 0)
            {
                MessageService.ShowInformationMessage("Please select a valid row.", "Nothing Selected");
                return false;
            }

            return true;
        }

        private void PromotionSettingsUI_Load(object sender, EventArgs e)
        {

        }

        private void cmbFilterByArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeletePromotionCriteria();
        }

        private void btnModifyExisting_Click(object sender, EventArgs e)
        {
            AddModifyPromotionCriteria(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddModifyPromotionCriteria(true);
        }

        private void dgvPromotionSettings_SelectionChanged(object sender, EventArgs e)
        {
            LoadInformationForSelectedPromotionCriteria();
        }
    }
}
