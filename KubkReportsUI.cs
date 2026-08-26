using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public enum KubkReportKind
    {
        RankRegister,
        UnpaidDues
    }

    /// <summary>
    /// Hosts the two KUBK organization reports. One form serves both: the scope controls that do
    /// not apply to the chosen report are hidden, and the grid's columns are built per report, so
    /// adding a report later does not mean another near-identical form.
    /// </summary>
    public partial class KubkReportsUI : Form
    {
        private readonly KubkManagementFunctions kubkFunctions;
        private readonly KubkReportKind reportKind;

        private List<KubkRosterEntry> registerRows = new List<KubkRosterEntry>();
        private List<UnpaidDuesEntry> unpaidRows = new List<UnpaidDuesEntry>();

        private bool suppressCriteriaEvents;

        public KubkReportsUI(IDataRepository dataRepository, KubkReportKind reportKind)
        {
            InitializeComponent();

            kubkFunctions = new KubkManagementFunctions(dataRepository);
            this.reportKind = reportKind;

            Text = reportKind == KubkReportKind.RankRegister
                ? "KUBK Rank Register"
                : "KUBK Unpaid Dues";
        }

        private void KubkReportsUI_Load(object sender, EventArgs e)
        {
            suppressCriteriaEvents = true;

            ConfigureControlsForReport();
            numDuesYear.Value = DateTime.Today.Year;

            if (reportKind == KubkReportKind.RankRegister)
                PopulateDojoSelector();

            suppressCriteriaEvents = false;

            RefreshReport();
        }

        private void ConfigureControlsForReport()
        {
            bool isRegister = reportKind == KubkReportKind.RankRegister;

            lblDojoPrompt.Visible = isRegister;
            cmbDojo.Visible = isRegister;
            cbIncludeWindsong.Visible = isRegister;

            lblYearPrompt.Visible = !isRegister;
            numDuesYear.Visible = !isRegister;
            cbActiveOnly.Visible = !isRegister;
        }

        private void PopulateDojoSelector()
        {
            var options = new List<DojoOption> { new DojoOption { ClubID = string.Empty, Display = "All member dojos" } };

            options.AddRange(KubkManagementFunctions.GetSelectableRosterDojos(kubkFunctions.GetDojos())
                .Select(d => new DojoOption
                {
                    ClubID = d.ClubID,
                    Display = string.IsNullOrWhiteSpace(d.Name) ? d.ClubID : d.Name
                }));

            cmbDojo.DisplayMember = nameof(DojoOption.Display);
            cmbDojo.ValueMember = nameof(DojoOption.ClubID);
            cmbDojo.DataSource = options;
            cmbDojo.SelectedIndex = 0;
        }

        private string SelectedClubId => (cmbDojo.SelectedItem as DojoOption)?.ClubID ?? string.Empty;

        private void ReportCriteria_Changed(object sender, EventArgs e)
        {
            if (suppressCriteriaEvents)
                return;

            RefreshReport();
        }

        private void RefreshReport()
        {
            if (reportKind == KubkReportKind.RankRegister)
                RefreshRankRegister();
            else
                RefreshUnpaidDues();
        }

        #region Rank register

        private void RefreshRankRegister()
        {
            // A dojo chosen explicitly wins; the Windsong checkbox only widens the all-dojos case.
            registerRows = kubkFunctions.GetRankRegister(SelectedClubId, cbIncludeWindsong.Checked);

            BuildColumns("Dojo", "Student Name", "Active", "Art", "Current Rank",
                "Verified", "Last Promotion", "Recommended By");

            dgvReport.SuspendLayout();
            try
            {
                dgvReport.Rows.Clear();

                if (registerRows.Count == 0)
                    return;

                var gridRows = new DataGridViewRow[registerRows.Count];

                for (int i = 0; i < registerRows.Count; i++)
                {
                    KubkRosterEntry entry = registerRows[i];
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvReport,
                        entry.Dojo,
                        entry.FullName,
                        entry.IsActive ? "Yes" : "No",
                        entry.Art,
                        entry.Rank,
                        entry.RankIsVerified ? entry.RankVerifiedDate.Value.ToString("MM/dd/yyyy") : "UNVERIFIED",
                        FormatDate(entry.LastPromotionDate),
                        entry.RecommendedBy ?? string.Empty);
                    gridRows[i] = row;
                }

                dgvReport.Rows.AddRange(gridRows);
            }
            finally
            {
                dgvReport.ResumeLayout();
            }

            int unverified = registerRows.Count(e => !e.RankIsVerified);
            lblSummary.Text = $"{registerRows.Count} rank rows across " +
                $"{registerRows.Select(e => e.Dojo).Distinct().Count()} dojo(s);  {unverified} unverified";
        }

        private IEnumerable<IEnumerable<string>> BuildRankRegisterCsvRows()
        {
            return registerRows.Select(entry => new[]
            {
                entry.Dojo,
                entry.FullName,
                entry.IsActive ? "Yes" : "No",
                entry.Art,
                entry.Rank,
                entry.RankIsVerified ? entry.RankVerifiedDate.Value.ToString("yyyy-MM-dd") : "UNVERIFIED",
                entry.LastPromotionDate.HasValue ? entry.LastPromotionDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                entry.RecommendedBy ?? string.Empty
            });
        }

        #endregion

        #region Unpaid dues

        private void RefreshUnpaidDues()
        {
            unpaidRows = kubkFunctions.GetUnpaidDues((int)numDuesYear.Value, cbActiveOnly.Checked);

            BuildColumns("Dojo", "Student Name", "Active", "Instructor", "Email", "Phone");

            dgvReport.SuspendLayout();
            try
            {
                dgvReport.Rows.Clear();

                if (unpaidRows.Count == 0)
                    return;

                var gridRows = new DataGridViewRow[unpaidRows.Count];

                for (int i = 0; i < unpaidRows.Count; i++)
                {
                    UnpaidDuesEntry entry = unpaidRows[i];
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvReport,
                        entry.Dojo,
                        entry.FullName,
                        entry.IsActive ? "Yes" : "No",
                        entry.Instructor ?? string.Empty,
                        entry.EmailAddress ?? string.Empty,
                        entry.PhoneNumber ?? string.Empty);
                    gridRows[i] = row;
                }

                dgvReport.Rows.AddRange(gridRows);
            }
            finally
            {
                dgvReport.ResumeLayout();
            }

            List<KeyValuePair<string, int>> subtotals = KubkManagementFunctions.BuildUnpaidDuesSubtotals(unpaidRows);

            lblSummary.Text = $"{unpaidRows.Count} student(s) unpaid for {(int)numDuesYear.Value}" +
                (subtotals.Count == 0 ? string.Empty : "   -   " + string.Join(",  ", subtotals.Select(s => $"{s.Key}: {s.Value}")));
        }

        private IEnumerable<IEnumerable<string>> BuildUnpaidDuesCsvRows()
        {
            var rows = unpaidRows.Select(entry => new[]
            {
                entry.Dojo,
                entry.FullName,
                entry.IsActive ? "Yes" : "No",
                entry.Instructor ?? string.Empty,
                entry.EmailAddress ?? string.Empty,
                entry.PhoneNumber ?? string.Empty
            }).ToList();

            // Subtotals ride along at the end so the exported file answers "how many per dojo?"
            // without the reader having to pivot it.
            rows.Add(new[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty });

            foreach (KeyValuePair<string, int> subtotal in KubkManagementFunctions.BuildUnpaidDuesSubtotals(unpaidRows))
                rows.Add(new[] { subtotal.Key, $"{subtotal.Value} unpaid", string.Empty, string.Empty, string.Empty, string.Empty });

            return rows;
        }

        #endregion

        private void BuildColumns(params string[] headers)
        {
            dgvReport.Columns.Clear();

            foreach (string header in headers)
            {
                dgvReport.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = header,
                    Name = "col" + header.Replace(" ", string.Empty),
                    ReadOnly = true,
                    Width = header == "Student Name" || header == "Email" ? 220 : 140
                });
            }
        }

        private string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            bool isRegister = reportKind == KubkReportKind.RankRegister;
            int rowCount = isRegister ? registerRows.Count : unpaidRows.Count;

            if (rowCount == 0)
            {
                MessageService.ShowInformationMessage("There is nothing to export.", "No Rows");
                return;
            }

            string[] headers = isRegister
                ? new[] { "Dojo", "Student Name", "Active", "Art", "Current Rank", "Verified", "Last Promotion", "Recommended By" }
                : new[] { "Dojo", "Student Name", "Active", "Instructor", "Email", "Phone" };

            IEnumerable<IEnumerable<string>> rows = isRegister ? BuildRankRegisterCsvRows() : BuildUnpaidDuesCsvRows();

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.AddExtension = true;
                saveDialog.FileName = isRegister
                    ? $"KUBK-Rank-Register-{DateTime.Today:yyyy-MM-dd}.csv"
                    : $"KUBK-Unpaid-Dues-{(int)numDuesYear.Value}.csv";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    CsvWriter.WriteToFile(saveDialog.FileName, CsvWriter.BuildCsv(headers, rows));
                    Log.Information($"Exported {rowCount} rows to {saveDialog.FileName}");
                    MessageService.ShowInformationMessage($"Exported {rowCount} rows to{Environment.NewLine}{saveDialog.FileName}", "Export Complete");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error exporting report to {saveDialog.FileName}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    MessageService.ShowErrorMessage($"Error writing the file:{Environment.NewLine}{ex.Message}", "Export Failed");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private class DojoOption
        {
            public string ClubID { get; set; }
            public string Display { get; set; }
        }
    }
}
