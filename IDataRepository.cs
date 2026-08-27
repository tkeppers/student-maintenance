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
        DataTable GetStudentTable(string dojoFilter);

        bool AddNewStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(int studentID);

        DataTable GetStudentPromotionHistory(int studentID);
        DataTable GetStudentSignInHistory(int studentID);

        DataTable GetStudentArtsAndRanks(int studentID);
        bool AddNewStudentArt(StudentArtsAndRank artsAndRank);
        bool UpdateStudentArt(StudentArtsAndRank artsAndRank);
        bool DeleteStudentArt(int studentArtID, string studentArtName);

        bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank, string recommendedBy = null);

        bool UpdateStudentSignIn(int studentID, string studentArtName, double cumulativeTrainingHours, out double newCumulativeHours);

        //bool AddPromotionCriteria(PromotionCriteria promotionCriteria)
        void UpdatePromotionCriteria(DataTable promotionCriteriaTable);
        //bool DeletePromotionCriteria(string artName, string rankName);
        DataTable GetStudentPromotionRequirements();

        // KUBK organization tracking (dojo management, multi-dojo roster, dues, rank verification)
        List<Dojo> GetDojos();
        bool AddDojo(Dojo dojo);
        bool UpdateDojo(Dojo dojo);
        bool DeleteDojo(string clubId);

        /// <summary>Student headcount per dojo, keyed by the Students.stud_club value.</summary>
        Dictionary<string, int> GetStudentCountsByDojo();

        DataTable GetKubkRoster(string clubId, int duesYear);

        /// <summary>The rank ladder from the Ranks table, ordered from lowest rank to highest.</summary>
        List<Rank> GetRankLadder();

        bool RecordDuesPayment(StudentDuesRecord dues);
        bool RemoveDuesPayment(int studentID, int year);
        List<StudentDuesRecord> GetDuesHistory(int studentID);

        bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate);

        /// <summary>
        /// Administrative rank correction: updates the recorded rank and stamps it verified.
        /// Deliberately writes no Promo_History row - this fixes a record, it is not a promotion.
        /// </summary>
        bool CorrectStudentRank(int studentID, string artName, string newRank, DateTime verifiedDate);

        bool SetStudentActiveStatus(int studentID, bool active);

        /// <summary>Student/art rows for the rank register, with student contact details.</summary>
        DataTable GetRankRegister(string clubId, bool includeWindsong);

        /// <summary>Every promotion history row carrying a recommender, for the rank register.</summary>
        DataTable GetPromotionRecommenders();

        /// <summary>
        /// Student/art rows for the activity report: rank, hours, last sign-in, and the dates
        /// promotion eligibility is calculated from.
        /// </summary>
        DataTable GetStudentActivity(string clubId);

        /// <summary>
        /// Every sign-in event in a date range, one row per attendance. Both bounds are
        /// inclusive of the whole day.
        /// </summary>
        DataTable GetSignInHistory(string clubId, DateTime fromDate, DateTime toDate);

        /// <summary>Dues-paid dates for one year, keyed by student id.</summary>
        Dictionary<int, DateTime?> GetDuesPaidDatesForYear(int duesYear);
    }
}
