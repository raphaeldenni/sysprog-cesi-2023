using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Types;

namespace EasySave.ViewModels
{
    public class ExecuteViewModel
    {
        // Properties
        
        //// Views
        public ExecuteView ExecuteView { get; set; }
        public HelpView HelpView { get; set; }
        
        //// Models
        public TaskModel TaskModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public LogModel LogModel { get; set; }
        
        //// Others
        public string TaskName { get; set; }
        public string TaskState { get; set; }
        public BackupType TaskType { get; set; }
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
        public int FilesCount { get; set; }
        public long FilesSize { get; set; }
        
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
                ExecuteTask(args[1]);
            }
        }
        
        // Methods
        public void ExecuteTask(string taskName)
        {
            var task = TaskModel.TasksList?.FirstOrDefault(t => t.Name == taskName);
            
            if (task == null)
            {
                ExecuteView.Message = "Task not found";
                ExecuteView.DisplayMessage();
            }
            else
            {
                // Get task info
                TaskName = task.Name;
                TaskType = Enum.Parse<BackupType>(task.Type);
                TaskState = "Active";
                SourcePath = task.SourcePath;
                DestPath = task.DestPath;
                
                // Set copy model
                CopyModel = new CopyModel(SourcePath, DestPath, TaskType);
                FilesCount = CopyModel.LeftFilesNumber;
                FilesSize = CopyModel.LeftFilesSize;
                
                // Update task state
                TaskModel.UpdateTaskState(
                    TaskName, 
                    TaskState, 
                    FilesCount, 
                    FilesSize, 
                    FilesCount, 
                    FilesSize, 
                    SourcePath, 
                    DestPath
                    );
                
                // Start copy
                CopyModel.CopyFiles();
                
                ExecuteView.Message = "Task started";
                ExecuteView.DisplayMessage();
            }
        }
    }
}
