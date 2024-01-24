using System;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using Serilog;
using System.Data.SqlClient;

namespace DojoStudentManagement
{
    class DatabaseAccess : IDataAccess
    {
        private readonly string databasePath;
        private readonly string databasePassword;
        private readonly string connectionString;

        public DatabaseAccess()
        {
            // Read values from configuration file
            databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            databasePassword = ConfigurationManager.AppSettings["DatabasePassword"];

            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + 
                databasePath + ";Jet OLEDB:Database Password=" + databasePassword;
        }

        public bool DatabaseExistsAndIsValid()
        {
            if (!System.IO.File.Exists(databasePath))
            {
                string errText = "Database is invalid or inaccessible. Please check the database path in application settings.";
                MessageService.ShowErrorMessage(errText, "Database Access Error");
                Log.Error($"{DateTime.Now}: Database file not found at {databasePath}");
                return false;
            }

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (OleDbException e)
                {
                    MessageService.ShowErrorMessage("Error connecting to database", "Database Access Error");
                    Log.Error($"{DateTime.Now}: Error connecting to database {databasePath}\n{e.Message}\n{e.Source}\n{e.StackTrace}");
                    return false;
                }
            }
        }

        public DataTable GetStudentTable()
        {
            return ExecuteQuery("select * from Students where stud_club='Windsong'");
        }

        public DataTable GetStudentArtsAndRanks(int studentID)
        {
            return ExecuteQuery($"select * from StudArts where StudArt_ID={studentID}");
        }

        public DataTable GetStudentPromotionHistory(int studentID)
        {
            return ExecuteQuery($"select * from Promo_History where promo_student={studentID}");
        }

        public DataTable GetStudentSignInHistory(int studentID)
        {
            return ExecuteQuery($"select * from Signin_History where sign_student={studentID}");
        }

        public DataTable GetListOfArts()
        {
            return ExecuteQuery("select art_id from Arts");
        }

