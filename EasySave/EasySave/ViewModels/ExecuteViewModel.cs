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
               
               foreach (var taskIdString in tasksIdString)
               {
                   var taskId = int.Parse(taskIdString) - 1;
                   var taskName = TaskModel.TasksList?.FirstOrDefault(t => t.Id == taskId)?.Name;
                   
                   ExecuteOneTask(taskName);
               }
           }
           
           // Execute task to task
           if (!string.IsNullOrEmpty(tasksRow) && tasksRow.Contains('-'))
           {
                var tasksIdString = tasksRow.Split('-');

                var firstTaskId = int.Parse(tasksIdString[0]) - 1;
                var secondTaskId = int.Parse(tasksIdString[1]) - 1;
                
                for (var taskId = firstTaskId; taskId >= secondTaskId; taskId++)
                {
                    var taskName = TaskModel.TasksList?.FirstOrDefault(t => t.Id == taskId)?.Name;
                    
                    ExecuteOneTask(taskName);
                }
           }
        }
        
        public void ExecuteOneTask(string? taskName)
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
                
                ExecuteView.Message = "Task started";
                ExecuteView.DisplayMessage();
                
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
                
                ExecuteView.Message = "Task finished";
                ExecuteView.DisplayMessage();
            }
        }
    }
}
