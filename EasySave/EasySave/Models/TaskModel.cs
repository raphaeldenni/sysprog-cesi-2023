using System.Text.Json;

namespace EasySave.Models;

public class TaskModel : TaskEntity
{
    // Properties
    private static string StateFileName => "state.json"; // Set the state file name
    public List<TaskEntity>? TasksList { get; private set; } // A list that contains all the tasks from the state file
    
    // Constructors
    public TaskModel()
    {
        // If the state file doesn't exist, create a default list
        if (!File.Exists(StateFileName))
        {
            UpdateStateFile(null); 
        }
        
        PullStateFile();
    }
    
    // Methods
    
    //// State file methods
    private void UpdateStateFile(List<TaskEntity>? tasksList)
    {
        // If tasksList is null, create a default list
        TasksList = tasksList ?? Enumerable.Range(0, 5).Select(i => new TaskEntity { Id = i }).ToList();
        
        var jsonTasksList = JsonSerializer.Serialize(TasksList);
        File.WriteAllText(StateFileName, jsonTasksList);
    }
    
    public void PullStateFile()
    {
        var jsonTasksList = File.ReadAllText(StateFileName);
        TasksList = JsonSerializer.Deserialize<List<TaskEntity>>(jsonTasksList);
    }
    
    //// Task methods
    public string UpdateTask(string taskName, string? taskSourcePath, string? taskDestPath, string? taskType)
    {
        var isNew = TasksList.FindIndex(task => task.Name == taskName) == -1;
        var searchValue = isNew ? null : taskName;
            
        Name = taskName;
        SourcePath = taskSourcePath;
        DestPath = taskDestPath;
        Type = taskType;
        
        Id = TasksList.FindIndex(task => task.Name == searchValue);
        
        UpdateTasksList();
        
        return $"Task {Id + 1} named {Name} has been {(isNew ? "created" : "modified")}.";
    }
    
    public void UpdateTaskState
    (
        string taskName, 
        string taskState, 
        int? taskFilesNumber, 
        float? taskFilesSize, 
        int? taskLeftFilesNumber, 
        float? taskLeftFilesSize, 
        string? taskFileSourcePath, 
        string? taskFileDestPath
    )
    {
        Name = taskName;
        State = taskState;
        FilesNumber = taskFilesNumber;
        FilesSize = taskFilesSize;
        LeftFilesNumber = taskLeftFilesNumber;
        LeftFilesSize = taskLeftFilesSize;
        FileSourcePath = taskFileSourcePath;
        FileDestPath = taskFileDestPath;
        
        Id = TasksList.FindIndex(task => task.Name == Name);
        if (Id == -1) return;
        
        UpdateTasksList();
    }
    
    //// Tasks list methods
    private void UpdateTasksList()
    {
        var task = TasksList[Id.Value];
        
        // If Name is null, the task is deleted
        if (Name != null)
        {
            UpdateTaskProperties(task);
        }
        else
        {
            task = null;
        }
        
        task.Id = Id;
        task.Name = Name;
        
        UpdateStateFile(TasksList);
    }
    
    private void UpdateTaskProperties(TaskEntity task)
    {
        // If a property is null, the property is not updated
        
        // Task properties
        task.SourcePath = SourcePath ?? task.SourcePath;
        task.DestPath = DestPath ?? task.DestPath;
        task.Type = Type ?? task.Type;
        task.Timestamp ??= DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        
        // State properties
        task.State = State ?? task.State;
        task.FilesNumber = FilesNumber ?? task.FilesNumber;
        task.FilesSize = FilesSize ?? task.FilesSize;
        task.LeftFilesNumber = LeftFilesNumber ?? task.LeftFilesNumber;
        task.LeftFilesSize = LeftFilesSize ?? task.LeftFilesSize;
        task.FileSourcePath = FileSourcePath ?? task.FileSourcePath;
        task.FileDestPath = FileDestPath ?? task.FileDestPath;
    }
}