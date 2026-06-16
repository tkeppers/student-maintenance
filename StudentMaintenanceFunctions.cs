using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    public class StudentMaintenanceFunctions
    {
        public Student PopulateStudentData(IDataRepository dataRepository, int studentID)
        {
            Student currentStudent = new Student();

            DataTable studentDataTable = dataRepository.GetStudentTable();
            DataRow[] selectedStudent = studentDataTable.Select("StudentID = " + studentID);

            if (selectedStudent.Length == 0)
            {
                Log.Error($"PopulateStudentData: No student found with ID {studentID}");
                return currentStudent;
            }

            var row = selectedStudent[0];

            currentStudent.StudentID = studentID;
            currentStudent.FirstName = row.Field<string>("StudentFirstName");
            currentStudent.LastName = row.Field<string>("StudentLastName");
            currentStudent.Address1 = row.Field<string>("StudentAddress1");
            currentStudent.Address2 = row.Field<string>("StudentAddress2");
            currentStudent.AddressCity = row.Field<string>("StudentCity");
            currentStudent.AddressState = row.Field<string>("StudentState");
            currentStudent.AddressZip = row.Field<string>("StudentPostalCode");
            currentStudent.PrimaryPhoneNumber = row.Field<string>("StudentPrimaryPhone");
            currentStudent.SecondaryPhoneNumber = row.Field<string>("StudentSecondaryPhone");
            currentStudent.EmailAddress = row.Field<string>("StudentEmailAddress");
            currentStudent.HomeDojo = row.Field<string>("StudentDojo");

            currentStudent.ActiveMember =
                string.Equals(row.Field<string>("StudentStatus"), "A", StringComparison.Ordinal);

            if (row["StudentBirthDate"] != DBNull.Value)
                currentStudent.DateOfBirth = (DateTime)row["StudentBirthDate"];

            currentStudent.StudentGender = GetStudentGender(row.Field<string>("StudentGender"));

            PopulateArtsAndRanks(dataRepository, currentStudent);

            return currentStudent;
        }

        public Gender GetStudentGender(string gender)
        {
            if (gender == null)
                return Gender.UNKNOWN;

            if (gender.ToUpper().Contains("F"))
                return Gender.FEMALE;
            else if (gender.ToUpper().Contains("M"))
                return Gender.MALE;
            else
                return Gender.UNKNOWN;
        }

        public bool IsValidStudent(int studentID)
        {   
            if (studentID <= 0)
            {
                MessageService.ShowErrorMessage("Please select a valid student", "Student Not Selected");
                return false;
            }

            return true;
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;
            
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch (Exception e)
            {
                Log.Error($"Error validating email address {email}\n{e.Message}\n{e.Source}\n{e.StackTrace}");
                return false;
            }
        }

        private void PopulateArtsAndRanks(IDataRepository dataRepository, Student student)
        {
            DataTable artsAndRanks = dataRepository.GetStudentArtsAndRanks(student.StudentID);

            foreach (DataRow row in artsAndRanks.Rows)
            {
                StudentArtsAndRank artsAndRank = new StudentArtsAndRank
                {
                    StudentArtID = int.TryParse(row["StudArt_ID"].ToString(), out int studentArtID) ? studentArtID : 0,
                    StudentArt = row["studArt_art"].ToString(),
                    Rank = row["studArt_rank"].ToString(),
                    DateStarted =
                    DateTime.TryParse(row["studArt_begin"].ToString(), out DateTime startDate)
                        ? startDate : (DateTime?)null,
                    HoursInArt = double.TryParse(row["studArt_cumm"].ToString(), out double cumulativeHours)
                        ? cumulativeHours : 0.0,
                    DateOfLatestSignIn = DateTime.TryParse(row["studArt_signin"].ToString(), out DateTime lastSignInDate)
                        ? lastSignInDate : (DateTime?)null,
                    DatePromoted =
                    DateTime.TryParse(row["studArt_prodate"].ToString(), out DateTime promotionDate)
                        ? promotionDate : (DateTime?)null,
                    PromotionHours = double.TryParse(row["studArt_prohrs"].ToString(), out double promotionHours)
                        ? promotionHours : 0.0
                };

                student.StudentArtsAndRanks.Add(artsAndRank);
            }
        }
    }
}
