using NUnit.Framework;
using DojoStudentManagement;

namespace StudentManagementTests
{
    public class Tests
    {
        StudentMaintenanceFunctions studentMaintenanceFunctions;
        [SetUp]
        public void Setup()
        {
            studentMaintenanceFunctions = new StudentMaintenanceFunctions();
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(15, true)]
        [TestCase(9047, true)]
        [Test]
        public void IsValidStudentIDTest(int studentID, bool expectedResult)
        {
            if (studentMaintenanceFunctions.IsValidStudent(studentID) == expectedResult)
                Assert.Pass();
            else
                Assert.Fail();
        }

        [TestCase("Male", Gender.MALE)]
        [TestCase("man", Gender.MALE)]
        [TestCase("m", Gender.MALE)]
        [TestCase("Female", Gender.FEMALE)]
        [TestCase("F", Gender.FEMALE)]
        [TestCase("fem", Gender.FEMALE)]
        [TestCase("Nonbinary", Gender.UNKNOWN)]
        [TestCase("X", Gender.UNKNOWN)]
        [TestCase("", Gender.UNKNOWN)]
        [TestCase(null, Gender.UNKNOWN)]
        [Test]
        public void GetStudentGenderTest(string genderString, Gender gender)
        {
            if (studentMaintenanceFunctions.GetStudentGender(genderString) == gender)
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}