using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public partial class DatabaseConfigUI : Form
    {
        string selectedDatabasePath;

        public DatabaseConfigUI()
        {
            InitializeComponent();
            txtDatabaseFilePath.Text = ConfigurationManager.AppSettings["DatabasePath"];
        }

        private void UpdateAppConfig(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Access Database Files (*.accdb;*.mdb)|*.accdb;*.mdb|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Database File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedDatabasePath = openFileDialog.FileName;
                    txtDatabaseFilePath.Text = selectedDatabasePath;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateAppConfig("DatabasePath", selectedDatabasePath);
            MessageBox.Show($"Database path updated to: {selectedDatabasePath}.\nPlease restart software for changes to take effect.");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DatabaseConfigUI_Load(object sender, EventArgs e)
        {

        }
    }
}
