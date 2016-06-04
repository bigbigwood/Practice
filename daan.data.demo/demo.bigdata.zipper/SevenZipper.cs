using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace demo.bigdata.zipper
{
    public class SevenZipper
    {
        public static readonly string Zip_Application_Path = AppDomain.CurrentDomain.BaseDirectory + @"\7z\7z.exe";
        public const string ZipCommandFormat = @"a ""{0}"" ""{1}"""; // 0 = zipped file name; 1 = free file name;
        public const string UpZipCommandFormat = @"x ""{0}"" -o{1}"; // 0 = zipped file name; 1 = target path

        public bool Zip(string zippedFileName, List<string> fileNames)
        {
            throw new NotImplementedException();
        }

        public bool Zip(string zippedFileName, string fileName)
        {
            return ExecuteSevenZip(zippedFileName, fileName);
        }

        public bool UnZip(string zippedFileName, List<string> unzippedfileNames)
        {
            throw new NotImplementedException();
        }

        public bool UnZip(string zippedFileName, string unzippedFileName, string zipPath)
        {
            Process zipProcess = new Process();
            zipProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            zipProcess.StartInfo.CreateNoWindow = false;
            zipProcess.StartInfo.FileName = zipPath;
            zipProcess.StartInfo.Arguments = string.Format(UpZipCommandFormat, zippedFileName, unzippedFileName.Substring(0, unzippedFileName.LastIndexOf(@"\")));
            zipProcess.Start();
            zipProcess.WaitForExit();

            if (zipProcess.HasExited)
            {
                int iExitCode = zipProcess.ExitCode;
                zipProcess.Close();
                if (iExitCode != 0 && iExitCode != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ExecuteSevenZip(string zippedFileName, string unzipFileNames)
        {
            Process zipProcess = new Process();
            zipProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            zipProcess.StartInfo.CreateNoWindow = false;
            zipProcess.StartInfo.FileName = Zip_Application_Path;
            zipProcess.StartInfo.Arguments = string.Format(ZipCommandFormat, zippedFileName, unzipFileNames);
            zipProcess.Start();
            zipProcess.WaitForExit();

            if (zipProcess.HasExited)
            {
                int iExitCode = zipProcess.ExitCode;
                zipProcess.Close();
                if (iExitCode != 0 && iExitCode != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
