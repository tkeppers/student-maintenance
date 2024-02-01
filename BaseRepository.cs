using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DojoStudentManagement
{
    /// <summary>
    /// Contains the common database functionality for all repositories
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string databasePath;
        protected readonly string databasePassword;
        protected readonly string connectionString;

        public BaseRepository()
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

        protected DataTable ExecuteQuery(string sql)
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
                    Log.Error($"{DateTime.Now}: Error executing query:\n{sql}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
                    return new DataTable();
                }
            }

            return dataTable;
        }
    }
}
