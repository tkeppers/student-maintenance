using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class Student
    {
        public int StudentID;
        public string FirstName;
        public string LastName;
        public string HomeDojo;
        public DateTime DateOfBirth;
        public string Address1;
        public string Address2;
        public string AddressCity;
        public string AddressState;
        public string AddressZip;
        public bool ActiveMember;

        public string PrimaryPhoneNumber;
        public string SecondaryPhoneNumber;
        public Gender StudentGender;
        public string EmailAddress;
        public int StartMonth;

        internal List<StudentArtsAndRank> StudentArtsAndRanks = new List<StudentArtsAndRank>();

        public Student()
        {

        }

        private double StudentAgeInYears()
        {
            TimeSpan span = DateTime.Now - DateOfBirth;
            double studentAgeInYears = Math.Round(span.TotalDays / 365.25, 2); // Using 365.25 to account for leap years

            return studentAgeInYears;
        }

        public bool IsEligibleForPromotion(StudentArtsAndRank art, PromotionCriteria eligibility)
        { 
            if (StudentAgeInYears() < eligibility.MinimumAge)
                return false;

            if (art.HoursInArt < eligibility.MinimumTrainingHours)
                return false;

            if (art.TotalYearsInArt() < eligibility.YearsInArt)
                return false;

            if (art.YearsAtCurrentLevel() < eligibility.YearsAtCurrentRank)
                return false;

            //Fixes bug where the function was erroneously returning true if a match for the student's current rank 
            //could not be found in the eligibility table. This should also fix the edge case where a student has
            //achieved the maximum rank and there are no further promotions available.
            if (string.IsNullOrWhiteSpace(eligibility.NextRank))
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
