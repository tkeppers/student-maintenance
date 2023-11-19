using System;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    class DatabaseAccess : IDataAccess
    {
        //Full path to Windsong dojo student database. 
        private string databasePath;

        //Password for database access
        string databasePassword;

        private string connectionString;

        public DatabaseAccess()
        {
            // Read values from configuration file
            databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            databasePassword = ConfigurationManager.AppSettings["DatabasePassword"];

            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + 
                databasePath + ";Jet OLEDB:Database Password=" + databasePassword;
        }

        //TODO: Refactor this into something maintainable
        //TODO: Implement logging (use SeriLog)
        public DataTable GetStudentTable()
        {
            DataTable dataTable;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = "select * from Students where stud_club='Windsong'";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                dataTable = dataset.Tables[0];
            }

            return dataTable;
        }

        public DataTable GetStudentArtsAndRanks(int studentID)
        {
            DataTable dataTable;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = "select * from StudArts where StudArt_ID=" + studentID;
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                dataTable = dataset.Tables[0];
            }

            return dataTable;
        }

        public DataTable GetStudentPromotionHistory(int studentID)
        {
            DataTable dataTable;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = "select * from Promo_History where promo_student=" + studentID;
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                dataTable = dataset.Tables[0];
            }

            return dataTable;
        }

        public DataTable GetStudentSignInHistory(int studentID)
        {
            DataTable dataTable;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = "select * from Signin_History where sign_student=" + studentID;
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                dataTable = dataset.Tables[0];
            }

            return dataTable;
        }

        public bool AddNewStudent(Student student)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"INSERT INTO Students (stud_status, 
                    stud_lastName,
                    stud_firstname,
                    stud_club,
                    stud_birthdate,
                    stud_addr1,
                    stud_addr2,
                    stud_city,
                    stud_state,
                    stud_zip,
                    stud_homephone,
                    stud_workphone,
                    stud_gender,
                    stud_email,
                    stud_start_month) 
                   VALUES (@Status, 
                    @LastName, 
                    @FirstName, 
                    @HomeDojo, 
                    @Birthdate, 
                    @Address1, 
                    @Address2, 
                    @City, 
                    @State, 
                    @Zip, 
                    @PrimaryPhone, 
                    @SecondaryPhone, 
                    @Gender, 
                    @Email, 
                    @StartMonth)", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@Status", OleDbType.VarChar).Value = student.ActiveMember ? "A" : "I";
                    command.Parameters.Add("@LastName", OleDbType.VarChar).Value = student.LastName;
                    command.Parameters.Add("@FirstName", OleDbType.VarChar).Value = student.FirstName;
                    command.Parameters.Add("@HomeDojo", OleDbType.VarChar).Value = student.HomeDojo;
                    command.Parameters.Add("@Birthdate", OleDbType.DBDate).Value = student.DateOfBirth;
                    command.Parameters.Add("@Address1", OleDbType.VarChar).Value = student.Address1;
                    command.Parameters.Add("@Address2", OleDbType.VarChar).Value = student.Address2;
                    command.Parameters.Add("@City", OleDbType.VarChar).Value = student.AddressCity;
                    command.Parameters.Add("@State", OleDbType.VarChar).Value = student.AddressState;
                    command.Parameters.Add("@Zip", OleDbType.VarChar).Value = student.AddressZip;
                    command.Parameters.Add("@PrimaryPhone", OleDbType.VarChar).Value = student.PrimaryPhoneNumber;
                    command.Parameters.Add("@SecondaryPhone", OleDbType.VarChar).Value = student.SecondaryPhoneNumber;

                    if (student.StudentGender == Gender.MALE)
                        command.Parameters.Add("@Gender", OleDbType.VarChar).Value = "M";
                    else if (student.StudentGender == Gender.FEMALE)
                        command.Parameters.Add("@Gender", OleDbType.VarChar).Value = "F";
                    else
                        command.Parameters.Add("@Gender", OleDbType.VarChar).Value = "X";

                    command.Parameters.Add("@Email", OleDbType.VarChar).Value = student.EmailAddress;
                    command.Parameters.Add("@StartMonth", OleDbType.Integer).Value = 1; //TODO: Fix later
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show(@"Data Added");
                        connection.Close();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        MessageBox.Show(ex.Source + "\n" + ex.Message);
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    MessageBox.Show(@"Connection Failed");
                }
            }

            return success;
        }
    }
}
