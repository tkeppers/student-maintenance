using System;
using System.Collections.Generic;
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
            // Set up the global Serilog logger configuration
            Log.Logger = new LoggerConfiguration().
                    MinimumLevel.Debug().
                    WriteTo.File(@Environment.GetEnvironmentVariable("LocalAppData") + "\\Logs\\Logs1.log").
                    CreateLogger();

            IDataAccess dataAccess = new DatabaseAccess();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StudentMaintenanceUI(dataAccess));

            Log.CloseAndFlush();
        }
    }
}
