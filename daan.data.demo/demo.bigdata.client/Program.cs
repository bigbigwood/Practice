using BBW.Utility.File;
using log4net;
using log4net.Config;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace demo.bigdata.client
{
    static class Program
    {
        private static ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Initialize Log4net
            XmlConfigurator.Configure();
            Log.Info("Starting Application.");

            SevenZipExtractor.SetLibraryPath(FilePathUtility.BuildPath(ConfigurationManager.AppSettings.Get("7zFilePath")));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
