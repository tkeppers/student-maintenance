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
                    StudentID = 1, Art = "Aikido", Rank = "SANKYU", HoursInArt = 250,
                    NextRank = "NIKYU", IsEligibleForPromotion = true
                },
                new StudentActivityEntry
                {
                    StudentID = 1, Art = "Judo", Rank = "WHITE", HoursInArt = 12,
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
            Assert.AreEqual(1.5, entries[0].SessionHours);
        }

        #endregion

        #region Running totals

        [Test]
        public void ApplyRunningTotals_CountsBackFromTheRecordedTotal()
        {
            // Anchored to the student's recorded 250 hours: the newest session shows 250, and
            // each earlier one shows the total as it stood then.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 1), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 3), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 5), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());
            SignInHistoryReportFunctions.ApplyRunningTotals(entries, BuildStanding());

            Assert.AreEqual(250, entries.First(e => e.SignInDate == new DateTime(2026, 6, 5)).CumulativeHours);
            Assert.AreEqual(248.5, entries.First(e => e.SignInDate == new DateTime(2026, 6, 3)).CumulativeHours);
            Assert.AreEqual(247, entries.First(e => e.SignInDate == new DateTime(2026, 6, 1)).CumulativeHours);
        }

        [Test]
        public void ApplyRunningTotals_TracksEachArtSeparately()
        {
            // Promotion hours are per art, so the totals must not be pooled across arts.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 5), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Judo", new DateTime(2026, 6, 5), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());
            SignInHistoryReportFunctions.ApplyRunningTotals(entries, BuildStanding());

            Assert.AreEqual(250, entries.First(e => e.Art == "Aikido").CumulativeHours);
            Assert.AreEqual(12, entries.First(e => e.Art == "Judo").CumulativeHours);
        }

        [Test]
        public void ApplyRunningTotals_WithNoRecordedTotal_LeavesTheHoursBlank()
        {
            // No current enrollment means nothing to count back from; better an empty cell than
            // a number invented from a partial log.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Jyodo", new DateTime(2026, 6, 5), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());
            SignInHistoryReportFunctions.ApplyRunningTotals(entries, BuildStanding());

            Assert.IsNull(entries[0].CumulativeHours);
            Assert.IsEmpty(entries[0].CumulativeHoursDisplay);
        }

        [Test]
        public void ApplyRunningTotals_ShowsWhenTheStudentCrossedAPromotionThreshold()
        {
            // The point of the running total: seeing which session took them past the required
            // hours. With 250 recorded and a 248 requirement, that is the 6/3 session.
            DataTable table = BuildSignInTable();
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 1), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 3), 1.5);
            AddSignIn(table, 1, "Jane", "Smith", "A", "Aikido", new DateTime(2026, 6, 5), 1.5);

            List<SignInHistoryEntry> entries = SignInHistoryReportFunctions.BuildEntries(table, BuildStanding());
            SignInHistoryReportFunctions.ApplyRunningTotals(entries, BuildStanding());

            SignInHistoryEntry crossing = entries
                .Where(e => e.CumulativeHours >= 248)
                .OrderBy(e => e.SignInDate)
                .First();

            Assert.AreEqual(new DateTime(2026, 6, 3), crossing.SignInDate);
        }

        [Test]
        public void ApplyRunningTotals_WithNoEntries_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => SignInHistoryReportFunctions.ApplyRunningTotals(null, BuildStanding()));
            Assert.DoesNotThrow(() => SignInHistoryReportFunctions.ApplyRunningTotals(
                new List<SignInHistoryEntry>(), null));
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
