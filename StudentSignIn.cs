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
    public partial class StudentSignIn : Form
    {
        IDataAccess dataAccess;

        public StudentSignIn(IDataAccess dataAccess)
        {
            InitializeComponent();
            this.dataAccess = dataAccess;

            SetupStudentArtsList();
            SetupStudentList();
        }

        private void SetupStudentArtsList()
        {
            DataTable studentArtsTable = dataAccess.GetListOfArts();

            listboxSelectArt.DataSource = studentArtsTable;
            listboxSelectArt.DisplayMember = "art_id";
        }

        private void SetupStudentList()
        {
            DataTable studentTable = dataAccess.GetStudentTable();
            studentTable = studentTable.Select("stud_status = 'A' AND stud_club = 'Windsong'").CopyToDataTable();
            dgvStudentList.DataSource = studentTable;

            //Only display the most basic information (last name, first name, and student ID)
            foreach (DataGridViewColumn column in dgvStudentList.Columns)
            {
                column.Visible = false;
            }

            dgvStudentList.Columns["stud_firstname"].Visible = true;
            dgvStudentList.Columns["stud_lastname"].Visible = true;
            dgvStudentList.Columns["stud_id"].Visible = true;

        }

    }
}
