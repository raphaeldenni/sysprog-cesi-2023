using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

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
    public void CreateLog(
        LogType logType, 
        string taskName, 
        string fileSourcePath, 
        string fileDestPath, 
        int fileSize, 
        float fileTransferTime
        )
    {
        // Set EasySave data path
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var easySaveFolderPath = Path.Combine(appDataPath, "EasySave");
        
        // Set log file properties
        var logFileExtension = "." + logType.ToString().ToLower();
        var logFileName = string.Format(LogFileNameFormat, "log", DateTime.Now) + logFileExtension;
            
        var logDirectoryPath = Path.Combine(easySaveFolderPath, LogFolderName);
        var logFilePath = Path.Combine(logDirectoryPath, logFileName);
        
        if (!Directory.Exists(logDirectoryPath))
            Directory.CreateDirectory(logDirectoryPath);
        
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
        using var streamLogFile = new StreamWriter(logFilePath, append: true);
            
        switch (logType) 
        {
            case LogType.Json:
            {
                var jsonSettings = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                
                var jsonData = JsonSerializer.Serialize(
                    newLogEntry, 
                    jsonSettings
                );
                
                streamLogFile.WriteLine(jsonData);
                
                streamLogFile.Flush();
                streamLogFile.Close();
                    
                break;
            }

            case LogType.Xml:
            {
                var xmlSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CloseOutput = true
                };

                var xmlDoc = XmlWriter.Create(streamLogFile, xmlSettings);

                var xmlSerializer = new XmlSerializer(typeof(LogData));
                xmlSerializer.Serialize(xmlDoc, newLogEntry);
                
                xmlDoc.Flush();
                xmlDoc.Close();
                    
                break;   
            }
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
