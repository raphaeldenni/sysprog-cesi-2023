using EasySave.Types;
using System.Text.Json;
using System.Xml.Serialization;

namespace EasySave.Models
{
    public class LogData
    {
        public DateTime Timestamp { get; set; }
        public string TaskName { get; set; }
        public string FileSourcePath { get; set; }
        public string FileDestPath { get; set; }
        public int? FileSize { get; set; }
        public float? FileTransferTime { get; set; }
    }
    public class LogModel
    {
        private const string LogFileNameFormat = "{0}-{1:dd-MM-yyyy}";
        private const string LogFolderName = "Logs";

        public string? TaskName { get; set; }
        public string? FileSourcePath { get; set; }
        public string? FileDestPath { get; set; }
        public string LogFileName { get; set; }
        public string LogFolderPath { get; set; }
        public LogType LogType { get; set; }
        public int? FileSize { get; set; }
        public float? FileTransferTime { get; set; }

        public LogModel(LogType logType)
        {

            LogFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolderName);
            LogType = logType;
            LogFileName = "log" + GetLogFileName() + "." + LogType.ToString().ToLower() ;

            CheckLogFile();
        }

        public void CheckLogFile()
        {
            if (!Directory.Exists(LogFolderPath))
            {
                Directory.CreateDirectory(LogFolderPath);
            }

            LogFileName = Path.Combine(LogFolderPath, LogFileName);

            if (!LogFileExists(LogFileName))
            {
                CreateLogFile();
            }

        }

        private string GetLogFileName()
        {
            return string.Format(LogFileNameFormat, TaskName, DateTime.Now);
        }

        private void CreateLogFile()
        {
            try
            {

                using (StreamWriter writer = File.CreateText(LogFileName))
                {
                    var logData = new LogData
                    {
                        TaskName = TaskName,
                        FileSourcePath = FileSourcePath,
                        FileDestPath = FileDestPath,
                        FileSize = FileSize,
                        FileTransferTime = FileTransferTime
                    };

                    if (LogType == LogType.Json)
                    {
                        string jsonData = JsonSerializer.Serialize(logData, new JsonSerializerOptions { WriteIndented = true });
                        writer.WriteLine(jsonData);
                    }
                    else if (LogType == LogType.Xml)
                    {
                        var xmlSerializer = new XmlSerializer(typeof(LogData));
                        xmlSerializer.Serialize(writer, logData);
                    }

                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void CreateLog(string taskName, string fileSourcePath, string fileDestPath, int fileSize, float fileTransferTime)
        {

            try
            {
                TaskName = taskName;
                FileSourcePath = fileSourcePath;
                FileDestPath = fileDestPath;
                FileSize = fileSize;
                FileTransferTime = fileTransferTime;

                if (LogFileExists(LogFileName))
                {
                    using (StreamWriter writer = File.AppendText(LogFileName))
                    {
                        var logEntry = new LogData
                        {
                            Timestamp = DateTime.Now,
                            TaskName = TaskName,
                            FileSourcePath = FileSourcePath,
                            FileDestPath = FileDestPath,
                            FileSize = FileSize,
                            FileTransferTime = FileTransferTime
                        };

                        if (LogType == LogType.Json)
                        {
                            string jsonData = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
                            writer.WriteLine(jsonData);
                        }
                        else if (LogType == LogType.Xml)
                        {
                            var xmlSerializer = new XmlSerializer(typeof(LogData));
                            xmlSerializer.Serialize(writer, logEntry);
                        }
                    }

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private bool LogFileExists(string logFileName)
        {
            return File.Exists(logFileName);
        }

    }
}
