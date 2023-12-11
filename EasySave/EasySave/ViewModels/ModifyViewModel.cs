using EasySave.Models;
using EasySave.Types;
using EasySave.Views;

using static EasySave.Models.TaskModel;

namespace EasySave.ViewModels
{
    internal class ModifyViewModel
    {
        public ModifyView ModifyView { get; set; }
        public HelpView HelpView { get; set; }
        public TaskModel TaskModel { get; set; }
        public ConfigModel ConfigModel { get; set; }

        public ModifyViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            ModifyView = new ModifyView(ConfigModel.Config.Language);
            HelpView = new HelpView(ConfigModel.Config.Language);

            if (!(args.Length == 3))
            {
                HelpView.DisplayModify();
                HelpView.DisplayMessage();
            }
            else
            {
                string methodName = "Modify" + args[1];

                var method = typeof(ModifyViewModel).GetMethod(methodName);

                var parameters = new object[1];
                parameters[0] = args;

                if (method != null)
                {
                    method.Invoke(this, parameters);
                }
            }
        }

        public void ModifyName(string[] args)
        {
            TaskModel = new TaskModel();

            try
            {
                string[] result = TaskModel.UpdateTask(false, args[0], null, null, null, args[2]);
                ModifyView.SuccessfulModify(result);
            }
            catch (TaskNameNotFoundException)
            {
                ModifyView.ErrorTaskNameNotFound();
            }
            catch (DuplicateTaskNameException)
            {
                ModifyView.ErrorTaskNameNotFound();
            }

            ModifyView.DisplayMessage();
        }

        public void ModifySource(string[] args)
        {
            TaskModel = new TaskModel();

            try
            {
                string[] result = TaskModel.UpdateTask(false, args[0], args[2], null, null, null);
                ModifyView.SuccessfulModify(result);
            }
            catch (TaskNameNotFoundException)
            {
                ModifyView.ErrorTaskNameNotFound();
            }
            catch (SourcePathNotFoundException)
            {
                ModifyView.ErrorSourcePathNotFound();
            }

            ModifyView.DisplayMessage();
        }

        public void ModifyDest(string[] args)
        {
            TaskModel = new TaskModel();

            try
            {
                string[] result = TaskModel.UpdateTask(false, args[0], null, args[2], null, null);
                ModifyView.SuccessfulModify(result);
            }
            catch (SourcePathNotFoundException)
            {
                ModifyView.ErrorSourcePathNotFound();
            }

            ModifyView.DisplayMessage();
        }

        public void ModifyType(string[] args)
        {
            TaskModel = new TaskModel();

            if (Enum.TryParse<BackupType>(args[2], true, out BackupType backupType))
            {
                try
                {
                    string[] result = TaskModel.UpdateTask(false, args[0], null, null, backupType, null);
                    ModifyView.SuccessfulModify(result);
                }
                catch (TaskNameNotFoundException)
                {
                    ModifyView.ErrorTaskNameNotFound();
                }
            }
            else
            {
                string validBackupTypes = string.Join("|", Enum.GetNames(typeof(BackupType)));
                ModifyView.ErrorBackupType(validBackupTypes);
            }

            ModifyView.DisplayMessage();
        }

    }
}
