using EasySave.Models;
using EasySave.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveGraphic.ViewModels
{
    public class HomeViewModel
    {
        public ConfigModel ConfigModel { get; set; }
        public CopyModel CopyModel { get; set; }
        public TaskModel TaskModel { get; set; }
        public TaskEntity TaskEntity { get; set; }
        public List<TaskEntity> Tasks { get; set; }

        public HomeViewModel()
        {
            ConfigModel = new ConfigModel();
            TaskModel = new TaskModel();
            Tasks = GetAllTasks();
        }

        public List<TaskEntity> GetAllTasks()
        {
            List<TaskEntity>? tasks = TaskModel.TasksList;
            tasks = tasks.Where(task => task.Name != null).ToList();
            foreach (var task in tasks)
            {
                task.IsChecked = false;
            }
            return tasks;
        }

        public bool DeleteSelectedTasks(List<TaskEntity> tasks)
        {
            try
            {
                foreach (TaskEntity task in tasks)
                {
                    TaskModel.DeleteTask(task.Name);
                }
            } catch (Exception ex) 
            {
                return false;
            }

            return true;

        }

        //public void StartSelectedTasks(List<TaskEntity> tasks)
        //{
        //    try
        //    {
        //        foreach (TaskEntity task in tasks)
        //        {
        //            CopyModel = new CopyModel(task.SourcePath, task.DestPath, (BackupType)task.Type);
        //        }
        //    }
        //}
    }
}
