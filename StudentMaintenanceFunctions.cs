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
            //TODO: Remove database implementation logic to a lower-level class
            //TODO: Considerations: A lot can go wrong here. What if the student doesn't exist? What if the database is down?
            Student currentStudent = new Student();

            DataTable studentDataTable = dataRepository.GetStudentTable();
            DataRow[] selectedStudent = studentDataTable.Select("StudentID = " + studentID);

            currentStudent.StudentID = studentID;
            currentStudent.FirstName = selectedStudent[0].Field<string>("StudentFirstName");
            currentStudent.LastName = selectedStudent[0].Field<string>("StudentLastName");
            currentStudent.Address1 = selectedStudent[0].Field<string>("StudentAddress1");
            currentStudent.Address2 = selectedStudent[0].Field<string>("StudentAddress2");
            currentStudent.AddressCity = selectedStudent[0].Field<string>("StudentCity");
            currentStudent.AddressState = selectedStudent[0].Field<string>("StudentState");
            currentStudent.AddressZip = selectedStudent[0].Field<string>("StudentPostalCode");
            currentStudent.PrimaryPhoneNumber = selectedStudent[0].Field<string>("StudentPrimaryPhone");
            currentStudent.SecondaryPhoneNumber = selectedStudent[0].Field<string>("StudentSecondaryPhone");
            currentStudent.EmailAddress = selectedStudent[0].Field<string>("StudentEmailAddress");
            currentStudent.HomeDojo = selectedStudent[0].Field<string>("StudentDojo");
            currentStudent.ActiveMember = selectedStudent[0].Field<string>("StudentStatus").Equals("A");
            currentStudent.HomeDojo = selectedStudent[0].Field<string>("StudentDojo");

            if (selectedStudent[0]["StudentBirthDate"] != DBNull.Value)
                currentStudent.DateOfBirth = (DateTime)selectedStudent[0]["StudentBirthDate"];

            currentStudent.StudentGender = GetStudentGender(selectedStudent[0].Field<string>("StudentGender"));

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

        internal bool IsValidEmail(string email)
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
