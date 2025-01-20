using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";

            }
        }
        public string HomeDojo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZip { get; set; }
        public bool ActiveMember { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public Gender StudentGender { get; set; }
        public string EmailAddress { get; set; }
        public int StartMonth { get; set; }
        public double StudentAgeInYears
        {
            get
            {
                const double daysInYear = 365.25;
                TimeSpan span = DateTime.Now - DateOfBirth;
                return Math.Round(span.TotalDays / daysInYear, 2);
            }
        }

        internal List<StudentArtsAndRank> StudentArtsAndRanks = new List<StudentArtsAndRank>();

        public Student()
        {

        }

        public bool IsEligibleForPromotion(StudentArtsAndRank art, PromotionCriteria eligibility)
        { 
            if (StudentHasReachedMinimumAgeForPromotion(eligibility) == false)
                return false;

            if (StudentHasSufficientTrainingHoursForPromotion(art.HoursInArt, eligibility) == false)
                return false;

            if (StudentHasSufficientTimeInArtForPromotion(art.TotalYearsInArt(), eligibility) == false)
                return false;

            if (StudentHasSufficientTimeInArtForPromotion(art.YearsAtCurrentLevel(), eligibility) == false)
                return false;

            //Fixes bug where the function was erroneously returning true if a match for the student's current rank 
            //could not be found in the eligibility table. This should also fix the edge case where a student has
            //achieved the maximum rank and there are no further promotions available.
            if (string.IsNullOrWhiteSpace(eligibility.NextRank))
                return false;

            return true;
        }

        public bool StudentHasReachedMinimumAgeForPromotion(PromotionCriteria eligibility)
        {
            if (StudentAgeInYears < eligibility.MinimumAge)
                return false;

            return true;
        }

        public bool StudentHasSufficientTrainingHoursForPromotion(double hoursInArt, PromotionCriteria eligibility)
        {
            if (hoursInArt < eligibility.MinimumTrainingHours)
                return false;

            return true;
        }

        public bool StudentHasSufficientTimeInArtForPromotion(double totalYearsInArt, PromotionCriteria eligibility)
        {
            if (totalYearsInArt < eligibility.YearsInArt)
                return false;

            return true;
        }

        public bool StudentHasSufficientTimeAtCurrentRank(double totalYearsAtCurrentRank, PromotionCriteria eligibility)
        {
            if (totalYearsAtCurrentRank < eligibility.YearsAtCurrentRank)
                return false;

            return true;
        }
    }

    public enum Gender
    {
        FEMALE,
        MALE,
        UNKNOWN
    }

}
