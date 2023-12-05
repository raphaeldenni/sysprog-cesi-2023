using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Types;

namespace EasySave.ViewModels
{
    public class ExecuteViewModel
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

        // Constructor
        public ExecuteViewModel(string[] args)
        {
            ExecuteView = new ExecuteView();
            HelpView = new HelpView();
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
                ExecuteView.Message = "Tasks " + tasksNamesString + " finished";
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
                ExecuteView.Message = "Tasks " + tasksNamesString + " finished";
                ExecuteView.DisplayMessage();
            }
        }

        public void ExecuteOneTask(string? taskName)
        {
            var task = TaskModel.TasksList?.FirstOrDefault(t => t.Name == taskName);
            initializeLeftFilesSize = 0;

            if (task == null)
            {
                ExecuteView.Message = "Task not found";
                ExecuteView.DisplayMessage();
            }
            else
            {
                // Get task info
                var taskType = Enum.Parse<BackupType>(task.Type);
                var taskState = "Active";

                // Set copy model
                CopyModel = new CopyModel(task.SourcePath, task.DestPath, taskType);
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

                ExecuteView.Message = "Task " + TaskModel.Name + " started";
                ExecuteView.DisplayMessage();

                CopyModel.FileCopied += LogFileCopied;

                // Start copy
                CopyModel.CopyFiles();

                taskState = "Finished";

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

                ExecuteView.Message = "Task " + TaskModel.Name + " finished";
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
                    TaskModel.State,
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

                LogModel = new LogModel();
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
