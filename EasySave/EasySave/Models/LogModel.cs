using EasySave.Types;
using System.Text.Json;
using System.Xml;
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
                        using (StreamWriter writer = File.AppendText(LogFileName))
                        {
                            string jsonData = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
                            writer.WriteLine(jsonData);
                        }
                    }
                    else if (LogType == LogType.Xml)
                    {
                        // Load existing XML document
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(LogFileName);

                        // Create a new log entry node
                        XmlNode logEntryNode = xmlDoc.CreateElement("LogEntry");

                        // Add timestamp node
                        XmlNode timestampNode = xmlDoc.CreateElement("Timestamp");
                        timestampNode.InnerText = logEntry.Timestamp.ToString();
                        logEntryNode.AppendChild(timestampNode);

                        // Add task name node
                        XmlNode taskNameNode = xmlDoc.CreateElement("TaskName");
                        taskNameNode.InnerText = logEntry.TaskName;
                        logEntryNode.AppendChild(taskNameNode);

                        // Add file source path node
                        XmlNode fileSourcePathNode = xmlDoc.CreateElement("FileSourcePath");
                        fileSourcePathNode.InnerText = logEntry.FileSourcePath;
                        logEntryNode.AppendChild(fileSourcePathNode);

                        // Add file dest path node
                        XmlNode fileDestPathNode = xmlDoc.CreateElement("FileDestPath");
                        fileDestPathNode.InnerText = logEntry.FileDestPath;
                        logEntryNode.AppendChild(fileDestPathNode);

                        // Add file size node
                        XmlNode fileSizeNode = xmlDoc.CreateElement("FileSize");
                        fileSizeNode.InnerText = logEntry.FileSize.ToString();
                        logEntryNode.AppendChild(fileSizeNode);

                        // Add file transfer time node
                        XmlNode fileTransferTimeNode = xmlDoc.CreateElement("FileTransferTime");
                        fileTransferTimeNode.InnerText = logEntry.FileTransferTime.ToString();
                        logEntryNode.AppendChild(fileTransferTimeNode);

                        // Append the new log entry node to the root
                        xmlDoc.DocumentElement.AppendChild(logEntryNode);

                        // Save the modified XML document
                        xmlDoc.Save(LogFileName);
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
