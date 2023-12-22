using EasySave.Models;
using EasySave.Types;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public ConfigModel ConfigModel { get; set; }
        public TaskModel TaskModel { get; set; }
        public LogModel LogModel { get; set; }
        public List<TaskEntity> Tasks { get; set; }
        public bool IsJobApplicationDetected { get; set; }
        public bool ProcessWasDetected { get; set; }
        public bool PriorityFilesRun { get; set; } = true;

        public Dictionary<int, bool> IsManualPause { get; set; } = new Dictionary<int, bool>();
        private Dictionary<int, ManualResetEvent> TaskPauseEvents = new Dictionary<int, ManualResetEvent>();

        List<Thread> Threads = new List<Thread>();

        static object TaskLock = new object();
        static object LogLock = new object();

        public HomeViewModel()
        {
            ConfigModel = new ConfigModel();
            TaskModel = new TaskModel();
            Tasks = GetAllTasks(null);
            IsManualPause = Tasks.ToDictionary(task => (int)task.Id, task => false);
            Task.Run(() => JobApplicationDetected());
            Task.Run(() => PriorityFiles());
        }

        public void UpdateTask(string name, bool isModification, string? newName, string? source, string? dest, BackupType? backupType)
        {
            lock (TaskLock)
            {
                try
                {
                    PriorityFilesRun = false;
                    TaskModel.UpdateTask(!isModification, name, source, dest, backupType, newName);
                    Tasks = GetAllTasks(null);
                    IsManualPause = Tasks.ToDictionary(task => (int)task.Id, task => false);
                }
                finally
                {
                    PriorityFilesRun = true;
                }
            }
        }

        public void JobApplicationDetected()
        {
            while (true)
            {
                string[] processNames = null;
                processNames = ConfigModel.Config.JobApplications;

                if (processNames != null)
                {
                    bool anyApplicationDetected = false; 

                    foreach (var processName in processNames)
                    {
                        Process[] processes = Process.GetProcessesByName(processName);

                        if (processes.Length > 0)
                        {
                            anyApplicationDetected = true;
                            ProcessWasDetected = true;
                        }
                    }

                    IsJobApplicationDetected = anyApplicationDetected;

                    if (IsJobApplicationDetected)
                    {
                        PauseAllTasks();
                    }
                    else if (ProcessWasDetected)
                    {
                        ProcessWasDetected = false;
                        ResumeAllTasks();
                    }
                }
            }
        }

        public void PriorityFiles()
        {
            while (PriorityFilesRun)
            {

                foreach (var task in Tasks)
                {
                    var tasks = Tasks.Any(t => t != task && t.LeftNumberPriorityFiles > 0) && task.State == StateType.Active;

                    if (task.State == StateType.Active && task.LeftNumberPriorityFiles == 0 && tasks)
                    {
                        PauseTask(task);
                    }

                    if (task.LeftNumberPriorityFiles != 0 && !IsManualPause[(int)task.Id])
                    {
                        ResumeTask(task);
                    }

                    if (!Tasks.Any(t => t.LeftNumberPriorityFiles > 0 && t.State == StateType.Active) && !IsManualPause[(int)task.Id])
                    {
                        if (task.State == StateType.Active || task.State == StateType.Pause)
                        {
                            ResumeTask(task);
                        }
                    }
                }


            }
        }

        public void PauseAllTasks()
        {
           foreach (var task in Tasks)
           {
                if (task.State == StateType.Active)
                {
                    PauseTask(task);
                }
           }
        }

        public void ResumeAllTasks()
        {
            foreach (var task in Tasks)
            {
                if (task.State == StateType.Pause && !IsManualPause[(int)task.Id])
                {
                    ResumeTask(task);
                }
            }
        }

        public void PauseTask(TaskEntity task)
        {
            if (TaskPauseEvents.ContainsKey((int)task.Id))
            {
                if (!TaskPauseEvents[(int)task.Id].WaitOne(0))
                {
                    return;
                }
                TaskPauseEvents[(int)task.Id].Reset();
                lock (TaskLock)
                {
                    TaskModel.UpdateTaskState(
                    task.Name,
                    StateType.Pause,
                    task.FilesNumber,
                    task.FilesSize,
                    task.LeftFilesNumber,
                    task.LeftFilesSize,
                    task.SourcePath,
                    task.DestPath
                    );
                }
            }
        }

        public void ResumeTask(TaskEntity task)
        {
            if (TaskPauseEvents.ContainsKey((int)task.Id) && !IsJobApplicationDetected)
            {
                if (TaskPauseEvents[(int)task.Id].WaitOne(0))
                {
                    return;
                }
                TaskPauseEvents[(int)task.Id].Set();
                lock (TaskLock)
                {
                    TaskModel.UpdateTaskState(
                    task.Name,
                    StateType.Active,
                    task.FilesNumber,
                    task.FilesSize,
                    task.LeftFilesNumber,
                    task.LeftFilesSize,
                    task.SourcePath,
                    task.DestPath
                    );
                }
            }
        }

        public void StopTask(TaskEntity task)
        {
            if (TaskPauseEvents.ContainsKey((int)task.Id))
            {
                TaskPauseEvents[(int)task.Id].Reset();
                lock (TaskLock)
                {
                    task.Loading = 0;
                    task.LeftFilesNumber = 0;
                    task.LeftFilesSize = 0;

                    TaskModel.UpdateTaskState(
                        task.Name,
                        StateType.Inactive,
                        task.FilesNumber,
                        task.FilesSize,
                        task.LeftFilesNumber,
                        task.LeftFilesSize,
                        task.SourcePath,
                        task.DestPath
                     );
                }


                var thread = Threads.Where(thread => thread.Name != task.Name).FirstOrDefault();
                thread?.Abort();
                //TaskPauseEvents.Remove(task.Name);

            }
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
                task.FileDestPath = task.SourcePath;
                task.FileSourcePath = task.DestPath;
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
            TaskPauseEvents[(int)task.Id].WaitOne();

            // Get task info
            var taskType = task.Type;
            StateType taskState = StateType.Active;

            // Set copy model
            var copyModel = new CopyModel(
                task,
                ConfigModel.Config.Key,
                ConfigModel.Config.ExtensionsToEncrypt,
                ConfigModel.Config.PriorityFilesExtensions
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
        }

        private void LogFileCopied(TaskEntity task, string[] data)
        {
            TaskPauseEvents[(int)task.Id].WaitOne();
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
                decimal decimalPercentage = Math.Round((decimal)percentage, 2);

                task.Loading = decimalPercentage;

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
                if (task.State == StateType.Inactive)
                {
                    Thread thread = new Thread(() => ExecuteOneTask(task));
                    if (!TaskPauseEvents.ContainsKey((int)task.Id))
                    {
                        TaskPauseEvents[(int)task.Id] = new ManualResetEvent(true);
                    }
                    thread.Name = task.Name;
                    Threads.Add(thread);
                    thread.Start();
                }
            }
        }
    }
}
