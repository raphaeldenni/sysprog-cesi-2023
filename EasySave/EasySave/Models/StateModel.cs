namespace EasySave.Models;

public class StateModel : ITaskModel
{
    // IProperties
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    
    // Properties
    public string TaskState { get; set; }
    public int FilesNumber { get; set; }
    public int FilesSize { get; set; }
    public int LeftFilesNumber { get; set; }
    public int LeftFilesSize { get; set; }
    public string FileSourcePath { get; set; }
    public string FileDestPath { get; set; }
    
    // Constructors
    public StateModel(int taskId)
    {
        TaskId = taskId;
    }
    
    public StateModel(string taskName)
    {
        TaskName = taskName;
    }
    
    public StateModel()
    {
        
    }
    
    // Methods
    public void UpdateStateFile()
    {
        
    }

    public void UpdateState()
    {
        
    }
}