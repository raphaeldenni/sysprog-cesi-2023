using System;
using System.IO;
using System.Text.Json;

namespace EasySave.Models
{
    public class LogModel
    {
        private static int logFileCounter = 1;

        private const string LogFileNameFormat = "{0}-{1:dd-MM-yyyy}.json";

        public string TaskName { get; set; }
        public string FileSourcePath { get; set; }
        public string FileDestPath { get; set; }
        public int FileSize { get; set; }
        public float FileTransferTime { get; set; }

        public LogModel(string taskName, string fileSourcePath, string fileDestPath, int fileSize, float fileTransferTime)
        {
            TaskName = taskName;
            FileSourcePath = fileSourcePath;
            FileDestPath = fileDestPath;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
        }

        public LogModel(string taskName, string fileSourcePath, string fileDestPath, int fileSize, float fileTransferTime, string logFileName)
            : this(taskName, fileSourcePath, fileDestPath, fileSize, fileTransferTime)
        {
            if (!string.IsNullOrEmpty(logFileName))
            {
                logFileCounter = 0;
                CreateLogFile(logFileName);
            }
        }

        public void CheckLogFile()
        {
            string logFileName = GetLogFileName();

            if (LogFileExists(logFileName))
            {
                Log("Log file exists.");
            }
            else
            {
                Log("Log file does not exist.");
                CreateLogFile();
            }
        }

        private string GetLogFileName()
        {
            return string.Format(LogFileNameFormat, TaskName, DateTime.Now);
        }

        public void CreateLogFile()
        {
            string logFileName = GetLogFileName();

            if (LogFileExists(logFileName))
            {
                Log($"Log file '{logFileName}' already exists. Skipping creation.");
                return;
            }

            CreateLogFile(logFileName);
        }

        private void CreateLogFile(string logFileName)
        {
            try
            {
                using (StreamWriter writer = File.CreateText(logFileName))
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

                Log($"Log file '{logFileName}' created successfully.");

                if (!string.IsNullOrEmpty(logFileName))
                {
                    return;
                }

                logFileCounter++;
            }
            catch (Exception ex)
            {
                Log($"Error creating log file: {ex.Message}");
            }
        }

        public void CreateLog()
        {
            string logFileName = GetLogFileName();

            try
            {
                if (LogFileExists(logFileName))
                {
                    using (StreamWriter writer = File.AppendText(logFileName))
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

                    Log($"Log entry added to '{logFileName}' successfully.");
                }
                else
                {
                    Log($"Log file '{logFileName}' does not exist. Please create the log file first.");
                }
            }
            catch (Exception ex)
            {
                Log($"Error adding log entry: {ex.Message}");
            }
        }

        private bool LogFileExists(string logFileName)
        {
            return File.Exists(logFileName);
        }

        private void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }


    }
}
