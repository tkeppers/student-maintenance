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
                StudentsUnpaid = entryList.Where(e => !e.DuesArePaid).Select(e => e.StudentID).Distinct().Count(),
                VerifiedRanks = entryList.Count(e => e.RankIsVerified),
                TotalRanks = entryList.Count
            };
        }

        /// <summary>
        /// Progress text for the roster status line, e.g. "DENTON: 12 of 19 ranks verified".
        /// Re-verification is per (student, art), so the counts are rank rows, not students.
        /// </summary>
        public static string BuildVerificationProgressText(string dojoLabel, KubkRosterSummary summary)
        {
            if (summary == null || summary.TotalRanks == 0)
                return $"{dojoLabel}: no ranks to verify";

            string progress = $"{dojoLabel}: {summary.VerifiedRanks} of {summary.TotalRanks} ranks verified";

            if (summary.VerifiedRanks == summary.TotalRanks)
                progress += " - complete";

            return progress;
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
        ///
        /// The art name is required. DeleteStudentArt takes it as an optional parameter and
        /// drops the art predicate entirely when it is null, which would delete every one of the
        /// student's enrollments instead of the single one this is meant to undo.
        /// </summary>
        public bool RemoveStudentArtEnrollment(int studentID, string artName)
        {
            if (string.IsNullOrWhiteSpace(artName))
            {
                Log.Error($"Refusing to remove an enrollment for student {studentID} without an art name, " +
                    "because that would delete every art they are enrolled in.");
                return false;
            }

            return dataRepository.DeleteStudentArt(studentID, artName);
        }

        #endregion KUBK promotions

        #region Member dojo student registration

        /// <summary>
        /// Validates a registration before anything is written. The dojo comes from a list rather
        /// than a text box, so this checks that one was chosen rather than that it exists.
        ///
        /// An art and rank are required because a student with no StudArts row never appears on
        /// the roster: GetKubkRoster inner-joins StudArts, so registering a student without one
        /// would look like the add had silently failed.
        /// </summary>
        public bool ValidateMemberDojoStudent(MemberDojoStudentRegistration registration, out string error)
        {
            error = string.Empty;

            if (registration == null)
            {
                error = "Student information is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(registration.FirstName))
            {
                error = "First name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(registration.LastName))
            {
                error = "Last name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(registration.ClubID))
            {
                error = "Please select the student's dojo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(registration.Art))
            {
                error = "Please select the martial art the student trains in.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(registration.Rank))
            {
                error = "Please select the rank the student currently holds.";
                return false;
            }

            // A day of slack keeps time-zone and clock skew from rejecting a same-day entry.
            if (registration.RankHeldSince.Date > DateTime.Today.AddDays(1))
            {
                error = "The date the rank was awarded cannot be in the future.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(registration.EmailAddress) && !IsValidEmail(registration.EmailAddress))
            {
                error = "The email address entered is not valid.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Students already on file under the same name, across every dojo including Windsong.
        /// Used to warn before adding, because a duplicate record splits one person's rank history
        /// across two student ids and there is no tool to merge them back together.
        /// </summary>
        public List<KubkRosterEntry> GetStudentsWithSameName(string firstName, string lastName)
        {
            return FindStudentsByName(dataRepository.GetStudentTable(null), firstName, lastName);
        }

        /// <summary>
        /// Pure name matching behind GetStudentsWithSameName, split out so it is testable without
        /// a database. Trimmed and case-insensitive, because the stored names are inconsistently
        /// cased and padded.
        /// </summary>
        public static List<KubkRosterEntry> FindStudentsByName(DataTable studentTable, string firstName, string lastName)
        {
            var matches = new List<KubkRosterEntry>();

            if (studentTable == null || studentTable.Columns.Count == 0)
                return matches;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                return matches;

            foreach (DataRow row in studentTable.Rows)
            {
                string rowFirstName = row["StudentFirstName"] == DBNull.Value ? string.Empty : row["StudentFirstName"].ToString();
                string rowLastName = row["StudentLastName"] == DBNull.Value ? string.Empty : row["StudentLastName"].ToString();

                if (!NamesMatch(rowFirstName, firstName) || !NamesMatch(rowLastName, lastName))
                    continue;

                matches.Add(new KubkRosterEntry
                {
                    StudentID = row["StudentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["StudentID"]),
                    FirstName = rowFirstName,
                    LastName = rowLastName,
                    IsActive = row["StudentStatus"] != DBNull.Value &&
                        string.Equals(row["StudentStatus"].ToString(), "A", StringComparison.OrdinalIgnoreCase),
                    Dojo = row["StudentDojo"] == DBNull.Value ? string.Empty : row["StudentDojo"].ToString()
                });
            }

            return matches;
        }

        private static bool NamesMatch(string storedName, string enteredName)
        {
            return string.Equals(storedName?.Trim(), enteredName?.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Registers a student at a member dojo: the Students row, then the StudArts row for the
        /// art and rank they arrive holding.
        ///
        /// The rank is left unverified on purpose. It is what the home dojo has reported, and the
        /// roster's existing Verify Rank step is where hombu confirms it - so a newly registered
        /// student turns up highlighted alongside everyone else still awaiting verification.
        ///
        /// The two inserts cannot be one Jet transaction from here, so a failed enrollment is
        /// compensated by deleting the student created a moment earlier: a failed add leaves no
        /// half-created student behind.
        /// </summary>
        public bool AddMemberDojoStudent(MemberDojoStudentRegistration registration, out int newStudentID, out string error)
        {
            newStudentID = 0;

            if (!ValidateMemberDojoStudent(registration, out error))
                return false;

            Student student = BuildStudentFromRegistration(registration);

            if (!dataRepository.AddNewStudent(student, out int studentID))
            {
                error = $"Error adding {student.FullName}. The student was not saved.";
                return false;
            }

            if (studentID <= 0)
            {
                // The student row is committed but there is no id to hang the enrollment on, and
                // none to delete either. Report the half-finished state rather than a clean
                // success or a clean failure, both of which would be untrue.
                Log.Error($"Added student {student.FullName} but could not read back their student id");

                error = $"{student.FullName} was added, but the new student record could not be identified " +
                    $"afterwards, so the {registration.Art.Trim()} enrollment was not created. They will not " +
                    "appear on the roster until an art is recorded for them.";

                return false;
            }

            var enrollment = new StudentArtsAndRank
            {
                StudentArtID = studentID,
                StudentArt = registration.Art.Trim(),
                Rank = registration.Rank.Trim(),
                HoursInArt = 0,
                DateStarted = registration.RankHeldSince.Date
            };

            if (!dataRepository.AddNewStudentArt(enrollment))
            {
                error = UndoStudentAddedForFailedEnrollment(studentID, student.FullName, enrollment.StudentArt);
                return false;
            }

            newStudentID = studentID;

            Log.Information($"Registered student {studentID} ({student.FullName}) at {registration.ClubID.Trim()} " +
                $"in {enrollment.StudentArt} at {enrollment.Rank}");

            return true;
        }

        /// <summary>
        /// Compensating delete for a student created moments before an enrollment that then
        /// failed. Returns the message to show the user, which has to differ depending on whether
        /// the half-created record could actually be removed.
        /// </summary>
        private string UndoStudentAddedForFailedEnrollment(int studentID, string studentName, string artName)
        {
            if (dataRepository.DeleteStudent(studentID))
            {
                Log.Information($"Removed student {studentID} after their {artName} enrollment failed");
                return $"Error enrolling {studentName} in {artName}. The student was not added.";
            }

            Log.Error($"Could not remove student {studentID} after their {artName} enrollment failed");

            return $"Error enrolling {studentName} in {artName}, and the partly-created student record could " +
                $"not be removed. Student id {studentID} may need to be deleted manually.";
        }

        /// <summary>
        /// Maps a registration onto the Student the repository writes. Fields the Windsong form
        /// collects and this one does not are left empty rather than invented; the birthdate is
        /// left unset, which the repository stores as null.
        /// </summary>
        private static Student BuildStudentFromRegistration(MemberDojoStudentRegistration registration)
        {
            return new Student
            {
                FirstName = registration.FirstName.Trim(),
                LastName = registration.LastName.Trim(),
                HomeDojo = registration.ClubID.Trim(),
                ActiveMember = registration.IsActive,
                EmailAddress = (registration.EmailAddress ?? string.Empty).Trim(),
                PrimaryPhoneNumber = (registration.PhoneNumber ?? string.Empty).Trim(),
                SecondaryPhoneNumber = string.Empty,
                Address1 = string.Empty,
                Address2 = string.Empty,
                AddressCity = string.Empty,
                AddressState = string.Empty,
                AddressZip = string.Empty,

                // stud_gender is NOT NULL in Access; the repository writes "X" for UNKNOWN.
                StudentGender = Gender.UNKNOWN,
                StartMonth = DateTime.Today.Month
            };
        }

        #endregion Member dojo student registration

        #region Rank verification

        /// <summary>
        /// Validates an administrative rank correction. Correcting a rank to the value it already
        /// holds is refused with a friendly message rather than writing a pointless update.
        /// </summary>
        public static bool ValidateRankCorrection(string currentRank, string newRank, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(newRank))
            {
                error = "Please select the rank the student actually holds.";
                return false;
            }

            if (string.Equals(currentRank?.Trim(), newRank.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                error = $"The recorded rank is already {newRank.Trim()}. " +
                    "Use Verify Rank to confirm it without changing it.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies a rank correction. This is a data fix, not a promotion: the repository updates
        /// the rank and the verified stamp but writes no promotion history.
        /// </summary>
        public bool CorrectStudentRank(int studentID, string artName, string currentRank, string newRank, out string error)
        {
            if (!ValidateRankCorrection(currentRank, newRank, out error))
                return false;

            bool success = dataRepository.CorrectStudentRank(studentID, artName, newRank.Trim(), DateTime.Today);

            if (!success)
                error = "Error saving the rank correction. The change may not have been saved.";

            return success;
        }

        public bool SetStudentActiveStatus(int studentID, bool active)
        {
            return dataRepository.SetStudentActiveStatus(studentID, active);
        }

        #endregion Rank verification

        #region Reports

        /// <summary>
        /// Rank register rows for the chosen scope, each carrying the recommender of the
        /// student's most recent promotion in that art where one is recorded.
        /// </summary>
        public List<KubkRosterEntry> GetRankRegister(string clubId, bool includeWindsong)
        {
            List<KubkRosterEntry> entries = MapRegister(dataRepository.GetRankRegister(clubId, includeWindsong));
            Dictionary<string, string> recommenders = BuildLatestRecommenderLookup(dataRepository.GetPromotionRecommenders());

            foreach (KubkRosterEntry entry in entries)
            {
                if (recommenders.TryGetValue(BuildRecommenderKey(entry.StudentID, entry.Art), out string recommendedBy))
                    entry.RecommendedBy = recommendedBy;
            }

            return entries;
        }

        public static List<KubkRosterEntry> MapRegister(DataTable registerTable)
        {
            var entries = new List<KubkRosterEntry>();

            if (registerTable == null || registerTable.Columns.Count == 0)
                return entries;

            foreach (DataRow row in registerTable.Rows)
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
                    EmailAddress = row["StudentEmailAddress"] == DBNull.Value ? string.Empty : row["StudentEmailAddress"].ToString(),
                    PhoneNumber = row["StudentPrimaryPhone"] == DBNull.Value ? string.Empty : row["StudentPrimaryPhone"].ToString()
                });
            }

            return entries;
        }

        /// <summary>
        /// Finds each (student, art)'s most recent promotion and returns that promotion's
        /// recommender, which may be blank. Done in memory because Jet has no clean way to
        /// express "latest row per group".
        ///
        /// The latest promotion wins even when it names no recommender. Picking the latest row
        /// that happens to have one instead would pair an old instructor's name with the newer
        /// promotion date the register shows beside it.
        /// </summary>
        public static Dictionary<string, string> BuildLatestRecommenderLookup(DataTable promotionRows)
        {
            var latestDates = new Dictionary<string, DateTime>();
            var recommenders = new Dictionary<string, string>();

            if (promotionRows == null || promotionRows.Columns.Count == 0)
                return recommenders;

            foreach (DataRow row in promotionRows.Rows)
            {
                int studentID = row["promo_student"] == DBNull.Value ? 0 : Convert.ToInt32(row["promo_student"]);
                string art = row["promo_art"] == DBNull.Value ? string.Empty : row["promo_art"].ToString();
                DateTime promoDate = row["promo_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["promo_date"]);

                string recommendedBy = row["promo_recommended_by"] == DBNull.Value
                    ? string.Empty
                    : row["promo_recommended_by"].ToString().Trim();

                string key = BuildRecommenderKey(studentID, art);

                if (latestDates.TryGetValue(key, out DateTime existing) && existing >= promoDate)
                    continue;

                latestDates[key] = promoDate;
                recommenders[key] = recommendedBy;
            }

            return recommenders;
        }

        private static string BuildRecommenderKey(int studentID, string art)
        {
            return studentID + "|" + (art ?? string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Member-dojo students with no dues recorded for the year. One row per student, not per
        /// art, and ordered by dojo then name so the report reads as a chase-up list.
        /// </summary>
        public List<UnpaidDuesEntry> GetUnpaidDues(int duesYear, bool activeStudentsOnly)
        {
            // Built from Students, not from the rank register. The register inner-joins StudArts,
            // so a student enrolled in no art at all would never appear - and owing dues has
            // nothing to do with being enrolled in an art.
            List<KubkRosterEntry> students = MapMemberDojoStudents(dataRepository.GetStudentTable(null));
            Dictionary<int, DateTime?> duesPaid = dataRepository.GetDuesPaidDatesForYear(duesYear);
            Dictionary<string, string> instructors = BuildInstructorLookup(dataRepository.GetDojos());

            return BuildUnpaidDuesEntries(students, duesPaid, instructors, activeStudentsOnly);
        }

        /// <summary>
        /// Maps the full student table down to the member-dojo students, one entry each. Art,
        /// rank and verification are left unset: this population is about who owes dues, and is
        /// deliberately independent of any StudArts enrollment.
        /// </summary>
        public static List<KubkRosterEntry> MapMemberDojoStudents(DataTable studentTable)
        {
            var students = new List<KubkRosterEntry>();

            if (studentTable == null || studentTable.Columns.Count == 0)
                return students;

            foreach (DataRow row in studentTable.Rows)
            {
                string dojo = row["StudentDojo"] == DBNull.Value ? string.Empty : row["StudentDojo"].ToString();

                if (string.Equals(dojo.Trim(), "Windsong", StringComparison.OrdinalIgnoreCase))
                    continue;

                students.Add(new KubkRosterEntry
                {
                    StudentID = row["StudentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["StudentID"]),
                    FirstName = row["StudentFirstName"] == DBNull.Value ? string.Empty : row["StudentFirstName"].ToString(),
                    LastName = row["StudentLastName"] == DBNull.Value ? string.Empty : row["StudentLastName"].ToString(),
                    IsActive = row["StudentStatus"] != DBNull.Value &&
                        string.Equals(row["StudentStatus"].ToString(), "A", StringComparison.OrdinalIgnoreCase),
                    Dojo = dojo,
                    EmailAddress = row["StudentEmailAddress"] == DBNull.Value ? string.Empty : row["StudentEmailAddress"].ToString(),
                    PhoneNumber = row["StudentPrimaryPhone"] == DBNull.Value ? string.Empty : row["StudentPrimaryPhone"].ToString()
                });
            }

            return students;
        }

        public static Dictionary<string, string> BuildInstructorLookup(IEnumerable<Dojo> dojos)
        {
            var instructors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (dojos == null)
                return instructors;

            foreach (Dojo dojo in dojos)
            {
                if (!string.IsNullOrWhiteSpace(dojo.ClubID))
                    instructors[dojo.ClubID.Trim()] = dojo.Instructor ?? string.Empty;
            }

            return instructors;
        }

        public static List<UnpaidDuesEntry> BuildUnpaidDuesEntries(IEnumerable<KubkRosterEntry> register,
            Dictionary<int, DateTime?> duesPaidByStudent, Dictionary<string, string> instructorsByDojo,
            bool activeStudentsOnly)
        {
            if (register == null)
                return new List<UnpaidDuesEntry>();

            return register
                .Where(e => !activeStudentsOnly || e.IsActive)
                .Where(e => !HasPaidDues(duesPaidByStudent, e.StudentID))
                // The register is per (student, art); dues are per student, so collapse first.
                .GroupBy(e => e.StudentID)
                .Select(group => group.First())
                .Select(e => new UnpaidDuesEntry
                {
                    StudentID = e.StudentID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    IsActive = e.IsActive,
                    Dojo = e.Dojo,
                    Instructor = LookupInstructor(instructorsByDojo, e.Dojo),
                    EmailAddress = e.EmailAddress,
                    PhoneNumber = e.PhoneNumber
                })
                .OrderBy(e => e.Dojo)
                .ThenBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToList();
        }

        private static bool HasPaidDues(Dictionary<int, DateTime?> duesPaidByStudent, int studentID)
        {
            return duesPaidByStudent != null &&
                duesPaidByStudent.TryGetValue(studentID, out DateTime? paidDate) &&
                paidDate.HasValue;
        }

        private static string LookupInstructor(Dictionary<string, string> instructorsByDojo, string dojo)
        {
            if (instructorsByDojo == null || string.IsNullOrWhiteSpace(dojo))
                return string.Empty;

            return instructorsByDojo.TryGetValue(dojo.Trim(), out string instructor) ? instructor : string.Empty;
        }

        /// <summary>Per-dojo subtotals for the unpaid dues report, in the report's own order.</summary>
        public static List<KeyValuePair<string, int>> BuildUnpaidDuesSubtotals(IEnumerable<UnpaidDuesEntry> entries)
        {
            if (entries == null)
                return new List<KeyValuePair<string, int>>();

            return entries
                .GroupBy(e => e.Dojo)
                .OrderBy(group => group.Key)
                .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))
                .ToList();
        }

        #endregion Reports
    }
}
