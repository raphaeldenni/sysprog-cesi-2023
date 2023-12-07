using EasySave.Models;
using EasySave.Views;
using EasySave.Types;

namespace EasySave.ViewModels
{
    internal class ExecuteViewModel
    {
        // Properties
        private float initializeLeftFilesSize = 0;

        //// Views
        public ExecuteView ExecuteView { get; set; }
        public HelpView HelpView { get; set; }

        //// Models
        public LogModel LogModel { get; set; }
        public TaskModel TaskModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public ConfigModel ConfigModel { get; set; }

        // Constructor
        public ExecuteViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            ExecuteView = new ExecuteView(ConfigModel.Config.Language);
            HelpView = new HelpView(ConfigModel.Config.Language);
            TaskModel = new TaskModel();

            if (args.Length != 1)
            {
                HelpView.DisplayExecute();
                HelpView.DisplayMessage();
            }
            else
            {
                ExecuteTasks(args[0]);
            }
        }

        // Methods
        public void ExecuteTasks(string tasksRow)
        {
            // If taskRow begin with a char, execute one task
            if (!string.IsNullOrEmpty(tasksRow) && Char.IsLetter(tasksRow[0]))
            {
                ExecuteOneTask(tasksRow);
            }

            // Execute multiple tasks
            if (!string.IsNullOrEmpty(tasksRow) && tasksRow.Contains(';'))
            {
                var tasksIdString = tasksRow.Split(';');
                List<string> tasksNameList = new List<string>();

                foreach (var taskIdString in tasksIdString)
                {
                    var taskId = int.Parse(taskIdString) - 1;
                    var taskName = TaskModel.TasksList?.FirstOrDefault(t => t.Id == taskId)?.Name;

                    if (taskName != null)
                    {
                        ExecuteOneTask(taskName);
                        tasksNameList.Add(taskName);
                    }
                }

                string tasksNamesString = string.Join(", ", tasksNameList);
                ExecuteView.TasksState(tasksNamesString, false);
                ExecuteView.DisplayMessage();
            }

            // Execute task to task
            if (!string.IsNullOrEmpty(tasksRow) && tasksRow.Contains('-'))
            {
                var tasksIdString = tasksRow.Split('-');
                List<string> tasksNameList = new List<string>();

                var firstTaskId = int.Parse(tasksIdString[0]) - 1;
                var secondTaskId = int.Parse(tasksIdString[1]) - 1;

                for (var taskId = firstTaskId; taskId < secondTaskId; taskId++)
                {
                    var taskName = TaskModel.TasksList?.FirstOrDefault(t => t.Id == taskId)?.Name;
                    if (taskName != null)
                    {
                        ExecuteOneTask(taskName);
                        tasksNameList.Add(taskName);
                    }
                }

                string tasksNamesString = string.Join(", ", tasksNameList);
                ExecuteView.TasksState(tasksNamesString, false);
                ExecuteView.DisplayMessage();
            }
        }

        public void ExecuteOneTask(string? taskName)
        {
            var task = TaskModel.TasksList?.FirstOrDefault(t => t.Name == taskName);
            initializeLeftFilesSize = 0;

            if (task == null)
            {
                ExecuteView.ErrorTaskNameNotFound();
                ExecuteView.DisplayMessage();
            }
            else
            {
                // Get task info
                var taskType = task.Type;
                StateType taskState = StateType.Active;

                // Set copy model
                CopyModel = new CopyModel(task.SourcePath, task.DestPath, (BackupType)taskType);
                var filesCount = CopyModel.LeftFilesNumber;
                var filesSize = CopyModel.LeftFilesSize;

                // Update task state
                TaskModel.UpdateTaskState(
                    task.Name,
                    taskState,
                    filesCount,
                    filesSize,
                    filesCount,
                    filesSize,
                    task.SourcePath,
                    task.DestPath
                    );

                ExecuteView.TasksState(task.Name, true);
                ExecuteView.DisplayMessage();

                CopyModel.FileCopied += LogFileCopied;

                // Start copy
                CopyModel.CopyFiles();

                taskState = StateType.Inactive;

                // Update task state
                TaskModel.UpdateTaskState(
                    task.Name,
                    taskState,
                    filesCount,
                    filesSize,
                    0,
                    0,
                    task.SourcePath,
                    task.DestPath
                    );

                ExecuteView.TasksState(task.Name, false);
                ExecuteView.DisplayMessage();
            }
        }

        private void LogFileCopied(string[] data)
        {
            int value2;
            float value3;

            if (int.TryParse(data[2], out value2) && float.TryParse(data[3], out value3))
            {
                TaskModel.UpdateTaskState(
                    TaskModel.Name,
                    (StateType)TaskModel.State,
                    TaskModel.LeftFilesNumber,
                    TaskModel.LeftFilesSize,
                    CopyModel.LeftFilesNumber,
                    CopyModel.LeftFilesSize,
                    TaskModel.SourcePath,
                    TaskModel.DestPath
                );

                if (initializeLeftFilesSize == 0)
                {
                    initializeLeftFilesSize = TaskModel.LeftFilesSize ?? 0;
                }

                int pourcentage = (int)((initializeLeftFilesSize - TaskModel.LeftFilesSize ?? 0) / initializeLeftFilesSize * 100);

                LogModel = new LogModel(ConfigModel.Config.LogExtension);
                LogModel.CreateLog(
                    TaskModel.Name,
                    data[0],
                    data[1],
                    value2,
                    value3
                );

                ExecuteView.DisplayLoading(pourcentage);
            }
        }
    }
}
