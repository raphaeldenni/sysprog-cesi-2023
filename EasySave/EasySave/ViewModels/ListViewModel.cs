using EasySave.Models;
using EasySave.Types;
using EasySave.Views;

namespace EasySave.ViewModels
{
    public class ListViewModel
    {
        public ListView ListView { get; set; }
        public TaskModel TaskModel { get; set; }
        public ConfigModel ConfigModel { get; set; }

        public ListViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            ListView = new ListView(ConfigModel.Config.Language);
            
            TaskModel = new TaskModel();
            CreateTaskList(); // Display the task list upon instance creation.
        }

        public void CreateTaskList()
        {
            List<TaskEntity>? tasks = TaskModel.TasksList;
            bool allNamesEmpty = tasks.All(task => string.IsNullOrWhiteSpace(task.Name));

            if (tasks == null || allNamesEmpty)
            {
                ListView.ErrorNoTask();
                ListView.DisplayMessage();
            }
            else
            {
                // Filter out tasks with null names
                tasks = tasks.Where(task => task.Name != null).ToList();

                foreach (var task in tasks)
                {
                    ListView.DisplayTask((int)task.Id, task.Name, task.State ?? StateType.Inactive, (BackupType)task.Type);
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
