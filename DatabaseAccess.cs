using System;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
    }
}
