namespace EasySave.Models;

public class TaskModel : AbstractTaskModel
{
    // Properties
    
    private float TaskTime { get; set; }
    private string SourcePath { get; set; }
    private string DestPath { get; set; }
    private string TaskType { get; set; }
    
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

    public string CreateTask()
    {
        PullStateFile();
        
        if (TasksList.Any(task => task.TaskName == TaskName))
        {
            return "Task already exist";
        }
        
        var task = new TaskModel(TaskName, SourcePath, DestPath, TaskType);
        TasksList.Add(task);
        UpdateStateFile(TasksList);
        
        return "Task created";
    }

    public string ModifyTask()
    {
        PullStateFile();
        var task = TasksList.Find(task => task.TaskName == TaskName);
        task.SourcePath = SourcePath;
        task.DestPath = DestPath;
        task.TaskType = TaskType;
        UpdateStateFile(TasksList);

        return "Task modified";
    }

    public string DeleteTask()
    {
        PullStateFile();
        var task = TasksList.Find(task => task.TaskName == TaskName);
        TasksList.Remove(task);
        UpdateStateFile(TasksList);
        
        return "Task deleted";
    }
}