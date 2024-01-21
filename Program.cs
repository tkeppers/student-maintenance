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
        static void Main()
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

            //TODO: Run a different form based on parameters passed in
            Application.Run(new StudentSignIn(dataAccess));
            //Application.Run(new StudentMaintenanceUI(dataAccess));

            Log.CloseAndFlush();
        }
    }
}
