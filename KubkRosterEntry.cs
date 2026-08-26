using System;
using System.Collections.Generic;

namespace DojoStudentManagement
{
    /// <summary>
    /// One row of the KUBK roster: a single (student, art) enrollment together with its
    /// rank-verification state and the dues status for the roster's selected year.
    /// </summary>
    public class KubkRosterEntry
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string Dojo { get; set; }
        public string Art { get; set; }
        public string Rank { get; set; }
        public DateTime? LastPromotionDate { get; set; }
        public DateTime? RankVerifiedDate { get; set; }
        public DateTime? DuesPaidDate { get; set; }

        /// <summary>
        /// Instructor who recommended the most recent promotion, when one is recorded. Populated
        /// for the rank register; the roster screen leaves it null.
        /// </summary>
        public string RecommendedBy { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool RankIsVerified => RankVerifiedDate.HasValue;
        public bool DuesArePaid => DuesPaidDate.HasValue;

        public string RankVerifiedDisplay =>
            RankIsVerified ? RankVerifiedDate.Value.ToString("MM/dd/yyyy") : "Unverified";

        /// <summary>
        /// Years since the last promotion. Delegates to StudentArtsAndRank.YearsAtCurrentLevel()
        /// rather than repeating the 365.25-day math: with both dates set to the promotion date,
        /// that method returns time since promotion, or 0 when there is no promotion on record.
        /// </summary>
        public double YearsAtRank()
        {
            var art = new StudentArtsAndRank
            {
                DateStarted = LastPromotionDate,
                DatePromoted = LastPromotionDate
            };

            return art.YearsAtCurrentLevel();
        }
    }

    /// <summary>
    /// Counts displayed on the roster screen's status line.
    /// </summary>
    public class KubkRosterSummary
    {
        public int StudentsShown { get; set; }
        public int UnverifiedRanks { get; set; }
        public int StudentsUnpaid { get; set; }

        /// <summary>Rank rows already verified, out of TotalRanks - re-verification progress.</summary>
        public int VerifiedRanks { get; set; }
        public int TotalRanks { get; set; }
    }
}
