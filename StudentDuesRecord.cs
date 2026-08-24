using System;

namespace DojoStudentManagement
{
    public class StudentDuesRecord
    {
        public int StudentID { get; set; }
        public int Year { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal Amount { get; set; }

        public bool IsPaid => PaidDate.HasValue;
    }
}
