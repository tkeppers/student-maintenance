using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for the KUBK roster view logic added in Phase 3: DataTable mapping, the roster
    /// screen's display filters, status-line counts, and the dojo-selector/club-id helpers.
    /// </summary>
    [TestFixture]
    public class KubkRosterTests
    {
        #region KubkRosterEntry

        [Test]
        public void RankIsVerified_ReflectsWhetherVerifiedDateIsSet()
        {
            Assert.IsTrue(new KubkRosterEntry { RankVerifiedDate = DateTime.Today }.RankIsVerified);
            Assert.IsFalse(new KubkRosterEntry { RankVerifiedDate = null }.RankIsVerified);
        }

        [Test]
        public void DuesArePaid_ReflectsWhetherPaidDateIsSet()
        {
            Assert.IsTrue(new KubkRosterEntry { DuesPaidDate = DateTime.Today }.DuesArePaid);
            Assert.IsFalse(new KubkRosterEntry { DuesPaidDate = null }.DuesArePaid);
        }

        [Test]
        public void RankVerifiedDisplay_WhenNotVerified_ReadsUnverified()
        {
            var entry = new KubkRosterEntry { RankVerifiedDate = null };

            Assert.AreEqual("Unverified", entry.RankVerifiedDisplay);
        }

        [Test]
        public void RankVerifiedDisplay_WhenVerified_ShowsTheDate()
        {
            var entry = new KubkRosterEntry { RankVerifiedDate = new DateTime(2026, 3, 14) };

            Assert.AreEqual("03/14/2026", entry.RankVerifiedDisplay);
        }

        [Test]
        public void FullName_JoinsFirstAndLastName()
        {
            var entry = new KubkRosterEntry { FirstName = "Jane", LastName = "Smith" };

            Assert.AreEqual("Jane Smith", entry.FullName);
        }

        [Test]
        public void YearsAtRank_WhenPromotedTwoYearsAgo_ReturnsApproximatelyTwo()
        {
            var entry = new KubkRosterEntry { LastPromotionDate = DateTime.Now.AddYears(-2) };

            Assert.AreEqual(2.0, entry.YearsAtRank(), 0.05);
        }

        [Test]
        public void YearsAtRank_WhenNeverPromoted_ReturnsZero()
        {
            var entry = new KubkRosterEntry { LastPromotionDate = null };

            Assert.AreEqual(0, entry.YearsAtRank());
        }

        #endregion

        #region MapRoster

        [Test]
        public void MapRoster_MapsEveryColumnIncludingNulls()
        {
            DataTable table = BuildRosterTable();
            AddRosterRow(table, 42, "Jane", "Smith", "A", "DENTON", "Aikido", "SANKYU",
                new DateTime(2024, 5, 1), null, null);

            List<KubkRosterEntry> entries = KubkManagementFunctions.MapRoster(table);

            Assert.AreEqual(1, entries.Count);
            KubkRosterEntry entry = entries[0];
            Assert.AreEqual(42, entry.StudentID);
            Assert.AreEqual("Jane", entry.FirstName);
            Assert.AreEqual("Smith", entry.LastName);
            Assert.IsTrue(entry.IsActive);
            Assert.AreEqual("DENTON", entry.Dojo);
            Assert.AreEqual("Aikido", entry.Art);
            Assert.AreEqual("SANKYU", entry.Rank);
            Assert.AreEqual(new DateTime(2024, 5, 1), entry.LastPromotionDate);
            Assert.IsNull(entry.RankVerifiedDate, "A null verified date must map to null, not DateTime.MinValue");
            Assert.IsNull(entry.DuesPaidDate);
        }

        [Test]
        public void MapRoster_InactiveStatusMapsToNotActive()
        {
            DataTable table = BuildRosterTable();
            AddRosterRow(table, 1, "A", "B", "I", "DENTON", "Judo", "WHITE", null, null, null);

            Assert.IsFalse(KubkManagementFunctions.MapRoster(table)[0].IsActive);
        }

        [Test]
        public void MapRoster_WhenTableIsEmptyOrNull_ReturnsEmptyList()
        {
            Assert.IsEmpty(KubkManagementFunctions.MapRoster(null));
            Assert.IsEmpty(KubkManagementFunctions.MapRoster(new DataTable()));
        }

        #endregion

        #region ApplyRosterFilters

        [Test]
        public void ApplyRosterFilters_ActiveOnly_ExcludesInactiveStudents()
        {
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, IsActive = true },
                new KubkRosterEntry { StudentID = 2, IsActive = false }
            };

            List<KubkRosterEntry> filtered = KubkManagementFunctions.ApplyRosterFilters(entries, true, false);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(1, filtered[0].StudentID);
        }

        [Test]
        public void ApplyRosterFilters_UnpaidOnly_ExcludesStudentsWhoHavePaid()
        {
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, IsActive = true, DuesPaidDate = DateTime.Today },
                new KubkRosterEntry { StudentID = 2, IsActive = true, DuesPaidDate = null }
            };

            List<KubkRosterEntry> filtered = KubkManagementFunctions.ApplyRosterFilters(entries, false, true);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(2, filtered[0].StudentID);
        }

        [Test]
        public void ApplyRosterFilters_BothFiltersApplyTogether()
        {
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, IsActive = true, DuesPaidDate = null },
                new KubkRosterEntry { StudentID = 2, IsActive = false, DuesPaidDate = null },
                new KubkRosterEntry { StudentID = 3, IsActive = true, DuesPaidDate = DateTime.Today }
            };

            List<KubkRosterEntry> filtered = KubkManagementFunctions.ApplyRosterFilters(entries, true, true);

            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(1, filtered[0].StudentID);
        }

        [Test]
        public void ApplyRosterFilters_WithNoFilters_ReturnsEverything()
        {
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, IsActive = false, DuesPaidDate = DateTime.Today },
                new KubkRosterEntry { StudentID = 2, IsActive = true, DuesPaidDate = null }
            };

            Assert.AreEqual(2, KubkManagementFunctions.ApplyRosterFilters(entries, false, false).Count);
        }

        #endregion

        #region SummarizeRoster

        [Test]
        public void SummarizeRoster_CountsStudentsDistinctlyAcrossArts()
        {
            // One student enrolled in two arts is still one student, but two rank rows.
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, Art = "Aikido", RankVerifiedDate = null, DuesPaidDate = null },
                new KubkRosterEntry { StudentID = 1, Art = "Judo", RankVerifiedDate = null, DuesPaidDate = null }
            };

            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(entries);

            Assert.AreEqual(1, summary.StudentsShown);
            Assert.AreEqual(2, summary.UnverifiedRanks);
            Assert.AreEqual(1, summary.StudentsUnpaid);
        }

        [Test]
        public void SummarizeRoster_CountsVerifiedAndPaidCorrectly()
        {
            var entries = new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, RankVerifiedDate = DateTime.Today, DuesPaidDate = DateTime.Today },
                new KubkRosterEntry { StudentID = 2, RankVerifiedDate = null, DuesPaidDate = null }
            };

            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(entries);

            Assert.AreEqual(2, summary.StudentsShown);
            Assert.AreEqual(1, summary.UnverifiedRanks);
            Assert.AreEqual(1, summary.StudentsUnpaid);
        }

        [Test]
        public void SummarizeRoster_WhenEmpty_ReturnsZeroes()
        {
            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(new List<KubkRosterEntry>());

            Assert.AreEqual(0, summary.StudentsShown);
            Assert.AreEqual(0, summary.UnverifiedRanks);
            Assert.AreEqual(0, summary.StudentsUnpaid);
        }

        #endregion

        #region IsClubIdInUse

        [TestCase("DENTON", true)]
        [TestCase("denton", true)]
        [TestCase("  DENTON  ", true)]
        [TestCase("NEWDOJO", false)]
        [Test]
        public void IsClubIdInUse_MatchesCaseInsensitivelyAndIgnoresSurroundingWhitespace(string clubId, bool expected)
        {
            var existing = new List<Dojo>
            {
                new Dojo { ClubID = "DENTON" },
                new Dojo { ClubID = "Windsong" }
            };

            Assert.AreEqual(expected, KubkManagementFunctions.IsClubIdInUse(existing, clubId));
        }

        [Test]
        public void IsClubIdInUse_WithNoExistingDojosOrBlankId_ReturnsFalse()
        {
            Assert.IsFalse(KubkManagementFunctions.IsClubIdInUse(null, "ANY"));
            Assert.IsFalse(KubkManagementFunctions.IsClubIdInUse(new List<Dojo>(), "   "));
        }

        #endregion

        #region GetSelectableRosterDojos

        [Test]
        public void GetSelectableRosterDojos_ExcludesInactiveAndSortsByName()
        {
            var dojos = new List<Dojo>
            {
                new Dojo { ClubID = "STILLWATER", Name = "Stillwater", Active = true },
                new Dojo { ClubID = "Windsong", Name = "Windsong", Active = true },
                new Dojo { ClubID = "CLOSED", Name = "Closed Dojo", Active = false },
                new Dojo { ClubID = "DENTON", Name = "Denton", Active = true }
            };

            List<Dojo> selectable = KubkManagementFunctions.GetSelectableRosterDojos(dojos);

            Assert.AreEqual(3, selectable.Count);
            CollectionAssert.AreEqual(new[] { "Denton", "Stillwater", "Windsong" }, selectable.Select(d => d.Name).ToArray());
        }

        [Test]
        public void GetSelectableRosterDojos_IncludesWindsong()
        {
            // Annual dues are owed per student at every dojo, Windsong included, so Windsong
            // students must be reachable from the roster screen to be marked paid.
            var dojos = new List<Dojo> { new Dojo { ClubID = "Windsong", Name = "Windsong", Active = true } };

            Assert.AreEqual(1, KubkManagementFunctions.GetSelectableRosterDojos(dojos).Count);
        }

        [Test]
        public void GetSelectableRosterDojos_WhenNull_ReturnsEmptyList()
        {
            Assert.IsEmpty(KubkManagementFunctions.GetSelectableRosterDojos(null));
        }

        #endregion

        #region Helpers

        private static DataTable BuildRosterTable()
        {
            var table = new DataTable();
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("StudentFirstName", typeof(string));
            table.Columns.Add("StudentLastName", typeof(string));
            table.Columns.Add("StudentStatus", typeof(string));
            table.Columns.Add("StudentDojo", typeof(string));
            table.Columns.Add("Art", typeof(string));
            table.Columns.Add("Rank", typeof(string));
            table.Columns.Add("LastPromotionDate", typeof(DateTime));
            table.Columns.Add("RankVerifiedDate", typeof(DateTime));
            table.Columns.Add("DuesPaidDate", typeof(DateTime));
            return table;
        }

        private static void AddRosterRow(DataTable table, int studentID, string firstName, string lastName,
            string status, string dojo, string art, string rank,
            DateTime? lastPromotion, DateTime? rankVerified, DateTime? duesPaid)
        {
            DataRow row = table.NewRow();
            row["StudentID"] = studentID;
            row["StudentFirstName"] = firstName;
            row["StudentLastName"] = lastName;
            row["StudentStatus"] = status;
            row["StudentDojo"] = dojo;
            row["Art"] = art;
            row["Rank"] = rank;
            row["LastPromotionDate"] = (object)lastPromotion ?? DBNull.Value;
            row["RankVerifiedDate"] = (object)rankVerified ?? DBNull.Value;
            row["DuesPaidDate"] = (object)duesPaid ?? DBNull.Value;
            table.Rows.Add(row);
        }

        #endregion
    }
}
