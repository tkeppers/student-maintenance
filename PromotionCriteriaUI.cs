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
    public partial class PromotionCriteriaUI : Form
    {
        DataTable promotionRequirementsTable;
        DataTable artsTable;
        IDataAccess dataAccess;

        public PromotionCriteriaUI(IDataAccess dataAccess)
        {
            InitializeComponent();

            this.dataAccess = dataAccess;

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

        private void DeletePromotionCriteria()
        {

        }

        private void AddModifyPromotionCriteria(bool addNew)
        {

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
            AddModifyPromotionCriteria(true);
        }

        private void btnModifyExisting_Click(object sender, EventArgs e)
        {
            AddModifyPromotionCriteria(false);
        }
    }
}
