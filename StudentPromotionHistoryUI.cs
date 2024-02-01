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
    public partial class StudentPromotionHistoryUI : Form
    {
        IDataRepository dataRepository;
        int studentID;

        public StudentPromotionHistoryUI(IDataRepository dataRepository, int studentID, string studentName)
        {
            this.studentID = studentID;
            this.dataRepository = dataRepository;

            this.Text = "Promotion History for " + studentName;

            InitializeComponent();
        }

        private void PopulateStudentPromotionHistory()
        {
            StudentPromotionHistoryFunctions sphf = new StudentPromotionHistoryFunctions(dataRepository, studentID);
            DataTable studentPromotionHistory = sphf.GetStudentPromotionHistory();
            dgvPromotionHistory.AutoGenerateColumns = false;

            dgvPromotionHistory.Columns["Art"].DataPropertyName = "promo_art";
            dgvPromotionHistory.Columns["Rank"].DataPropertyName = "promo_rank";
            dgvPromotionHistory.Columns["PromoDate"].DataPropertyName = "promo_date";
            dgvPromotionHistory.Columns["PromoHours"].DataPropertyName = "promo_hours";

            dgvPromotionHistory.DataSource = studentPromotionHistory;

            dgvPromotionHistory.Sort(dgvPromotionHistory.Columns["PromoHours"], ListSortDirection.Ascending);
            dgvPromotionHistory.Sort(dgvPromotionHistory.Columns["Art"], ListSortDirection.Ascending);
        }

        private void StudentPromotionHistoryUI_Load(object sender, EventArgs e)
        {
            PopulateStudentPromotionHistory();
        }
    }
}
