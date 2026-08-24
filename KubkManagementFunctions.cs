using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace DojoStudentManagement
{
    public class KubkManagementFunctions
    {
        private const decimal DefaultAnnualDuesFallback = 70m;

        private readonly IDataRepository dataRepository;

        public KubkManagementFunctions(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public List<Dojo> GetDojos()
        {
            return dataRepository.GetDojos();
        }

        public bool SaveDojo(Dojo dojo, bool isNew, out string validationError)
        {
            if (!ValidateDojo(dojo, out validationError))
                return false;

            // A new dojo must not reuse an existing club_id: that key is what Students.stud_club
            // points at, so a collision would silently merge two dojos' rosters.
            if (isNew && IsClubIdInUse(dataRepository.GetDojos(), dojo.ClubID))
            {
                validationError = $"Club ID '{dojo.ClubID}' is already in use by another dojo.";
                return false;
            }

            bool success = isNew ? dataRepository.AddDojo(dojo) : dataRepository.UpdateDojo(dojo);

            if (!success)
                Log.Error($"Failed to save dojo {dojo.ClubID}");

            return success;
        }

        public Dictionary<string, int> GetStudentCountsByDojo()
        {
            return dataRepository.GetStudentCountsByDojo();
        }

        public static int GetStudentCountForDojo(Dictionary<string, int> countsByDojo, string clubId)
        {
            if (countsByDojo == null || string.IsNullOrWhiteSpace(clubId))
                return 0;

            return countsByDojo.TryGetValue(clubId.Trim(), out int count) ? count : 0;
        }

        /// <summary>
        /// A dojo may only be deleted while no student points at it. Dojos with students are
        /// deactivated instead, so historical records keep resolving to a real dojo - deleting
        /// one in use would orphan every student whose stud_club still names it.
        /// </summary>
        public static bool CanDeleteDojo(Dojo dojo, int studentCount, out string reason)
        {
            reason = string.Empty;

            if (dojo == null || string.IsNullOrWhiteSpace(dojo.ClubID))
            {
                reason = "No dojo is selected.";
                return false;
            }

            if (studentCount > 0)
            {
                string dojoName = string.IsNullOrWhiteSpace(dojo.Name) ? dojo.ClubID : dojo.Name;
                reason = $"{dojoName} still has {studentCount} student(s) assigned to it and cannot be deleted. " +
                    "Uncheck Active to retire it instead.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes a dojo, re-checking the headcount against the database first so a stale screen
        /// cannot delete a dojo that has since gained students.
        /// </summary>
        public bool DeleteDojo(Dojo dojo, out string error)
        {
            int studentCount = GetStudentCountForDojo(dataRepository.GetStudentCountsByDojo(), dojo?.ClubID);

            if (!CanDeleteDojo(dojo, studentCount, out error))
                return false;

            bool success = dataRepository.DeleteDojo(dojo.ClubID);

            if (!success)
                error = $"Error deleting dojo {dojo.ClubID}. The change may not have been saved.";

            return success;
        }

        public bool ValidateDojo(Dojo dojo, out string validationError)
        {
            validationError = string.Empty;

            if (dojo == null)
            {
                validationError = "Dojo information is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dojo.ClubID))
            {
                validationError = "Club ID is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dojo.Name))
            {
                validationError = "Dojo name is required.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(dojo.InstructorEmail) && !IsValidEmail(dojo.InstructorEmail))
            {
                validationError = "Instructor email address is not valid.";
                return false;
            }

            return true;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch (Exception e)
            {
                Log.Error($"Error validating email address {email}\n{e.Message}\n{e.Source}\n{e.StackTrace}");
                return false;
            }
        }

        public DataTable GetKubkRoster(string clubId, int duesYear)
        {
            return dataRepository.GetKubkRoster(clubId, duesYear);
        }

        /// <summary>
        /// Loads the roster for a club/year and maps it to KubkRosterEntry objects so the UI
        /// (and the filter/summary logic below) never has to deal with raw DataRows.
        /// </summary>
        public List<KubkRosterEntry> GetRosterEntries(string clubId, int duesYear)
        {
            return MapRoster(dataRepository.GetKubkRoster(clubId, duesYear));
        }

        public static List<KubkRosterEntry> MapRoster(DataTable rosterTable)
        {
            var entries = new List<KubkRosterEntry>();

            if (rosterTable == null || rosterTable.Columns.Count == 0)
                return entries;

            foreach (DataRow row in rosterTable.Rows)
            {
                entries.Add(new KubkRosterEntry
                {
                    StudentID = row["StudentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["StudentID"]),
                    FirstName = row["StudentFirstName"] == DBNull.Value ? string.Empty : row["StudentFirstName"].ToString(),
                    LastName = row["StudentLastName"] == DBNull.Value ? string.Empty : row["StudentLastName"].ToString(),
                    IsActive = row["StudentStatus"] != DBNull.Value &&
                        string.Equals(row["StudentStatus"].ToString(), "A", StringComparison.OrdinalIgnoreCase),
                    Dojo = row["StudentDojo"] == DBNull.Value ? string.Empty : row["StudentDojo"].ToString(),
                    Art = row["Art"] == DBNull.Value ? string.Empty : row["Art"].ToString(),
                    Rank = row["Rank"] == DBNull.Value ? string.Empty : row["Rank"].ToString(),
                    LastPromotionDate = row["LastPromotionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastPromotionDate"]),
                    RankVerifiedDate = row["RankVerifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["RankVerifiedDate"]),
                    DuesPaidDate = row["DuesPaidDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DuesPaidDate"])
                });
            }

            return entries;
        }

        /// <summary>
        /// Applies the roster screen's display filters. Both filters are independent; passing
        /// false for both returns every entry.
        /// </summary>
        public static List<KubkRosterEntry> ApplyRosterFilters(IEnumerable<KubkRosterEntry> entries,
            bool activeStudentsOnly, bool unpaidDuesOnly)
        {
            if (entries == null)
                return new List<KubkRosterEntry>();

            return entries
                .Where(e => !activeStudentsOnly || e.IsActive)
                .Where(e => !unpaidDuesOnly || !e.DuesArePaid)
                .ToList();
        }

        /// <summary>
        /// Counts for the roster status line. Student counts are distinct by student id because
        /// a student appears once per art they are enrolled in; unverified ranks are counted per
        /// (student, art) row since verification is tracked per art.
        /// </summary>
        public static KubkRosterSummary SummarizeRoster(IEnumerable<KubkRosterEntry> entries)
        {
            var entryList = entries?.ToList() ?? new List<KubkRosterEntry>();

            return new KubkRosterSummary
            {
                StudentsShown = entryList.Select(e => e.StudentID).Distinct().Count(),
                UnverifiedRanks = entryList.Count(e => !e.RankIsVerified),
                StudentsUnpaid = entryList.Where(e => !e.DuesArePaid).Select(e => e.StudentID).Distinct().Count()
            };
        }

        /// <summary>
        /// True when the club id already exists (case-insensitive, matching how Jet compares
        /// the club_id key). Used to stop an Add from colliding with an existing dojo.
        /// </summary>
        public static bool IsClubIdInUse(IEnumerable<Dojo> existingDojos, string clubId)
        {
            if (existingDojos == null || string.IsNullOrWhiteSpace(clubId))
                return false;

            return existingDojos.Any(d =>
                string.Equals(d.ClubID?.Trim(), clubId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Dojos offered in the roster screen's selector: every active member dojo. Windsong is
        /// included because annual dues are owed per student at every dojo, so Windsong students
        /// need somewhere to be marked paid too.
        /// </summary>
        public static List<Dojo> GetSelectableRosterDojos(IEnumerable<Dojo> allDojos)
        {
            if (allDojos == null)
                return new List<Dojo>();

            return allDojos
                .Where(d => d.Active)
                .OrderBy(d => d.Name)
                .ToList();
        }

        /// <summary>
        /// Default annual dues amount from App.config's KubkAnnualDuesAmount setting, falling
        /// back to $70 if the key is missing or not a parseable decimal. The caller can always
        /// override this with a different amount when recording a payment.
        /// </summary>
        public decimal GetDefaultAnnualDuesAmount()
        {
            return ParseAnnualDuesAmount(ConfigurationManager.AppSettings["KubkAnnualDuesAmount"]);
        }

        /// <summary>
        /// Pure parsing logic behind GetDefaultAnnualDuesAmount, split out so the missing/unparseable
        /// fallback path is directly testable without needing to control the process's App.config.
        /// </summary>
        public static decimal ParseAnnualDuesAmount(string configValue)
        {
            if (decimal.TryParse(configValue, out decimal amount))
                return amount;

            return DefaultAnnualDuesFallback;
        }

        public bool RecordDuesPayment(int studentID, int year, DateTime paidDate, decimal? amount = null)
        {
            var dues = new StudentDuesRecord
            {
                StudentID = studentID,
                Year = year,
                PaidDate = paidDate,
                Amount = amount ?? GetDefaultAnnualDuesAmount()
            };

            return dataRepository.RecordDuesPayment(dues);
        }

        public bool RemoveDuesPayment(int studentID, int year)
        {
            return dataRepository.RemoveDuesPayment(studentID, year);
        }

        /// <summary>
        /// Marks a student's annual dues paid or unpaid for a year - the only dues operation the
        /// roster screen offers. The dojo tracks the money itself in its accounting system, so no
        /// amount is recorded here: the stored row is purely a "confirmed paid" marker, stamped
        /// with the date the box was ticked.
        /// </summary>
        public bool SetDuesPaid(int studentID, int year, bool isPaid)
        {
            if (!isPaid)
                return dataRepository.RemoveDuesPayment(studentID, year);

            var dues = new StudentDuesRecord
            {
                StudentID = studentID,
                Year = year,
                PaidDate = DateTime.Today,
                Amount = 0m
            };

            return dataRepository.RecordDuesPayment(dues);
        }

        public List<StudentDuesRecord> GetDuesHistory(int studentID)
        {
            return dataRepository.GetDuesHistory(studentID);
        }

        public bool IsDuesPaidForYear(int studentID, int year)
        {
            return dataRepository.GetDuesHistory(studentID).Any(d => d.Year == year && d.IsPaid);
        }

        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate)
        {
            return dataRepository.VerifyStudentRank(studentID, artName, verifiedDate);
        }
    }
}
