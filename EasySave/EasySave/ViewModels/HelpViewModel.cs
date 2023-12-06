using EasySave.Models;
using EasySave.Views;

namespace EasySave.ViewModels
{
    public class HelpViewModel
    {
        public HelpView HelpView { get; set; }
        public ConfigModel ConfigModel { get; set; }

        public HelpViewModel()
        {
            ConfigModel = new ConfigModel();

            HelpView = new HelpView(ConfigModel.Config.Language);
            HelpView.DisplayAll();
        }

        public HelpViewModel(string[] args)
        {
            ConfigModel = new ConfigModel();

            HelpView = new HelpView(ConfigModel.Config.Language);

            string methodName = "Display" + args[0];

            var method = typeof(HelpView).GetMethod(methodName);

            if (method != null)
            {
                method.Invoke(HelpView, null);
            }
            else
            {
                HelpView.ErrorCommandName();
            }

            HelpView.DisplayMessage();
        }
    }
}
