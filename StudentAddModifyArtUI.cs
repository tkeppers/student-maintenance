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

        public StudentAddModifyArtUI(int studentID, string studentName)
        {
            modifyExistingArt = false;
            this.Text = "Add New Art for " + studentName;
            InitializeComponent();
        }

        public StudentAddModifyArtUI(int studentID, string studentName, int artID, string artName)
        {
            modifyExistingArt = true;
            this.Text = "Modifying " + artName + " for " + studentName;
            InitializeComponent();
        }

        private void StudentAddModifyArtUI_Load(object sender, EventArgs e)
        {

        }
    }
}
