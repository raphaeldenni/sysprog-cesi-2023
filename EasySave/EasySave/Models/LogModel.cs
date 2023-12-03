namespace EasySave.Models;

public class LogModel
{
    // Properties
    public string TaskName { get; set; }
    public string FileSourcePath { get; set; }
    public string FileDestPath { get; set; }
    public string FileSize { get; set; }
    public string FileTransferTime { get; set; }
    
    // Constructors
    public LogModel(string taskName, string fileSourcePath, string fileDestPath, string fileSize, string fileTransferTime)
    {
        TaskName = taskName;
        FileSourcePath = fileSourcePath;
        FileDestPath = fileDestPath;
        FileSize = fileSize;
        FileTransferTime = fileTransferTime;
    }
    
    // Methods
    public void CheckLogFile()
    {
        
    }
    
    public void CreateLogFile()
    {
        
    }
    
    public void CreateLog()
    {
        
    }
}