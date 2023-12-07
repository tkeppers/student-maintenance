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
        private string StartMonth;

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
            //BUG: This function is returning true if the user has a rank that isn't recognized by the system. For example, 
            //the dojo used to have a rank in Judo called GOKYU. With current business rules, we consider any rank below 
            //YONKYU to just be "WHITE" (white belt). However, there are still a few students in the system with such ranks. 
            //The real solution is to change their rank in the database. However, need to put in a patch to NOT return 
            //true in these instances. 
            if (StudentAgeInYears() < eligibility.MinimumAge)
                return false;

            if (art.HoursInArt < eligibility.MinimumTrainingHours)
                return false;

            if (art.TotalYearsInArt() < eligibility.YearsInArt)
                return false;

            if (art.YearsAtCurrentLevel() < eligibility.YearsAtCurrentRank)
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
