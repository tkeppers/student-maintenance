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
        public void IsEligibleForPromotion_WhenEligible_ShouldReturnTrue(string art, string rank, int hours, int eligibilityHours)
        {
            // Set up a student that is eligible based on age and total time in art. Only variable 
            // is training hours
            var eligibleStudentArts = new StudentArtsAndRank
            {
                StudentArt = art,
                Rank = rank,
                DateStarted = new System.DateTime(1900, 01, 01),
                HoursInArt = hours,
                DatePromoted = new System.DateTime(1900, 01, 01),
                PromotionHours = 10000
            };

            // Create a PromotionRequirements object with eligibility criteria
            var eligibilityCriteria = new PromotionRequirements
            {
                MinimumAge = 18,
                MinimumTrainingHours = eligibilityHours,
                YearsInArt = 1,
                YearsAtCurrentRank = 0.25
            };

            // Act
            var result = student1.IsEligibleForPromotion(eligibleStudentArts, eligibilityCriteria);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsEligibleForPromotion_WhenNotEligible_ShouldReturnFalse()
        {
            // Create a StudentArtsAndRank object with insufficient data for eligibility
            var notEligibleStudentArts = new StudentArtsAndRank
            {
                StudentArt = "Aikido",
                Rank = "IKKYU",
                DateStarted = new System.DateTime(2021, 01, 01),
                HoursInArt = 100,
                DatePromoted = new System.DateTime(2021, 01, 15),
                PromotionHours = 251
            };

            // Create a PromotionRequirements object with eligibility criteria
            var eligibilityCriteria = new PromotionRequirements
            {
                MinimumAge = 18,
                MinimumTrainingHours = 250,
                YearsInArt = 1,
                YearsAtCurrentRank = 0.25
            };

            // Act
            var result = student1.IsEligibleForPromotion(notEligibleStudentArts, eligibilityCriteria);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
