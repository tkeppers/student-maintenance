using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagement
{
    /// <summary>
    /// Assembles the sign-in history report. Rank and promotion eligibility describe the
    /// student's position today and are looked up from their current enrollment, so they repeat
    /// across that student's sign-in rows rather than being reconstructed as of each session.
    /// </summary>
    public class SignInHistoryReportFunctions
    {
        private readonly IDataRepository dataRepository;
        private readonly StudentActivityReportFunctions activityFunctions;

        public SignInHistoryReportFunctions(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
            activityFunctions = new StudentActivityReportFunctions(dataRepository);
        }

        public List<SignInHistoryEntry> GetSignInHistory(string clubId, DateTime fromDate, DateTime toDate, bool activeStudentsOnly)
        {
            // The running total is worked backwards from each student's recorded total today, so
            // every sign-in between the end of the requested period and now has to be loaded
            // too - otherwise the totals would be overstated by whatever came after.
            DateTime queryEnd = toDate.Date > DateTime.Today ? toDate.Date : DateTime.Today;

            DataTable signIns = dataRepository.GetSignInHistory(clubId, fromDate, queryEnd);

            // Unfiltered: the lookup must cover every student who appears in the sign-in range,
            // including ones now marked inactive.
            Dictionary<string, StudentActivityEntry> currentStanding =
                BuildCurrentStandingLookup(activityFunctions.GetStudentActivity(clubId, activeStudentsOnly: false));

            List<SignInHistoryEntry> entries = BuildEntries(signIns, currentStanding);
            ApplyRunningTotals(entries, currentStanding);

            // Only now drop the trailing rows that were loaded purely to make the totals right.
            entries = entries.Where(e => e.SignInDate.Date <= toDate.Date).ToList();

            return SortByMostRecent(ApplyActiveFilter(entries, activeStudentsOnly));
        }

        /// <summary>
        /// Fills in each row's cumulative hours by starting from the student's recorded total for
        /// the art and subtracting back through the later sessions. Anchoring to the stored total
        /// rather than adding sign-ins up from zero means the newest row agrees with the figure
        /// the rest of the app shows, and nothing depends on the sign-in log reaching all the way
        /// back to when the student started.
        ///
        /// Callers must pass every sign-in from the period through to the present, or the totals
        /// will be too high by whatever was left out.
        /// </summary>
        public static void ApplyRunningTotals(List<SignInHistoryEntry> entries,
            Dictionary<string, StudentActivityEntry> currentStanding)
        {
            if (entries == null)
                return;

            foreach (IGrouping<string, SignInHistoryEntry> group in entries.GroupBy(e => BuildKey(e.StudentID, e.Art)))
            {
                if (currentStanding == null ||
                    !currentStanding.TryGetValue(group.Key, out StudentActivityEntry standing))
                {
                    continue;   // no recorded total to anchor to; these rows stay blank
                }

                double runningTotal = standing.HoursInArt;

                foreach (SignInHistoryEntry entry in group.OrderByDescending(e => e.SignInDate))
                {
                    // The total as of this session includes it, so record before subtracting.
                    entry.CumulativeHours = runningTotal;
                    runningTotal -= entry.SessionHours;
                }
            }
        }

        public static Dictionary<string, StudentActivityEntry> BuildCurrentStandingLookup(IEnumerable<StudentActivityEntry> activity)
        {
            var lookup = new Dictionary<string, StudentActivityEntry>();

            if (activity == null)
                return lookup;

            foreach (StudentActivityEntry entry in activity)
                lookup[BuildKey(entry.StudentID, entry.Art)] = entry;

            return lookup;
        }

        private static string BuildKey(int studentID, string art)
        {
            return studentID + "|" + (art ?? string.Empty).Trim().ToUpperInvariant();
        }

        public static List<SignInHistoryEntry> BuildEntries(DataTable signInTable,
            Dictionary<string, StudentActivityEntry> currentStanding)
        {
            var entries = new List<SignInHistoryEntry>();

            if (signInTable == null || signInTable.Columns.Count == 0)
                return entries;

            foreach (DataRow row in signInTable.Rows)
            {
                // A sign-in with no date is not an attendance record we can place in a period.
                if (row["SignInDate"] == DBNull.Value)
                    continue;

                var entry = new SignInHistoryEntry
                {
                    StudentID = row["StudentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["StudentID"]),
                    FirstName = row["StudentFirstName"] == DBNull.Value ? string.Empty : row["StudentFirstName"].ToString(),
                    LastName = row["StudentLastName"] == DBNull.Value ? string.Empty : row["StudentLastName"].ToString(),
                    IsActive = row["StudentStatus"] != DBNull.Value &&
                        string.Equals(row["StudentStatus"].ToString(), "A", StringComparison.OrdinalIgnoreCase),
                    Art = row["Art"] == DBNull.Value ? string.Empty : row["Art"].ToString(),
                    SignInDate = Convert.ToDateTime(row["SignInDate"]),
                    SessionHours = row["Hours"] == DBNull.Value ? 0 : Convert.ToDouble(row["Hours"])
                };

                // Rank and eligibility come from the student's enrollment as it stands now. A
                // sign-in for an art they are no longer enrolled in simply has no rank to show.
                if (currentStanding != null &&
                    currentStanding.TryGetValue(BuildKey(entry.StudentID, entry.Art), out StudentActivityEntry standing))
                {
                    entry.Rank = standing.Rank;
                    entry.NextRank = standing.NextRank;
                    entry.IsEligibleForPromotion = standing.IsEligibleForPromotion;
                }

                entries.Add(entry);
            }

            return entries;
        }

        public static List<SignInHistoryEntry> ApplyActiveFilter(IEnumerable<SignInHistoryEntry> entries, bool activeStudentsOnly)
        {
            if (entries == null)
                return new List<SignInHistoryEntry>();

            return entries.Where(e => !activeStudentsOnly || e.IsActive).ToList();
        }

        /// <summary>Most recent sign-in first.</summary>
        public static List<SignInHistoryEntry> SortByMostRecent(IEnumerable<SignInHistoryEntry> entries)
        {
            if (entries == null)
                return new List<SignInHistoryEntry>();

            return entries
                .OrderByDescending(e => e.SignInDate)
                .ThenBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToList();
        }
    }
}
