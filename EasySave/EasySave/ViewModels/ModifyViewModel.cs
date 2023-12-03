using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class ExecuteViewModel
    {
        public IView ExecuteView { get; set; }
        public ITaskModel StateModel { get; set; }
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
