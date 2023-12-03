using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class DeleteViewModel
    {
        public IView DeleteView { get; set; }
        public ITaskModel TaskModel { get; set; }

        public DeleteViewModel()
        {
        }

        public void DeleteTask()
        {
        }
    }
}
