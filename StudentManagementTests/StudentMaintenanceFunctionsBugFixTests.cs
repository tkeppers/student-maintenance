using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;

namespace DojoStudentManagementTests
{
    /// <summary>
    /// Tests covering the bug fixes applied to StudentMaintenanceFunctions:
    ///   1. PopulateStudentData no longer throws IndexOutOfRangeException when the student ID is not found.
    ///   2. IsValidEmail correctly validates and rejects malformed addresses (used by the fixed StudentAddUI validation).
    /// </summary>
    [TestFixture]
    public class StudentMaintenanceFunctionsBugFixTests
    {
        StudentMaintenanceFunctions functions;

        [SetUp]
        public void Setup()
        {
            functions = new StudentMaintenanceFunctions();
        }

        #region PopulateStudentData — bounds check fix

        [Test]
        public void PopulateStudentData_WhenStudentIdNotFound_ReturnsDefaultStudentWithoutThrowing()
        {
            var repo = new FakeDataRepository(); // returns an empty student table

            Student result = null;
            Assert.DoesNotThrow(() => result = functions.PopulateStudentData(repo, 99999),
                "PopulateStudentData must not throw when the student ID is absent from the table");

            Assert.IsNotNull(result, "Should return an empty Student object, not null");
            Assert.AreEqual(0, result.StudentID, "Returned student should have the default ID of 0");
        }

        [Test]
        public void PopulateStudentData_WhenStudentIdIsFound_ReturnsPopulatedStudent()
        {
            var repo = new FakeDataRepository(includeStudent: true, studentID: 42,
                firstName: "Jane", lastName: "Smith");

            Student result = functions.PopulateStudentData(repo, 42);

            Assert.AreEqual("Jane", result.FirstName);
            Assert.AreEqual("Smith", result.LastName);
        }

        #endregion

        #region IsValidEmail — used by the fixed ValidateStudentData

        [TestCase("user@example.com", true)]
        [TestCase("firstname.lastname@domain.org", true)]
        [TestCase("user+tag@sub.domain.com", true)]
        [Test]
        public void IsValidEmail_WithValidAddress_ReturnsTrue(string email, bool expected)
        {
            Assert.AreEqual(expected, functions.IsValidEmail(email));
        }

        [TestCase("", false)]
        [TestCase("notanemail", false)]
        [TestCase("missing@tld.", false)]
        [TestCase("no-at-sign.com", false)]
        [Test]
        public void IsValidEmail_WithInvalidAddress_ReturnsFalse(string email, bool expected)
        {
            Assert.AreEqual(expected, functions.IsValidEmail(email));
        }

        [Test]
        public void IsValidEmail_WithLeadingAndTrailingSpaces_ValidatesTheTrimmedAddress()
        {
            // The implementation trims before validating, so a padded valid address should pass
            Assert.IsTrue(functions.IsValidEmail("  user@example.com  "));
        }

        #endregion
    }

    /// <summary>
    /// Minimal fake repository for unit tests that need a controllable IDataRepository.
    /// Only the methods exercised by StudentMaintenanceFunctions are implemented;
    /// all others throw NotImplementedException to surface accidental calls.
    /// </summary>
    internal class FakeDataRepository : IDataRepository
    {
        private readonly bool _includeStudent;
        private readonly int _studentID;
        private readonly string _firstName;
        private readonly string _lastName;

        public FakeDataRepository(bool includeStudent = false, int studentID = 0,
            string firstName = "", string lastName = "")
        {
            _includeStudent = includeStudent;
            _studentID = studentID;
            _firstName = firstName;
            _lastName = lastName;
        }

        public DataTable GetStudentTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("StudentFirstName", typeof(string));
            table.Columns.Add("StudentLastName", typeof(string));
            table.Columns.Add("StudentStatus", typeof(string));
            table.Columns.Add("StudentAddress1", typeof(string));
            table.Columns.Add("StudentAddress2", typeof(string));
            table.Columns.Add("StudentCity", typeof(string));
            table.Columns.Add("StudentState", typeof(string));
            table.Columns.Add("StudentPostalCode", typeof(string));
            table.Columns.Add("StudentPrimaryPhone", typeof(string));
            table.Columns.Add("StudentSecondaryPhone", typeof(string));
            table.Columns.Add("StudentEmailAddress", typeof(string));
            table.Columns.Add("StudentDojo", typeof(string));
            table.Columns.Add("StudentBirthDate", typeof(DateTime));
            table.Columns.Add("StudentGender", typeof(string));
            table.Columns.Add("StudentStartMonth", typeof(int));

