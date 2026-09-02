using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagement
{
    /// <summary>
    /// Assembles the student activity report. Promotion eligibility reuses the same
    /// Student.IsEligibleForPromotion gates the promotion screen applies, so the report cannot
    /// drift from what the app will actually let you do.
    /// </summary>
    public class StudentActivityReportFunctions
    {
        private readonly IDataRepository dataRepository;

        public StudentActivityReportFunctions(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public List<StudentActivityEntry> GetStudentActivity(string clubId, bool activeStudentsOnly)
        {
            List<StudentActivityEntry> entries = BuildEntries(
                dataRepository.GetStudentActivity(clubId),
                dataRepository.GetStudentPromotionRequirements());

            return SortByMostRecentSignIn(ApplyActiveFilter(entries, activeStudentsOnly));
        }

        public static List<StudentActivityEntry> BuildEntries(DataTable activityTable, DataTable promotionRequirements)
        {
            var entries = new List<StudentActivityEntry>();

            if (activityTable == null || activityTable.Columns.Count == 0)
                return entries;

            foreach (DataRow row in activityTable.Rows)
            {
                var entry = new StudentActivityEntry
                {
                    StudentID = row["StudentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["StudentID"]),
                    FirstName = row["StudentFirstName"] == DBNull.Value ? string.Empty : row["StudentFirstName"].ToString(),
                    LastName = row["StudentLastName"] == DBNull.Value ? string.Empty : row["StudentLastName"].ToString(),
                    IsActive = row["StudentStatus"] != DBNull.Value &&
                        string.Equals(row["StudentStatus"].ToString(), "A", StringComparison.OrdinalIgnoreCase),
                    Art = row["Art"] == DBNull.Value ? string.Empty : row["Art"].ToString(),
                    Rank = row["Rank"] == DBNull.Value ? string.Empty : row["Rank"].ToString(),
                    HoursInArt = row["HoursInArt"] == DBNull.Value ? 0 : Convert.ToDouble(row["HoursInArt"]),
                    LastSignInDate = row["LastSignInDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastSignInDate"])
                };

                ApplyPromotionEligibility(entry, row, promotionRequirements);
                entries.Add(entry);
            }

            return entries;
        }

        private static void ApplyPromotionEligibility(StudentActivityEntry entry, DataRow row, DataTable promotionRequirements)
        {
            if (promotionRequirements == null || promotionRequirements.Columns.Count == 0)
                return;

            var art = new StudentArtsAndRank
            {
                StudentArtID = entry.StudentID,
                StudentArt = entry.Art,
                Rank = entry.Rank,
                HoursInArt = entry.HoursInArt,
                DateStarted = row["DateStarted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DateStarted"]),
                DatePromoted = row["LastPromotionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastPromotionDate"])
            };

            var student = new Student();

            if (row["StudentBirthDate"] != DBNull.Value)
                student.DateOfBirth = Convert.ToDateTime(row["StudentBirthDate"]);

            // A fresh PromotionCriteria per row on purpose. GetNextPromotionCriteria only writes
            // its fields when it finds exactly one matching requirement, so a shared instance
            // would silently keep the previous student's thresholds - and its non-empty NextRank
            // would slip past the "no criteria on record" guard inside IsEligibleForPromotion.
            var criteria = new PromotionCriteria(promotionRequirements);
            criteria.GetNextPromotionCriteria(art);

            entry.NextRank = criteria.NextRank;
            entry.IsEligibleForPromotion = student.IsEligibleForPromotion(art, criteria);
        }

        public static List<StudentActivityEntry> ApplyActiveFilter(IEnumerable<StudentActivityEntry> entries, bool activeStudentsOnly)
        {
            if (entries == null)
                return new List<StudentActivityEntry>();

            return entries.Where(e => !activeStudentsOnly || e.IsActive).ToList();
        }

        /// <summary>
        /// Most recent sign-in first. Students who have never signed in have no date to sort on
        /// and go to the end rather than leading the report.
        /// </summary>
        public static List<StudentActivityEntry> SortByMostRecentSignIn(IEnumerable<StudentActivityEntry> entries)
        {
            if (entries == null)
                return new List<StudentActivityEntry>();

            return entries
                .OrderByDescending(e => e.LastSignInDate.HasValue)
                .ThenByDescending(e => e.LastSignInDate ?? DateTime.MinValue)
                .ThenBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToList();
        }
    }
}
