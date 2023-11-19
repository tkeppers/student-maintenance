using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    class StudentMaintenanceFunctions
    {
        internal Student PopulateStudentData(IDataAccess dataAccess, int studentID)
        {
            //TODO: Remove database implementation logic to a lower-level class
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
            currentStudent.DateOfBirth = selectedStudent[0].Field<DateTime>("stud_birthdate");
            currentStudent.StudentGender = GetStudentGender(selectedStudent[0].Field<string>("stud_gender"));

            PopulateArtsAndRanks(dataAccess, currentStudent);

            return currentStudent;
        }

        private Gender GetStudentGender(string gender)
        {
            if (gender.ToUpper().Contains("M"))
                return Gender.MALE;
            else if (gender.ToUpper().Contains("F"))
                return Gender.FEMALE;
            else
                return Gender.UNKNOWN;
        }

        internal bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
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
                StudentArtsAndRank artsAndRank = new StudentArtsAndRank();
                artsAndRank.Art = row["studArt_art"].ToString();
                artsAndRank.Rank = row["studArt_rank"].ToString();
                artsAndRank.DateStarted =
                    DateTime.TryParse(row["studArt_begin"].ToString(), out DateTime startDate)
                        ? startDate : (DateTime?)null; 
                artsAndRank.DatePromoted = 
                    DateTime.TryParse(row["studArt_prodate"].ToString(), out DateTime promotionDate)
                        ? promotionDate : (DateTime?)null;
                artsAndRank.PromotionHours = double.TryParse(row["studArt_prohrs"].ToString(), out double promotionHours)
                        ? promotionHours : 0.0;

                student.StudentArtsAndRanks.Add(artsAndRank);
            }
        }
    }
}
