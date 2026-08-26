using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for Phase 5: the rank re-verification workflow, the CSV helper, and assembly of the
    /// rank register and unpaid dues reports.
    /// </summary>
    [TestFixture]
    public class KubkReportsTests
    {
        #region CSV escaping

        [TestCase("plain", "plain")]
        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("has space", "has space")]
        [Test]
        public void EscapeField_LeavesSimpleValuesAlone(string input, string expected)
        {
            Assert.AreEqual(expected, CsvWriter.EscapeField(input));
        }

        [Test]
        public void EscapeField_QuotesValuesContainingAComma()
        {
            // Names are stored "LAST, FIRST" in places, which is exactly what splits a naive CSV.
            Assert.AreEqual("\"SMITH, JANE\"", CsvWriter.EscapeField("SMITH, JANE"));
        }

        [Test]
        public void EscapeField_DoublesEmbeddedQuotes()
        {
            Assert.AreEqual("\"He said \"\"hi\"\"\"", CsvWriter.EscapeField("He said \"hi\""));
        }

        [Test]
        public void EscapeField_QuotesValuesContainingLineBreaks()
        {
            Assert.AreEqual("\"line one\r\nline two\"", CsvWriter.EscapeField("line one\r\nline two"));
            Assert.AreEqual("\"line one\nline two\"", CsvWriter.EscapeField("line one\nline two"));
        }

        [Test]
        public void BuildLine_JoinsAndEscapesEachField()
        {
            string line = CsvWriter.BuildLine(new[] { "Jane", "SMITH, JANE", "said \"hi\"", null });

            Assert.AreEqual("Jane,\"SMITH, JANE\",\"said \"\"hi\"\"\",", line);
        }

        [Test]
        public void BuildCsv_WritesHeadersThenRowsSeparatedByCrLf()
        {
            string csv = CsvWriter.BuildCsv(
                new[] { "Name", "Dojo" },
                new[] { new[] { "Jane", "DENTON" }, new[] { "Bob", "UCO" } });

            Assert.AreEqual("Name,Dojo\r\nJane,DENTON\r\nBob,UCO\r\n", csv);
        }

        [Test]
        public void BuildCsv_WithNoRows_StillWritesHeaders()
        {
            Assert.AreEqual("Name,Dojo\r\n", CsvWriter.BuildCsv(new[] { "Name", "Dojo" }, null));
        }

        #endregion

        #region Rank correction validation

        [Test]
        public void ValidateRankCorrection_WithADifferentRank_Passes()
        {
            Assert.IsTrue(KubkManagementFunctions.ValidateRankCorrection("SANKYU", "NIKYU", out string error));
            Assert.IsEmpty(error);
        }

        [Test]
        public void ValidateRankCorrection_WithNoRankSelected_Fails()
        {
            Assert.IsFalse(KubkManagementFunctions.ValidateRankCorrection("SANKYU", "", out string error));
            Assert.IsNotEmpty(error);
        }

        [TestCase("SANKYU")]
        [TestCase("sankyu")]
        [TestCase("  SANKYU  ")]
        [Test]
        public void ValidateRankCorrection_WithTheSameRank_FailsWithAFriendlyMessage(string newRank)
        {
            // Correcting a rank to what it already is means the reviewer meant to confirm it.
            Assert.IsFalse(KubkManagementFunctions.ValidateRankCorrection("SANKYU", newRank, out string error));
            StringAssert.Contains("Verify Rank", error);
        }

        [Test]
        public void CorrectStudentRank_WhenValid_WritesTheCorrection()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.CorrectStudentRank(42, "Aikido", "SANKYU", "NIKYU", out string error));
            Assert.IsEmpty(error);

            Assert.AreEqual(1, repo.RankCorrections.Count);
            Assert.AreEqual(42, repo.RankCorrections[0].Item1);
            Assert.AreEqual("Aikido", repo.RankCorrections[0].Item2);
            Assert.AreEqual("NIKYU", repo.RankCorrections[0].Item3);
            Assert.AreEqual(DateTime.Today, repo.RankCorrections[0].Item4);
        }

        [Test]
        public void CorrectStudentRank_WhenInvalid_WritesNothing()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.CorrectStudentRank(42, "Aikido", "SANKYU", "SANKYU", out _));
            CollectionAssert.IsEmpty(repo.RankCorrections);
        }

        [Test]
        public void SetStudentActiveStatus_PassesTheFlagThrough()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            functions.SetStudentActiveStatus(42, false);
            functions.SetStudentActiveStatus(43, true);

            CollectionAssert.AreEqual(
                new[] { Tuple.Create(42, false), Tuple.Create(43, true) }, repo.ActiveStatusChanges);
        }

        #endregion

        #region Verification progress

        private static List<KubkRosterEntry> BuildEntries(int verified, int unverified)
        {
            var entries = new List<KubkRosterEntry>();

            for (int i = 0; i < verified; i++)
                entries.Add(new KubkRosterEntry { StudentID = 100 + i, IsActive = true, RankVerifiedDate = DateTime.Today });

            for (int i = 0; i < unverified; i++)
                entries.Add(new KubkRosterEntry { StudentID = 200 + i, IsActive = true, RankVerifiedDate = null });

            return entries;
        }

        [Test]
        public void SummarizeRoster_CountsVerifiedAndTotalRanks()
        {
            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(BuildEntries(verified: 12, unverified: 7));

            Assert.AreEqual(12, summary.VerifiedRanks);
            Assert.AreEqual(19, summary.TotalRanks);
            Assert.AreEqual(7, summary.UnverifiedRanks);
        }

        [Test]
        public void BuildVerificationProgressText_ReadsAsProgress()
        {
            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(BuildEntries(12, 7));

            Assert.AreEqual("DENTON: 12 of 19 ranks verified",
                KubkManagementFunctions.BuildVerificationProgressText("DENTON", summary));
        }

        [Test]
        public void BuildVerificationProgressText_WhenAllVerified_SaysComplete()
        {
            KubkRosterSummary summary = KubkManagementFunctions.SummarizeRoster(BuildEntries(19, 0));

            StringAssert.EndsWith("complete", KubkManagementFunctions.BuildVerificationProgressText("DENTON", summary));
        }

        [Test]
        public void BuildVerificationProgressText_WithNothingToVerify_SaysSo()
        {
            Assert.AreEqual("DENTON: no ranks to verify",
                KubkManagementFunctions.BuildVerificationProgressText("DENTON", KubkManagementFunctions.SummarizeRoster(null)));
            Assert.AreEqual("DENTON: no ranks to verify",
                KubkManagementFunctions.BuildVerificationProgressText("DENTON", null));
        }

        #endregion

        #region Latest recommender lookup

        private static DataTable BuildRecommenderTable()
        {
            var table = new DataTable();
            table.Columns.Add("promo_student", typeof(int));
            table.Columns.Add("promo_art", typeof(string));
            table.Columns.Add("promo_date", typeof(DateTime));
            table.Columns.Add("promo_recommended_by", typeof(string));
            return table;
        }

        private static void AddRecommenderRow(DataTable table, int student, string art, DateTime date, string recommendedBy)
        {
            DataRow row = table.NewRow();
            row["promo_student"] = student;
            row["promo_art"] = art;
            row["promo_date"] = date;
            row["promo_recommended_by"] = (object)recommendedBy ?? DBNull.Value;
            table.Rows.Add(row);
        }

        [Test]
        public void BuildLatestRecommenderLookup_KeepsTheMostRecentPerStudentAndArt()
        {
            DataTable table = BuildRecommenderTable();
            AddRecommenderRow(table, 42, "Aikido", new DateTime(2020, 1, 1), "Old Sensei");
            AddRecommenderRow(table, 42, "Aikido", new DateTime(2026, 1, 1), "New Sensei");
            AddRecommenderRow(table, 42, "Judo", new DateTime(2023, 1, 1), "Judo Sensei");

            Dictionary<string, string> lookup = KubkManagementFunctions.BuildLatestRecommenderLookup(table);

            Assert.AreEqual(2, lookup.Count);
            Assert.AreEqual("New Sensei", lookup["42|AIKIDO"]);
            Assert.AreEqual("Judo Sensei", lookup["42|JUDO"]);
        }

        [Test]
        public void BuildLatestRecommenderLookup_IgnoresRowsWithoutARecommender()
        {
            DataTable table = BuildRecommenderTable();
            AddRecommenderRow(table, 42, "Aikido", new DateTime(2026, 1, 1), null);
            AddRecommenderRow(table, 42, "Aikido", new DateTime(2020, 1, 1), "   ");

            Assert.IsEmpty(KubkManagementFunctions.BuildLatestRecommenderLookup(table));
        }

        [Test]
        public void BuildLatestRecommenderLookup_WithNoTable_ReturnsEmpty()
        {
            // An unmigrated database has no promo_recommended_by column, so the repository
            // returns an empty table rather than failing.
            Assert.IsEmpty(KubkManagementFunctions.BuildLatestRecommenderLookup(null));
            Assert.IsEmpty(KubkManagementFunctions.BuildLatestRecommenderLookup(new DataTable()));
        }

        #endregion

        #region Unpaid dues report

        private static List<KubkRosterEntry> BuildRegister()
        {
            return new List<KubkRosterEntry>
            {
                new KubkRosterEntry { StudentID = 1, FirstName = "Jane", LastName = "Smith", IsActive = true, Dojo = "DENTON", Art = "Aikido" },
                new KubkRosterEntry { StudentID = 1, FirstName = "Jane", LastName = "Smith", IsActive = true, Dojo = "DENTON", Art = "Judo" },
                new KubkRosterEntry { StudentID = 2, FirstName = "Bob", LastName = "Adams", IsActive = true, Dojo = "DENTON", Art = "Aikido" },
                new KubkRosterEntry { StudentID = 3, FirstName = "Cal", LastName = "Jones", IsActive = false, Dojo = "UCO", Art = "Aikido" },
                new KubkRosterEntry { StudentID = 4, FirstName = "Dee", LastName = "Brown", IsActive = true, Dojo = "UCO", Art = "Aikido" }
            };
        }

        [Test]
        public void BuildUnpaidDuesEntries_ExcludesStudentsWhoHavePaid()
        {
            var paid = new Dictionary<int, DateTime?> { { 2, DateTime.Today } };

            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), paid, null, activeStudentsOnly: true);

            CollectionAssert.DoesNotContain(unpaid.Select(e => e.StudentID).ToArray(), 2);
        }

        [Test]
        public void BuildUnpaidDuesEntries_TreatsARowWithNoPaidDateAsUnpaid()
        {
            // A dues row with a null paid date is a record that exists but is not confirmed paid.
            var paid = new Dictionary<int, DateTime?> { { 2, null } };

            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), paid, null, activeStudentsOnly: true);

            CollectionAssert.Contains(unpaid.Select(e => e.StudentID).ToArray(), 2);
        }

        [Test]
        public void BuildUnpaidDuesEntries_ReturnsOneRowPerStudentNotPerArt()
        {
            // Jane is enrolled in two arts but owes one set of dues.
            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), new Dictionary<int, DateTime?>(), null, activeStudentsOnly: true);

            Assert.AreEqual(1, unpaid.Count(e => e.StudentID == 1));
        }

        [Test]
        public void BuildUnpaidDuesEntries_HonoursTheActiveOnlyFilter()
        {
            var noneHavePaid = new Dictionary<int, DateTime?>();

            List<UnpaidDuesEntry> activeOnly = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), noneHavePaid, null, activeStudentsOnly: true);
            List<UnpaidDuesEntry> everyone = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), noneHavePaid, null, activeStudentsOnly: false);

            CollectionAssert.DoesNotContain(activeOnly.Select(e => e.StudentID).ToArray(), 3);
            CollectionAssert.Contains(everyone.Select(e => e.StudentID).ToArray(), 3);
        }

        [Test]
        public void BuildUnpaidDuesEntries_OrdersByDojoThenName()
        {
            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), new Dictionary<int, DateTime?>(), null, activeStudentsOnly: false);

            CollectionAssert.AreEqual(
                new[] { "Adams", "Smith", "Brown", "Jones" },
                unpaid.Select(e => e.LastName).ToArray());
        }

        [Test]
        public void BuildUnpaidDuesEntries_FillsInTheDojoInstructor()
        {
            var instructors = KubkManagementFunctions.BuildInstructorLookup(new List<Dojo>
            {
                new Dojo { ClubID = "DENTON", Instructor = "Denton Sensei" }
            });

            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), new Dictionary<int, DateTime?>(), instructors, activeStudentsOnly: true);

            Assert.AreEqual("Denton Sensei", unpaid.First(e => e.Dojo == "DENTON").Instructor);
            Assert.IsEmpty(unpaid.First(e => e.Dojo == "UCO").Instructor, "A dojo with no instructor should give an empty cell, not null");
        }

        [Test]
        public void BuildUnpaidDuesSubtotals_CountsPerDojo()
        {
            List<UnpaidDuesEntry> unpaid = KubkManagementFunctions.BuildUnpaidDuesEntries(
                BuildRegister(), new Dictionary<int, DateTime?>(), null, activeStudentsOnly: false);

            List<KeyValuePair<string, int>> subtotals = KubkManagementFunctions.BuildUnpaidDuesSubtotals(unpaid);

            Assert.AreEqual(2, subtotals.Count);
            Assert.AreEqual(new KeyValuePair<string, int>("DENTON", 2), subtotals[0]);
            Assert.AreEqual(new KeyValuePair<string, int>("UCO", 2), subtotals[1]);
        }

        #endregion

        #region Rank register mapping

        [Test]
        public void MapRegister_MapsContactDetailsAndVerificationState()
        {
            var table = new DataTable();
            foreach (string column in new[] { "StudentID", "StudentFirstName", "StudentLastName", "StudentStatus",
                "StudentDojo", "StudentEmailAddress", "StudentPrimaryPhone", "Art", "Rank" })
            {
                table.Columns.Add(column, column == "StudentID" ? typeof(int) : typeof(string));
            }
            table.Columns.Add("LastPromotionDate", typeof(DateTime));
            table.Columns.Add("RankVerifiedDate", typeof(DateTime));

            DataRow row = table.NewRow();
            row["StudentID"] = 42;
            row["StudentFirstName"] = "Jane";
            row["StudentLastName"] = "Smith";
            row["StudentStatus"] = "A";
            row["StudentDojo"] = "DENTON";
            row["StudentEmailAddress"] = "jane@example.com";
            row["StudentPrimaryPhone"] = "555-1234";
            row["Art"] = "Aikido";
            row["Rank"] = "SANKYU";
            row["LastPromotionDate"] = new DateTime(2024, 6, 1);
            row["RankVerifiedDate"] = DBNull.Value;
            table.Rows.Add(row);

            List<KubkRosterEntry> entries = KubkManagementFunctions.MapRegister(table);

            Assert.AreEqual(1, entries.Count);
            Assert.AreEqual("jane@example.com", entries[0].EmailAddress);
            Assert.AreEqual("555-1234", entries[0].PhoneNumber);
            Assert.IsFalse(entries[0].RankIsVerified);
            Assert.AreEqual("Unverified", entries[0].RankVerifiedDisplay);
        }

        [Test]
        public void MapRegister_WithNoTable_ReturnsEmpty()
        {
            Assert.IsEmpty(KubkManagementFunctions.MapRegister(null));
            Assert.IsEmpty(KubkManagementFunctions.MapRegister(new DataTable()));
        }

        [Test]
        public void GetRankRegister_AttachesTheLatestRecommenderToEachRow()
        {
            var repo = new FakeKubkDataRepository { RankRegister = BuildRegisterTable() };
            repo.PromotionRecommenders = BuildRecommenderTable();
            AddRecommenderRow(repo.PromotionRecommenders, 42, "Aikido", new DateTime(2020, 1, 1), "Old Sensei");
            AddRecommenderRow(repo.PromotionRecommenders, 42, "Aikido", new DateTime(2026, 1, 1), "New Sensei");

            var functions = new KubkManagementFunctions(repo);
            List<KubkRosterEntry> register = functions.GetRankRegister(null, includeWindsong: false);

            Assert.AreEqual(2, register.Count);
            Assert.AreEqual("New Sensei", register.First(e => e.Art == "Aikido").RecommendedBy);
            Assert.IsNull(register.First(e => e.Art == "Judo").RecommendedBy,
                "An art with no recommended promotion should leave the column empty");
        }

        private static DataTable BuildRegisterTable()
        {
            var table = new DataTable();
            foreach (string column in new[] { "StudentFirstName", "StudentLastName", "StudentStatus",
                "StudentDojo", "StudentEmailAddress", "StudentPrimaryPhone", "Art", "Rank" })
            {
                table.Columns.Add(column, typeof(string));
            }
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("LastPromotionDate", typeof(DateTime));
            table.Columns.Add("RankVerifiedDate", typeof(DateTime));

            foreach (string art in new[] { "Aikido", "Judo" })
            {
                DataRow row = table.NewRow();
                row["StudentID"] = 42;
                row["StudentFirstName"] = "Jane";
                row["StudentLastName"] = "Smith";
                row["StudentStatus"] = "A";
                row["StudentDojo"] = "DENTON";
                row["StudentEmailAddress"] = "jane@example.com";
                row["StudentPrimaryPhone"] = "555-1234";
                row["Art"] = art;
                row["Rank"] = "SANKYU";
                row["LastPromotionDate"] = DBNull.Value;
                row["RankVerifiedDate"] = DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        #endregion
    }
}
