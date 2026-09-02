using NUnit.Framework;
using DojoStudentManagement;
using System;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for StudentArtsAndRank — specifically the YearsAtCurrentLevel() fix that previously
    /// returned 0 when DatePromoted was null (new students), instead of falling back to TotalYearsInArt().
    /// </summary>
    [TestFixture]
    public class StudentArtsAndRankTests
    {
        [Test]
        public void YearsAtCurrentLevel_WhenDatePromotedIsNull_ReturnsTotalYearsInArt()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = DateTime.Now.AddYears(-2),
                DatePromoted = null
            };

            double yearsAtLevel = art.YearsAtCurrentLevel();
            double yearsInArt = art.TotalYearsInArt();

            Assert.AreEqual(yearsInArt, yearsAtLevel, 0.01,
                "A student who has never been promoted should report time-at-level equal to total time in art");
        }

        [Test]
        public void YearsAtCurrentLevel_WhenDatePromotedIsNull_IsNotZero()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = DateTime.Now.AddYears(-1),
                DatePromoted = null
            };

            Assert.Greater(art.YearsAtCurrentLevel(), 0,
                "A student who has never been promoted but has been training should not report 0 years at level");
        }

        [Test]
        public void YearsAtCurrentLevel_WhenDateStartedAndDatePromotedAreBothNull_ReturnsZero()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = null,
                DatePromoted = null
            };

            Assert.AreEqual(0, art.YearsAtCurrentLevel(),
                "With no start date and no promotion date there is no time to report");
        }

        [Test]
        public void YearsAtCurrentLevel_WhenDatePromotedIsSet_ReflectsTimeSincePromotion()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = DateTime.Now.AddYears(-3),
                DatePromoted = DateTime.Now.AddYears(-1)
            };

            double yearsAtLevel = art.YearsAtCurrentLevel();

            Assert.AreEqual(1.0, yearsAtLevel, 0.05,
                "Years at current level should reflect time since promotion date, not time since start");
        }

        [Test]
        public void YearsAtCurrentLevel_WhenPromotedRecentlyAndStartedLongAgo_IsLessThanTotalYearsInArt()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = DateTime.Now.AddYears(-5),
                DatePromoted = DateTime.Now.AddMonths(-6)
            };

            Assert.Less(art.YearsAtCurrentLevel(), art.TotalYearsInArt(),
                "Years at current rank must be less than total years in art when student was promoted after starting");
        }

        [Test]
        public void TotalYearsInArt_WhenDateStartedIsNull_ReturnsZero()
        {
            var art = new StudentArtsAndRank { DateStarted = null };

            Assert.AreEqual(0, art.TotalYearsInArt());
        }

        [Test]
        public void TotalYearsInArt_WhenStartedTwoYearsAgo_ReturnsApproximatelyTwo()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = DateTime.Now.AddYears(-2)
            };

            Assert.AreEqual(2.0, art.TotalYearsInArt(), 0.05);
        }

        /// <summary>
        /// Verifies that the promotion eligibility check (which calls YearsAtCurrentLevel) does not
        /// incorrectly pass a new student through the time-at-rank gate when DatePromoted is null.
        /// Before the fix, YearsAtCurrentLevel() returned 0, which is less than any threshold > 0,
        /// making the check fail for new students even when they should qualify.
        /// </summary>
        [Test]
        public void IsEligibleForPromotion_NewStudentWithNullDatePromoted_IsNotBlockedByTimeAtRankGate()
        {
            var student = new Student { DateOfBirth = DateTime.Now.AddYears(-30) };

            var art = new StudentArtsAndRank
            {
                StudentArt = "Aikido",
                Rank = "WHITE",
                NextRank = "YONKYU",
                HoursInArt = 500,
                DateStarted = DateTime.Now.AddYears(-2),
                DatePromoted = null   // brand-new student, never promoted
            };

            var criteria = new PromotionCriteria
            {
                MinimumTrainingHours = 40,
                MinimumAge = 1,
                YearsInArt = 0.5,
                YearsAtCurrentRank = 0.25,
                NextRank = "YONKYU"
            };

            bool eligible = student.IsEligibleForPromotion(art, criteria);

            Assert.IsTrue(eligible,
                "A student who has trained long enough should not be blocked by a null DatePromoted");
        }

        [Test]
        public void RankIsVerified_WhenRankVerifiedDateIsSet_ReturnsTrue()
        {
            var art = new StudentArtsAndRank { RankVerifiedDate = DateTime.Now };

            Assert.IsTrue(art.RankIsVerified);
        }

        [Test]
        public void RankIsVerified_WhenRankVerifiedDateIsNull_ReturnsFalse()
        {
            var art = new StudentArtsAndRank { RankVerifiedDate = null };

            Assert.IsFalse(art.RankIsVerified);
        }
    }
}
