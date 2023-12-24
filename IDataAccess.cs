using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{ 
    //TODO: Refactor this into a series of Repository classes for each data object
    public interface IDataAccess
    {
        DataTable GetStudentTable();

        DataTable GetStudentArtsAndRanks(int studentID);

        DataTable GetStudentPromotionHistory(int studentID);

        DataTable GetStudentSignInHistory(int studentID);

        DataTable GetStudentPromotionRequirements();

        bool AddNewStudent(Student student);

        bool UpdateStudent(Student student);

        bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank);

        bool AddNewStudentArt(StudentArtsAndRank artsAndRank);

        bool UpdateStudentArt(StudentArtsAndRank artsAndRank);

        bool DatabaseExistsAndIsValid();
    }
}
