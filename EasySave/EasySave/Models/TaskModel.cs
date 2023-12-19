using EasySave.Types;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EasySave.Models;

public class TaskEntity : INotifyPropertyChanged
{
    // Events
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Properties

    //// Task
    private bool? _isChecked;
    public bool? IsChecked
    {
        get => _isChecked;
        set { if (_isChecked != value) { _isChecked = value; OnPropertyChanged(); } }
    }

    private decimal? _loading;
    public decimal? Loading
    {
        get => _loading;
        set { if (_loading != value) { _loading = value; OnPropertyChanged(); } }
    }

    private int? _id;
    public int? Id
    {
        get => _id;
        set { if (_id != value) { _id = value; OnPropertyChanged(); } }
    }

    private string? _name;
    public string? Name
    {
        get => _name;
        set { if (_name != value) { _name = value; OnPropertyChanged(); } }
    }

    private string? _sourcePath;
    public string? SourcePath
    {
        get => _sourcePath;
        set { if (_sourcePath != value) { _sourcePath = value; OnPropertyChanged(); } }
    }

    private string? _destPath;
    public string? DestPath
    {
        get => _destPath;
        set { if (_destPath != value) { _destPath = value; OnPropertyChanged(); } }
    }

    private BackupType? _type;
    public BackupType? Type
    {
        get => _type;
        set { if (_type != value) { _type = value; OnPropertyChanged(); } }
    }

    private string? _timestamp;
    public string? Timestamp
    {
        get => _timestamp;
        set { if (_timestamp != value) { _timestamp = value; OnPropertyChanged(); } }
    }

    //// State
    private StateType? _state;
    public StateType? State
    {
        get => _state;
        set { if (_state != value) { _state = value; OnPropertyChanged(); } }
    }

    private int? _filesNumber;
    public int? FilesNumber
    {
        get => _filesNumber;
        set { if (_filesNumber != value) { _filesNumber = value; OnPropertyChanged(); } }
    }

    private float? _filesSize;
    public float? FilesSize
    {
        get => _filesSize;
        set { if (_filesSize != value) { _filesSize = value; OnPropertyChanged(); } }
    }

    private int? _leftFilesNumber;
    public int? LeftFilesNumber
    {
        get => _leftFilesNumber;
        set { if (_leftFilesNumber != value) { _leftFilesNumber = value; OnPropertyChanged(); } }
    }

    private float? _leftFilesSize;
    public float? LeftFilesSize
    {
        get => _leftFilesSize;
        set { if (_leftFilesSize != value) { _leftFilesSize = value; OnPropertyChanged(); } }
    }

    private string? _fileSourcePath;
    public string? FileSourcePath
    {
        get => _fileSourcePath;
        set { if (_fileSourcePath != value) { _fileSourcePath = value; OnPropertyChanged(); } }
    }

    private string? _fileDestPath;
    public string? FileDestPath
    {
        get => _fileDestPath;
        set { if (_fileDestPath != value) { _fileDestPath = value; OnPropertyChanged(); } }
    }
}


public class TaskModel : TaskEntity
{
    // Properties
    private const string EasySaveFolderName = "EasySave";
    private const string StateFileName = "state.json";
    private string StateFilePath { get; set;  }

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
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var easySaveFolderPath = Path.Combine(appDataPath, EasySaveFolderName);
        
        StateFilePath = Path.Combine(easySaveFolderPath, StateFileName);

        if (!File.Exists(StateFilePath))
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
        TasksList = tasksList ?? new List<TaskEntity>();

        var jsonTasksList = JsonSerializer.Serialize(TasksList);
        File.WriteAllText(StateFilePath, jsonTasksList);
    }
    
    /// <summary>
    /// Pulls the state file and stores it in the TasksList property
    /// </summary>
    public void PullStateFile()
    {
        var jsonTasksList = File.ReadAllText(StateFilePath);
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

        // Update the task
        Name = newTaskName ?? taskName;
        SourcePath = taskSourcePath;
        DestPath = taskDestPath;
        Type = taskType;
        State = StateType.Inactive;
        
        UpdateTasksList();

        return new[] { Id.ToString()!, Name };
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

        TasksList.RemoveAt(taskId);
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
        if (FilesNumber != null)
        {
            FilesNumber = taskFilesNumber;
        }

        if (FilesSize != null) {
            FilesSize = taskFilesSize;
        }

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
        if (Id == -1)
        {
            var newTask = new TaskEntity
            {
                Id = FindNextAvailableId(),
                Name = Name,
                SourcePath = SourcePath,
                DestPath = DestPath,
                Type = Type,
                Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                State = StateType.Inactive
            };

            TasksList!.Add(newTask);
        }
        else
        {
            var task = TasksList![Id!.Value];
            UpdateTaskProperties(task);
        }

        UpdateStateFile(TasksList);
    }

    /// <summary>
    /// Find the next available ID
    /// </summary>
    private int FindNextAvailableId()
    {
        var nextId = 0;

        // Find the biggest ID available
        if (TasksList != null && TasksList.Count > 0)
        {
            nextId = TasksList.Max(task => task.Id.GetValueOrDefault()) + 1;
        }

        return nextId;
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