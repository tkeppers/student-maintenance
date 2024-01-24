using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    internal class StudentSignInFunctions
    {
        /// <summary>
        /// Validates that the student is enrolled in the selected art and is not already signed in.
        /// Calls the necessary routines to record the student's sign in
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="studentID">ID number of student being signed in</param>
        /// <param name="studentArt">Martial art for which student is signing in</param>
        /// <returns>True if the transaction was successful</returns>
        /// 
        public bool SignInStudent(IDataAccess dataAccess, int studentID, string studentArt)
        {
            if (!ValidateStudentIsEnrolledInArt(dataAccess, studentID, studentArt, out double cumulativeTrainingHours))
                return false;
            
            if (!ValidateStudentIsNotAlreadySignedIn(dataAccess, studentID, studentArt))
                return false;

            return dataAccess.UpdateStudentSignIn(studentID, studentArt, cumulativeTrainingHours);
        }

        private bool ValidateStudentIsEnrolledInArt(IDataAccess dataAccess, int studentID, string studentArt, out double cumulativeTrainingHours)
        {

            DataTable studentArtsTable = dataAccess.GetStudentArtsAndRanks(studentID);
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
                cumulativeTrainingHours = Convert.ToDouble(selectedStudentArt[0].Field<decimal>("studArt_cumm"));
                return true;
            }
        }

        private bool ValidateStudentIsNotAlreadySignedIn(IDataAccess dataAccess, int studentID, string studentArt)
        {
            DataTable studentSignInTable = dataAccess.GetStudentSignInHistory(studentID);

            //Students cannot sign in more than once for the same art within an hour on the same day. Check to see if the student has signed in within the last hour.
            DataRow[] selectedStudentSignIn = studentSignInTable.Select("sign_art = '" + studentArt + "' AND sign_date >= '" + DateTime.Now.AddHours(-1).ToString() + "'");

            if (selectedStudentSignIn.Length > 0)
            {
                MessageService.ShowErrorMessage("The student is already signed in to the selected art.", "Student Already Signed In");
                return false;
            }

            return true;
        }
    }
}
