using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using Serilog.Sinks.File;


namespace DojoStudentManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "Logs-.log");

            // Ensure the log directory exists
            Directory.CreateDirectory(logDirectory);

            // Set up the global Serilog logger configuration
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(
                        path: logFilePath,
                        rollingInterval: RollingInterval.Month,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: null, // Set the retained file count as needed
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

            IDataAccess dataAccess = new DatabaseAccess();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Upon login, if the "maintenance" argument is passed, the maintenance form will be displayed, which will allow the 
            //instructor to perform student maintenance tasks (like adding students, promotions, etc.). If no argument is passed,
            //the student sign in form will be displayed.
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "maintenance":
                        Application.Run(new StudentMaintenanceUI(dataAccess));
                        break;
                    default:
                        Application.Run(new StudentSignIn(dataAccess));
                        break;
                }
            }
            else
            {
                Application.Run(new StudentSignIn(dataAccess));
            }

            Log.CloseAndFlush();
        }
    }
}
