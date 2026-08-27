using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for the sign-in history report: mapping of individual attendance rows, the
    /// most-recent-first ordering, the active filter, and how each row picks up the student's
    /// current rank and eligibility.
    /// </summary>
    [TestFixture]
    public class SignInHistoryReportTests
    {
        #region Helpers

        private static DataTable BuildSignInTable()
        {
            var table = new DataTable();
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("StudentFirstName", typeof(string));
            table.Columns.Add("StudentLastName", typeof(string));
            table.Columns.Add("StudentStatus", typeof(string));
            table.Columns.Add("Art", typeof(string));
            table.Columns.Add("SignInDate", typeof(DateTime));
            table.Columns.Add("Hours", typeof(double));
            return table;
        }

        private static void AddSignIn(DataTable table, int id, string first, string last, string status,
            string art, DateTime? signInDate, double hours)
        {
            DataRow row = table.NewRow();
            row["StudentID"] = id;
            row["StudentFirstName"] = first;
            row["StudentLastName"] = last;
            row["StudentStatus"] = status;
            row["Art"] = art;
            row["SignInDate"] = (object)signInDate ?? DBNull.Value;
            row["Hours"] = hours;
            table.Rows.Add(row);
        }

        private static Dictionary<string, StudentActivityEntry> BuildStanding()
        {
            return SignInHistoryReportFunctions.BuildCurrentStandingLookup(new List<StudentActivityEntry>
            {
                new StudentActivityEntry
                {
                    StudentID = 1, Art = "Aikido", Rank = "SANKYU",
                    NextRank = "NIKYU", IsEligibleForPromotion = true
                },
                new StudentActivityEntry
                {
                    StudentID = 1, Art = "Judo", Rank = "WHITE",
                    NextRank = "YONKYU", IsEligibleForPromotion = false
                }
            });
        }

        #endregion

        #region Mapping

        [Test]
        public void BuildEntries_MapsOneRowPerAttendance()
        {
            // The same student attending the same art twice must produce two rows: this report
            // is every sign-in, not the latest one.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 1, 18, 30, 0), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 3, 18, 30, 0), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());

            Assert.AreEqual(2, entries.Count);
            Assert.AreEqual("Jane Smith", entries[0].FullName);
            Assert.AreEqual(1.5, entries[0].Hours);
        }

        [Test]
        public void BuildEntries_TakesRankAndEligibilityFromCurrentStanding()
        {
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 1), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Judo", new DateTime(2026, 6, 1), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());

            SignInHistoryEntry aikido = entries.First(e => e.Art == "Aikido");
            SignInHistoryEntry judo = entries.First(e => e.Art == "Judo");

            Assert.AreEqual("SANKYU", aikido.Rank);
            Assert.AreEqual("Yes - NIKYU", aikido.EligibilityDisplay);
            Assert.AreEqual("WHITE", judo.Rank);
            Assert.AreEqual("No", judo.EligibilityDisplay);
        }

        [Test]
        public void BuildEntries_ForAnArtTheStudentNoLongerTakes_LeavesRankBlank()
        {
            // Signin_History keeps the attendance even after the enrollment is removed, so the
            // sign-in must still be reported - just with no rank to show against it.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Jyodo", new DateTime(2026, 6, 1), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());

            Assert.AreEqual(1, entries.Count, "The sign-in must not be dropped");
            Assert.IsNull(entries[0].Rank);
            Assert.AreEqual("No", entries[0].EligibilityDisplay);
        }

        [Test]
        public void BuildEntries_SkipsRowsWithNoSignInDate()
        {
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", null, 1.5);

            Assert.IsEmpty(SignInHistoryReportFunctions.BuildEntries(table, BuildStanding()));
        }

        [Test]
        public void BuildEntries_WithNoTable_ReturnsEmpty()
        {
            Assert.IsEmpty(SignInHistoryReportFunctions.BuildEntries(null, BuildStanding()));
            Assert.IsEmpty(SignInHistoryReportFunctions.BuildEntries(new DataTable(), BuildStanding()));
        }

        [Test]
        public void BuildEntries_WithNoStandingLookup_StillReportsTheSignIn()
        {
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 1), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, null);

            Assert.AreEqual(1, entries.Count);
            Assert.IsNull(entries[0].Rank);
        }

        #endregion

        #region Ordering and filtering

        [Test]
        public void SortByMostRecent_PutsTheLatestSignInFirst()
        {
            var entries = new List<SignInHistoryEntry>
            {
                new SignInHistoryEntry { LastName = "Older", SignInDate = new DateTime(2026, 1, 1) },
                new SignInHistoryEntry { LastName = "Newest", SignInDate = new DateTime(2026, 8, 15, 11, 12, 0) },
                new SignInHistoryEntry { LastName = "Middle", SignInDate = new DateTime(2026, 5, 1) }
            };

            List<SignInHistoryEntry> sorted = SignInHistoryReportFunctions.SortByMostRecent(entries);

            CollectionAssert.AreEqual(new[] { "Newest", "Middle", "Older" }, sorted.Select(e => e.LastName).ToArray());
        }

        [Test]
        public void SortByMostRecent_OrdersWithinTheSameDayByTime()
        {
            var entries = new List<SignInHistoryEntry>
            {
                new SignInHistoryEntry { LastName = "Morning", SignInDate = new DateTime(2026, 8, 15, 9, 0, 0) },
                new SignInHistoryEntry { LastName = "Evening", SignInDate = new DateTime(2026, 8, 15, 18, 0, 0) }
            };

            List<SignInHistoryEntry> sorted = SignInHistoryReportFunctions.SortByMostRecent(entries);

            CollectionAssert.AreEqual(new[] { "Evening", "Morning" }, sorted.Select(e => e.LastName).ToArray());
        }

        [Test]
        public void ApplyActiveFilter_HonoursTheFlag()
        {
            var entries = new List<SignInHistoryEntry>
            {
                new SignInHistoryEntry { LastName = "Active", IsActive = true },
                new SignInHistoryEntry { LastName = "Inactive", IsActive = false }
            };

            Assert.AreEqual(1, SignInHistoryReportFunctions.ApplyActiveFilter(entries, true).Count);
            Assert.AreEqual(2, SignInHistoryReportFunctions.ApplyActiveFilter(entries, false).Count);
        }

        [Test]
        public void SignInDisplay_ShowsTheDateAndTime()
        {
            var entry = new SignInHistoryEntry { SignInDate = new DateTime(2026, 8, 15, 11, 12, 0) };

            Assert.AreEqual("08/15/2026 11:12 AM", entry.SignInDisplay);
        }

        #endregion

        #region Current standing lookup

        [Test]
        public void BuildCurrentStandingLookup_KeysByStudentAndArtCaseInsensitively()
        {
            Dictionary<string, StudentActivityEntry> lookup = BuildStanding();

            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "aikido", new DateTime(2026, 6, 1), 1.5);

            Assert.AreEqual("SANKYU", SignInHistoryReportFunctions.BuildEntries(table, lookup)[0].Rank);
        }

        [Test]
        public void BuildCurrentStandingLookup_WithNoActivity_ReturnsEmpty()
        {
            Assert.IsEmpty(SignInHistoryReportFunctions.BuildCurrentStandingLookup(null));
        }

        #endregion
    }
}
