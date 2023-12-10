using System.Xml.Serialization;
using System.Text.Json;

using EasySave.Types;

namespace EasySave.Models;

public class LogData
{
    public DateTime? Timestamp { get; set; }
    public string? TaskName { get; set; }
    public string? FileSourcePath { get; set; }
    public string? FileDestPath { get; set; }
    public int? FileSize { get; set; }
    public float? FileTransferTime { get; set; }
}

public class LogModel
{
    private const string LogFileNameFormat = "{0}-{1:dd-MM-yyyy}";
    private const string LogFolderName = "Logs";
    
    private LogType LogType { get; set; }
    private string? LogFileName { get; set; }
    private string? LogFolderPath { get; set; }
    private string? LogFile { get; set;  }
        
    /// <summary>
    /// Create a log entry in a log file depending on the log type (json or xml)
    /// </summary>
    /// <param name="logType"></param>
    /// <param name="taskName"></param>
    /// <param name="fileSourcePath"></param>
    /// <param name="fileDestPath"></param>
    /// <param name="fileSize"></param>
    /// <param name="fileTransferTime"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void CreateLog(LogType logType, string taskName, string fileSourcePath, string fileDestPath, int fileSize, float fileTransferTime)
    {
        // Set log file properties
        LogType = logType;
        LogFileName = "log" 
                      + string.Format(LogFileNameFormat, taskName, DateTime.Now) 
                      + "." 
                      + LogType.ToString().ToLower();
            
        LogFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolderName);
        LogFile = Path.Combine(LogFolderPath, LogFileName);
        
        if (!Directory.Exists(LogFolderPath))
            Directory.CreateDirectory(LogFolderPath);
        
        // Create log entry
        var newLogEntry = new LogData
        {
            Timestamp = DateTime.Now,
            TaskName = taskName,
            FileSourcePath = fileSourcePath,
            FileDestPath = fileDestPath,
            FileSize = fileSize,
            FileTransferTime = fileTransferTime
        };
            
        // Write log entry to log file
        using var streamLogFile = new StreamWriter(LogFile, append: true);
            
        switch (LogType) 
        {
            case LogType.Json:
                var jsonData = JsonSerializer.Serialize(newLogEntry, new JsonSerializerOptions { WriteIndented = true });
                streamLogFile.WriteLine(jsonData);
                    
                break;
            
            case LogType.Xml:
                var xmlSerializer = new XmlSerializer(typeof(LogData));
                xmlSerializer.Serialize(streamLogFile, newLogEntry);
                    
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
