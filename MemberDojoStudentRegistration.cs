using System;

namespace DojoStudentManagement
{
    /// <summary>
    /// What is collected to register a student at a KUBK member dojo: the Students row, plus the
    /// one art enrollment that makes them visible on the roster at all.
    ///
    /// Deliberately narrower than the Windsong student form. Hombu tracks who a member dojo's
    /// students are, what rank they hold and how to reach them; the address and birthdate the
    /// Windsong screen collects belong to the home dojo, not to the organization.
    /// </summary>
    public class MemberDojoStudentRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// The club id from Club_Parameters, not a typed dojo name. Students.stud_club has to
        /// match it exactly or the student falls off every per-dojo view, so this is always
        /// chosen from the list of dojos rather than entered as text.
        /// </summary>
        public string ClubID { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>The art the student trains in, and the rank their home dojo reports them at.</summary>
        public string Art { get; set; }

        public string Rank { get; set; }

        /// <summary>
        /// When the student reached that rank. Stored as both the start and promotion date of the
        /// new enrollment, which is what the roster's Years at Rank column and the rank register
        /// report from - defaulting it to today would show a transferring yondan as newly promoted.
        /// </summary>
        public DateTime RankHeldSince { get; set; } = DateTime.Today;

        public string FullName => $"{FirstName} {LastName}";
    }
}
