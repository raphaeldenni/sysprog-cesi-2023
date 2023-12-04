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
        public TaskModel TaskModel { get; set; }

        public CreateViewModel()
        {
        }

        public void CreateTask()
        {
        }
    }
}
