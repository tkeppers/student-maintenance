using NUnit.Framework;
using DojoStudentManagement;
using System;

namespace DojoStudentManagementTests
{
    [TestFixture]
    public class StudentDuesRecordTests
    {
        [Test]
        public void IsPaid_WhenPaidDateIsSet_ReturnsTrue()
        {
            var dues = new StudentDuesRecord { PaidDate = DateTime.Now };

            Assert.IsTrue(dues.IsPaid);
        }

        [Test]
        public void IsPaid_WhenPaidDateIsNull_ReturnsFalse()
        {
            var dues = new StudentDuesRecord { PaidDate = null };

            Assert.IsFalse(dues.IsPaid);
        }
    }
}
