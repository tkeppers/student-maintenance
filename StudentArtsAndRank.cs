using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class StudentArtsAndRank
    {
        //TODO: Change the setters to private during refactoring
        public int StudentArtID { get; set; }
        public string StudentArt { get; set; }
        public string Rank { get; set; }
        public double HoursInArt { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateOfLatestSignIn { get; set; }
        public DateTime? DatePromoted { get; set; }
        public double PromotionHours { get; set; }
        public string NextRank { get; set; }
        public bool EligibleForPromotion { get; internal set; }

        /// <summary>
        /// Returns the amount of time in years since the student first began taking the martial art.
        /// </summary>
        /// <remarks>This logic is somewhat flawed, since it doesn't account for breaks in the art (example: student 
        /// started taking aikido 18 months ago, but had an 8 months break during that time).</remarks>
        public double TotalYearsInArt()
        {
            if (DateStarted == null)
                return 0;

            TimeSpan span = DateTime.Now - (DateTime)DateStarted;
            double yearsInArt = Math.Round(span.TotalDays / 365.25, 2); // Using 365.25 to account for leap years

            return yearsInArt;
        }

        /// <summary>
        /// Returns the amount of time in years the student has been at the current level (e.g. shodan) in the 
        /// martial art.
        /// </summary>
        public double YearsAtCurrentLevel()
        {
            if (DatePromoted == null)
                return 0;

            TimeSpan span = DateTime.Now - (DateTime)DatePromoted;
            double yearsAtCurrentLevel = Math.Round(span.TotalDays / 365.25, 2); // Using 365.25 to account for leap years

            return yearsAtCurrentLevel;
        }
    }
}
