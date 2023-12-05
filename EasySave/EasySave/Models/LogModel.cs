using System;
using System.IO;
using System.Text.Json;

namespace EasySave.Models
{
    public class LogModel
    {
        private const string LogFileNameFormat = "{0}-{1:dd-MM-yyyy}.json";
        private const string LogFolderName = "Logs";

        public string? TaskName { get; set; }
        public string? FileSourcePath { get; set; }
        public string? FileDestPath { get; set; }
        public string LogFileName { get; set; }
        public string LogFolderPath { get; set; }
        public int? FileSize { get; set; }
        public float? FileTransferTime { get; set; }

        public LogModel()
        {
            LogFileName = "log" + GetLogFileName();
            LogFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolderName);

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
                    var logData = new
                    {
                        TaskName,
                        FileSourcePath,
                        FileDestPath,
                        FileSize,
                        FileTransferTime
                    };

                    string jsonData = JsonSerializer.Serialize(logData, new JsonSerializerOptions { WriteIndented = true });
                    writer.WriteLine(jsonData);
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
                        var logEntry = new
                        {
                            Timestamp = DateTime.Now,
                            TaskName,
                            FileSourcePath,
                            FileDestPath,
                            FileSize,
                            FileTransferTime
                        };

                        string jsonData = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
                        writer.WriteLine(jsonData);
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