            if (_includeStudent)
            {
                DataRow row = table.NewRow();
                row["StudentID"] = _studentID;
                row["StudentFirstName"] = _firstName;
                row["StudentLastName"] = _lastName;
                row["StudentStatus"] = "A";
                row["StudentAddress1"] = string.Empty;
                row["StudentAddress2"] = string.Empty;
                row["StudentCity"] = string.Empty;
                row["StudentState"] = string.Empty;
                row["StudentPostalCode"] = string.Empty;
                row["StudentPrimaryPhone"] = string.Empty;
                row["StudentSecondaryPhone"] = string.Empty;
                row["StudentEmailAddress"] = string.Empty;
                row["StudentDojo"] = "Windsong";
                row["StudentBirthDate"] = DBNull.Value;
                row["StudentGender"] = "X";
                row["StudentStartMonth"] = 1;
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetStudentArtsAndRanks(int studentID)
        {
            DataTable table = new DataTable();
            table.Columns.Add("StudArt_ID", typeof(int));
            table.Columns.Add("studArt_art", typeof(string));
            table.Columns.Add("studArt_rank", typeof(string));
            table.Columns.Add("studArt_begin", typeof(DateTime));
            table.Columns.Add("studArt_cumm", typeof(decimal));
            table.Columns.Add("studArt_signin", typeof(DateTime));
            table.Columns.Add("studArt_prodate", typeof(DateTime));
            table.Columns.Add("studArt_prohrs", typeof(double));
            return table;
        }

        // Unused by the tests in this file — throw to catch accidental calls
        public DataTable GetStudentTable(string dojoFilter) => throw new NotImplementedException();
        public DataTable GetListOfArts() => throw new NotImplementedException();
        public bool AddNewStudent(Student student) => throw new NotImplementedException();
        public bool UpdateStudent(Student student) => throw new NotImplementedException();
        public bool DeleteStudent(int studentID) => throw new NotImplementedException();
        public DataTable GetStudentPromotionHistory(int studentID) => throw new NotImplementedException();
        public DataTable GetStudentSignInHistory(int studentID) => throw new NotImplementedException();
        public bool AddNewStudentArt(StudentArtsAndRank artsAndRank) => throw new NotImplementedException();
        public bool UpdateStudentArt(StudentArtsAndRank artsAndRank) => throw new NotImplementedException();
        public bool DeleteStudentArt(int studentArtID, string studentArtName) => throw new NotImplementedException();
        public bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank, string recommendedBy = null) => throw new NotImplementedException();
        public bool UpdateStudentSignIn(int studentID, string studentArtName, double cumulativeTrainingHours, out double newCumulativeHours)
        {
            newCumulativeHours = 0;
            throw new NotImplementedException();
        }
        public void UpdatePromotionCriteria(DataTable promotionCriteriaTable) => throw new NotImplementedException();
        public DataTable GetStudentPromotionRequirements() => throw new NotImplementedException();
        public List<Dojo> GetDojos() => throw new NotImplementedException();
        public bool AddDojo(Dojo dojo) => throw new NotImplementedException();
        public bool UpdateDojo(Dojo dojo) => throw new NotImplementedException();
        public bool DeleteDojo(string clubId) => throw new NotImplementedException();
        public Dictionary<string, int> GetStudentCountsByDojo() => throw new NotImplementedException();
        public List<Rank> GetRankLadder() => throw new NotImplementedException();
        public bool CorrectStudentRank(int studentID, string artName, string newRank, DateTime verifiedDate) => throw new NotImplementedException();
        public bool SetStudentActiveStatus(int studentID, bool active) => throw new NotImplementedException();
        public DataTable GetRankRegister(string clubId, bool includeWindsong) => throw new NotImplementedException();
        public DataTable GetPromotionRecommenders() => throw new NotImplementedException();
        public DataTable GetStudentActivity(string clubId) => throw new NotImplementedException();
        public Dictionary<int, DateTime?> GetDuesPaidDatesForYear(int duesYear) => throw new NotImplementedException();
        public DataTable GetKubkRoster(string clubId, int duesYear) => throw new NotImplementedException();
        public bool RecordDuesPayment(StudentDuesRecord dues) => throw new NotImplementedException();
        public bool RemoveDuesPayment(int studentID, int year) => throw new NotImplementedException();
        public List<StudentDuesRecord> GetDuesHistory(int studentID) => throw new NotImplementedException();
        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate) => throw new NotImplementedException();
    }
}
