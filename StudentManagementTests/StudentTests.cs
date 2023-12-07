using NUnit.Framework;
using DojoStudentManagement;
namespace DojoStudentManagementTests
{
    class StudentTests
    {
        Student student1;

        [SetUp]
        public void Setup()
        {
            student1 = new Student();
            student1.DateOfBirth = new System.DateTime(1979, 7, 11);
        }

        [Test]
        [TestCase("Aikido", "WHITE", 40, 40)]
        [TestCase("Aikido", "YONKYU", 80, 80)]
        [TestCase("Aikido", "SANKYU", 120, 120)]
        [TestCase("Aikido", "NIKYU", 160, 160)]
        [TestCase("Aikido", "IKKYU", 250, 250)]
        [TestCase("Aikido", "SHODAN", 500, 500)]
        public void IsEligibleForPromotion_WithSufficientTrainingHours_ReturnsTrue(string art, string rank, int hours, int eligibilityHours)
        {
            StudentArtsAndRank eligibleStudentArts = CreateEligibleStudentArts(art, rank, hours);
            PromotionCriteria eligibilityCriteria = CreatePromotionCriteria(eligibilityHours);

            bool result = student1.IsEligibleForPromotion(eligibleStudentArts, eligibilityCriteria);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Aikido", "WHITE", 39, 40)]
        [TestCase("Aikido", "YONKYU", 79, 80)]
        [TestCase("Aikido", "SANKYU", 119, 120)]
        [TestCase("Aikido", "NIKYU", 159, 160)]
        [TestCase("Aikido", "IKKYU", 249, 250)]
        [TestCase("Aikido", "SHODAN", 499, 500)]
        public void IsEligibleForPromotion_WhenNotEligibleBasedOnHours_ShouldReturnFalse(string art, string rank, int hours, int eligibilityHours)
        {
            StudentArtsAndRank eligibleStudentArts = CreateEligibleStudentArts(art, rank, hours);
            PromotionCriteria eligibilityCriteria = CreatePromotionCriteria(eligibilityHours);

            bool result = student1.IsEligibleForPromotion(eligibleStudentArts, eligibilityCriteria);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsEligibleForPromotion_MartialArtDoesNotHavePromotionCriteria_ShouldReturnFalse()
        {
            StudentArtsAndRank invalidStudentArt = new StudentArtsAndRank
            {
                StudentArt = "Underwater Basketweaving",
                HoursInArt = 0
            };
            PromotionCriteria eligibilityCriteria = CreatePromotionCriteria(5); //5 = hours in art

            bool result = student1.IsEligibleForPromotion(invalidStudentArt, eligibilityCriteria);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsEligibleForPromotion_CurrentRankIsInvalid_ShouldReturnFalse()
        {
            StudentArtsAndRank invalidStudentArt = new StudentArtsAndRank
            {
                StudentArt = "Judo",
                Rank = "NINJA",
                HoursInArt = 40
            };
            PromotionCriteria eligibilityCriteria = CreatePromotionCriteria(5); //5 = hours in art

            bool result = student1.IsEligibleForPromotion(invalidStudentArt, eligibilityCriteria);

            Assert.IsFalse(result);
        }


        //Helper methods for test data creation
        private StudentArtsAndRank CreateEligibleStudentArts(string art, string rank, int hours)
        {
            // Set up a student that is eligible based on age and total time in art. Only variable 
            // is training hours
            StudentArtsAndRank eligibleStudentArts = new StudentArtsAndRank
            {
                StudentArt = art,
                Rank = rank,
                HoursInArt = hours,

                //Choose start/promotion/hours critera to ensure test case will 
                //pass successfully no matter what rank
                DateStarted = new System.DateTime(1900, 01, 01),
                DatePromoted = new System.DateTime(1900, 01, 01),
                PromotionHours = 10000
            };

            return eligibleStudentArts;
        }

       /* private StudentArtsAndRank CreateIneligibleStudentArts(string art, string rank, int hours)
        {
            // Set up a student that is ineligible based on age and total time in art. Only variable 
            // is training hours
            StudentArtsAndRank ineligibleStudentArts = new StudentArtsAndRank
            {
                StudentArt = art,
                Rank = rank,
                DateStarted = new System.DateTime(2021, 01, 01),
                HoursInArt = 100,
                DatePromoted = new System.DateTime(2021, 01, 15),
                PromotionHours = 5
            };

            return ineligibleStudentArts;
        }
       */

        private PromotionCriteria CreatePromotionCriteria(int eligibilityHours)
        {
            PromotionCriteria eligibilityCriteria = new PromotionCriteria
            {
                MinimumTrainingHours = eligibilityHours,

                //These values ignore the age / time in art promotion criteria.\
                MinimumAge = 1,
                YearsInArt = 1,
                YearsAtCurrentRank = 0.25
            };

            return eligibilityCriteria;
        }
    }
}
