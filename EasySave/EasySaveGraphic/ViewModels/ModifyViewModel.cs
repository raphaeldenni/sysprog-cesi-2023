using EasySave.Models;
using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EasySaveGraphic.ViewModels
{
    internal class ModifyViewModel
    {
        public TaskEntity TaskEntity { get; set; }
        public ConfigModel ConfigModel { get; set; }
        public TaskModel TaskModel { get; set; }

        public ModifyViewModel()
        {
            TaskModel = new TaskModel();
        }

        public void UpdateTask (string name, bool isNew, string? newName, string? source, string? dest, BackupType? backupType)
        {
            TaskModel.UpdateTask(isNew, name, source, dest, backupType, newName);
        }
    }
}
