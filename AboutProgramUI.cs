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
    public partial class AboutProgramUI : Form
    {
        public AboutProgramUI()
        {
            InitializeComponent();
        }

        private void AboutProgramUI_Load(object sender, EventArgs e)
        {
            SetVersionNumber();
            SetCopyrightInfo();
        }

        private void SetVersionNumber()
        {
            // Get the assembly version
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string versionString = $"Version: {version.Major}.{version.Minor}.{version.Build}";

            lblVersion.Text = versionString;
        }

        private void SetCopyrightInfo()
        {
            lblCopyright.Text = $"\u00A9 Copyright 2023 - {DateTime.Now.Year}, 12 Winds, LLC (Windsong Dojo)";
        }
    }
}
