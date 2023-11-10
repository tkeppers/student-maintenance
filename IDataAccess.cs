using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{ 
    public interface IDataAccess
    {
        DataTable GetStudentTable();

        DataTable GetStudentArtsAndRanks(int studentID);
        DataTable GetStudentPromotionHistory(int studentID);
    }
}
