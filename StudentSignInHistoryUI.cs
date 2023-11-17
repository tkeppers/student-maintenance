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
    public partial class StudentSignInHistoryUI : Form
    {
        IDataAccess dataAccess;
        string studentName;
        int studentID;

        public StudentSignInHistoryUI(IDataAccess dataAccess, int currentStudentID, string studentName)
        {
            InitializeComponent();

            this.dataAccess = dataAccess;
            this.studentID = currentStudentID;
            this.studentName = studentName;

            this.Text = "Sign-In History for " + this.studentName;
        }

        private void StudentSignInHistoryUI_Load(object sender, EventArgs e)
        {
            PopulateStudentSignInHistory();
        }

        private void PopulateStudentSignInHistory()
        {
            StudentSignInHistoryFunctions ssihf = new StudentSignInHistoryFunctions(dataAccess, studentID);
            DataTable studentSignInHistory = ssihf.GetStudentSignInHistory();

            dgvStudentSignInHistory.AutoGenerateColumns = false;

            dgvStudentSignInHistory.Columns["colSignInArt"].DataPropertyName = "sign_art";
            dgvStudentSignInHistory.Columns["colSignInDateTime"].DataPropertyName = "sign_date";
            dgvStudentSignInHistory.Columns["colSignInHours"].DataPropertyName = "sign_reg_hours";

            dgvStudentSignInHistory.DataSource = studentSignInHistory;

            dgvStudentSignInHistory.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvStudentSignInHistory.Rows[e.RowIndex];

                    // Set values for non-data-bound columns
                    if (e.ColumnIndex == dgvStudentSignInHistory.Columns["colStudentID"].Index)
                    {
                        e.Value = studentID.ToString();
                    }
                    else if (e.ColumnIndex == dgvStudentSignInHistory.Columns["colStudentName"].Index)
                    {
                        e.Value = studentName;
                    }
                }
            };

            dgvStudentSignInHistory.Sort(dgvStudentSignInHistory.Columns["colSignInDateTime"], ListSortDirection.Descending);
        }
    }
}
