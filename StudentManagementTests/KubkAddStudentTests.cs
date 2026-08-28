using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests for registering a new student at a KUBK member dojo: what the form is required to
    /// collect, the same-name warning that guards against creating another duplicate record, and
    /// the two-write registration itself - including the compensation that stops a failed art
    /// enrollment from leaving a half-created student behind.
    /// </summary>
    [TestFixture]
    public class KubkAddStudentTests
    {
        private static MemberDojoStudentRegistration BuildRegistration()
        {
            return new MemberDojoStudentRegistration
            {
                FirstName = "Kenji",
                LastName = "Tanaka",
                ClubID = "DENTON",
                EmailAddress = "kenji@example.com",
                PhoneNumber = "555-0100",
                IsActive = true,
                Art = "Aikido",
                Rank = "SANDAN",
                RankHeldSince = new DateTime(2019, 4, 6)
            };
        }

        private static KubkManagementFunctions BuildFunctions(FakeKubkDataRepository repository)
        {
            return new KubkManagementFunctions(repository);
        }

        /// <summary>
        /// A student table shaped the way GetStudentTable renames its columns, which is what the
        /// duplicate check reads.
        /// </summary>
        private static DataTable BuildStudentTable(params string[] firstLastDojoStatus)
        {
            var table = new DataTable();
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("StudentFirstName", typeof(string));
            table.Columns.Add("StudentLastName", typeof(string));
            table.Columns.Add("StudentDojo", typeof(string));
            table.Columns.Add("StudentStatus", typeof(string));

            int id = 1000;

            foreach (string row in firstLastDojoStatus)
            {
                string[] fields = row.Split('|');
                table.Rows.Add(id++, fields[0], fields[1], fields[2], fields[3]);
            }

            return table;
        }

        #region Validation

        [Test]
        public void ValidateMemberDojoStudent_WithEverythingFilledIn_Passes()
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());

            Assert.IsTrue(functions.ValidateMemberDojoStudent(BuildRegistration(), out string error));
            Assert.IsEmpty(error);
        }

        [Test]
        public void ValidateMemberDojoStudent_WithNoRegistration_Fails()
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());

            Assert.IsFalse(functions.ValidateMemberDojoStudent(null, out string error));
            Assert.IsNotEmpty(error);
        }

        [TestCase("FirstName")]
        [TestCase("LastName")]
        [TestCase("ClubID")]
        [TestCase("Art")]
        [TestCase("Rank")]
        [Test]
        public void ValidateMemberDojoStudent_WithARequiredFieldMissing_Fails(string missingField)
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();

            typeof(MemberDojoStudentRegistration).GetProperty(missingField).SetValue(registration, "   ");

            Assert.IsFalse(functions.ValidateMemberDojoStudent(registration, out string error));
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateMemberDojoStudent_WithoutADojo_SaysToChooseOne()
        {
            // The dojo is chosen from a list rather than typed, so an empty club id means nothing
            // was selected - the message has to point at the selector, not at a text field.
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.ClubID = null;

            Assert.IsFalse(functions.ValidateMemberDojoStudent(registration, out string error));
            Assert.That(error, Does.Contain("dojo"));
        }

        [Test]
        public void ValidateMemberDojoStudent_WithAFutureRankDate_Fails()
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.RankHeldSince = DateTime.Today.AddDays(30);

            Assert.IsFalse(functions.ValidateMemberDojoStudent(registration, out string error));
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateMemberDojoStudent_WithTodaysRankDate_Passes()
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.RankHeldSince = DateTime.Today;

            Assert.IsTrue(functions.ValidateMemberDojoStudent(registration, out string error));
        }

        [Test]
        public void ValidateMemberDojoStudent_WithABadEmailAddress_Fails()
        {
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.EmailAddress = "not-an-address";

            Assert.IsFalse(functions.ValidateMemberDojoStudent(registration, out string error));
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateMemberDojoStudent_WithNoEmailAddress_Passes()
        {
            // Contact details are optional; only the email that is actually entered is checked.
            var functions = BuildFunctions(new FakeKubkDataRepository());
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.EmailAddress = string.Empty;
            registration.PhoneNumber = null;

            Assert.IsTrue(functions.ValidateMemberDojoStudent(registration, out string error));
        }

        #endregion Validation

        #region Duplicate name warning

        [Test]
        public void FindStudentsByName_MatchesRegardlessOfCaseAndPadding()
        {
            DataTable students = BuildStudentTable("  greg  |  ABLES |DENTON|A");

            List<KubkRosterEntry> matches = KubkManagementFunctions.FindStudentsByName(students, "GREG", "Ables");

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("DENTON", matches[0].Dojo);
            Assert.IsTrue(matches[0].IsActive);
        }

        [Test]
        public void FindStudentsByName_IncludesWindsongStudents()
        {
            // The warning is about creating a second record for one person, which is just as wrong
            // when the existing record is at Windsong as when it is at a member dojo.
            DataTable students = BuildStudentTable(
                "GREG|ABLES|Windsong|I",
                "GREG|ABLES|Windsong|A");

            List<KubkRosterEntry> matches = KubkManagementFunctions.FindStudentsByName(students, "GREG", "ABLES");

            Assert.AreEqual(2, matches.Count);
            Assert.IsTrue(matches.Any(m => m.IsActive));
            Assert.IsTrue(matches.Any(m => !m.IsActive));
        }

        [Test]
        public void FindStudentsByName_RequiresBothNamesToMatch()
        {
            DataTable students = BuildStudentTable(
                "GREG|ABLES|DENTON|A",
                "GREG|SMITH|DENTON|A",
                "PAT|ABLES|DENTON|A");

            List<KubkRosterEntry> matches = KubkManagementFunctions.FindStudentsByName(students, "GREG", "ABLES");

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(1000, matches[0].StudentID);
        }

        [Test]
        public void FindStudentsByName_WithNothingToMatchAgainst_ReturnsEmpty()
        {
            DataTable students = BuildStudentTable("GREG|ABLES|DENTON|A");

            Assert.IsEmpty(KubkManagementFunctions.FindStudentsByName(null, "GREG", "ABLES"));
            Assert.IsEmpty(KubkManagementFunctions.FindStudentsByName(new DataTable(), "GREG", "ABLES"));
            Assert.IsEmpty(KubkManagementFunctions.FindStudentsByName(students, "   ", "ABLES"));
            Assert.IsEmpty(KubkManagementFunctions.FindStudentsByName(students, "GREG", null));
        }

        [Test]
        public void GetStudentsWithSameName_ReadsEveryDojo()
        {
            // Passing a null dojo filter is what makes this span all dojos. The Windsong-only
            // parameterless overload would miss exactly the records worth warning about.
            var repository = new FakeKubkDataRepository
            {
                StudentTable = BuildStudentTable("GREG|ABLES|Windsong|I")
            };

            List<KubkRosterEntry> matches = BuildFunctions(repository).GetStudentsWithSameName("GREG", "ABLES");

            Assert.AreEqual(1, matches.Count);
        }

        #endregion Duplicate name warning

        #region Registration

        [Test]
        public void AddMemberDojoStudent_WritesTheStudentAndTheirFirstArt()
        {
            var repository = new FakeKubkDataRepository { NextStudentID = 4242 };
            var functions = BuildFunctions(repository);

            Assert.IsTrue(functions.AddMemberDojoStudent(BuildRegistration(), out int newStudentID, out string error));

            Assert.AreEqual(4242, newStudentID);
            Assert.IsEmpty(error);

            Student added = repository.AddedStudents.Single();
            Assert.AreEqual("Kenji", added.FirstName);
            Assert.AreEqual("Tanaka", added.LastName);
            Assert.AreEqual("DENTON", added.HomeDojo);
            Assert.AreEqual("kenji@example.com", added.EmailAddress);
            Assert.AreEqual("555-0100", added.PrimaryPhoneNumber);
            Assert.IsTrue(added.ActiveMember);

            StudentArtsAndRank enrollment = repository.AddedArts.Single();
            Assert.AreEqual(4242, enrollment.StudentArtID);
            Assert.AreEqual("Aikido", enrollment.StudentArt);
            Assert.AreEqual("SANDAN", enrollment.Rank);
            Assert.AreEqual(new DateTime(2019, 4, 6), enrollment.DateStarted);
            Assert.AreEqual(0, enrollment.HoursInArt);
        }

        [Test]
        public void AddMemberDojoStudent_LeavesTheNewRankUnverified()
        {
            // The rank is what the home dojo reported. Verifying it is a separate, deliberate act
            // on the roster, so nothing here may stamp it as already verified.
            var repository = new FakeKubkDataRepository();

            Assert.IsTrue(BuildFunctions(repository).AddMemberDojoStudent(BuildRegistration(), out _, out _));

            Assert.IsNull(repository.AddedArts.Single().RankVerifiedDate);
            Assert.IsFalse(repository.AddedArts.Single().RankIsVerified);
        }

        [Test]
        public void AddMemberDojoStudent_TrimsWhatItIsGiven()
        {
            var repository = new FakeKubkDataRepository();
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.FirstName = "  Kenji  ";
            registration.LastName = "  Tanaka  ";
            registration.ClubID = "  DENTON  ";
            registration.Art = "  Aikido  ";
            registration.Rank = "  SANDAN  ";

            Assert.IsTrue(BuildFunctions(repository).AddMemberDojoStudent(registration, out _, out _));

            Assert.AreEqual("Kenji", repository.AddedStudents.Single().FirstName);
            Assert.AreEqual("DENTON", repository.AddedStudents.Single().HomeDojo);
            Assert.AreEqual("Aikido", repository.AddedArts.Single().StudentArt);
            Assert.AreEqual("SANDAN", repository.AddedArts.Single().Rank);
        }

        [Test]
        public void AddMemberDojoStudent_RecordsAnInactiveStudentAsInactive()
        {
            var repository = new FakeKubkDataRepository();
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.IsActive = false;

            Assert.IsTrue(BuildFunctions(repository).AddMemberDojoStudent(registration, out _, out _));

            Assert.IsFalse(repository.AddedStudents.Single().ActiveMember);
        }

        [Test]
        public void AddMemberDojoStudent_WithAnInvalidRegistration_WritesNothing()
        {
            var repository = new FakeKubkDataRepository();
            MemberDojoStudentRegistration registration = BuildRegistration();
            registration.Rank = null;

            Assert.IsFalse(BuildFunctions(repository).AddMemberDojoStudent(registration, out int newStudentID, out string error));

            Assert.AreEqual(0, newStudentID);
            Assert.IsNotEmpty(error);
            Assert.IsEmpty(repository.AddedStudents);
            Assert.IsEmpty(repository.AddedArts);
        }

        [Test]
        public void AddMemberDojoStudent_WhenTheStudentInsertFails_DoesNotWriteAnArt()
        {
            var repository = new FakeKubkDataRepository { WritesSucceed = false };

            Assert.IsFalse(BuildFunctions(repository).AddMemberDojoStudent(BuildRegistration(), out int newStudentID, out string error));

            Assert.AreEqual(0, newStudentID);
            Assert.IsNotEmpty(error);
            Assert.IsEmpty(repository.AddedArts);
            Assert.IsEmpty(repository.DeletedStudents);
        }

        [Test]
        public void AddMemberDojoStudent_WhenTheNewIdCannotBeReadBack_ReportsTheHalfFinishedState()
        {
            // The student row is committed but there is no id to enroll them against and none to
            // delete either, so this can be reported neither as a success nor as a clean failure.
            var repository = new FakeKubkDataRepository { StudentIdIsReadable = false };

            Assert.IsFalse(BuildFunctions(repository).AddMemberDojoStudent(BuildRegistration(), out int newStudentID, out string error));

            Assert.AreEqual(0, newStudentID);
            Assert.That(error, Does.Contain("was added"));
            Assert.That(error, Does.Contain("Aikido"));
            Assert.IsEmpty(repository.AddedArts);
            Assert.IsEmpty(repository.DeletedStudents);
        }

        [Test]
        public void AddMemberDojoStudent_WhenTheArtInsertFails_RemovesTheStudentItJustCreated()
        {
            var repository = new FakeKubkDataRepository { NextStudentID = 4242, AddArtSucceeds = false };

            Assert.IsFalse(BuildFunctions(repository).AddMemberDojoStudent(BuildRegistration(), out int newStudentID, out string error));

            Assert.AreEqual(0, newStudentID);
            Assert.AreEqual(4242, repository.DeletedStudents.Single());
            Assert.That(error, Does.Contain("was not added"));
        }

        [Test]
        public void AddMemberDojoStudent_WhenTheCompensatingDeleteAlsoFails_SaysWhatWasLeftBehind()
        {
            var repository = new FakeKubkDataRepository
            {
                NextStudentID = 4242,
                AddArtSucceeds = false,
                DeleteStudentSucceeds = false
            };

            Assert.IsFalse(BuildFunctions(repository).AddMemberDojoStudent(BuildRegistration(), out int newStudentID, out string error));

            Assert.AreEqual(0, newStudentID);

            // The id has to appear, because manual cleanup is the only remaining option.
            Assert.That(error, Does.Contain("4242"));
            Assert.That(error, Does.Contain("manually"));
        }

        #endregion Registration
    }
}
