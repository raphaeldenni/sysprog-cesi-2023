using EasySave.Models;
using EasySave.Types;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class CreateViewModel
    {
        public CreateView CreateView { get; set; }

        public HelpView HelpView { get; set; }

        public TaskModel TaskModel { get; set; }

        public CreateViewModel(string[] args)
        {
            CreateView = new CreateView();
            HelpView = new HelpView();

            if (!(args.Length == 4))
            {
                HelpView.DisplayCreate();
                HelpView.DisplayMessage();
            }
            else
            {
                CreateTask(args);
            }
        }

        public void CreateTask(string[] args)
        {

            TaskModel = new TaskModel();

            if (Enum.TryParse<BackupType>(args[3], true, out BackupType backupType))
            {
                string result = TaskModel.UpdateTask(true, args[0], args[1], args[2], backupType, null);
                CreateView.Message = result;
            } 
            else 
            {
                CreateView.Message = "This is a wrong backup type";
            }


            CreateView.DisplayMessage();
        }
    }
}
