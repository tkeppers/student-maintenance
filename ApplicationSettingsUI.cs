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
    public partial class ApplicationSettingsUI : Form
    {
        string selectedDatabasePath;
        bool showPromotionEligibility;

        public ApplicationSettingsUI()
        {
            InitializeComponent();
            txtDatabaseFilePath.Text = ConfigurationManager.AppSettings["DatabasePath"];
            showPromotionEligibility = bool.Parse(ConfigurationManager.AppSettings["ShowPromotionEligibilityOnSignIn"]);
            cbShowPromotionEligibility.Checked = showPromotionEligibility;
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

        private void cbShowPromotionEligibility_CheckedChanged(object sender, EventArgs e)
        {
            showPromotionEligibility = cbShowPromotionEligibility.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateAppConfig("DatabasePath", selectedDatabasePath);
            UpdateAppConfig("ShowPromotionEligibilityOnSignIn", showPromotionEligibility.ToString());
            Serilog.Log.Information($"DatabaseConfigUI: Settings updated.\n Database path = {selectedDatabasePath}\n " +
                $"Show promotion eligibility on sign-in = {showPromotionEligibility}. ");
            MessageService.ShowInformationMessage("Settings updated.\nPlease restart software for changes to take effect.");
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
