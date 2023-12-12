using EasySave.Models;
using EasySave.Types;
using EasySave.Views;

namespace EasySave.ViewModels;

internal class ConfigViewModel
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
            ConfigModel.UpdateConfigFile(logExtensionOut, null, null, null);
            ConfigView.SuccessfulLogExtension(logExtensionOut);
        }
        else 
        {
            string validLogTypes = string.Join("|", Enum.GetNames(typeof(LogType)));
            ConfigView.ErrorLogExtension(validLogTypes);
        }

        ConfigView.DisplayMessage();
    }

    public void UpdateLang(string lang)
    {
        if (Enum.TryParse<LangType>(lang, out LangType langOut))
        {
            ConfigModel.UpdateConfigFile(null, langOut, null, null);
            ConfigView.SuccessfulLang(langOut);
        }
        else
        {
            string validLangTypes = string.Join("|", Enum.GetNames(typeof(LangType)));
            ConfigView.ErrorLang(validLangTypes);
        }

        ConfigView.DisplayMessage();
    }

    public void UpdateKey(string key)
    {
        ConfigModel.UpdateConfigFile(null, null, key, null);
        ConfigView.SuccessfulKey();
        ConfigView.DisplayMessage();
    }

    public void UpdateExtensionsToEncrypt(string extensionsToEncrypt)
    {
        string[] extensions = extensionsToEncrypt.Split(',');
        ConfigModel.UpdateConfigFile(null, null, null, extensions);
        ConfigView.SuccessfulExtensionsToEncrypt();
        ConfigView.DisplayMessage();
    }
}