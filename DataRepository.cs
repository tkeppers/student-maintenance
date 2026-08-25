using Serilog;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    internal class DataRepository : BaseRepository, IDataRepository
    {
        public DataRepository()
        {
            DatabaseExistsAndIsValid();
        }

        public DataTable GetListOfArts()
        {
            return ExecuteQuery("select art_id, art_hours from Arts");
        }

        /// <summary>
        /// Retrieves the Windsong student table from the database (kiosk sign-in and the
        /// current maintenance UI only ever deal with Windsong, so this overload is kept for
        /// every existing call site to stay untouched).
        /// </summary>
        /// <returns>The DataTable containing the student records.</returns>
        public DataTable GetStudentTable()
        {
            return GetStudentTable("Windsong");
        }

        /// <summary>
        /// Retrieves the student table from the database, optionally filtered to one dojo.
        /// </summary>
        /// <param name="dojoFilter">Club id to filter by. Null or empty returns students from every dojo.</param>
        /// <returns>The DataTable containing the student records.</returns>
        public DataTable GetStudentTable(string dojoFilter)
        {
            if (DatabaseExistsAndIsValid() == false)
                return new DataTable();

            DataTable studentTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string sql = string.IsNullOrEmpty(dojoFilter)
                    ? "select * from Students"
                    : "select * from Students where stud_club = @Dojo";

                OleDbCommand command = new OleDbCommand(sql, connection);

                if (!string.IsNullOrEmpty(dojoFilter))
                    command.Parameters.Add("@Dojo", OleDbType.VarChar).Value = dojoFilter;

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);

                try
                {
                    connection.Open();
                    dataAdapter.Fill(studentTable);
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving student table for dojo filter '{dojoFilter}':\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    return new DataTable();
                }
            }

            // Rename columns from database schema to something more generic and readable
            studentTable.Columns["stud_id"].ColumnName = "StudentID";
            studentTable.Columns["stud_status"].ColumnName = "StudentStatus";
            studentTable.Columns["stud_lastname"].ColumnName = "StudentLastName";
            studentTable.Columns["stud_firstname"].ColumnName = "StudentFirstName";
            studentTable.Columns["stud_club"].ColumnName = "StudentDojo";
            studentTable.Columns["stud_birthdate"].ColumnName = "StudentBirthDate";
            studentTable.Columns["stud_addr1"].ColumnName = "StudentAddress1";
            studentTable.Columns["stud_addr2"].ColumnName = "StudentAddress2";
            studentTable.Columns["stud_city"].ColumnName = "StudentCity";
            studentTable.Columns["stud_state"].ColumnName = "StudentState";
            studentTable.Columns["stud_zip"].ColumnName = "StudentPostalCode";
            studentTable.Columns["stud_homephone"].ColumnName = "StudentPrimaryPhone";
            studentTable.Columns["stud_workphone"].ColumnName = "StudentSecondaryPhone";
            studentTable.Columns["stud_gender"].ColumnName = "StudentGender";
            studentTable.Columns["stud_email"].ColumnName = "StudentEmailAddress";
            studentTable.Columns["stud_start_month"].ColumnName = "StudentStartMonth";

            return studentTable;

        }

        public DataTable GetStudentPromotionHistory(int studentID)
        {
            return ExecuteQuery($"select * from Promo_History where promo_student={studentID}");
        }

        public DataTable GetStudentSignInHistory(int studentID)
        {
            return ExecuteQuery($"select * from Signin_History where sign_student={studentID}");
        }

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        /// <param name="student">The student object to be added.</param>
        /// <returns>True if the student was added successfully, otherwise false.</returns>
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
                        Log.Error($"Error inserting into Students table.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
                        Log.Error($"Error executing update command.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
                        Log.Error($"Error executing delete command.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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

        #region StudentArtsAndRanks

        public DataTable GetStudentArtsAndRanks(int studentID)
        {
            return ExecuteQuery($"select * from StudArts where StudArt_ID={studentID}");
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
                            command.Parameters.Add("@StudentRank", OleDbType.VarChar).Value = artsAndRank.Rank;
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
                        Log.Error($"Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }
        #endregion StudentArtsAndRanks

        #region StudentPromotion

        /// <summary>
        /// After a student is promoted, update the main arts/rank data with the new information, then
        /// add a new record in the promotion history table.
        /// </summary>
        /// <param name="recommendedBy">Optional instructor who recommended the promotion; written to
        /// Promo_History.promo_recommended_by when provided.</param>
        public bool UpdateStudentPromotion(int studentID, StudentArtsAndRank artsAndRank, string recommendedBy = null)
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
                        InsertPromotionHistory(connection, transaction, studentID, artsAndRank, recommendedBy);

                        transaction.Commit();
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error connecting to database {databasePath}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }

            return success;
        }

        private void UpdateStudentArts(OleDbConnection connection, OleDbTransaction transaction, StudentArtsAndRank artsAndRank)
        {
            // A promotion just recorded here is by definition a verified rank, so stamp
            // studArt_rank_verified with the promotion date in the same statement - but only
            // when that column exists. Promotion is core Windsong functionality that predates
            // the KUBK work, and it must keep working against a database that has not had the
            // Phase 1 migration applied (a restored backup, for instance).
            string verifiedAssignment = SupportsRankVerifiedColumn
                ? "," + Environment.NewLine + "                studArt_rank_verified = @RankVerifiedDate"
                : string.Empty;

            using (OleDbCommand command = new OleDbCommand($@"UPDATE StudArts SET
                studArt_rank = @NewRank,
                studArt_prodate = @PromotionDate,
                studArt_prohrs = @PromotionHours{verifiedAssignment}
                WHERE StudArt_ID = @ArtID AND studArt_art = @Art", connection, transaction))
            {
                // OleDb parameters are positional, so these must be added in exactly the order
                // their placeholders appear above - including the conditional one.
                command.Parameters.Add("@NewRank", OleDbType.VarChar).Value = artsAndRank.NextRank;
                command.Parameters.Add("@PromotionDate", OleDbType.DBDate).Value = artsAndRank.DatePromoted;
                command.Parameters.Add("@PromotionHours", OleDbType.Numeric).Value = artsAndRank.PromotionHours;

                if (SupportsRankVerifiedColumn)
                    command.Parameters.Add("@RankVerifiedDate", OleDbType.DBDate).Value = artsAndRank.DatePromoted;

                command.Parameters.Add("@ArtID", OleDbType.Integer).Value = artsAndRank.StudentArtID;
                command.Parameters.Add("@Art", OleDbType.VarChar).Value = artsAndRank.StudentArt;

                command.ExecuteNonQuery();

                Log.Information($"Updated promotion date and rank in {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
            }
        }

        private void InsertPromotionHistory(OleDbConnection connection, OleDbTransaction transaction, int studentID, StudentArtsAndRank artsAndRank, string recommendedBy)
        {
            bool includeRecommender = SupportsRecommendedByColumn;

            if (!includeRecommender && !string.IsNullOrWhiteSpace(recommendedBy))
            {
                Log.Warning($"Promo_History has no promo_recommended_by column, so the recommender " +
                    $"'{recommendedBy}' was not recorded for student {studentID}. Run the KUBK schema migration to enable it.");
            }

            string recommenderColumn = includeRecommender ? "," + Environment.NewLine + "                promo_recommended_by" : string.Empty;
            string recommenderValue = includeRecommender ? "," + Environment.NewLine + "                @RecommendedBy" : string.Empty;

            using (OleDbCommand command = new OleDbCommand($@"INSERT INTO Promo_History (
                promo_student,
                promo_art,
                promo_date,
                promo_rank,
                promo_hours{recommenderColumn})
               VALUES (
                @StudentID,
                @PromotionArt,
                @PromotionDate,
                @PromotionRank,
                @PromotionHours{recommenderValue})", connection, transaction))
            {
                command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
                command.Parameters.Add("@PromotionArt", OleDbType.VarChar).Value = artsAndRank.StudentArt;
                command.Parameters.Add("@PromotionDate", OleDbType.DBDate).Value = artsAndRank.DatePromoted;
                command.Parameters.Add("@PromotionRank", OleDbType.VarChar).Value = artsAndRank.NextRank.ToUpper();
                command.Parameters.Add("@PromotionHours", OleDbType.Double).Value = artsAndRank.HoursInArt;

                if (includeRecommender)
                    command.Parameters.Add("@RecommendedBy", OleDbType.VarChar).Value = (object)recommendedBy ?? DBNull.Value;

                command.ExecuteNonQuery();

                Log.Information($"Updated promotion history in {artsAndRank.StudentArt} for student ID {artsAndRank.StudentArtID}");
            }
        }

        #region Schema capability checks

        private bool? supportsRankVerifiedColumn;
        private bool? supportsRecommendedByColumn;

        /// <summary>
        /// Whether the Phase 1 KUBK columns are present. Cached per repository instance: the
        /// schema cannot change while the app is running, and the promotion path would otherwise
        /// pay for a schema lookup on every promotion.
        /// </summary>
        private bool SupportsRankVerifiedColumn =>
            (supportsRankVerifiedColumn ?? (supportsRankVerifiedColumn = ColumnExists("StudArts", "studArt_rank_verified"))).Value;

        private bool SupportsRecommendedByColumn =>
            (supportsRecommendedByColumn ?? (supportsRecommendedByColumn = ColumnExists("Promo_History", "promo_recommended_by"))).Value;

        private bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Fetch the whole column schema and filter here: the restricted overload of
                    // GetSchema is not reliably supported across the OLE DB providers this app
                    // and its migration scripts run against.
                    DataTable schema = connection.GetSchema("Columns");

                    foreach (DataRow row in schema.Rows)
                    {
                        if (string.Equals(row["TABLE_NAME"].ToString(), tableName, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(row["COLUMN_NAME"].ToString(), columnName, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Assume absent on failure: that path writes the legacy SQL, which works on both
                // schemas, so a bad guess here degrades rather than breaks.
                Log.Error($"Error checking whether {tableName}.{columnName} exists.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
            }

            return false;
        }

        #endregion Schema capability checks

        #endregion StudentPromotion

        #region StudentSignIn

        /// <summary>
        /// Updates the student's signin date and cumulative hours in their StudArts record, as well as adds a
        /// log of their signin to the SigninHistory table.
        /// </summary>
        /// <returns>True if transaction was successful; false otherwise</returns>
        public bool UpdateStudentSignIn(int studentID, string studentArtName, double cumulativeTrainingHours, out double updatedCumulativeHours)
        {
            float trainingHoursPerClass = GetTrainingHoursPerClassForArt(studentArtName);
            updatedCumulativeHours = cumulativeTrainingHours;

            if (trainingHoursPerClass < 0)
                return false;

            //Declaring signInDate at the top-level function so that the date/timestamp remains consistent
            //between the database updates
            DateTime signInDate = DateTime.Now;

            if (InsertStudentSignInRecord(studentID, studentArtName, trainingHoursPerClass, signInDate) == false)
                return false;

            //Update the student arts record with the new latest signin date and cumulative training hours
            updatedCumulativeHours = cumulativeTrainingHours + trainingHoursPerClass;
            return UpdateStudentArtsRecordAfterSignIn(studentID, studentArtName, updatedCumulativeHours, signInDate);
        }

        private float GetTrainingHoursPerClassForArt(string artName)
        {
            float hours = 0;
            const string sql = "SELECT art_hours FROM Arts WHERE art_id = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.Parameters.Add("@ArtName", OleDbType.VarChar).Value = artName;

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
                    Log.Error($"Error retrieving student promotion requirements:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
                        Log.Error($"Error inserting into Promo_Requirements table.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
                        Log.Error($"Error updating Promo_Requirements table.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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
                        Log.Error($"Error executing delete command.\n{command.CommandText}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
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

        #region KUBK

        public List<Dojo> GetDojos()
        {
            List<Dojo> dojos = new List<Dojo>();
            const string sql = @"SELECT club_id, club_name, club_instructor, club_instructor_email,
                club_phone, club_addr1, club_addr2, club_addr3, club_active, club_notes, annual_dues
                FROM Club_Parameters ORDER BY club_name";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dojos.Add(new Dojo
                            {
                                ClubID = reader["club_id"] == DBNull.Value ? string.Empty : (string)reader["club_id"],
                                Name = reader["club_name"] == DBNull.Value ? string.Empty : (string)reader["club_name"],
                                Instructor = reader["club_instructor"] == DBNull.Value ? null : (string)reader["club_instructor"],
                                InstructorEmail = reader["club_instructor_email"] == DBNull.Value ? null : (string)reader["club_instructor_email"],
                                Phone = reader["club_phone"] == DBNull.Value ? null : (string)reader["club_phone"],
                                Address1 = reader["club_addr1"] == DBNull.Value ? null : (string)reader["club_addr1"],
                                Address2 = reader["club_addr2"] == DBNull.Value ? null : (string)reader["club_addr2"],
                                Address3 = reader["club_addr3"] == DBNull.Value ? null : (string)reader["club_addr3"],
                                Active = reader["club_active"] != DBNull.Value && Convert.ToBoolean(reader["club_active"]),
                                Notes = reader["club_notes"] == DBNull.Value ? null : (string)reader["club_notes"],
                                AnnualDues = reader["annual_dues"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["annual_dues"])
                            });
                        }
                    }

                    Log.Information($"Retrieved {dojos.Count} dojos from Club_Parameters");
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving dojos:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return dojos;
        }

        public bool AddDojo(Dojo dojo)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"INSERT INTO Club_Parameters (
                    club_id, club_name, club_instructor, club_instructor_email, club_phone,
                    club_addr1, club_addr2, club_addr3, club_active, club_notes, annual_dues)
                    VALUES (
                    @ClubID, @Name, @Instructor, @InstructorEmail, @Phone,
                    @Address1, @Address2, @Address3, @Active, @Notes, @AnnualDues)", connection);

                if (connection.State == ConnectionState.Open)
                {
                    SetDojoCommandParameters(command, dojo, isUpdate: false);

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Added new dojo {dojo.ClubID}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error inserting into Club_Parameters table.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when adding new dojo.\n");
                }
            }

            return success;
        }

        public bool UpdateDojo(Dojo dojo)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(@"UPDATE Club_Parameters SET
                    club_name = @Name,
                    club_instructor = @Instructor,
                    club_instructor_email = @InstructorEmail,
                    club_phone = @Phone,
                    club_addr1 = @Address1,
                    club_addr2 = @Address2,
                    club_addr3 = @Address3,
                    club_active = @Active,
                    club_notes = @Notes,
                    annual_dues = @AnnualDues
                    WHERE club_id = @ClubID", connection);

                if (connection.State == ConnectionState.Open)
                {
                    SetDojoCommandParameters(command, dojo, isUpdate: true);

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Updated dojo {dojo.ClubID}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error updating Club_Parameters table.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when updating dojo.\n");
                }
            }

            return success;
        }

        public List<Rank> GetRankLadder()
        {
            var ladder = new List<Rank>();
            const string sql = "SELECT rank_id, rank_order, rank_next FROM Ranks ORDER BY rank_order";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ladder.Add(new Rank
                            {
                                RankID = reader["rank_id"] == DBNull.Value ? string.Empty : reader["rank_id"].ToString().Trim(),
                                RankOrder = reader["rank_order"] == DBNull.Value ? 0 : Convert.ToInt32(reader["rank_order"]),
                                RankNext = reader["rank_next"] == DBNull.Value ? null : reader["rank_next"].ToString().Trim()
                            });
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving rank ladder:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return ladder;
        }

        /// <summary>
        /// Removes a dojo outright. Only safe for a dojo no student points at - callers must
        /// check GetStudentCountsByDojo first, because Students.stud_club stores the club_id as
        /// text with no enforced relationship, so deleting a dojo in use silently orphans its
        /// students.
        /// </summary>
        public bool DeleteDojo(string clubId)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("DELETE FROM Club_Parameters WHERE club_id = @ClubID", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@ClubID", OleDbType.VarChar).Value = clubId;

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Deleted dojo {clubId}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error deleting dojo {clubId}.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when deleting dojo.\n");
                }
            }

            return success;
        }

        /// <summary>
        /// Student headcount per dojo in a single grouped query, so the dojo screen can decide
        /// which dojos are safe to delete without a round trip per selection.
        /// </summary>
        public Dictionary<string, int> GetStudentCountsByDojo()
        {
            // Keyed case-insensitively to match how Jet compares stud_club to club_id.
            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            const string sql = "SELECT stud_club, COUNT(*) AS student_count FROM Students WHERE stud_club IS NOT NULL GROUP BY stud_club";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string club = reader["stud_club"].ToString().Trim();
                            counts[club] = Convert.ToInt32(reader["student_count"]);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving student counts by dojo:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return counts;
        }

        /// <summary>
        /// Adds parameters in the exact order their placeholders appear in the AddDojo/UpdateDojo
        /// SQL text above (OleDbCommand parameters are positional, not named).
        /// </summary>
        private void SetDojoCommandParameters(OleDbCommand command, Dojo dojo, bool isUpdate)
        {
            if (!isUpdate)
                command.Parameters.Add("@ClubID", OleDbType.VarChar).Value = dojo.ClubID;

            command.Parameters.Add("@Name", OleDbType.VarChar).Value = dojo.Name;
            command.Parameters.Add("@Instructor", OleDbType.VarChar).Value = (object)dojo.Instructor ?? DBNull.Value;
            command.Parameters.Add("@InstructorEmail", OleDbType.VarChar).Value = (object)dojo.InstructorEmail ?? DBNull.Value;
            command.Parameters.Add("@Phone", OleDbType.VarChar).Value = (object)dojo.Phone ?? DBNull.Value;
            command.Parameters.Add("@Address1", OleDbType.VarChar).Value = (object)dojo.Address1 ?? DBNull.Value;
            command.Parameters.Add("@Address2", OleDbType.VarChar).Value = (object)dojo.Address2 ?? DBNull.Value;
            command.Parameters.Add("@Address3", OleDbType.VarChar).Value = (object)dojo.Address3 ?? DBNull.Value;
            command.Parameters.Add("@Active", OleDbType.Boolean).Value = dojo.Active;
            command.Parameters.Add("@Notes", OleDbType.LongVarChar).Value = (object)dojo.Notes ?? DBNull.Value;
            command.Parameters.Add("@AnnualDues", OleDbType.Currency).Value = dojo.AnnualDues;

            if (isUpdate)
                command.Parameters.Add("@ClubID", OleDbType.VarChar).Value = dojo.ClubID;
        }

        /// <summary>
        /// One row per (student, art) for the given club, or every member dojo when clubId is
        /// null/empty. Windsong is excluded from the all-dojos case: this screen is for the
        /// other KUBK dojos, and Windsong's own students are managed through the main
        /// maintenance screen. Passing "Windsong" explicitly still returns its students.
        ///
        /// Jet cannot LEFT JOIN on a compound (student, year) condition with a parameter, so the
        /// dues-paid date for duesYear is fetched in a second query and merged into the roster in
        /// memory - simpler than fighting Jet's join syntax for a lookup this small (at most one
        /// dues row per student per year).
        /// </summary>
        public DataTable GetKubkRoster(string clubId, int duesYear)
        {
            if (DatabaseExistsAndIsValid() == false)
                return new DataTable();

            string sql = @"SELECT s.stud_id, s.stud_firstname, s.stud_lastname, s.stud_status, s.stud_club,
                sa.studArt_art, sa.studArt_rank, sa.studArt_prodate, sa.studArt_rank_verified
                FROM Students AS s INNER JOIN StudArts AS sa ON s.stud_id = sa.StudArt_ID
                WHERE " + (string.IsNullOrEmpty(clubId) ? "s.stud_club <> 'Windsong'" : "s.stud_club = @ClubID") + @"
                ORDER BY s.stud_lastname, s.stud_firstname, sa.studArt_art";

            DataTable rosterTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);

                if (!string.IsNullOrEmpty(clubId))
                    command.Parameters.Add("@ClubID", OleDbType.VarChar).Value = clubId;

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);

                try
                {
                    connection.Open();
                    dataAdapter.Fill(rosterTable);
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving KUBK roster for club '{clubId}':\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    return new DataTable();
                }
            }

            rosterTable.Columns["stud_id"].ColumnName = "StudentID";
            rosterTable.Columns["stud_firstname"].ColumnName = "StudentFirstName";
            rosterTable.Columns["stud_lastname"].ColumnName = "StudentLastName";
            rosterTable.Columns["stud_status"].ColumnName = "StudentStatus";
            rosterTable.Columns["stud_club"].ColumnName = "StudentDojo";
            rosterTable.Columns["studArt_art"].ColumnName = "Art";
            rosterTable.Columns["studArt_rank"].ColumnName = "Rank";
            rosterTable.Columns["studArt_prodate"].ColumnName = "LastPromotionDate";
            rosterTable.Columns["studArt_rank_verified"].ColumnName = "RankVerifiedDate";

            rosterTable.Columns.Add("DuesPaidDate", typeof(DateTime));

            Dictionary<int, DateTime?> duesByStudent = GetDuesPaidDatesForYear(duesYear);

            foreach (DataRow row in rosterTable.Rows)
            {
                int studentID = Convert.ToInt32(row["StudentID"]);
                row["DuesPaidDate"] = duesByStudent.TryGetValue(studentID, out DateTime? paidDate) && paidDate.HasValue
                    ? (object)paidDate.Value
                    : DBNull.Value;
            }

            return rosterTable;
        }

        private Dictionary<int, DateTime?> GetDuesPaidDatesForYear(int duesYear)
        {
            Dictionary<int, DateTime?> result = new Dictionary<int, DateTime?>();
            const string sql = "SELECT dues_student, dues_paid_date FROM KUBK_Dues WHERE dues_year = @DuesYear";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.Parameters.Add("@DuesYear", OleDbType.Integer).Value = duesYear;

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int studentID = Convert.ToInt32(reader["dues_student"]);
                            DateTime? paidDate = reader["dues_paid_date"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["dues_paid_date"]);
                            result[studentID] = paidDate;
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving KUBK dues for year {duesYear}:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a KUBK_Dues row for (student, year), or updates the existing one if a payment
        /// was already recorded for that student/year. This check-then-write is not atomic against
        /// a concurrent insert, but the unique index on (dues_student, dues_year) is the backstop.
        /// </summary>
        public bool RecordDuesPayment(StudentDuesRecord dues)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                if (connection.State != ConnectionState.Open)
                {
                    Log.Error($"{DateTime.Now}: Connection failed when recording dues payment.\n");
                    return false;
                }

                // The existence probe has to be inside the try as well: it is a query in its own
                // right, and letting it throw would escape this method and reach the UI, unlike
                // every other failure here which is reported by returning false.
                try
                {
                    bool existingRecord = DuesRecordExists(connection, dues.StudentID, dues.Year);

                    OleDbCommand command = existingRecord
                        ? new OleDbCommand(@"UPDATE KUBK_Dues SET dues_paid_date = @PaidDate, dues_amount = @Amount
                            WHERE dues_student = @StudentID AND dues_year = @Year", connection)
                        : new OleDbCommand(@"INSERT INTO KUBK_Dues (dues_paid_date, dues_amount, dues_student, dues_year)
                            VALUES (@PaidDate, @Amount, @StudentID, @Year)", connection);

                    command.Parameters.Add("@PaidDate", OleDbType.DBDate).Value = (object)dues.PaidDate ?? DBNull.Value;
                    command.Parameters.Add("@Amount", OleDbType.Currency).Value = dues.Amount;
                    command.Parameters.Add("@StudentID", OleDbType.Integer).Value = dues.StudentID;
                    command.Parameters.Add("@Year", OleDbType.Integer).Value = dues.Year;

                    command.ExecuteNonQuery();
                    Log.Information($"Recorded dues payment for student {dues.StudentID}, year {dues.Year}");
                }
                catch (OleDbException ex)
                {
                    success = false;
                    Log.Error($"Error recording dues payment.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return success;
        }

        private bool DuesRecordExists(OleDbConnection connection, int studentID, int year)
        {
            OleDbCommand command = new OleDbCommand(
                "SELECT COUNT(*) FROM KUBK_Dues WHERE dues_student = @StudentID AND dues_year = @Year", connection);
            command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
            command.Parameters.Add("@Year", OleDbType.Integer).Value = year;

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public bool RemoveDuesPayment(int studentID, int year)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(
                    "DELETE FROM KUBK_Dues WHERE dues_student = @StudentID AND dues_year = @Year", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
                    command.Parameters.Add("@Year", OleDbType.Integer).Value = year;

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Removed dues payment for student {studentID}, year {year}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error removing dues payment.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when removing dues payment.\n");
                }
            }

            return success;
        }

        public List<StudentDuesRecord> GetDuesHistory(int studentID)
        {
            List<StudentDuesRecord> history = new List<StudentDuesRecord>();
            const string sql = @"SELECT dues_year, dues_paid_date, dues_amount FROM KUBK_Dues
                WHERE dues_student = @StudentID ORDER BY dues_year DESC";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new StudentDuesRecord
                            {
                                StudentID = studentID,
                                Year = Convert.ToInt32(reader["dues_year"]),
                                PaidDate = reader["dues_paid_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dues_paid_date"]),
                                Amount = reader["dues_amount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["dues_amount"])
                            });
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Log.Error($"Error retrieving dues history for student {studentID}:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                }
            }

            return history;
        }

        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate)
        {
            bool success = true;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(
                    "UPDATE StudArts SET studArt_rank_verified = @VerifiedDate WHERE StudArt_ID = @StudentID AND studArt_art = @Art", connection);

                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.Add("@VerifiedDate", OleDbType.DBDate).Value = verifiedDate;
                    command.Parameters.Add("@StudentID", OleDbType.Integer).Value = studentID;
                    command.Parameters.Add("@Art", OleDbType.VarChar).Value = artName;

                    try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        Log.Information($"Verified rank for student {studentID} in {artName} as of {verifiedDate}");
                    }
                    catch (OleDbException ex)
                    {
                        success = false;
                        Log.Error($"Error verifying student rank.\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                        connection.Close();
                    }
                }
                else
                {
                    success = false;
                    Log.Error($"{DateTime.Now}: Connection failed when verifying student rank.\n");
                }
            }

            return success;
        }

        #endregion KUBK

    }
}
