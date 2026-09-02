using System;

namespace DojoStudentManagement
{
    /// <summary>
    /// One sign-in event: a student attending one art on one occasion. Unlike the activity
    /// report, which shows a student's latest sign-in per art, this is every individual
    /// attendance record.
    /// </summary>
    public class SignInHistoryEntry
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Art { get; set; }
        public DateTime SignInDate { get; set; }

        /// <summary>Hours credited for this one session.</summary>
        public double SessionHours { get; set; }

        /// <summary>
        /// The student's total hours in this art as of this sign-in, session included. Null when
        /// it cannot be established - the student is no longer enrolled in the art, so there is
        /// no recorded total to work back from.
        /// </summary>
        public double? CumulativeHours { get; set; }

        public string CumulativeHoursDisplay =>
            CumulativeHours.HasValue ? CumulativeHours.Value.ToString("0.#") : string.Empty;

        /// <summary>
        /// The student's rank in this art now, carried over from their current enrollment. Blank
        /// when they are no longer enrolled in the art they signed in to.
        /// </summary>
        public string Rank { get; set; }

        public bool IsEligibleForPromotion { get; set; }
        public string NextRank { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string SignInDisplay => SignInDate.ToString("MM/dd/yyyy h:mm tt");

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
