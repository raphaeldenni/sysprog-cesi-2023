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

        static object TaskLock = new object();
        static object LogLock = new object();

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

        public void ExecuteOneTask(TaskEntity task)
        {
            // Get task info
            var taskType = task.Type;
            StateType taskState = StateType.Active;

            // Set copy model
            var copyModel = new CopyModel(
                task,
                ConfigModel.Config.Key,
                ConfigModel.Config.ExtensionsToEncrypt
                );

            task = copyModel.Task;

            lock (TaskLock)
            {
                // Update task state
                TaskModel.UpdateTaskState(
                    task.Name,
                    taskState,
                    task.FilesNumber,
                    task.FilesSize,
                    task.LeftFilesNumber,
                    task.LeftFilesSize,
                    task.SourcePath,
                    task.DestPath
                    );
            }

            copyModel.FileCopied += LogFileCopied;
            // Start copy
            copyModel.CopyFiles();

            taskState = StateType.Inactive;

            task.Loading = 0;
            task.LeftFilesNumber = 0;
            task.LeftFilesSize = 0;

            lock (TaskLock) {
                // Update task state
                TaskModel.UpdateTaskState(
                    task.Name,
                    taskState,
                    task.FilesNumber,
                    task.FilesSize,
                    task.LeftFilesNumber,
                    task.LeftFilesSize,
                    task.SourcePath,
                    task.DestPath
                    );
            }
        }

        private void LogFileCopied(TaskEntity task, string[] data)
        {
            int fileSize;
            float fileTransferTime;

            if (int.TryParse(data[2], out fileSize) && float.TryParse(data[3], out fileTransferTime))
            {
                lock (TaskLock)
                {
                    TaskModel.UpdateTaskState(
                        task.Name,
                        (StateType)task.State,
                        task.FilesNumber,
                        task.FilesSize,
                        task.LeftFilesNumber,
                        task.LeftFilesSize,
                        task.SourcePath,
                        task.DestPath
                    );
                }

                float percentage = (float)((task.FilesSize - task.LeftFilesSize) / task.FilesSize * 100);

                task.Loading = percentage;

                if (task.Loading == 100)
                {
                    task.Loading = 0;
                }

                lock (LogLock)
                {

                    LogModel = new LogModel();
                    LogModel.CreateLog(
                        ConfigModel.Config.LogExtension,
                        TaskModel.Name,
                        data[0],
                        data[1],
                        fileSize,
                        fileTransferTime
                    );
                }
            }
        }

        public void StartSelectedTasks(List<TaskEntity> tasks)
        {
            foreach (TaskEntity task in tasks)
            {
                Thread thread = new Thread(() => ExecuteOneTask(task));
                thread.Start();
                thread.Name = task.Name;
            }
        }
    }
}
