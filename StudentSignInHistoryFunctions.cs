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
        IDataRepository dataRepository;
        int studentID;

        public StudentSignInHistoryFunctions(IDataRepository dataRepository, int studentID)
        {
            this.dataRepository = dataRepository;
            this.studentID = studentID;
        }

        internal DataTable GetStudentSignInHistory()
        {
            return dataRepository.GetStudentSignInHistory(this.studentID);
        }
    }
}
