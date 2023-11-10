using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    class StudentPromotionHistoryFunctions
    {
        IDataAccess dataAccess;
        int studentID;

        public StudentPromotionHistoryFunctions(IDataAccess dataAccess, int studentID)
        {
            this.dataAccess = dataAccess;
            this.studentID = studentID;
        }

        internal DataTable GetStudentPromotionHistory()
        {
            return dataAccess.GetStudentPromotionHistory(studentID);
        }
    }
}
