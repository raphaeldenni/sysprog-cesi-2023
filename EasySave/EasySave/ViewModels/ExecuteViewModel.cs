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
        public ExecuteView ExecuteView { get; set; }
        public HelpView HelpView { get; set; }
        public TaskModel TaskModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public LogModel LogModel { get; set; }

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
                CopyModel = new CopyModel(task.SourcePath, task.DestPath, Enum.Parse<BackupType>(task.Type));
                CopyModel.CopyFiles();
                
                ExecuteView.Message = "Task started";
                ExecuteView.DisplayMessage();
            }
        }
    }
}
