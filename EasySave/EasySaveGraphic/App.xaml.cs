using System.Configuration;
using System.Data;
using System.Windows;
using EasySave.Models;

namespace EasySaveGraphic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load the language setting from the config.json file
            string language = LoadLang();

            // Set the CultureInfo of the current thread to the loaded language
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);

            // Set the Culture property of the Resources class
            EasySaveGraphic.Lang.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        private string LoadLang()
        {
            var model = new ConfigModel();
            return model.Config.Language.ToString();
        }
    }

}
