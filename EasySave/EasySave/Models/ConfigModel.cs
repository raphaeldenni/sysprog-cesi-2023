using EasySave.Types;
using System.Text.Json;

namespace EasySave.Models;

public class ConfigEntity 
{
    public LangType Language { get; set; }
    public LogType LogExtension { get; set; }
    public string? Key { get; set; }
    public string[]? ExtensionsToEncrypt { get; set; }
    public string[]? JobApplications { get; set; }
    public string[]? PriorityFilesExtensions { get; set; }
}

public class ConfigModel
{
    private const string EasySaveFolderName = "EasySave";
    private const string ConfigFileName = "config.json";
    private string ConfigFilePath { get; set; }
    private string EasySaveFolderPath { get; set;}
    public ConfigEntity? Config { get; private set; }
        
    /// <summary>
    /// Config model constructor
    /// </summary>
    public ConfigModel()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        EasySaveFolderPath = Path.Combine(appDataPath, EasySaveFolderName);
        ConfigFilePath = Path.Combine(EasySaveFolderPath, ConfigFileName);

        if (!File.Exists(ConfigFilePath))
        {
            CreateConfigFile();
        }

        PullConfigFile();
    }

    /// <summary>
    /// Creates the config file with default values
    /// </summary>
    private void CreateConfigFile()
    {
        if (!Directory.Exists(EasySaveFolderPath))
        {
            Directory.CreateDirectory(EasySaveFolderPath);
        }

        var defaultConfig = new ConfigEntity
        {
            LogExtension = LogType.Json,
            Language = LangType.En,
            Key = "",
            ExtensionsToEncrypt = new[] { "" },
            JobApplications = new[] { "" },
            PriorityFilesExtensions = new[] { "" }
        };

        var defaultConfigJson = JsonSerializer.Serialize(defaultConfig);
        File.WriteAllText(ConfigFilePath, defaultConfigJson);
    }

    /// <summary>
    /// Pulls the config file and stores it in the Config property
    /// </summary>
    private void PullConfigFile()
    {
        var configJson = File.ReadAllText(ConfigFilePath);
        Config = JsonSerializer.Deserialize<ConfigEntity>(configJson);
    }

    /// <summary>
    /// Updates the config file with the given parameters
    /// </summary>
    /// <param name="logExtension"></param>
    /// <param name="lang"></param>
    /// <param name="key"></param>
    /// <param name="extensions"></param>
    public void UpdateConfigFile(LogType? logExtension, LangType? lang, string? key, string[]? extensions, string[]? jobsApplications)
    {
        if (logExtension != null)
        {
            Config!.LogExtension = (LogType)logExtension;
        }

        if (lang != null)
        {
            Config!.Language = (LangType)lang;
        }

        if (key != null)
        {
            Config!.Key = key;
        }

        if (extensions != null)
        {
            Config!.ExtensionsToEncrypt = extensions;
        }

        if (jobsApplications != null)
        {
            Config!.JobApplications = jobsApplications;
        }

        var updatedConfigJson = JsonSerializer.Serialize(Config);
        File.WriteAllText(ConfigFilePath, updatedConfigJson);
    }
}
