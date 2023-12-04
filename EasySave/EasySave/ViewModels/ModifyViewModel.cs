using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class ExecuteViewModel
    {
        public ExecuteView ExecuteView { get; set; }
        //public StateM StateModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public LogModel LogModel { get; set; }

        public ExecuteViewModel()
        {
        }

        public void ExecuteTask()
        {
        }
    }
}
