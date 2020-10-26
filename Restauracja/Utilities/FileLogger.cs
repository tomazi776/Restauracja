using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class FileLogger : LogBase
    {
        private readonly string fileName = "Logs.txt";
        private readonly DateTime logDateTime;
        //private static DirectoryInfo directory = Directory.Exists(Environment.CurrentDirectory + @"Data\Logs") ? Directory.CreateDirectory(@"Data\Logs");

        private static DirectoryInfo directory = Directory.CreateDirectory(@"Data\Logs");
        private string folderPath = Path.Combine(Environment.CurrentDirectory, directory.FullName);

        //string pathToNewFolder = System.IO.Path.Combine(parentFolderPath, "NewSubFolder");
        public FileLogger()
        {
            logDateTime = DateTime.Now;
            fileName = String.Format("{0}_{1:dd_MM_yyyy H_mm}", "Logs", DateTime.Now) + ".txt";
        }

        public override void Log(Exception exception)
        {
            var filePath = Path.Combine(folderPath, fileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                var logInfo = BuildLogs(exception);
                streamWriter.WriteLine(logInfo);
            }
        }
    }
}
