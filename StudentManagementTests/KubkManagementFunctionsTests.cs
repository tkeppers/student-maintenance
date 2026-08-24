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

        #region SetDuesPaid

        [Test]
        public void SetDuesPaid_WhenMarkingPaid_RecordsAPaymentDatedToday()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.SetDuesPaid(7, 2026, true));

            Assert.AreEqual(1, repo.RecordedPayments.Count, "Marking paid should write one dues row");
            Assert.AreEqual(0, repo.RemovedPayments.Count);
            StudentDuesRecord recorded = repo.RecordedPayments[0];
            Assert.AreEqual(7, recorded.StudentID);
            Assert.AreEqual(2026, recorded.Year);
            Assert.AreEqual(DateTime.Today, recorded.PaidDate);
            Assert.IsTrue(recorded.IsPaid);
        }

        [Test]
        public void SetDuesPaid_WhenMarkingPaid_DoesNotRecordAnAmount()
        {
            // The dojo tracks money in its own accounting system; the row is only a confirmation
            // marker, so it must not imply an amount that was never collected here.
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            functions.SetDuesPaid(7, 2026, true);

            Assert.AreEqual(0m, repo.RecordedPayments[0].Amount);
        }

        [Test]
        public void SetDuesPaid_WhenClearing_RemovesThePayment()
        {
            var repo = new FakeKubkDataRepository();
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.SetDuesPaid(7, 2026, false));

            Assert.AreEqual(0, repo.RecordedPayments.Count);
            Assert.AreEqual(1, repo.RemovedPayments.Count);
            Assert.AreEqual(Tuple.Create(7, 2026), repo.RemovedPayments[0]);
        }

        [Test]
        public void SetDuesPaid_PropagatesRepositoryFailure()
        {
            var repo = new FakeKubkDataRepository { WritesSucceed = false };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.SetDuesPaid(7, 2026, true));
            Assert.IsFalse(functions.SetDuesPaid(7, 2026, false));
        }

        #endregion

        #region Deleting dojos

        [Test]
        public void CanDeleteDojo_WhenDojoHasNoStudents_ReturnsTrue()
        {
            var dojo = new Dojo { ClubID = "EMPTY", Name = "Empty Dojo" };

            Assert.IsTrue(KubkManagementFunctions.CanDeleteDojo(dojo, 0, out string reason));
            Assert.IsEmpty(reason);
        }

        [Test]
        public void CanDeleteDojo_WhenDojoHasStudents_ReturnsFalseAndExplainsWhy()
        {
            var dojo = new Dojo { ClubID = "DENTON", Name = "Denton" };

            Assert.IsFalse(KubkManagementFunctions.CanDeleteDojo(dojo, 12, out string reason));
            StringAssert.Contains("12", reason, "The reason should say how many students block the delete");
            StringAssert.Contains("Active", reason, "The reason should point the user at deactivating instead");
        }

        [Test]
        public void CanDeleteDojo_WithNoDojoSelected_ReturnsFalse()
        {
            Assert.IsFalse(KubkManagementFunctions.CanDeleteDojo(null, 0, out _));
            Assert.IsFalse(KubkManagementFunctions.CanDeleteDojo(new Dojo { ClubID = "  " }, 0, out _));
        }

        [Test]
        public void GetStudentCountForDojo_MatchesCaseInsensitivelyAndDefaultsToZero()
        {
            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { { "DENTON", 7 } };

            Assert.AreEqual(7, KubkManagementFunctions.GetStudentCountForDojo(counts, "denton"));
            Assert.AreEqual(7, KubkManagementFunctions.GetStudentCountForDojo(counts, "  DENTON "));
            Assert.AreEqual(0, KubkManagementFunctions.GetStudentCountForDojo(counts, "NOBODY"));
            Assert.AreEqual(0, KubkManagementFunctions.GetStudentCountForDojo(null, "DENTON"));
        }

        [Test]
        public void DeleteDojo_WhenDojoIsEmpty_DeletesIt()
        {
            var repo = new FakeKubkDataRepository();
            var dojo = new Dojo { ClubID = "EMPTY", Name = "Empty Dojo" };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsTrue(functions.DeleteDojo(dojo, out string error));
            Assert.IsEmpty(error);
            CollectionAssert.AreEqual(new[] { "EMPTY" }, repo.DeletedDojos);
        }

        [Test]
        public void DeleteDojo_RefusesWhenTheDatabaseStillShowsStudents()
        {
            // Guards against a stale screen: the button may have been enabled when the form
            // loaded, but the headcount is re-checked against the database before deleting.
            var repo = new FakeKubkDataRepository();
            repo.StudentCounts["DENTON"] = 3;
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.DeleteDojo(new Dojo { ClubID = "DENTON", Name = "Denton" }, out string error));
            Assert.IsNotEmpty(error);
            CollectionAssert.IsEmpty(repo.DeletedDojos, "Nothing should be deleted when students still reference the dojo");
        }

        [Test]
        public void DeleteDojo_PropagatesRepositoryFailure()
        {
            var repo = new FakeKubkDataRepository { WritesSucceed = false };
            var functions = new KubkManagementFunctions(repo);

            Assert.IsFalse(functions.DeleteDojo(new Dojo { ClubID = "EMPTY", Name = "Empty" }, out string error));
            Assert.IsNotEmpty(error);
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
        public List<StudentDuesRecord> RecordedPayments = new List<StudentDuesRecord>();
        public List<Tuple<int, int>> RemovedPayments = new List<Tuple<int, int>>();
        public bool WritesSucceed = true;

        public List<StudentDuesRecord> GetDuesHistory(int studentID) => DuesHistory;

        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate) => true;

        public bool RecordDuesPayment(StudentDuesRecord dues)
        {
            RecordedPayments.Add(dues);
            return WritesSucceed;
        }

        public bool RemoveDuesPayment(int studentID, int year)
        {
            RemovedPayments.Add(Tuple.Create(studentID, year));
            return WritesSucceed;
        }
        public List<Dojo> Dojos = new List<Dojo>();
        public Dictionary<string, int> StudentCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public List<string> DeletedDojos = new List<string>();

        public List<Dojo> GetDojos() => Dojos;
        public bool AddDojo(Dojo dojo) => true;
        public bool UpdateDojo(Dojo dojo) => true;
        public Dictionary<string, int> GetStudentCountsByDojo() => StudentCounts;

        public bool DeleteDojo(string clubId)
        {
            DeletedDojos.Add(clubId);
            return WritesSucceed;
        }

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
