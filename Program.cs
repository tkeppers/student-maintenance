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
            SetupLogging();
            var dataRepository = InitializeDataRepository();
            RunApplication(args, dataRepository);
            Log.CloseAndFlush();
        }

        static void SetupLogging()
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            var logFilePath = Path.Combine(logDirectory, "Logs-.log");
            Directory.CreateDirectory(logDirectory);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(path: logFilePath, rollingInterval: RollingInterval.Month,
                    rollOnFileSizeLimit: true, retainedFileCountLimit: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        static IDataRepository InitializeDataRepository()
        {
            return new DataRepository();
        }

        static void RunApplication(string[] args, IDataRepository dataRepository)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0 && args[0].ToLower() == "maintenance")
                Application.Run(new StudentMaintenanceUI(dataRepository));
            else
                Application.Run(new StudentSignIn(dataRepository));
        }
    }
}
