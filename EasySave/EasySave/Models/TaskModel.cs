using EasySave.Types;
using System.Text.Json;

namespace EasySave.Models;

public class TaskEntity
{
    // Properties

    //// Task
    public bool? IsChecked { get; set; }
    public int? Loading { get; set; }
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? SourcePath { get; set; }
    public string? DestPath { get; set; }
    public BackupType? Type { get; set; }
    public string? Timestamp { get; set; }

    //// State
    public StateType? State { get; set; }
    public int? FilesNumber { get; set; }
    public float? FilesSize { get; set; }
    public int? LeftFilesNumber { get; set; }
    public float? LeftFilesSize { get; set; }
    public string? FileSourcePath { get; set; }
    public string? FileDestPath { get; set; }
}

public class TaskModel : TaskEntity
{
    // Properties
    private const string StateFileName = "state.json"; 
    
    // A list that contains all the tasks from the state file
    public List<TaskEntity>? TasksList { get; private set; }
    
    // Exceptions
    public class SourcePathNotFoundException : Exception
    {
    }

    public class DuplicateTaskNameException : Exception
    {
    }

    public class TaskNotFoundException : Exception
    {
    }

    public class TooMuchTasksException : Exception
    {
    }

    public class TaskNameNotFoundException : Exception
    {
    }

    // Constructors
    
    /// <summary>
    /// TaskModel constructor
    /// </summary>
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
    
    /// <summary>
    /// This method updates the state file with the given list
    /// </summary>
    /// <param name="tasksList"></param>
    private void UpdateStateFile(List<TaskEntity>? tasksList)
    {
        // If tasksList is null, create a default list
        TasksList = tasksList ?? Enumerable.Range(0, 5).Select(i => new TaskEntity { Id = i }).ToList();
        
        var jsonTasksList = JsonSerializer.Serialize(TasksList);
        File.WriteAllText(StateFileName, jsonTasksList);
    }
    
    /// <summary>
    /// Pulls the state file and stores it in the TasksList property
    /// </summary>
    public void PullStateFile()
    {
        var jsonTasksList = File.ReadAllText(StateFileName);
        TasksList = JsonSerializer.Deserialize<List<TaskEntity>>(jsonTasksList);
    }
    
    /// <summary>
    /// Method that updates the task with the given parameters
    /// </summary>
    /// <param name="isNew"></param>
    /// <param name="taskName"></param>
    /// <param name="taskSourcePath"></param>
    /// <param name="taskDestPath"></param>
    /// <param name="taskType"></param>
    /// <param name="newTaskName"></param>
    /// <returns></returns>
    /// <exception cref="SourcePathNotFoundException"></exception>
    /// <exception cref="DuplicateTaskNameException"></exception>
    /// <exception cref="TaskNotFoundException"></exception>
    /// <exception cref="TooMuchTasksException"></exception>
    public string[] UpdateTask(bool isNew, string taskName, string? taskSourcePath, string? taskDestPath, BackupType? taskType, string? newTaskName)
    {
        // If the source path is not null, check if it exists
        if (taskSourcePath != null && !Directory.Exists(taskSourcePath)) throw new SourcePathNotFoundException();

        // Verify the correspondence between new task and name 
        var sameName = TasksList!.Any(task => task.Name == taskName);

        switch (isNew)
        {
            case true when sameName:
                throw new DuplicateTaskNameException();
            
            case false when !sameName:
                throw new TaskNotFoundException();
        }

        // Retrieve task ID
        var searchValue = isNew ? null : taskName;

        Id = TasksList!.FindIndex(task => task.Name == searchValue);

        if (Id >= 5) throw new TooMuchTasksException();

        // Update the task
        Name = newTaskName ?? taskName;
        SourcePath = taskSourcePath;
        DestPath = taskDestPath;
        Type = taskType;
        State = StateType.Inactive;
        
        UpdateTasksList();

        var newTask = new[] { (Id + 1).ToString()!, Name };

        return newTask;
    }
    
    /// <summary>
    /// Deletes the task with the given name
    /// </summary>
    /// <param name="taskName"></param>
    /// <returns></returns>
    /// <exception cref="TaskNameNotFoundException"></exception>
    public string DeleteTask(string taskName)
    {
        var taskId = TasksList!.FindIndex(task => task.Name == taskName);

        if (taskId == -1)
        {
            throw new TaskNameNotFoundException();
        }

        TasksList[taskId] = new TaskEntity { Id = taskId };
        UpdateStateFile(TasksList);

        return taskName;
    }
    
    /// <summary>
    /// Updates the task state with the given parameters
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="taskState"></param>
    /// <param name="taskFilesNumber"></param>
    /// <param name="taskFilesSize"></param>
    /// <param name="taskLeftFilesNumber"></param>
    /// <param name="taskLeftFilesSize"></param>
    /// <param name="taskFileSourcePath"></param>
    /// <param name="taskFileDestPath"></param>
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
        
        Id = TasksList!.FindIndex(task => task.Name == Name);
        if (Id == -1) return;
        
        UpdateTasksList();
    }
    
    /// <summary>
    /// Updates the task with the given parameters
    /// </summary>
    private void UpdateTasksList()
    {
        var task = TasksList![Id!.Value];
        
        UpdateTaskProperties(task);
        UpdateStateFile(TasksList);
    }
    
    /// <summary>
    /// Copies the properties of the given task to the current task
    /// </summary>
    /// <param name="task"></param>
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