using System.Text.Json;

namespace CryptoSoft
{
    public class ConfigEntity 
    {
        public LangType Lang { get; set; }
        public string? Key { get; set; }
    }

    public class ConfigModel
    {
        private static string ConfigFileName => "cryptoSoftConfig.json";
        public ConfigEntity? Config { get; private set; }

        public ConfigModel()
        {
            if (!File.Exists(ConfigFileName))
            {
                CreateConfigFile();
            }

            PullConfigFile();
        }

        private void CreateConfigFile()
        {
            var defaultConfig = new ConfigEntity
            {
                Lang = LangType.En,
                Key = "0123456789ABCDEF"
            };

            var defaultConfigJson = JsonSerializer.Serialize(defaultConfig);
            File.WriteAllText(ConfigFileName, defaultConfigJson);
        }

        private void PullConfigFile()
        {
            var configJson = File.ReadAllText(ConfigFileName);
            Config = JsonSerializer.Deserialize<ConfigEntity>(configJson);
        }
    }
}