        private DataTable ExecuteQuery(string sql)
        {
            DataTable dataTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                    dataAdapter.Fill(dataTable);
                }
                catch (OleDbException ex)
                {
                    Log.Error($"{DateTime.Now}: Error executing query:\n{sql}\n{ ex.Message}\n{ ex.Source}\n{ ex.StackTrace}");
                    return new DataTable();
                }
            }

            return dataTable;
        }

        #region PromotionCriteria

        /// <summary>
        /// Gets the list of requirements for promotion to each level for each art available
        /// </summary>
        /// <remarks>We will read in the entire table (it's not very big) to minimize databate hits. This
        /// table will rarely ever be updated.</remarks>
        public DataTable GetStudentPromotionRequirements()
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = "SELECT rank_art, rank_id, rank_next, rank_min_hours, " +
                    "rank_min_age, rank_total_years, rank_time_in_rank " +
                    "FROM Promo_Requirements";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataSet dataset = new DataSet();

                try
                {
                    connection.Open();
                    dataAdapter.Fill(dataset);
                    DataTable dataTable = dataset.Tables[0];

                    // Rename columns from database schema to something more generic and readable
                    dataTable.Columns["rank_art"].ColumnName = "Art";
                    dataTable.Columns["rank_id"].ColumnName = "CurrentRank";
                    dataTable.Columns["rank_next"].ColumnName = "NextRank";
                    dataTable.Columns["rank_min_hours"].ColumnName = "MinimumTrainingHours";
                    dataTable.Columns["rank_min_age"].ColumnName = "MinimumAge";
                    dataTable.Columns["rank_total_years"].ColumnName = "YearsInArt";
                    dataTable.Columns["rank_time_in_rank"].ColumnName = "YearsAtCurrentRank";

                    return dataTable;
                }
                catch (OleDbException ex)
                {
                    Log.Error($"{DateTime.Now}: Error retrieving student promotion requirements:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    return new DataTable();
                }
            }
        }

        public void UpdatePromotionCriteria(DataTable promotionCriteriaTable)
        {
            PromotionCriteria promotionCriteria = new PromotionCriteria();

            foreach (DataRow row in promotionCriteriaTable.Rows)
            {
                promotionCriteria = promotionCriteria.GetPromotionCriteriaFromDataRow(row);

                if (row.RowState == DataRowState.Modified)
                    ModifyPromotionCriteria(promotionCriteria);

                else if (row.RowState == DataRowState.Added)
                    AddPromotionCriteria(promotionCriteria);

                else if (row.RowState == DataRowState.Deleted)
                    DeletePromotionCriteria(promotionCriteria.CurrentArt, promotionCriteria.CurrentRank);
            }
        }

        private void SetPromotionCriteriaCommandParameters(OleDbCommand command, PromotionCriteria promotionCriteria)
        {
            command.Parameters.Add("@RankArt", OleDbType.VarChar).Value = promotionCriteria.CurrentArt;
            command.Parameters.Add("@RankName", OleDbType.VarChar).Value = promotionCriteria.CurrentRank;
            command.Parameters.Add("@NextRank", OleDbType.VarChar).Value = promotionCriteria.NextRank;
            command.Parameters.Add("@MinimumTrainingHours", OleDbType.Integer).Value = promotionCriteria.MinimumTrainingHours;
            command.Parameters.Add("@MinimumAgeForRank", OleDbType.Integer).Value = promotionCriteria.MinimumAge;
            command.Parameters.Add("@TotalYearsInArt", OleDbType.Double).Value = promotionCriteria.YearsInArt;
            command.Parameters.Add("@TotalYearsAtRank", OleDbType.Double).Value = promotionCriteria.YearsAtCurrentRank;
            command.Parameters.Add("@RankFee", OleDbType.Integer).Value = promotionCriteria.RankFee;
        }

        private bool AddPromotionCriteria(PromotionCriteria promotionCriteria)
        {
            //TODO: Check to make sure adding the new record does not violate a primary key constraint (rank_art + rank_id)
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"INSERT INTO Promo_Requirements (
                    rank_art,
                    rank_id,
                    rank_next,
                    rank_min_hours,
                    rank_min_age,
                    rank_total_years,
                    rank_time_in_rank,
                    rank_fee)
                VALUES (
                    @RankArt,
                    @RankName,
                    @NextRank,
                    @MinimumTrainingHours,
                    @MinimumAgeForRank,
                    @TotalYearsInArt,
                    @TotalYearsAtRank,
                    @RankFee)", connection);

                if (connection.State == ConnectionState.Open)
                {
                    SetPromotionCriteriaCommandParameters(command, promotionCriteria);

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Added new promotion requirement for {promotionCriteria.CurrentArt} - {promotionCriteria.CurrentRank}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error inserting into Promo_Requirements table.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when adding new promotion criteria.\n");
                }
            }

            return success;
        }
 
        private bool ModifyPromotionCriteria(PromotionCriteria promotionCriteria)
        {
            //Check to make sure adding the new record does not violate a primary key constraint (rank_art + rank_id)
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"UPDATE Promo_Requirements SET
                    rank_next = @NextRank,
                    rank_min_hours = @MinimumTrainingHours,
                    rank_min_age = @MinimumAgeForRank,
                    rank_total_years = @TotalYearsInArt,
                    rank_time_in_rank = @TotalYearsAtRank,
                    rank_fee = @RankFee
                    WHERE rank_art = @RankArt AND rank_id = @RankName", connection);

                if (connection.State == ConnectionState.Open)
                {
                    SetPromotionCriteriaCommandParameters(command, promotionCriteria);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Updated {rowsAffected} rows in the following query:\n{GetCommandLogString(command)}");
                        Log.Information($"Updating promotion requirement for {promotionCriteria.CurrentArt} - {promotionCriteria.CurrentRank}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error updating Promo_Requirements table.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when updating promotion criteria.\n");
                }
            }

            return success;
        }

        private string GetCommandLogString(OleDbCommand command)
        {
            StringBuilder logString = new StringBuilder();
            logString.AppendLine("Executing SQL Command: ");
            logString.AppendLine(command.CommandText);

            foreach (OleDbParameter param in command.Parameters)
            {
                logString.AppendLine($"Parameter: {param.ParameterName}, Value: {param.Value}");
            }

            return logString.ToString();
        }

        private bool DeletePromotionCriteria(string artName, string rankName)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"DELETE FROM Promo_Requirements
                    WHERE rank_art = @ArtName and rank_id = @RankName", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@ArtName", OleDbType.VarChar).Value = artName;
                    command.Parameters.Add("@RankName", OleDbType.VarChar).Value = rankName;

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Successfully deleted promotion criteria {artName} - {rankName}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error executing delete command.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error("DeletePromotionCriteria: Connection failed during delete operation.\n");
                }
            }

            return success;
        }

        #endregion PromotionCriteria

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
                    SetStudentCommandParameters(command, student, false);

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Added new student {student.FullName}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error inserting into Students table.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when adding new student.\n");
                }
            }

            return success;
        }

        public bool UpdateStudent(Student student)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"UPDATE Students SET 
                    stud_status = @Status,
                    stud_lastName = @LastName,
                    stud_firstname = @FirstName,
                    stud_club = @HomeDojo,
                    stud_birthdate = @Birthdate,
                    stud_addr1 = @Address1,
                    stud_addr2 = @Address2,
                    stud_city = @City,
                    stud_state = @State,
                    stud_zip = @Zip,
                    stud_homephone = @PrimaryPhone,
                    stud_workphone = @SecondaryPhone,
                    stud_gender = @Gender,
                    stud_email = @Email,
                    stud_start_month = @StartMonth
                    WHERE stud_id = @StudentID", connection);

                if (connection.State == ConnectionState.Open)
                {
                    SetStudentCommandParameters(command, student, true);

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Successfully updated student {student.FullName}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error executing update command.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed for completing student update.\n");
                }
            }

            return success;
        }

        private void SetStudentCommandParameters(OleDbCommand command, Student student, bool isUpdate = false)
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
            command.Parameters.Add("@StartMonth", OleDbType.Integer).Value = student.StartMonth;

            // Only add the StudentID parameter if this is an update operation
            if (isUpdate)
            {
                command.Parameters.Add("@StudentID", OleDbType.Integer).Value = student.StudentID;
            }
        }

        public bool DeleteStudent(int studentID)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"DELETE FROM Students
                    WHERE stud_id = @StudentID", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Successfully deleted student with ID {studentID}");

                        //Also remove the enrolled martial art records associated with this student ID
                        DeleteStudentArt(studentID);
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error executing delete command.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error("DeleteStudent: Connection failed during delete operation.\n");
                }
            }

            return success;
        }

        public bool AddNewStudentArt(StudentArtsAndRank artsAndRank) 
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (OleDbCommand command = new OleDbCommand(@"INSERT INTO StudArts (
                             StudArt_ID, 
                             studArt_art,
                             studArt_rank,
                             studArt_cumm,
                             studArt_begin,
                             studArt_prodate)
                            VALUES (
                             @StudentID,
                             @StudentArt,
                             @StudentRank,
                             @CumulativeHours,
                             @DateStarted,
                             @DatePromoted)", connection, transaction))
                        {
                            command.Parameters.Add("@StudentID", OleDbType.Integer).Value = artsAndRank.StudentArtID;
                            command.Parameters.Add("@StudentArt", OleDbType.VarChar).Value = artsAndRank.StudentArt;
                            command.Parameters.Add("@Rank", OleDbType.VarChar).Value = artsAndRank.Rank;
                            command.Parameters.Add("@CumulativeHours", OleDbType.Numeric).Value = artsAndRank.HoursInArt;
                            command.Parameters.Add("@DateStarted", OleDbType.DBDate).Value = artsAndRank.DateStarted;
                            command.Parameters.Add("@DatePromoted", OleDbType.DBDate).Value = artsAndRank.DateStarted;  //Since this is a brand-new art, the promotion date is the start date

                            command.ExecuteNonQuery();

                            Log.Information($"Successfully added {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
                        }

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"AddNewStudentArt: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }

        public bool UpdateStudentArt(StudentArtsAndRank artsAndRank)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (OleDbCommand command = new OleDbCommand(@"UPDATE StudArts SET 
                            studArt_rank = @Rank,
                            studArt_cumm = @CumulativeHours,
                            studArt_begin = @DateStarted
                            WHERE StudArt_ID = @ArtID AND studArt_art = @StudentArt", connection, transaction))
                        {
                            command.Parameters.Add("@Rank", OleDbType.VarChar).Value = artsAndRank.Rank;
                            command.Parameters.Add("@CumulativeHours", OleDbType.Numeric).Value = artsAndRank.HoursInArt;
                            command.Parameters.Add("@DateStarted", OleDbType.DBDate).Value = artsAndRank.DateStarted;
                            command.Parameters.Add("@ArtID", OleDbType.Integer).Value = artsAndRank.StudentArtID;
                            command.Parameters.Add("@StudentArt", OleDbType.VarChar).Value = artsAndRank.StudentArt;

                            command.ExecuteNonQuery();

                            Log.Information($"Successfully updated information in {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
                        }

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"UpdateStudentArt: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// If the studentArtName parameter is null, deletes all of the martial arts associated 
        /// with the studentArtID record. Otherwise, it deletes the specific record that matches
        /// both parameters.
        /// </summary>
        public bool DeleteStudentArt(int studentArtID, string studentArtName = null)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sqlCommandText = studentArtName != null
                            ? @"DELETE FROM StudArts WHERE StudArt_ID = @StudentArtID AND studArt_art = @StudentArtName"
                            : @"DELETE FROM StudArts WHERE StudArt_ID = @StudentArtID";

                        using (OleDbCommand command = new OleDbCommand(sqlCommandText, connection, transaction))
                        {
                            command.Parameters.Add("@StudentArtID", OleDbType.Integer).Value = studentArtID;

                            if (studentArtName != null)
                            {
                                command.Parameters.Add("@StudentArtName", OleDbType.VarChar).Value = studentArtName;
                            }

                            command.ExecuteNonQuery();

                            if (studentArtName != null)
                            {
                                Log.Information($"Successfully removed {studentArtName} for student ID {studentArtID}");
                            }
                            else
                            {
                                Log.Information($"Successfully removed all martial arts for student ID {studentArtID}");
                            }
                        }

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }

    #region StudentPromotion

        /// <summary>
        /// After a student is promoted, update the main arts/rank data with the new information, then 
        /// add a new record in the promotion history table.
        /// </summary>
        public bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        UpdateStudentArts(connection, transaction, artsAndRank);
                        InsertPromotionHistory(connection, transaction, studentID, artsAndRank);

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"{DateTime.Now}: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }

        private void UpdateStudentArts(OleDbConnection connection, OleDbTransaction transaction, StudentArtsAndRank artsAndRank)
        {
            using (OleDbCommand command = new OleDbCommand(@"UPDATE StudArts SET 
                studArt_rank = @NewRank,
                studArt_prodate = @PromotionDate,
                studArt_prohrs = @PromotionHours
                WHERE StudArt_ID = @ArtID AND studArt_art = @Art", connection, transaction))
            {
                command.Parameters.Add("@NewRank", OleDbType.VarChar).Value = artsAndRank.NextRank;
                command.Parameters.Add("@PromotionDate", OleDbType.DBDate).Value = artsAndRank.DatePromoted;
                command.Parameters.Add("@PromotionHours", OleDbType.Numeric).Value = artsAndRank.PromotionHours;
                command.Parameters.Add("@ArtID", OleDbType.Integer).Value = artsAndRank.StudentArtID;
                command.Parameters.Add("@Art", OleDbType.VarChar).Value = artsAndRank.StudentArt;

                command.ExecuteNonQuery();

                Log.Information($"Updated promotion date and rank in {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
            }
        }

        private void InsertPromotionHistory(OleDbConnection connection, OleDbTransaction transaction, int studentID, StudentArtsAndRank artsAndRank)
        {
            using (OleDbCommand command = new OleDbCommand(@"INSERT INTO Promo_History (
                promo_student, 
                promo_art,
                promo_date,
                promo_rank,
                promo_hours)
               VALUES (
                @StudentID,
                @PromotionArt,
                @PromotionDate,
                @PromotionRank,
                @PromotionHours)", connection, transaction))
            {
                command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
                command.Parameters.Add("@PromotionArt", OleDbType.VarChar).Value = artsAndRank.StudentArt;
                command.Parameters.Add("@PromotionDate", OleDbType.DBDate).Value = artsAndRank.DatePromoted;
                command.Parameters.Add("@PromotionRank", OleDbType.VarChar).Value = artsAndRank.NextRank.ToUpper();
                command.Parameters.Add("@PromotionHours", OleDbType.Double).Value = artsAndRank.HoursInArt;

                command.ExecuteNonQuery();

                Log.Information($"Updated promotion history in {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
            }
        }

    #endregion StudentPromotion

    #region StudentSignIn

        /// <summary>
        /// Updates the student's signin date and cumulative hours in their StudArts record, as well as adds a
        /// log of their signin to the SigninHistory table.
        /// </summary>
        /// <returns>True if transaction was successful; false otherwise</returns>
        public bool UpdateStudentSignIn(int studentID, string studentArtName, double cumulativeTrainingHours)
        {
            float trainingHoursPerClass = GetTrainingHoursPerClassForArt(studentArtName);
            
            if (trainingHoursPerClass < 0)
                return false;

            //Declaring signInDate at the top-level function so that the date/timestamp remains consistent
            //between the database updates
            DateTime signInDate = DateTime.Now;

            if (InsertStudentSignInRecord(studentID, studentArtName, trainingHoursPerClass, signInDate) == false)
                return false;

            //Update the student arts record with the new latest signin date and cumulative training hours
            cumulativeTrainingHours += trainingHoursPerClass;
            return UpdateStudentArtsRecordAfterSignIn(studentID, studentArtName, cumulativeTrainingHours, signInDate);
        }

        private float GetTrainingHoursPerClassForArt(string artName)
        {
            float hours = 0;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = $"SELECT art_hours FROM Arts WHERE art_id = '{artName}'";
                OleDbCommand command = new OleDbCommand(sql, connection);

                try
                {
                    connection.Open();
                    hours = Convert.ToSingle(command.ExecuteScalar());
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving the hours per class for {artName}:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    return -1;
                }
            }

            return hours;
        }   

        private bool InsertStudentSignInRecord(int studentID, string artName, float hoursPerClass, DateTime signInDate)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (OleDbCommand command = new OleDbCommand(@"INSERT INTO Signin_History (
                            sign_student, 
                            sign_art,
                            sign_date,
                            sign_reg_hours)
                           VALUES (
                            @StudentID,
                            @SignInArt,
                            @SignInDate,
                            @SignInHours)", connection, transaction))
                        {
                            command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
                            command.Parameters.Add("@SignInArt", OleDbType.VarChar).Value = artName;
                            command.Parameters.Add("@SignInDate", OleDbType.Date).Value = signInDate.ToString("MM/dd/yyyy HH:mm:ss");
                            command.Parameters.Add("@SignInHours", OleDbType.Double).Value = hoursPerClass;

                            command.ExecuteNonQuery();

                            Log.Information($"Sign in for {studentID} in {artName} on {signInDate}");
                        }

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        Log.Error($"UpdateStudentArt: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        private bool UpdateStudentArtsRecordAfterSignIn(int studentID, string artName, double cumulativeTrainingHours, DateTime signInDate)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (OleDbCommand command = new OleDbCommand(@"UPDATE StudArts SET 
                            studArt_cumm = @CumulativeHours,
                            studArt_signin = @DateOfSignIn
                            WHERE StudArt_ID = @ArtID AND studArt_art = @StudentArt", connection, transaction))
                        {
                            command.Parameters.Add("@CumulativeHours", OleDbType.Numeric).Value = cumulativeTrainingHours;
                            command.Parameters.Add("@DateOfSignIn", OleDbType.DBDate).Value = signInDate;
                            command.Parameters.Add("@ArtID", OleDbType.Integer).Value = studentID;
                            command.Parameters.Add("@StudentArt", OleDbType.VarChar).Value = artName;

                            command.ExecuteNonQuery();

                            Log.Information($"Updating cumulative hours to {cumulativeTrainingHours} in {artName} for student ID {studentID}");
                        }

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        Log.Error($"UpdateStudentArtsRecordAfterSignIn: Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }
    #endregion StudentSignIn


    }
}
