namespace EasySave.Models;

public class TaskModel : AbstractTaskModel
{
    // Properties
    
    private float TaskTime { get; set; }
    private string? SourcePath { get; set; }
    private string? DestPath { get; set; }
    private string? TaskType { get; set; }
    
    // Constructors
    public TaskModel(int taskId)
    {
        Init();
        TaskId = taskId;
    }
    
    public TaskModel(string taskName)
    {
        Init();
        TaskName = taskName;
    }

    public TaskModel(string taskName, string sourcePath, string destPath, string taskType)
    {
        Init();
        TaskName = taskName;
        SourcePath = sourcePath;
        DestPath = destPath;
        TaskType = taskType;
    }

    public TaskModel()
    {
        Init();
    }
    
    // Methods
    private void Init()
    {
        if (!File.Exists(StateFileName))
        {
            CreateStateFile();
        }
    }
    
    public bool CheckTask()
    {
        PullStateFile();
        return false;
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
}