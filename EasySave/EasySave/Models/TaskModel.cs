namespace EasySave.Models;

public class TaskModel : ITaskModel
{
    // IProperties
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    
    // Properties
    public float TaskTime { get; set; }
    public string SourcePath { get; set; }
    public string DestPath { get; set; }
    public string TaskType { get; set; }
    
    // Constructors
    public TaskModel(int taskId)
    {
        TaskId = taskId;
    }

    public TaskModel(string taskName)
    {
        TaskName = taskName;
    }

    public TaskModel()
    {
        
    }
    
    // Methods
    public void UpdateStateFile()
    {
        
    }

    public string CreateTask()
    {
        return String.Empty;
    }

    public string ModifyTask()
    {
        return String.Empty;
    }

    public string DeleteTask()
    {
        return String.Empty;
    }

    public bool CheckTask()
    {
        return false;
    }

    public string[] ListTask()
    {
        return new[] { String.Empty };
    }
}