using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    class StudentSignInHistoryFunctions
    {
        IDataAccess dataAccess;
        int studentID;

        public StudentSignInHistoryFunctions(IDataAccess dataAccess, int studentID)
        {
            this.dataAccess = dataAccess;
            this.studentID = studentID;
        }

        internal DataTable GetStudentSignInHistory()
        {
            return dataAccess.GetStudentSignInHistory(this.studentID);
        }
    }
}
