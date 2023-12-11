using EasySave.Models;
using EasySave.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveGraphic.ViewModels
{
    public class HomeViewModel
    {
        public event Action<TaskEntity, int> NotifyTaskUpdated;
        public ConfigModel ConfigModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public TaskModel TaskModel { get; set; }
        public TaskEntity TaskEntity { get; set; }
        public LogModel LogModel { get; set; }
        public List<TaskEntity> Tasks { get; set; }

        public TaskEntity CurrentTask { get; set; }
        private float initializeLeftFilesSize = 0;

        public HomeViewModel()
        {
            ConfigModel = new ConfigModel();
            TaskModel = new TaskModel();
            Tasks = GetAllTasks(null);
        }

        public List<TaskEntity> GetAllTasks(string? search)
        {
            List<TaskEntity>? tasks = TaskModel.TasksList;

            tasks = tasks.Where(task => task.Name != null).ToList();

            if (search != null)
            {
                tasks = tasks.Where(task => task.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            foreach (var task in tasks)
            {
                task.IsChecked = false;
                task.Loading = task.Loading ?? 0;
            }
            return tasks;
        }

        public bool DeleteSelectedTasks(List<TaskEntity> tasks)
        {
            try
            {
                foreach (TaskEntity task in tasks)
                {
                    TaskModel.DeleteTask(task.Name);
                }
            } catch (Exception ex) 
            {
                return false;
            }

            return true;

        }

        public async Task ExecuteOneTask(TaskEntity task)
        {
            await Task.Run(() =>
            {
                CurrentTask = task;

                initializeLeftFilesSize = 0;

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

                Application.Current.Dispatcher.Invoke(() => NotifyTasksListUpdated(CurrentTask));
            });
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

                int percentage = (int)((initializeLeftFilesSize - TaskModel.LeftFilesSize ?? 0) / initializeLeftFilesSize * 100);

                CurrentTask.Loading = percentage;

                if (percentage == 100)
                {
                    CurrentTask.Loading = 0;
                }

                NotifyTasksListUpdated(CurrentTask);

                LogModel = new LogModel(ConfigModel.Config.LogExtension);
                LogModel.CreateLog(
                    TaskModel.Name,
                    data[0],
                    data[1],
                    value2,
                    value3
                );
            }
        }

        private void NotifyTasksListUpdated(TaskEntity currentTask)
        {
            int taskIndex = Tasks.FindIndex(task => task.Name == currentTask.Name);
            Application.Current.Dispatcher.Invoke(() => NotifyTaskUpdated?.Invoke(currentTask, taskIndex));
        }

        public async Task StartSelectedTasks(List<TaskEntity> tasks)
        {
            foreach (TaskEntity task in tasks)
            {
                await ExecuteOneTask(task);
            }
        }
    }
}
