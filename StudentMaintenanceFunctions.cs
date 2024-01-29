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
        public Student PopulateStudentData(IDataAccess dataAccess, int studentID)
        {
            //TODO: Remove database implementation logic to a lower-level class
            //TODO: Considerations: A lot can go wrong here. What if the student doesn't exist? What if the database is down?
            Student currentStudent = new Student();

            DataTable studentDataTable = dataAccess.GetStudentTable();
            DataRow[] selectedStudent = studentDataTable.Select("stud_id = " + studentID);

            currentStudent.StudentID = studentID;
            currentStudent.FirstName = selectedStudent[0].Field<string>("stud_firstname");
            currentStudent.LastName = selectedStudent[0].Field<string>("stud_lastname");
            currentStudent.Address1 = selectedStudent[0].Field<string>("stud_addr1");
            currentStudent.Address2 = selectedStudent[0].Field<string>("stud_addr2");
            currentStudent.AddressCity = selectedStudent[0].Field<string>("stud_city");
            currentStudent.AddressState = selectedStudent[0].Field<string>("stud_state");
            currentStudent.AddressZip = selectedStudent[0].Field<string>("stud_zip");
            currentStudent.PrimaryPhoneNumber = selectedStudent[0].Field<string>("stud_homephone");
            currentStudent.SecondaryPhoneNumber = selectedStudent[0].Field<string>("stud_workphone");
            currentStudent.EmailAddress = selectedStudent[0].Field<string>("stud_email");
            currentStudent.HomeDojo = selectedStudent[0].Field<string>("stud_club");
            currentStudent.ActiveMember = selectedStudent[0].Field<string>("stud_status").Equals("A");
            currentStudent.HomeDojo = selectedStudent[0].Field<string>("stud_club");
            //TODO: Handle null values for date of birth
            currentStudent.DateOfBirth = selectedStudent[0].Field<DateTime>("stud_birthdate");
            currentStudent.StudentGender = GetStudentGender(selectedStudent[0].Field<string>("stud_gender"));

            PopulateArtsAndRanks(dataAccess, currentStudent);

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
            catch
            {
                return false;
            }
        }

        private void PopulateArtsAndRanks(IDataAccess dataAccess, Student student)
        {
            DataTable artsAndRanks = dataAccess.GetStudentArtsAndRanks(student.StudentID);

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
