using NUnit.Framework;
using DojoStudentManagement;
using System;
using System.Collections.Generic;
using System.Data;

namespace DojoStudentManagementTests
{
    [TestFixture]
    public class KubkManagementFunctionsTests
    {
        #region ValidateDojo

        [Test]
        public void ValidateDojo_WhenClubIdIsEmpty_ReturnsFalse()
        {
            var functions = new KubkManagementFunctions(new FakeKubkDataRepository());
            var dojo = new Dojo { ClubID = "", Name = "Some Dojo" };

            bool isValid = functions.ValidateDojo(dojo, out string error);

            Assert.IsFalse(isValid);
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateDojo_WhenNameIsEmpty_ReturnsFalse()
        {
            var functions = new KubkManagementFunctions(new FakeKubkDataRepository());
            var dojo = new Dojo { ClubID = "ABC", Name = "" };

            bool isValid = functions.ValidateDojo(dojo, out string error);

            Assert.IsFalse(isValid);
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateDojo_WhenInstructorEmailIsInvalid_ReturnsFalse()
        {
            var functions = new KubkManagementFunctions(new FakeKubkDataRepository());
            var dojo = new Dojo { ClubID = "ABC", Name = "Some Dojo", InstructorEmail = "not-an-email" };

            bool isValid = functions.ValidateDojo(dojo, out string error);

            Assert.IsFalse(isValid);
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void ValidateDojo_WhenInstructorEmailIsEmpty_ReturnsTrue()
        {
            // Instructor email is optional; an empty value should not fail validation.
            var functions = new KubkManagementFunctions(new FakeKubkDataRepository());
            var dojo = new Dojo { ClubID = "ABC", Name = "Some Dojo", InstructorEmail = "" };

            bool isValid = functions.ValidateDojo(dojo, out string error);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(error);
        }

        [Test]
        public void ValidateDojo_WhenValid_ReturnsTrue()
        {
            var functions = new KubkManagementFunctions(new FakeKubkDataRepository());
            var dojo = new Dojo { ClubID = "ABC", Name = "Some Dojo", InstructorEmail = "instructor@example.com" };

            bool isValid = functions.ValidateDojo(dojo, out string error);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(error);
        }

        #endregion

        #region GetDefaultAnnualDuesAmount / ParseAnnualDuesAmount

        [Test]
        public void ParseAnnualDuesAmount_WhenValueIsMissing_ReturnsFallbackOf70()
        {
            Assert.AreEqual(70m, KubkManagementFunctions.ParseAnnualDuesAmount(null));
        }

        [Test]
        public void ParseAnnualDuesAmount_WhenValueIsUnparseable_ReturnsFallbackOf70()
        {
            Assert.AreEqual(70m, KubkManagementFunctions.ParseAnnualDuesAmount("not-a-number"));
        }

        [Test]
        public void ParseAnnualDuesAmount_WhenValueIsValid_ReturnsParsedAmount()
        {
            Assert.AreEqual(85.50m, KubkManagementFunctions.ParseAnnualDuesAmount("85.50"));
        }

        #endregion

        #region IsDuesPaidForYear

        [Test]
        public void IsDuesPaidForYear_WhenRecordExistsWithPaidDate_ReturnsTrue()
        {
            var repo = new FakeKubkDataRepository();
            repo.DuesHistory = new List<StudentDuesRecord>
            {
                new StudentDuesRecord { StudentID = 1, Year = 2026, PaidDate = DateTime.Now, Amount = 70m }
            };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.IsDuesPaidForYear(1, 2026));
        }

        [Test]
        public void IsDuesPaidForYear_WhenNoRecordForYear_ReturnsFalse()
        {
            var repo = new FakeKubkDataRepository();
            repo.DuesHistory = new List<StudentDuesRecord>
            {
                new StudentDuesRecord { StudentID = 1, Year = 2025, PaidDate = DateTime.Now, Amount = 70m }
            };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.IsDuesPaidForYear(1, 2026));
        }

        [Test]
        public void IsDuesPaidForYear_WhenRecordExistsButNotPaid_ReturnsFalse()
        {
            var repo = new FakeKubkDataRepository();
            repo.DuesHistory = new List<StudentDuesRecord>
            {
                new StudentDuesRecord { StudentID = 1, Year = 2026, PaidDate = null, Amount = 70m }
            };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.IsDuesPaidForYear(1, 2026));
        }

        #endregion
    }

    /// <summary>
    /// Minimal fake repository for KubkManagementFunctions tests. Only the KUBK-related
    /// methods are implemented; everything else throws to surface accidental calls.
    /// </summary>
    internal class FakeKubkDataRepository : IDataRepository
    {
        public List<StudentDuesRecord> DuesHistory = new List<StudentDuesRecord>();

        public List<StudentDuesRecord> GetDuesHistory(int studentID) => DuesHistory;

        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate) => true;
        public bool RecordDuesPayment(StudentDuesRecord dues) => true;
        public bool RemoveDuesPayment(int studentID, int year) => true;
        public List<Dojo> GetDojos() => new List<Dojo>();
        public bool AddDojo(Dojo dojo) => true;
        public bool UpdateDojo(Dojo dojo) => true;
        public DataTable GetKubkRoster(string clubId, int duesYear) => new DataTable();

        // Unused by the tests in this file — throw to catch accidental calls
        public DataTable GetStudentTable() => throw new NotImplementedException();
        public DataTable GetStudentTable(string dojoFilter) => throw new NotImplementedException();
        public DataTable GetListOfArts() => throw new NotImplementedException();
        public bool AddNewStudent(Student student) => throw new NotImplementedException();
        public bool UpdateStudent(Student student) => throw new NotImplementedException();
        public bool DeleteStudent(int studentID) => throw new NotImplementedException();
        public DataTable GetStudentPromotionHistory(int studentID) => throw new NotImplementedException();
        public DataTable GetStudentSignInHistory(int studentID) => throw new NotImplementedException();
        public DataTable GetStudentArtsAndRanks(int studentID) => throw new NotImplementedException();
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
    }
}
