using EasySave.Models;
using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
        logExtension = "." + logExtension.ToLower();
        ConfigModel.UpdateConfigFile(logExtension, null);
    }

    public void UpdateLang(string lang)
    {
        ConfigModel = new ConfigModel();
        ConfigModel.UpdateConfigFile(null, lang.ToLower());
    }
}