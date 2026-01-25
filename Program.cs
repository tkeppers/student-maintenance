using System;
using System.Collections.Generic;
using System.Configuration;
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
            var configPath = ConfigurationManager.AppSettings["LogFileLocation"];

            // Default fallback (your commented-out logic)
            var userLocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var defaultDir = Path.Combine(userLocalAppData, "WindsongStudentMaintenance", "Logs");

            var appLogDirectory = GetUsableLogDirectory(configPath, defaultDir);

            Directory.CreateDirectory(appLogDirectory);

            var logFilePath = Path.Combine(appLogDirectory, "SystemLog-.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    path: logFilePath,
                    rollingInterval: RollingInterval.Month,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();
        }

        static string GetUsableLogDirectory(string configured, string defaultDir)
        {
            // Prefer configured if it looks usable
            if (!string.IsNullOrWhiteSpace(configured) && IsDirectoryUsable(configured))
                return configured;

            // Fall back to default
            if (IsDirectoryUsable(defaultDir))
                return defaultDir;

            // Last-ditch fallback (temp)
            var tempDir = Path.Combine(Path.GetTempPath(), "WindsongStudentMaintenance", "Logs");
            return tempDir;
        }

        static bool IsDirectoryUsable(string dir)
        {
            try
            {
                // If they pass a file path accidentally, treat it as invalid.
                // (Heuristic: has an extension OR ends with .log etc)
                if (Path.HasExtension(dir))
                    return false;

                Directory.CreateDirectory(dir);

                // quick write test to catch permissions issues early
                var testFile = Path.Combine(dir, ".write-test");
                File.WriteAllText(testFile, "ok");
                File.Delete(testFile);

                return true;
            }
            catch
            {
                return false;
            }
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
