using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for the KUBK promotion flow added in Phase 4: next-rank defaulting from the rank
    /// ladder, promotion validation (which has no eligibility gates, unlike the Windsong flow),
    /// enrollment lookup, and the recording of a promotion.
    /// </summary>
    [TestFixture]
    public class KubkPromotionTests
    {
        private static List<Rank> BuildLadder()
        {
            return new List<Rank>
            {
                new Rank { RankID = "WHITE", RankOrder = 1, RankNext = "YONKYU" },
                new Rank { RankID = "YONKYU", RankOrder = 2, RankNext = "SANKYU" },
                new Rank { RankID = "SANKYU", RankOrder = 3, RankNext = "NIKYU" },
                new Rank { RankID = "HACHIDAN", RankOrder = 13, RankNext = "NA" }
            };
        }

        #region Rank ladder

        [Test]
        public void HasNextRank_IsFalseAtTheTopOfTheLadder()
        {
            Assert.IsFalse(new Rank { RankID = "HACHIDAN", RankNext = "NA" }.HasNextRank);
            Assert.IsFalse(new Rank { RankID = "HACHIDAN", RankNext = "na" }.HasNextRank);
            Assert.IsFalse(new Rank { RankID = "HACHIDAN", RankNext = "  " }.HasNextRank);
            Assert.IsFalse(new Rank { RankID = "HACHIDAN", RankNext = null }.HasNextRank);
            Assert.IsTrue(new Rank { RankID = "WHITE", RankNext = "YONKYU" }.HasNextRank);
        }

        [TestCase("WHITE", "YONKYU")]
        [TestCase("yonkyu", "SANKYU")]
        [TestCase("  SANKYU  ", "NIKYU")]
        [Test]
        public void GetDefaultNextRank_ReturnsTheNextRungOfTheLadder(string currentRank, string expected)
        {
            Assert.AreEqual(expected, KubkManagementFunctions.GetDefaultNextRank(BuildLadder(), currentRank));
        }

        [Test]
        public void GetDefaultNextRank_AtTopOfLadder_ReturnsEmpty()
        {
            // HACHIDAN has no promotion above it, so the dialog offers no pre-selection.
            Assert.IsEmpty(KubkManagementFunctions.GetDefaultNextRank(BuildLadder(), "HACHIDAN"));
        }

        [Test]
        public void GetDefaultNextRank_ForUnknownOrMissingRank_ReturnsEmpty()
        {
            Assert.IsEmpty(KubkManagementFunctions.GetDefaultNextRank(BuildLadder(), "NOT_A_RANK"));
            Assert.IsEmpty(KubkManagementFunctions.GetDefaultNextRank(BuildLadder(), null));
            Assert.IsEmpty(KubkManagementFunctions.GetDefaultNextRank(BuildLadder(), "   "));
            Assert.IsEmpty(KubkManagementFunctions.GetDefaultNextRank(null, "WHITE"));
        }

        #endregion

        #region Validation

        [Test]
        public void ValidateKubkPromotion_WithCompleteEntry_Passes()
        {
            Assert.IsTrue(KubkManagementFunctions.ValidateKubkPromotion(
                "SANKYU", DateTime.Today, "Jane Sensei", out string error));
            Assert.IsEmpty(error);
        }

        [Test]
        public void ValidateKubkPromotion_WithoutARank_Fails()
        {
            Assert.IsFalse(KubkManagementFunctions.ValidateKubkPromotion(
                "", DateTime.Today, "Jane Sensei", out string error));
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateKubkPromotion_WithoutARecommender_Fails()
        {
            // The recommending instructor is the whole basis for a KUBK promotion, so it is
            // the one field that is genuinely required.
            Assert.IsFalse(KubkManagementFunctions.ValidateKubkPromotion(
                "SANKYU", DateTime.Today, "   ", out string error));
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateKubkPromotion_WithAFutureDate_Fails()
        {
            Assert.IsFalse(KubkManagementFunctions.ValidateKubkPromotion(
                "SANKYU", DateTime.Today.AddDays(30), "Jane Sensei", out string error));
            StringAssert.Contains("future", error);
        }

        [Test]
        public void ValidateKubkPromotion_AllowsTodayAndYesterdayAndOneDayOfSlack()
        {
            // A day of slack absorbs clock skew so a same-day entry is never rejected.
            Assert.IsTrue(KubkManagementFunctions.ValidateKubkPromotion("SANKYU", DateTime.Today, "J", out _));
            Assert.IsTrue(KubkManagementFunctions.ValidateKubkPromotion("SANKYU", DateTime.Today.AddDays(-1), "J", out _));
            Assert.IsTrue(KubkManagementFunctions.ValidateKubkPromotion("SANKYU", DateTime.Today.AddDays(1), "J", out _));
        }

        [Test]
        public void ValidateKubkPromotion_DoesNotGateOnHoursAgeOrTimeInGrade()
        {
            // A brand-new student with no history can be promoted on recommendation alone -
            // this is the core difference from the Windsong flow.
            Assert.IsTrue(KubkManagementFunctions.ValidateKubkPromotion(
                "HACHIDAN", DateTime.Today, "Jane Sensei", out _));
        }

        #endregion

        #region GetStudentArtEnrollment

        private static DataTable BuildStudentArtsTable(params string[] arts)
        {
            var table = new DataTable();
            table.Columns.Add("StudArt_ID", typeof(int));
            table.Columns.Add("studArt_art", typeof(string));
            table.Columns.Add("studArt_rank", typeof(string));
            table.Columns.Add("studArt_begin", typeof(DateTime));
            table.Columns.Add("studArt_cumm", typeof(double));
            table.Columns.Add("studArt_signin", typeof(DateTime));
            table.Columns.Add("studArt_prodate", typeof(DateTime));
            table.Columns.Add("studArt_prohrs", typeof(double));
            table.Columns.Add("studArt_rank_verified", typeof(DateTime));

            foreach (string art in arts)
            {
                DataRow row = table.NewRow();
                row["StudArt_ID"] = 42;
                row["studArt_art"] = art;
                row["studArt_rank"] = "SANKYU";
                row["studArt_begin"] = new DateTime(2020, 1, 1);
                row["studArt_cumm"] = 250.0;
                row["studArt_signin"] = DBNull.Value;
                row["studArt_prodate"] = new DateTime(2024, 6, 1);
                row["studArt_prohrs"] = 120.0;
                row["studArt_rank_verified"] = DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        [Test]
        public void GetStudentArtEnrollment_WhenEnrolled_MapsTheEnrollment()
        {
            var repo = new FakeKubkDataRepository { StudentArts = BuildStudentArtsTable("Aikido") };
            var functions = new KubkManagementFunctions(repo);

            StudentArtsAndRank enrollment = functions.GetStudentArtEnrollment(42, "Aikido");

            Assert.IsNotNull(enrollment);
            Assert.AreEqual(42, enrollment.StudentArtID);
            Assert.AreEqual("Aikido", enrollment.StudentArt);
            Assert.AreEqual("SANKYU", enrollment.Rank);
            Assert.AreEqual(250.0, enrollment.HoursInArt);
            Assert.AreEqual(new DateTime(2024, 6, 1), enrollment.DatePromoted);
            Assert.IsFalse(enrollment.RankIsVerified, "A null verified date should map to null, not a default date");
        }

        [Test]
        public void GetStudentArtEnrollment_MatchesArtNameCaseInsensitively()
        {
            var repo = new FakeKubkDataRepository { StudentArts = BuildStudentArtsTable("Aikido") };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsNotNull(functions.GetStudentArtEnrollment(42, "aikido"));
        }

        [Test]
        public void GetStudentArtEnrollment_WhenNotEnrolled_ReturnsNull()
        {
            var repo = new FakeKubkDataRepository { StudentArts = BuildStudentArtsTable("Aikido") };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsNull(functions.GetStudentArtEnrollment(42, "Judo"));
            Assert.IsNull(functions.GetStudentArtEnrollment(42, null));
        }

        #endregion

        #region RecordKubkPromotion

        private static StudentArtsAndRank BuildEnrollment()
        {
            return new StudentArtsAndRank
            {
                StudentArtID = 42,
                StudentArt = "Aikido",
                Rank = "SANKYU",
                HoursInArt = 250,
                DateStarted = new DateTime(2020, 1, 1)
            };
        }

        [Test]
        public void RecordKubkPromotion_WritesTheNewRankDateAndRecommender()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);
            var art = BuildEnrollment();
            var promotionDate = new DateTime(2026, 3, 14);

            Assert.IsTrue(functions.RecordKubkPromotion(art, "NIKYU", promotionDate, "Jane Sensei", out string error));
            Assert.IsEmpty(error);

            Assert.AreEqual(1, repo.Promotions.Count);
            var recorded = repo.Promotions[0];
            Assert.AreEqual(42, recorded.Item1, "The promotion should be recorded against the student id");
            Assert.AreEqual("NIKYU", recorded.Item2.NextRank, "UpdateStudentPromotion reads the new rank off NextRank");
            Assert.AreEqual(promotionDate, recorded.Item2.DatePromoted);
            Assert.AreEqual("Jane Sensei", recorded.Item3);
        }

        [Test]
        public void RecordKubkPromotion_TrimsTheRankAndRecommender()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            functions.RecordKubkPromotion(BuildEnrollment(), "  NIKYU  ", DateTime.Today, "  Jane Sensei  ", out _);

            Assert.AreEqual("NIKYU", repo.Promotions[0].Item2.NextRank);
            Assert.AreEqual("Jane Sensei", repo.Promotions[0].Item3);
        }

        [Test]
        public void RecordKubkPromotion_WhenValidationFails_WritesNothing()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.RecordKubkPromotion(BuildEnrollment(), "NIKYU", DateTime.Today, "", out string error));
            Assert.IsNotEmpty(error);
            CollectionAssert.IsEmpty(repo.Promotions);
        }

        [Test]
        public void RecordKubkPromotion_WithNoArt_Fails()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.RecordKubkPromotion(null, "NIKYU", DateTime.Today, "Jane", out string error));
            Assert.IsNotEmpty(error);
            CollectionAssert.IsEmpty(repo.Promotions);
        }

        [Test]
        public void RecordKubkPromotion_PropagatesRepositoryFailure()
        {
            var repo = new FakeKubkDataRepository { WritesSucceed = false };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.RecordKubkPromotion(BuildEnrollment(), "NIKYU", DateTime.Today, "Jane", out string error));
            Assert.IsNotEmpty(error);
        }

        #endregion

        #region EnrollStudentInArt

        [Test]
        public void EnrollStudentInArt_CreatesTheEnrollmentAtTheGivenRankAndDate()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);
            var startDate = new DateTime(2026, 5, 1);

            Assert.IsTrue(functions.EnrollStudentInArt(42, "Judo", "WHITE", startDate));

            Assert.AreEqual(1, repo.AddedArts.Count);
            StudentArtsAndRank added = repo.AddedArts[0];
            Assert.AreEqual(42, added.StudentArtID);
            Assert.AreEqual("Judo", added.StudentArt);
            Assert.AreEqual("WHITE", added.Rank);
            Assert.AreEqual(startDate, added.DateStarted);
            Assert.AreEqual(0, added.HoursInArt, "A new enrollment starts with no training hours");
        }

        [Test]
        public void EnrollStudentInArt_PropagatesRepositoryFailure()
        {
            var repo = new FakeKubkDataRepository { WritesSucceed = false };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.EnrollStudentInArt(42, "Judo", "WHITE", DateTime.Today));
        }

        [Test]
        public void RemoveStudentArtEnrollment_DeletesJustThatArt()
        {
            // Backs out an enrollment created for a promotion that then failed, so a failed
            // promotion leaves no trace.
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.RemoveStudentArtEnrollment(42, "Judo"));

            CollectionAssert.AreEqual(new[] { Tuple.Create(42, "Judo") }, repo.DeletedArts);
        }

        [Test]
        public void RemoveStudentArtEnrollment_PropagatesRepositoryFailure()
        {
            var repo = new FakeKubkDataRepository { WritesSucceed = false };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.RemoveStudentArtEnrollment(42, "Judo"));
        }

        #endregion
    }
}
