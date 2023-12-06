using EasySave.Models;
using EasySave.Types;
using EasySave.Views;

namespace EasySave.ViewModels;

public class ConfigViewModel
{
    public ConfigView ConfigView { get; set; }

    public HelpView HelpView { get; set; }

    public ConfigModel ConfigModel { get; set; }

    public ConfigViewModel(string[] args)
    {
        ConfigModel = new ConfigModel();

        ConfigView = new ConfigView(ConfigModel.Config.Language);
        HelpView = new HelpView(ConfigModel.Config.Language);

        if (!(args.Length == 2))
        {
            HelpView.DisplayConfig();
            HelpView.DisplayMessage();
        }
        else
        {
            UpdateConfig(args);
        }
    }

    public void UpdateConfig(string[] args)
    {
        string methodName = "Update" + args[0];
        var method = GetType().GetMethod(methodName);

        if (method != null)
        {
            object[] parameters = { args[1] };
            method.Invoke(this, parameters);
        }
        else
        {
            HelpView.DisplayConfig();
            HelpView.DisplayMessage();
        }
    }

    public void UpdateLogextension(string logExtension)
    { 
        if (Enum.TryParse<LogType>(logExtension, out LogType logExtensionOut))
        {
            ConfigModel.UpdateConfigFile(logExtensionOut, null);
            ConfigView.SuccessfulLogExtension(logExtensionOut);
        }
        else 
        {
            ConfigView.ErrorLogExtension();
        }

        ConfigView.DisplayMessage();
    }

    public void UpdateLang(string lang)
    {
        if (Enum.TryParse<LangType>(lang, out LangType langOut))
        {
            ConfigModel.UpdateConfigFile(null, langOut);
            ConfigView.SuccessfulLang(langOut);
        }
        else
        {
            ConfigView.ErrorLang();
        }

        ConfigView.DisplayMessage();
    }
}