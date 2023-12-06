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
        ConfigView = new ConfigView();
        HelpView = new HelpView();

        if (!(args.Length == 2))
        {
            HelpView.DisplayCreate();
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
    }

    public void UpdateLogextension(string logExtension)
    { 
        ConfigModel = new ConfigModel();
        if (Enum.TryParse<LogType>(logExtension, out LogType logExtensionOut))
        {
            ConfigModel.UpdateConfigFile(logExtensionOut, null);
        }
        else 
        {
            ConfigView.Message = "Mauvaise extension json";
            ConfigView.DisplayMessage();
        }
    }

    public void UpdateLang(string lang)
    {
        ConfigModel = new ConfigModel();
        if (Enum.TryParse<LangType>(lang, out LangType langOut))
        {
            ConfigModel.UpdateConfigFile(null, langOut);
        }
        else
        {
            ConfigView.Message = "Mauvaise langue";
            ConfigView.DisplayMessage();
        }
    }
}