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

        public ListViewModel(string[] args)
        {
            ListView = new ListView();
            TaskModel = new TaskModel();
            CreateTaskList(); // Display the task list upon instance creation.
        }

        public void CreateTaskList()
        {
            List<TaskEntity>? tasks = TaskModel.TasksList;
            bool allNamesEmpty = tasks.All(task => string.IsNullOrWhiteSpace(task.Name));

            if (tasks == null || allNamesEmpty)
            {
                ListView.Message = "There are no existing tasks";
                ListView.DisplayMessage();
            }
            else
            {
                // Filter out tasks with null names
                tasks = tasks.Where(task => task.Name != null).ToList();

                foreach (var task in tasks)
                {
                    string message = $"ID: {task.Id}, Name: {task.Name}, State: {task.State}, Type: {task.Type}";
                    ListView.Message = message;
                    ListView.DisplayMessage();
                }
            }
        }
        //Updates the status file and refreshes the displayed task list.
        public void UpdateStatFile()
        {
            TaskModel.PullStateFile();
            CreateTaskList();
        }
    }
}
