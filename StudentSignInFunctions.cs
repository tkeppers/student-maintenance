using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    internal class StudentSignInFunctions
    {
        private static ILogger _signInLogger;

        /// <summary>
        /// Validates that the student is enrolled in the selected art and is not already signed in.
        /// Calls the necessary routines to record the student's sign in
        /// </summary>
        /// <param name="dataRepository"></param>
        /// <param name="studentID">ID number of student being signed in</param>
        /// <param name="studentArt">Martial art for which student is signing in</param>
        /// <returns>True if the transaction was successful</returns>
        /// 
        public bool SignInStudent(IDataRepository dataRepository, int studentID, string studentArt, out string signinMessage)
        {
            signinMessage = string.Empty;
            if (!ValidateStudentIsEnrolledInArt(dataRepository, studentID, studentArt, out double cumulativeTrainingHours))
            {
                signinMessage = $"Student is not currently enrolled in {studentArt}";
                return false;
            }

            if (!ValidateStudentIsNotAlreadySignedIn(dataRepository, studentID, studentArt))
            {
                signinMessage = $"Duplicate sign-in detected for student in {studentArt}";
                return false;
            }

            bool success = dataRepository.UpdateStudentSignIn(studentID, studentArt, cumulativeTrainingHours);

            if (success)
                signinMessage = $"Cumulative training hours in {studentArt}: {cumulativeTrainingHours}";

            return success;
        }

        private bool ValidateStudentIsEnrolledInArt(IDataRepository dataRepository, int studentID, string studentArt, out double cumulativeTrainingHours)
        {

            DataTable studentArtsTable = dataRepository.GetStudentArtsAndRanks(studentID);
            DataRow[] selectedStudentArt = studentArtsTable.Select("studArt_art = '" + studentArt + "'");

            if (selectedStudentArt.Length == 0)
            {
                MessageService.ShowErrorMessage("The student is not enrolled in the selected art.", "Student Not Enrolled");
                cumulativeTrainingHours = 0.0;
                return false;
            }
            else
            {
                //Note: The cumulative training hours are stored as a decimal in the database, but the field is defined as a double
                //in the StudentArtsAndRank class. There is no need to have decimal precision for the cumulative training hours, and 
                //the data in the database can be adequately represented as a double, with better performance.
                if (selectedStudentArt[0].IsNull("studArt_cumm"))
                    cumulativeTrainingHours = 0;
                else
                    cumulativeTrainingHours = Convert.ToDouble(selectedStudentArt[0].Field<decimal>("studArt_cumm"));
                return true;
            }
        }

        private bool ValidateStudentIsNotAlreadySignedIn(IDataRepository dataRepository, int studentID, string studentArt)
        {
            DataTable studentSignInTable = dataRepository.GetStudentSignInHistory(studentID);

            //Students cannot sign in more than once for the same art within an hour on the same day. Check to see if the student has signed in within the last hour.
            DataRow[] selectedStudentSignIn = studentSignInTable.Select("sign_art = '" + studentArt + "' AND sign_date >= '" + DateTime.Now.AddHours(-1).ToString() + "'");

            if (selectedStudentSignIn.Length > 0)
            {
                MessageService.ShowErrorMessage("The student is already signed in to the selected art.", "Student Already Signed In");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the student is eligible for promotion to the next rank in the selected art
        /// </summary>
        /// <returns>True if student is eligigle for promotion, false otherwise</returns>
        public bool IsEligibleForPromotion(IDataRepository dataRepository, int studentID, string studentArt, out string promotionMessage)
        {
            promotionMessage = string.Empty;
            try
            {
                // To determine promotion eligibility, we need the student's birthdate, the date they started training, the date they were
                // last promoted, the number of hours training in art, and the promotion requirements for their current rank. To do this, we 
                // will need to create the entire student object from the database (which will require an additional database hit)
                StudentMaintenanceFunctions s = new StudentMaintenanceFunctions();
                Student student = s.PopulateStudentData(dataRepository, studentID);

                // TODO: Not sure I like this approach because it duplicates some code from the StudentMaintenanceUI form. However, to not violate the 
                // DRY principle, we would need to refactor the Student class to include the promotion requirements and the student's current rank.
                DataTable promotionRequirements = dataRepository.GetStudentPromotionRequirements();
                PromotionCriteria eligibility = new PromotionCriteria(promotionRequirements);

                StudentArtsAndRank signInArtRank = student.StudentArtsAndRanks.FirstOrDefault(sar => sar.StudentArt == studentArt);

                eligibility.GetNextPromotionCriteria(signInArtRank);
                bool isEligible = student.IsEligibleForPromotion(signInArtRank, eligibility);
                if (isEligible)
                       promotionMessage = $"{student.FullName} is eligible for promotion to {signInArtRank.NextRank} in {studentArt}";

                return isEligible;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Error determining promotion eligibility\n{ex.Message}\n{ex.Source}\n{ex.Data}\n{ex.StackTrace}");
                return false;
            }
        }

        public static void SetupLogging()
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            var logFilePath = Path.Combine(logDirectory, "StudentSignInLog-.log");
            Directory.CreateDirectory(logDirectory);

            _signInLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(path: logFilePath, rollingInterval: RollingInterval.Month,
                    rollOnFileSizeLimit: true, retainedFileCountLimit: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        public static void LogStudentSignIn(string message)
        {
            _signInLogger.Information(message);
        }
    }
}
