using System;

namespace DojoStudentManagement
{
    /// <summary>
    /// One (student, art) row of the student activity report: where a student stands in an art,
    /// when they last trained, and whether they have met the promotion criteria.
    /// </summary>
    public class StudentActivityEntry
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Art { get; set; }
        public string Rank { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public double HoursInArt { get; set; }

        public bool IsEligibleForPromotion { get; set; }

        /// <summary>The rank they would move to, when the criteria table knows of one.</summary>
        public string NextRank { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string LastSignInDisplay =>
            LastSignInDate.HasValue ? LastSignInDate.Value.ToString("MM/dd/yyyy") : "Never";

        /// <summary>
        /// Reads as "Yes - SHODAN" so the reader can see what the student is eligible for, not
        /// just that they are.
        /// </summary>
        public string EligibilityDisplay
        {
            get
            {
                if (!IsEligibleForPromotion)
                    return "No";

                return string.IsNullOrWhiteSpace(NextRank) ? "Yes" : $"Yes - {NextRank}";
            }
        }
    }
}
