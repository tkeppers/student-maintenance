using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class PromotionCriteria
    {
        public string CurrentRank { get; set; }
        public string NextRank { get; set; }
        public string CurrentArt { get; set; }
        public double MinimumTrainingHours { get; set; }
        public double MinimumAge { get; set; }
        public double YearsInArt { get; set; }
        public double YearsAtCurrentRank { get; set; }

        DataTable promotionRequirementsTable;

        public PromotionCriteria()
        {

        }

        public PromotionCriteria(DataTable promotionRequirementsTable /*, StudentArtsAndRank art*/)
        {
            this.promotionRequirementsTable = promotionRequirementsTable;
        }

        public PromotionCriteria GetNextPromotionCriteria(StudentArtsAndRank art)
        {
            string filter = $"Art = '{art.StudentArt}' AND CurrentRank = '{art.Rank}'";
            DataRow[] nextPromotionInfo = promotionRequirementsTable.Select(filter);

            //TODO: Check for null. Maybe do TOUPPER on strings?
            if (nextPromotionInfo.Count() == 1)
            {
                CurrentArt = art.StudentArt;
                CurrentRank = art.Rank;
                NextRank = nextPromotionInfo[0]["NextRank"].ToString();
                MinimumTrainingHours = double.TryParse(nextPromotionInfo[0]["MinimumTrainingHours"].ToString(), out double minimumTrainingHours) ? minimumTrainingHours : 0;
                MinimumAge = double.TryParse(nextPromotionInfo[0]["MinimumAge"].ToString(), out double minimumAge) ? minimumAge: 0;
                YearsInArt = double.TryParse(nextPromotionInfo[0]["YearsInArt"].ToString(), out double yearsInArt) ? yearsInArt : 0;
                YearsAtCurrentRank = double.TryParse(nextPromotionInfo[0]["YearsAtCurrentRank"].ToString(), out double yearsAtCurrentRank) ? yearsAtCurrentRank : 0;
            }

            art.NextRank = NextRank;
            return this;
        }

        public DataTable GetPromotionRequirementsData()
        {
            return this.promotionRequirementsTable;
        }

    }
}
