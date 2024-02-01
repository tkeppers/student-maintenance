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
        IDataRepository dataRepository;
        PromotionCriteria selectedPromotionCriteria;

        public PromotionCriteriaUI(IDataRepository dataRepository)
        {
            InitializeComponent();

            this.dataRepository = dataRepository;
            selectedPromotionCriteria = new PromotionCriteria();

            LoadPromotionCriteriaData(dataRepository);
            SetUpFilterByArt(dataRepository);
        }

        private void LoadPromotionCriteriaData(IDataRepository dataRepository)
        {
            promotionRequirementsTable = dataRepository.GetStudentPromotionRequirements();
            promotionRequirementsTable.DefaultView.Sort = "Art ASC, YearsInArt ASC";

            dgvPromotionSettings.DataSource = promotionRequirementsTable.DefaultView;
        }

        private void SetUpFilterByArt(IDataRepository dataRepository)
        {
            //Arts table is just used for filtering via the on-screen combo box. Loading 
            //just the values (instead of using data binding) so we can also add
            //an option for "All"
            artsTable = dataRepository.GetListOfArts();

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
                //Display the value in the "Art" column of the selected row
                selectedPromotionCriteria.CurrentArt = selectedRow.Cells["Art"].Value.ToString();
                selectedPromotionCriteria.CurrentRank = selectedRow.Cells["CurrentRank"].Value.ToString();
                selectedPromotionCriteria.NextRank = selectedRow.Cells["NextRank"].Value.ToString();
                selectedPromotionCriteria.MinimumTrainingHours = double.TryParse(selectedRow.Cells["MinimumTrainingHours"].Value.ToString(),
                    out double minHours) ? minHours : 0;
                selectedPromotionCriteria.MinimumAge = double.TryParse(selectedRow.Cells["MinimumAge"].Value.ToString(),
                    out double minAge) ? minAge : 0;
                selectedPromotionCriteria.YearsInArt = double.TryParse(selectedRow.Cells["YearsInArt"].Value.ToString(),
                    out double yearsInArt) ? yearsInArt : 0;
                selectedPromotionCriteria.YearsAtCurrentRank = double.TryParse(selectedRow.Cells["YearsAtCurrentRank"].Value.ToString(),
                    out double yearsAtCurrentRank) ? yearsAtCurrentRank : 0;
            }
        }

        // Comment out DeletePromotionCriteria()
        /*
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
        */

       /* private bool ConfirmDeletion()
        {
            return MessageService.ShowAreYouSureMessage($"Delete promotion criteria for {selectedPromotionCriteria.CurrentArt} - {selectedPromotionCriteria.CurrentRank}?")
                == DialogResult.Yes;
        }*/

        /*private void AddModifyPromotionCriteria(bool addNew)
        {
            if (addNew == false && IsValidPromotionCriteriaRowSelected() == false)
                return;

            //Open a new form dialog (blank for add, pre-filled for modify) to allow the user 
            //to make changes as needed.
        }*/

        /*private bool IsValidPromotionCriteriaRowSelected()
        {
            if (dgvPromotionSettings.SelectedRows.Count == 0)
            {
                MessageService.ShowInformationMessage("Please select a valid row.", "Nothing Selected");
                return false;
            }

            return true;
        }*/
        private bool IsNumericColumn(string columnName)
        {
            return columnName == "MinimumAge" || columnName == "MinimumTrainingHours" ||
                   columnName == "YearsInArt" || columnName == "YearsAtCurrentRank";
        }

        private bool IsValidNumber(string value)
        {
            return float.TryParse(value, out _);
        }

        private bool IsValidString(string columnName)
        {
            return columnName == "Art" || columnName == "CurrentRank" || columnName == "NextRank";
        }

        private bool IsValidArt(string value)
        {
            return artsTable.Select($"art_id = '{value}'").Any();
        }

        private void CancelValidation(DataGridViewCellValidatingEventArgs e, string errorMessage)
        {
            e.Cancel = true;
            dgvPromotionSettings.Rows[e.RowIndex].ErrorText = errorMessage;
        }


        private void PromotionSettingsUI_Load(object sender, EventArgs e)
        {

        }

        private void cmbFilterByArt_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDataGridView();
        }

        private void dgvPromotionSettings_SelectionChanged(object sender, EventArgs e)
        {
            LoadInformationForSelectedPromotionCriteria();
        }

        private void dgvPromotionSettings_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string columnName = dgvPromotionSettings.Columns[e.ColumnIndex].Name;
            string columnValue = e.FormattedValue.ToString();

            // Validate numeric columns
            if (IsNumericColumn(columnName) && !IsValidNumber(columnValue))
            {
                CancelValidation(e, $"Column {columnName} must be a valid number");
            }
            // Validate non-blank string columns
            else if (IsValidString(columnName) && string.IsNullOrWhiteSpace(columnValue))
            {
                CancelValidation(e, $"Column {columnName} cannot be blank");
            }
            // Additional validation for 'Art' column
            else if (columnName == "Art" && !IsValidArt(columnValue))
            {
                CancelValidation(e, $"Column {columnName} must be a valid art");
            }
        }

        private void dgvPromotionSettings_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            dgvPromotionSettings.Rows[e.RowIndex].ErrorText = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataRepository.UpdatePromotionCriteria(promotionRequirementsTable);
        }
    }
}
