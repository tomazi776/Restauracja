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
        public const string fileName = "Logs.txt";
        public readonly static string folderPath = Path.Combine(Environment.CurrentDirectory, @"Data\");
        DirectoryInfo dir = Directory.CreateDirectory(folderPath);
        public string filePath = Path.Combine(folderPath, fileName);

        public override void Log(Exception exception)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                var logInfo = BuildLogs(exception);
                streamWriter.WriteLine(logInfo);
            }
        }
    }
}
