using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public interface IDataRepository
    {
        DataTable GetListOfArts();

        DataTable GetStudentTable();

        bool AddNewStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(int studentID);

        DataTable GetStudentPromotionHistory(int studentID);
        DataTable GetStudentSignInHistory(int studentID);

        DataTable GetStudentArtsAndRanks(int studentID);
        bool AddNewStudentArt(StudentArtsAndRank artsAndRank);
        bool UpdateStudentArt(StudentArtsAndRank artsAndRank);
        bool DeleteStudentArt(int studentArtID, string studentArtName);

        bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank);

        bool UpdateStudentSignIn(int studentID, string studentArtName, double cumulativeTrainingHours, out double newCumulativeHours);

        //bool AddPromotionCriteria(PromotionCriteria promotionCriteria)
        void UpdatePromotionCriteria(DataTable promotionCriteriaTable);
        //bool DeletePromotionCriteria(string artName, string rankName);
        DataTable GetStudentPromotionRequirements();

    }
}
