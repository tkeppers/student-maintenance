using System;

namespace DojoStudentManagement
{
    public class StudentDuesRecord
    {
        public int StudentID { get; set; }
        public int Year { get; set; }
        public DateTime? PaidDate { get; set; }

        /// <summary>
        /// What was paid, or null when the amount is not tracked here. The roster screen only
        /// confirms that dues were paid - the dojo records the money in its own accounting
        /// system - so null is the normal case and is stored as NULL rather than as a
        /// misleading 0.00.
        /// </summary>
        public decimal? Amount { get; set; }

        public bool IsPaid => PaidDate.HasValue;
    }
}
