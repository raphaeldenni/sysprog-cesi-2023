using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModels
{
    public class ListViewModel
    {
        public ListView ListView { get; set; }
        public TaskModel TaskModel { get; set; }

        public ListViewModel()
        {
        }
    }
}
