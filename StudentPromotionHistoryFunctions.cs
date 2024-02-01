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
        IDataRepository dataRepository;
        readonly int studentID;

        public StudentPromotionHistoryFunctions(IDataRepository dataRepository, int studentID)
        {
            this.dataRepository = dataRepository;
            this.studentID = studentID;
        }

        internal DataTable GetStudentPromotionHistory()
        {
            return dataRepository.GetStudentPromotionHistory(studentID);
        }
    }
}
