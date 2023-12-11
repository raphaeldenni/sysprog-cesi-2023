using EasySave.Types;
using System.Text.Json;

namespace EasySave.Models;

public class ConfigEntity 
{
    public LangType Language { get; set; }
    public LogType LogExtension { get; set; }
    public string? Key { get; set; }
    public string[]? ExtensionsToEncrypt { get; set; }
}

public class ConfigModel
{
    private static string ConfigFileName => "config.json";
    public ConfigEntity? Config { get; private set; }
        
    /// <summary>
    /// Config model constructor
    /// </summary>
    public ConfigModel()
    {
        if (!File.Exists(ConfigFileName))
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
        var defaultConfig = new ConfigEntity
        {
            LogExtension = LogType.Json,
            Language = LangType.En,
            Key = "0123456789ABCDEF",
            ExtensionsToEncrypt = new[] { ".txt", ".docx", ".doc", ".pdf", ".xlsx", ".xls" }
        };

        var defaultConfigJson = JsonSerializer.Serialize(defaultConfig);
        File.WriteAllText(ConfigFileName, defaultConfigJson);
    }
        
    /// <summary>
    /// Pulls the config file and stores it in the Config property
    /// </summary>
    private void PullConfigFile()
    {
        var configJson = File.ReadAllText(ConfigFileName);
        Config = JsonSerializer.Deserialize<ConfigEntity>(configJson);
    }
        
    /// <summary>
    /// Updates the config file with the given parameters
    /// </summary>
    /// <param name="logExtension"></param>
    /// <param name="lang"></param>
    public void UpdateConfigFile(LogType? logExtension, LangType? lang)
    {
        if (logExtension != null)
        {
            Config!.LogExtension = (LogType)logExtension;
        }

        if (lang != null)
        {
            Config!.Language = (LangType)lang;
        }

        var updatedConfigJson = JsonSerializer.Serialize(Config);
        File.WriteAllText(ConfigFileName, updatedConfigJson);
    }
}