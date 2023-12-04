using EasySave.Models;
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

        public LogModel LogModel { get; set; }

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
            CreateView.Message = TaskModel.UpdateTask(true, args[0], args[1], args[2], args[3]);
            CreateView.DisplayMessage();
        }
    }
}
