using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class DeleteViewModel
    {
        public DeleteView DeleteView { get; set; }
        public TaskModel TaskModel { get; set; }
        public HelpView HelpView { get; set; }

        public DeleteViewModel(string[] args)
        {
            DeleteView = new DeleteView();
            HelpView = new HelpView();

            if (!(args.Length == 1))
            {
                HelpView.DisplayDelete();
                HelpView.DisplayMessage();
            }
            else
            {
                DeleteTask(args);
            }
        }

        public void DeleteTask(string[] args)
        {
            TaskModel = new TaskModel();
            DeleteView.Message = TaskModel.DeleteTask(args[0]);
            DeleteView.DisplayMessage();
        }
    }
}
