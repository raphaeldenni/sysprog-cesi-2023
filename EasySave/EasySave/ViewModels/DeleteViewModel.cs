using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasySave.Models.TaskModel;

namespace EasySave.ViewModels
{
    public class DeleteViewModel
    {
        public DeleteView DeleteView { get; set; }
        public TaskModel TaskModel { get; set; }
        public HelpView HelpView { get; set; }
        public ConfigModel ConfigModel { get; set; }

        public DeleteViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            DeleteView = new DeleteView(ConfigModel.Config.Language);
            HelpView = new HelpView(ConfigModel.Config.Language);

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
            try
            {
                string result = TaskModel.DeleteTask(args[0]);
                DeleteView.SuccessfulDelete(result);
            }
            catch (TaskNameNotFoundException) 
            {
                DeleteView.ErrorTaskNameNotFound();
            }
            
            DeleteView.DisplayMessage();
        }
    }
}
