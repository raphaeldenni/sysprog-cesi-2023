namespace EasySave.Models;

public class StateModel : AbstractTaskModel
{
    // IProperties
    
    // Properties
    private string? TaskState { get; set; }
    private int FilesNumber { get; set; }
    private int FilesSize { get; set; }
    private int LeftFilesNumber { get; set; }
    private int LeftFilesSize { get; set; }
    private string? FileSourcePath { get; set; }
    private string? FileDestPath { get; set; }
    
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

    public void UpdateState()
    {
        
    }
}