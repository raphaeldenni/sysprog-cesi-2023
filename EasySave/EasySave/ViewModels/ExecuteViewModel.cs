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
        public void ExecuteTasks(string taskRow)
        {
           // If taskRow begin with a char, execute one task
           if (!string.IsNullOrEmpty(taskRow) && Char.IsLetter(taskRow[0]))
           {
               ExecuteOneTask(taskRow);
           }
           
           // Execute task row
           
           
           // Execute task to task
           
        }
        
        public void ExecuteOneTask(string taskName)
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
                
                ExecuteView.Message = "Task started";
                ExecuteView.DisplayMessage();
            }
        }
    }
}
