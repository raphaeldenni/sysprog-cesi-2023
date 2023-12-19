using System;
using System.Threading;
using System.Windows;
using EasySave.Models;

namespace EasySaveGraphic
{
    public partial class App : Application
    {
        // Define the name for the Mutex
        private const string MutexName = "Global\\EasySaveApplicationMutex";
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Load language settings from the config.json file
            string language = LoadLang();

            // Set the UI culture based on the loaded language
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);

            // Set the culture of the application's resources
            EasySaveGraphic.Lang.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            // Use a Mutex to ensure mono-instance (after loading the language)
            mutex = new Mutex(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                // Another instance of the application is already running
                MessageBox.Show(Lang.Resources.Message_ApplicationAlreadyRunning, "EasySave", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private string LoadLang()
        {
            var model = new ConfigModel();
            return model.Config.Language.ToString();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Release the Mutex resource upon application closure
            mutex.ReleaseMutex();
            mutex.Dispose();
            Environment.Exit(0);
            base.OnExit(e);
        }
    }
}
