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

        bool AddPromotionCriteria(PromotionCriteria promotionCriteria);

        bool ModifyPromotionCriteria(PromotionCriteria promotionCriteria);

        bool DeletePromotionCriteria(string artName, string rankName);

        DataTable GetListOfArts();

        bool AddNewStudent(Student student);

        bool UpdateStudent(Student student);

        bool DeleteStudent(int studentID);

        bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank);

        bool AddNewStudentArt(StudentArtsAndRank artsAndRank);

        bool UpdateStudentArt(StudentArtsAndRank artsAndRank);

        bool DeleteStudentArt(int studentArtID, string studentArtName);

        bool DatabaseExistsAndIsValid();
    }
}
