using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class CreateViewModel
    {
        public IView CreateView { get; set; }
        public ITaskModel TaskModel { get; set; }

        public CreateViewModel()
        {
        }

        public void CreateTask()
        {
        }
    }
}
