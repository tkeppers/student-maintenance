using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for the student activity report: mapping, the most-recent-sign-in ordering, the
    /// active filter, and that promotion eligibility matches the gates the promotion screen uses.
    /// </summary>
    [TestFixture]
    public class StudentActivityReportTests
    {
        #region Helpers

        private static DataTable BuildActivityTable()
        {
            var table = new DataTable();
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("StudentFirstName", typeof(string));
            table.Columns.Add("StudentLastName", typeof(string));
            table.Columns.Add("StudentStatus", typeof(string));
            table.Columns.Add("StudentBirthDate", typeof(DateTime));
            table.Columns.Add("Art", typeof(string));
            table.Columns.Add("Rank", typeof(string));
            table.Columns.Add("HoursInArt", typeof(double));
            table.Columns.Add("LastSignInDate", typeof(DateTime));
            table.Columns.Add("DateStarted", typeof(DateTime));
            table.Columns.Add("LastPromotionDate", typeof(DateTime));
            return table;
        }

        private static void AddActivityRow(DataTable table, int id, string first, string last, string status,
            DateTime? birthDate, string art, string rank, double hours, DateTime? lastSignIn,
            DateTime? started, DateTime? promoted)
        {
            DataRow row = table.NewRow();
            row["StudentID"] = id;
            row["StudentFirstName"] = first;
            row["StudentLastName"] = last;
            row["StudentStatus"] = status;
            row["StudentBirthDate"] = (object)birthDate ?? DBNull.Value;
            row["Art"] = art;
            row["Rank"] = rank;
            row["HoursInArt"] = hours;
            row["LastSignInDate"] = (object)lastSignIn ?? DBNull.Value;
            row["DateStarted"] = (object)started ?? DBNull.Value;
            row["LastPromotionDate"] = (object)promoted ?? DBNull.Value;
            table.Rows.Add(row);
        }

        /// <summary>Promotion criteria table shaped like GetStudentPromotionRequirements returns.</summary>
        private static DataTable BuildRequirements()
        {
            var table = new DataTable();
            table.Columns.Add("Art", typeof(string));
            table.Columns.Add("CurrentRank", typeof(string));
            table.Columns.Add("NextRank", typeof(string));
            table.Columns.Add("MinimumTrainingHours", typeof(int));
            table.Columns.Add("MinimumAge", typeof(int));
            table.Columns.Add("YearsInArt", typeof(double));
            table.Columns.Add("YearsAtCurrentRank", typeof(double));

            DataRow white = table.NewRow();
            white["Art"] = "Aikido"; white["CurrentRank"] = "WHITE"; white["NextRank"] = "YONKYU";
            white["MinimumTrainingHours"] = 40; white["MinimumAge"] = 10;
            white["YearsInArt"] = 0.5; white["YearsAtCurrentRank"] = 0.25;
            table.Rows.Add(white);

            return table;
        }

        #endregion

        #region Mapping and eligibility

        [Test]
        public void BuildEntries_MapsTheReportedColumns()
        {
            DataTable table = BuildActivityTable();
            AddActivityRow(table, 42, "Jane", "Smith", "A", DateTime.Today.AddYears(-30),
                "Aikido", "WHITE", 120.5, new DateTime(2026, 5, 1),
                DateTime.Today.AddYears(-3), DateTime.Today.AddYears(-2));

            List<StudentActivityEntry> entries = StudentActivityReportFunctions.BuildEntries(table, BuildRequirements());

            Assert.AreEqual(1, entries.Count);
            Assert.AreEqual("Jane Smith", entries[0].FullName);
            Assert.AreEqual("Aikido", entries[0].Art);
            Assert.AreEqual("WHITE", entries[0].Rank);
            Assert.AreEqual(120.5, entries[0].HoursInArt);
            Assert.AreEqual(new DateTime(2026, 5, 1), entries[0].LastSignInDate);
            Assert.IsTrue(entries[0].IsActive);
        }

        [Test]
        public void BuildEntries_MarksAStudentWhoMeetsEveryGateAsEligible()
        {
            DataTable table = BuildActivityTable();
            AddActivityRow(table, 42, "Jane", "Smith", "A", DateTime.Today.AddYears(-30),
                "Aikido", "WHITE", 100, DateTime.Today,
                DateTime.Today.AddYears(-3), DateTime.Today.AddYears(-2));

            StudentActivityEntry entry = StudentActivityReportFunctions.BuildEntries(table, BuildRequirements())[0];

            Assert.IsTrue(entry.IsEligibleForPromotion);
            Assert.AreEqual("YONKYU", entry.NextRank);
            Assert.AreEqual("Yes - YONKYU", entry.EligibilityDisplay);
        }

        [Test]
        public void BuildEntries_MarksAStudentShortOnHoursAsNotEligible()
        {
            DataTable table = BuildActivityTable();
            AddActivityRow(table, 42, "Jane", "Smith", "A", DateTime.Today.AddYears(-30),
                "Aikido", "WHITE", 10, DateTime.Today,
                DateTime.Today.AddYears(-3), DateTime.Today.AddYears(-2));

            StudentActivityEntry entry = StudentActivityReportFunctions.BuildEntries(table, BuildRequirements())[0];

            Assert.IsFalse(entry.IsEligibleForPromotion, "40 hours are required and only 10 have been earned");
            Assert.AreEqual("No", entry.EligibilityDisplay);
        }

        [Test]
        public void BuildEntries_WithNoCriteriaForTheRank_IsNotEligible()
        {
            // A rank the requirements table says nothing about must not inherit the previous
            // row's thresholds, which is what a shared PromotionCriteria instance would do.
            DataTable table = BuildActivityTable();
            AddActivityRow(table, 1, "Jane", "Smith", "A", DateTime.Today.AddYears(-30),
                "Aikido", "WHITE", 100, DateTime.Today, DateTime.Today.AddYears(-3), DateTime.Today.AddYears(-2));
            AddActivityRow(table, 2, "Bob", "Adams", "A", DateTime.Today.AddYears(-30),
                "Aikido", "HACHIDAN", 9999, DateTime.Today, DateTime.Today.AddYears(-30), DateTime.Today.AddYears(-20));

            List<StudentActivityEntry> entries = StudentActivityReportFunctions.BuildEntries(table, BuildRequirements());

            Assert.IsTrue(entries[0].IsEligibleForPromotion, "WHITE has criteria and they are met");
            Assert.IsFalse(entries[1].IsEligibleForPromotion, "HACHIDAN has no criteria row, so there is nothing to be eligible for");
        }

        [Test]
        public void BuildEntries_WithNoRequirementsTable_LeavesEveryoneNotEligible()
        {
            DataTable table = BuildActivityTable();
            AddActivityRow(table, 42, "Jane", "Smith", "A", DateTime.Today.AddYears(-30),
                "Aikido", "WHITE", 100, DateTime.Today, DateTime.Today.AddYears(-3), DateTime.Today.AddYears(-2));

            Assert.IsFalse(StudentActivityReportFunctions.BuildEntries(table, null)[0].IsEligibleForPromotion);
        }

        [Test]
        public void BuildEntries_WithNoTable_ReturnsEmpty()
        {
            Assert.IsEmpty(StudentActivityReportFunctions.BuildEntries(null, BuildRequirements()));
            Assert.IsEmpty(StudentActivityReportFunctions.BuildEntries(new DataTable(), BuildRequirements()));
        }

        #endregion

        #region Sorting

        [Test]
        public void SortByMostRecentSignIn_PutsTheMostRecentFirst()
        {
            var entries = new List<StudentActivityEntry>
            {
                new StudentActivityEntry { LastName = "Older", LastSignInDate = new DateTime(2024, 1, 1) },
                new StudentActivityEntry { LastName = "Newest", LastSignInDate = new DateTime(2026, 6, 1) },
                new StudentActivityEntry { LastName = "Middle", LastSignInDate = new DateTime(2025, 3, 1) }
            };

            List<StudentActivityEntry> sorted = StudentActivityReportFunctions.SortByMostRecentSignIn(entries);

            CollectionAssert.AreEqual(new[] { "Newest", "Middle", "Older" }, sorted.Select(e => e.LastName).ToArray());
        }

        [Test]
        public void SortByMostRecentSignIn_PutsStudentsWhoNeverSignedInLast()
        {
            var entries = new List<StudentActivityEntry>
            {
                new StudentActivityEntry { LastName = "NeverTrained", LastSignInDate = null },
                new StudentActivityEntry { LastName = "Trained", LastSignInDate = new DateTime(2020, 1, 1) }
            };

            List<StudentActivityEntry> sorted = StudentActivityReportFunctions.SortByMostRecentSignIn(entries);

            CollectionAssert.AreEqual(new[] { "Trained", "NeverTrained" }, sorted.Select(e => e.LastName).ToArray());
        }

        [Test]
        public void SortByMostRecentSignIn_WithNoEntries_ReturnsEmpty()
        {
            Assert.IsEmpty(StudentActivityReportFunctions.SortByMostRecentSignIn(null));
        }

        [Test]
        public void LastSignInDisplay_ReadsNeverWhenThereIsNoSignIn()
        {
            Assert.AreEqual("Never", new StudentActivityEntry { LastSignInDate = null }.LastSignInDisplay);
            Assert.AreEqual("06/01/2026", new StudentActivityEntry { LastSignInDate = new DateTime(2026, 6, 1) }.LastSignInDisplay);
        }

        #endregion

        #region Active filter

        [Test]
        public void ApplyActiveFilter_HonoursTheFlag()
        {
            var entries = new List<StudentActivityEntry>
            {
                new StudentActivityEntry { LastName = "Active", IsActive = true },
                new StudentActivityEntry { LastName = "Inactive", IsActive = false }
            };

            Assert.AreEqual(1, StudentActivityReportFunctions.ApplyActiveFilter(entries, true).Count);
            Assert.AreEqual(2, StudentActivityReportFunctions.ApplyActiveFilter(entries, false).Count);
        }

        #endregion
    }
}
