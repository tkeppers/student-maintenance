using System;

namespace DojoStudentManagement
{
    /// <summary>
    /// One member-dojo student with no dues recorded for the report's year. Carries the student's
    /// contact details and the dojo's instructor so renewals can actually be chased.
    /// </summary>
    public class UnpaidDuesEntry
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Dojo { get; set; }
        public string Instructor { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
