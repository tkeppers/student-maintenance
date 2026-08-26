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
        /// Dojos offered in the roster screen's selector: the active, non-Windsong member dojos.
        /// Windsong is excluded because its students are managed through the main maintenance
        /// screen; annual dues for Windsong students are not tracked here.
        /// </summary>
        public static List<Dojo> GetSelectableRosterDojos(IEnumerable<Dojo> allDojos)
        {
            if (allDojos == null)
                return new List<Dojo>();

            return allDojos
                .Where(d => d.Active)
                .Where(d => !string.Equals(d.ClubID?.Trim(), "Windsong", StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// Records a dues payment together with the amount paid, falling back to the configured
        /// annual dues when no amount is given. Unlike SetDuesPaid this always stores an amount,
        /// so it is the entry point for a dues workflow that tracks money; the roster screen does
        /// not use it today.
        /// </summary>
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
        /// with the date the box was ticked. The amount is left null rather than zero so the data
        /// says "not tracked" instead of "paid nothing".
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
                Amount = null
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

        #region KUBK promotions

        public List<Rank> GetRankLadder()
        {
            return dataRepository.GetRankLadder();
        }

        /// <summary>
        /// The rank the promotion dialog pre-selects: whatever Ranks.rank_next says follows the
        /// student's current rank. Empty when the student is at the top of the ladder or holds a
        /// rank the ladder does not know about. This is only a default - KUBK promotions are
        /// granted on instructor recommendation, and hombu may pick any rank, since transfers
        /// and corrections do skip rungs.
        /// </summary>
        public static string GetDefaultNextRank(IEnumerable<Rank> rankLadder, string currentRank)
        {
            if (rankLadder == null || string.IsNullOrWhiteSpace(currentRank))
                return string.Empty;

            Rank current = rankLadder.FirstOrDefault(r =>
                string.Equals(r.RankID?.Trim(), currentRank.Trim(), StringComparison.OrdinalIgnoreCase));

            if (current == null || !current.HasNextRank)
                return string.Empty;

            return current.RankNext.Trim();
        }

        public string GetDefaultNextRank(StudentArtsAndRank currentArt)
        {
            return GetDefaultNextRank(dataRepository.GetRankLadder(), currentArt?.Rank);
        }

        /// <summary>
        /// Loads a student's enrollment in one art, or null when they are not enrolled in it.
        /// Deliberately reads StudArts directly rather than going through
        /// StudentMaintenanceFunctions.PopulateStudentData, because that path calls the
        /// parameterless GetStudentTable() and so only ever sees Windsong students.
        /// </summary>
        public StudentArtsAndRank GetStudentArtEnrollment(int studentID, string artName)
        {
            if (string.IsNullOrWhiteSpace(artName))
                return null;

            DataTable artsTable = dataRepository.GetStudentArtsAndRanks(studentID);

            if (artsTable == null || artsTable.Columns.Count == 0)
                return null;

            foreach (DataRow row in artsTable.Rows)
            {
                if (!string.Equals(row["studArt_art"].ToString().Trim(), artName.Trim(), StringComparison.OrdinalIgnoreCase))
                    continue;

                return new StudentArtsAndRank
                {
                    StudentArtID = int.TryParse(row["StudArt_ID"].ToString(), out int id) ? id : 0,
                    StudentArt = row["studArt_art"].ToString(),
                    Rank = row["studArt_rank"].ToString(),
                    HoursInArt = double.TryParse(row["studArt_cumm"].ToString(), out double hours) ? hours : 0.0,
                    DateStarted = DateTime.TryParse(row["studArt_begin"].ToString(), out DateTime started) ? started : (DateTime?)null,
                    DateOfLatestSignIn = DateTime.TryParse(row["studArt_signin"].ToString(), out DateTime signIn) ? signIn : (DateTime?)null,
                    DatePromoted = DateTime.TryParse(row["studArt_prodate"].ToString(), out DateTime promoted) ? promoted : (DateTime?)null,
                    PromotionHours = double.TryParse(row["studArt_prohrs"].ToString(), out double promoHours) ? promoHours : 0.0,
                    RankVerifiedDate = artsTable.Columns.Contains("studArt_rank_verified") &&
                        DateTime.TryParse(row["studArt_rank_verified"].ToString(), out DateTime verified) ? verified : (DateTime?)null
                };
            }

            return null;
        }

        /// <summary>
        /// Validation for a KUBK promotion. Unlike the Windsong flow there are no eligibility
        /// gates - hours, age and time in grade are informational only - so this checks just
        /// that the entry itself is coherent.
        /// </summary>
        public static bool ValidateKubkPromotion(string newRank, DateTime promotionDate, string recommendedBy, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(newRank))
            {
                error = "Please select the rank the student is being promoted to.";
                return false;
            }

            // A day of slack keeps time-zone and clock skew from rejecting a same-day entry.
            if (promotionDate.Date > DateTime.Today.AddDays(1))
            {
                error = "The promotion date cannot be in the future.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(recommendedBy))
            {
                error = "Please enter the instructor who recommended this promotion.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Records a KUBK promotion. The write itself is the Phase 2 transaction, which updates
        /// the StudArts rank, promotion date and verified stamp and inserts the Promo_History
        /// row with the recommender, all atomically.
        /// </summary>
        public bool RecordKubkPromotion(StudentArtsAndRank art, string newRank, DateTime promotionDate,
            string recommendedBy, out string error)
        {
            if (art == null)
            {
                error = "No martial art was selected for this promotion.";
                return false;
            }

            if (!ValidateKubkPromotion(newRank, promotionDate, recommendedBy, out error))
                return false;

            // UpdateStudentPromotion reads the new rank and date off the art object.
            art.NextRank = newRank.Trim();
            art.DatePromoted = promotionDate.Date;

            bool success = dataRepository.UpdateStudentPromotion(art.StudentArtID, art, recommendedBy.Trim());

            if (!success)
                error = "Error saving the promotion. The change may not have been saved.";
            else
                Log.Information($"Recorded KUBK promotion to {newRank} in {art.StudentArt} for student {art.StudentArtID}");

            return success;
        }

        /// <summary>
        /// Enrolls a student in an art they have no StudArts row for yet, so they can then be
        /// promoted in it. The enrollment starts at the promotion date with the rank they are
        /// coming from.
        /// </summary>
        public bool EnrollStudentInArt(int studentID, string artName, string startingRank, DateTime startDate)
        {
            var enrollment = new StudentArtsAndRank
            {
                StudentArtID = studentID,
                StudentArt = artName,
                Rank = startingRank,
                HoursInArt = 0,
                DateStarted = startDate.Date
            };

            bool success = dataRepository.AddNewStudentArt(enrollment);

            if (success)
                Log.Information($"Enrolled student {studentID} in {artName} at rank {startingRank}");

            return success;
        }

        /// <summary>
        /// Removes a student's enrollment in one art. Used to undo an enrollment created for a
        /// promotion that then failed, so a cancelled or failed promotion leaves no trace.
        /// </summary>
        public bool RemoveStudentArtEnrollment(int studentID, string artName)
        {
            return dataRepository.DeleteStudentArt(studentID, artName);
        }

        #endregion KUBK promotions
    }
}
