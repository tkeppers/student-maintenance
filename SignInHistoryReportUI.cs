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
    /// Every Windsong sign-in over a chosen period, one row per attendance, most recent first.
    /// The activity report shows where each student stands; this shows how they got there.
    /// </summary>
    public partial class SignInHistoryReportUI : Form
    {
        private const string ReportClubId = "Windsong";

        /// <summary>Sign-ins are logged daily, so a month is a useful default window.</summary>
        private const int DefaultPeriodDays = 30;

        private static readonly Color EligibleRowColor = Color.FromArgb(212, 237, 218);

        private readonly DataGridViewCellStyle eligibleRowStyle =
            new DataGridViewCellStyle { BackColor = EligibleRowColor };

        private readonly SignInHistoryReportFunctions reportFunctions;

        private List<SignInHistoryEntry> reportRows = new List<SignInHistoryEntry>();

        private bool suppressCriteriaEvents;

        public SignInHistoryReportUI(IDataRepository dataRepository)
        {
            InitializeComponent();

            reportFunctions = new SignInHistoryReportFunctions(dataRepository);
        }

        private void SignInHistoryReportUI_Load(object sender, EventArgs e)
        {
            suppressCriteriaEvents = true;

            dtTo.Value = DateTime.Today;
            dtFrom.Value = DateTime.Today.AddDays(-DefaultPeriodDays);

            suppressCriteriaEvents = false;

            RefreshReport();
        }

        private void ReportCriteria_Changed(object sender, EventArgs e)
        {
            if (suppressCriteriaEvents)
                return;

            RefreshReport();
        }

        private void RefreshReport()
        {
            // An inverted range would silently return nothing; say so rather than showing an
            // empty report that looks like "nobody trained".
            if (dtFrom.Value.Date > dtTo.Value.Date)
            {
                reportRows = new List<SignInHistoryEntry>();
                PopulateGrid();
                lblSummary.Text = "The From date is after the To date.";
                return;
            }

            reportRows = reportFunctions.GetSignInHistory(
                ReportClubId, dtFrom.Value, dtTo.Value, cbActiveOnly.Checked);

            PopulateGrid();

            int students = reportRows.Select(r => r.StudentID).Distinct().Count();

            // Hours actually trained during the period - the sum of the sessions, not of the
            // running totals, which would be meaningless added together.
            double hours = reportRows.Sum(r => r.SessionHours);

            lblSummary.Text = $"{reportRows.Count} sign-ins by {students} student(s);  {hours:0.#} hours " +
                $"({dtFrom.Value:MM/dd/yyyy} - {dtTo.Value:MM/dd/yyyy})";
        }

        private void PopulateGrid()
        {
            DataGridViewColumn sortedColumn = dgvSignIns.SortedColumn;
            SortOrder sortOrder = dgvSignIns.SortOrder;

            dgvSignIns.SuspendLayout();

            try
            {
                dgvSignIns.Rows.Clear();

                if (reportRows.Count == 0)
                    return;

                var rows = new DataGridViewRow[reportRows.Count];

                for (int i = 0; i < reportRows.Count; i++)
                {
                    SignInHistoryEntry entry = reportRows[i];

                    var row = new DataGridViewRow();
                    row.CreateCells(dgvSignIns,
                        entry.FullName,
                        entry.Art,
                        entry.Rank ?? string.Empty,
                        entry.SignInDisplay,
                        entry.CumulativeHoursDisplay,
                        entry.EligibilityDisplay);

                    if (entry.IsEligibleForPromotion)
                        row.DefaultCellStyle = eligibleRowStyle;

                    row.Tag = entry;
                    rows[i] = row;
                }

                dgvSignIns.Rows.AddRange(rows);
            }
            finally
            {
                dgvSignIns.ResumeLayout();
            }

            if (sortedColumn != null && sortOrder != SortOrder.None && dgvSignIns.Rows.Count > 0)
            {
                dgvSignIns.Sort(sortedColumn,
                    sortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
            }

            dgvSignIns.ClearSelection();
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (reportRows.Count == 0)
            {
                MessageService.ShowInformationMessage("There is nothing to export.", "No Rows");
                return;
            }

            var headers = new[] { "Student Name", "Art", "Rank", "Sign-In", "Hours", "Eligible for Promotion" };

            IEnumerable<IEnumerable<string>> rows = reportRows.Select(entry => new[]
            {
                entry.FullName,
                entry.Art,
                entry.Rank ?? string.Empty,
                entry.SignInDate.ToString("yyyy-MM-dd HH:mm"),
                entry.CumulativeHoursDisplay,
                entry.EligibilityDisplay
            });

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.DefaultExt = "csv";
                saveDialog.AddExtension = true;
                saveDialog.FileName = $"Windsong-Sign-Ins-{dtFrom.Value:yyyy-MM-dd}-to-{dtTo.Value:yyyy-MM-dd}.csv";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    CsvWriter.WriteToFile(saveDialog.FileName, CsvWriter.BuildCsv(headers, rows));
                    Log.Information($"Exported {reportRows.Count} sign-in rows to {saveDialog.FileName}");
                    MessageService.ShowInformationMessage(
                        $"Exported {reportRows.Count} rows to{Environment.NewLine}{saveDialog.FileName}", "Export Complete");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error exporting sign-in history to {saveDialog.FileName}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
