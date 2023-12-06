using EasySave.Types;
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
    public string UpdateTask(bool isNew, string taskName, string? taskSourcePath, string? taskDestPath, BackupType? taskType, string? newTaskName)
    {
        // If the source path is not null, check if it exists
        if (taskSourcePath != null && !Directory.Exists(taskSourcePath)) return $"Source path {taskSourcePath} not found.";

        // Verify the correspondency between new task and name 
        var sameName = TasksList.Any(task => task.Name == taskName);

        if (isNew && sameName) return "You can't create two tasks with the same name.";
        if (!isNew && !sameName) return "You can't modify a task that doesn't exist.";

        // Retrieve task ID
        var searchValue = isNew ? null : taskName;

        Id = TasksList.FindIndex(task => task.Name == searchValue);

        if (Id >= 5) return "You can't create more than 5 tasks.";

        // Update the task
        Name = newTaskName ?? taskName;
        SourcePath = taskSourcePath;
        DestPath = taskDestPath;
        Type = taskType;
        
        UpdateTasksList();
        
        return $"Task {Id + 1} named {Name} has been {(isNew ? "created" : "modified")}.";
    }
    
    public string DeleteTask(string taskName)
    {
        int taskId = TasksList.FindIndex(task => task.Name == taskName);

        if (taskId == -1)
        {
            return $"Task named {taskName} not found.";
        }

        TasksList[taskId] = new TaskEntity { Id = taskId };
        UpdateStateFile(TasksList);

        return $"Task named {taskName} has been deleted.";
    }
    
    public void UpdateTaskState
    (
        string taskName, 
        StateType taskState, 
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
        
        UpdateTaskProperties(task);
        UpdateStateFile(TasksList);
    }
    
    private void UpdateTaskProperties(TaskEntity task)
    {
        // If a property is null, the property is not updated
        
        // Task properties
        task.Id = Id;
        task.Name = Name;
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