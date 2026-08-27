using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    /// <summary>
    /// Windsong student activity: where each student stands in each art, when they last trained,
    /// and whether they have met the promotion criteria. Ordered by most recent sign-in so the
    /// students currently training are at the top.
    /// </summary>
    public partial class StudentActivityReportUI : Form
    {
        /// <summary>This report covers the home dojo; member dojos have their own roster screen.</summary>
        private const string ReportClubId = "Windsong";

        private static readonly Color EligibleRowColor = Color.FromArgb(212, 237, 218);

        /// <summary>
        /// One shared style instance reused by every eligible row, rather than a fresh
        /// DataGridViewCellStyle per row across a couple of thousand of them.
        /// </summary>
        private readonly DataGridViewCellStyle eligibleRowStyle =
            new DataGridViewCellStyle { BackColor = EligibleRowColor };

        private readonly StudentActivityReportFunctions reportFunctions;

        private List<StudentActivityEntry> reportRows = new List<StudentActivityEntry>();

        public StudentActivityReportUI(IDataRepository dataRepository)
        {
            InitializeComponent();

            reportFunctions = new StudentActivityReportFunctions(dataRepository);
        }

        private void StudentActivityReportUI_Load(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void ReportCriteria_Changed(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void RefreshReport()
        {
            reportRows = reportFunctions.GetStudentActivity(ReportClubId, cbActiveOnly.Checked);

            if (cbEligibleOnly.Checked)
                reportRows = reportRows.Where(r => r.IsEligibleForPromotion).ToList();

            PopulateGrid();

            int eligible = reportRows.Count(r => r.IsEligibleForPromotion);
            lblSummary.Text = $"{reportRows.Count} student/art rows;  {eligible} eligible for promotion";
        }

        private void PopulateGrid()
        {
            // Keep whatever ordering the user has clicked into, rather than silently reverting
            // to the default sort while the header still shows their sort glyph.
            DataGridViewColumn sortedColumn = dgvActivity.SortedColumn;
            SortOrder sortOrder = dgvActivity.SortOrder;

            dgvActivity.SuspendLayout();

            try
            {
                dgvActivity.Rows.Clear();

                if (reportRows.Count == 0)
                    return;

                var rows = new DataGridViewRow[reportRows.Count];

                for (int i = 0; i < reportRows.Count; i++)
                {
                    StudentActivityEntry entry = reportRows[i];

                    var row = new DataGridViewRow();
                    row.CreateCells(dgvActivity,
                        entry.FullName,
                        entry.Art,
                        entry.Rank,
                        entry.LastSignInDisplay,
                        entry.HoursInArt.ToString("0.#"),
                        entry.EligibilityDisplay);

                    // Highlight the students who have met every promotion gate, so a long list
                    // can be scanned for who is due rather than read row by row.
                    if (entry.IsEligibleForPromotion)
                        row.DefaultCellStyle = eligibleRowStyle;

                    row.Tag = entry;
                    rows[i] = row;
                }

                dgvActivity.Rows.AddRange(rows);
            }
            finally
            {
                dgvActivity.ResumeLayout();
            }

            if (sortedColumn != null && sortOrder != SortOrder.None && dgvActivity.Rows.Count > 0)
            {
                dgvActivity.Sort(sortedColumn,
                    sortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
            }

            dgvActivity.ClearSelection();
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (reportRows.Count == 0)
            {
                MessageService.ShowInformationMessage("There is nothing to export.", "No Rows");
                return;
            }

            var headers = new[] { "Student Name", "Art", "Rank", "Last Sign-In", "Hours", "Eligible for Promotion" };

            IEnumerable<IEnumerable<string>> rows = reportRows.Select(entry => new[]
            {
                entry.FullName,
                entry.Art,
                entry.Rank,
                entry.LastSignInDate.HasValue ? entry.LastSignInDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                entry.HoursInArt.ToString("0.#"),
                entry.EligibilityDisplay
            });

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.AddExtension = true;
                saveDialog.FileName = $"Windsong-Student-Activity-{DateTime.Today:yyyy-MM-dd}.csv";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    CsvWriter.WriteToFile(saveDialog.FileName, CsvWriter.BuildCsv(headers, rows));
                    Log.Information($"Exported {reportRows.Count} activity rows to {saveDialog.FileName}");
                    MessageService.ShowInformationMessage(
                        $"Exported {reportRows.Count} rows to{Environment.NewLine}{saveDialog.FileName}", "Export Complete");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error exporting student activity to {saveDialog.FileName}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    MessageService.ShowErrorMessage($"Error writing the file:{Environment.NewLine}{ex.Message}", "Export Failed");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
