using EasySave.Models;
using EasySave.Types;
using EasySave.Views;

using static EasySave.Models.TaskModel;

namespace EasySave.ViewModels
{
    public class CreateViewModel
    {
        public CreateView CreateView { get; set; }

        public HelpView HelpView { get; set; }

        public TaskModel TaskModel { get; set; }

        public ConfigModel ConfigModel { get; set; }

        public CreateViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            CreateView = new CreateView(ConfigModel.Config.Language);
            HelpView = new HelpView(ConfigModel.Config.Language);

            if (!(args.Length == 4))
            {
                HelpView.DisplayCreate();
                HelpView.DisplayMessage();
            }
            else
            {
                CreateTask(args);
            }
        }

        public void CreateTask(string[] args)
        {

            TaskModel = new TaskModel();

            if (Enum.TryParse<BackupType>(args[3], true, out BackupType backupType))
            {
                try
                {
                    string[] result = TaskModel.UpdateTask(true, args[0], args[1], args[2], backupType, null);
                    CreateView.SuccessfulCreation(result);
                }
                catch (SourcePathNotFoundException)
                {
                    CreateView.ErrorSourcePathNotFound();
                }
                catch (DuplicateTaskNameException)
                {
                    CreateView.ErrorDuplicateTaskName();
                }
                catch (TooMuchTasksException)
                {
                    CreateView.ErrorTooMuchTasks();
                }
            } 
            else 
            {
                CreateView.ErrorBackupType();
            }


            CreateView.DisplayMessage();
        }
    }
}
