using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    class Student
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

        public List<StudentArtsAndRank> StudentArtsAndRanks = new List<StudentArtsAndRank>();
    }

    public enum Gender
    {
        FEMALE,
        MALE,
        UNKNOWN
    }

}
