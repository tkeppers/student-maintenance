using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class PromotionRequirements
    {
        public string CurrentRank { get; private set; }
        public string NextRank { get; private set; }
        public string CurrentArt { get; private set; }
        public double MinimumTrainingHours { get; private set; }
        public double MinimumAge { get; private set; }
        public double YearsInArt { get; private set; }
        public double YearsAtCurrentRank { get; private set; }

        DataTable promotionRequirementsTable;

        public PromotionRequirements()
        {

        }

        public PromotionRequirements(DataTable promotionRequirementsTable /*, StudentArtsAndRank art*/)
        {
            this.promotionRequirementsTable = promotionRequirementsTable;

            //GetNextPromotionCriteria(art);
        }

        public PromotionRequirements GetNextPromotionCriteria(StudentArtsAndRank art)
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

            return this;
        }

        public DataTable GetPromotionRequirementsData()
        {
            return this.promotionRequirementsTable;
        }

    }
}
