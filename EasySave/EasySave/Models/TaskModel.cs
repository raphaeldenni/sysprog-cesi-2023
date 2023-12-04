using System.Text.Json;

namespace EasySave.Models;

public class TaskModel : TaskBase
{
    // Properties
    
    //// State file
    private static string StateFileName => "state.json";
    public List<TaskBase>? TasksList { get; private set; }
    
    // Constructors
    public TaskModel()
    {
        if (!File.Exists(StateFileName)) UpdateStateFile(null);
        
        PullStateFile();
    }
    
    // Methods
    private void UpdateStateFile(List<TaskBase>? tasksList)
    {
        // If tasksList is null, create a default list
        TasksList = tasksList ?? Enumerable.Range(0, 5).Select(i => new TaskBase { Id = i }).ToList();
        
        var jsonState = JsonSerializer.Serialize(TasksList);
        File.WriteAllText(StateFileName, jsonState);
    }
    
    public void PullStateFile()
    {
        var jsonState = File.ReadAllText(StateFileName);
        TasksList = JsonSerializer.Deserialize<List<TaskBase>>(jsonState);
    }
    
    private void UpdateTask()
    {
        var task = TasksList?[Id.Value];
        task.Id = Id;
        task.Name = Name;
        task.Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        task.SourcePath = SourcePath;
        task.DestPath = DestPath;
        task.Type = Type;
        
        UpdateStateFile(TasksList);
    }
    
    public string CreateTask(string taskName, string taskSourcePath, string taskDestPath, string taskType)
    {
        Name = taskName;
        SourcePath = taskSourcePath;
        DestPath = taskDestPath;
        Type = taskType;
        
        Id = TasksList.FindIndex(task => task.Name == null);
        if (Id == -1) return "No more task slot available";
        
        UpdateTask();
        return $"Task {Id + 1} named {Name} created";
    }
    
    public string DeleteTask(string taskName)
    {
        Name = taskName;
        
        Id = TasksList.FindIndex(task => task.Name == Name);
        if (Id == -1) return "Task not found";
        
        UpdateTask();
        return $"Task {Name} deleted";
    }
}