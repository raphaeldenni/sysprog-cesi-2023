using System.Windows;
using EasySave.Models;
using EasySaveGraphic.ViewModels;

namespace EasySaveGraphic
{
    public partial class App : Application
    {
        // Define the name for the Mutex
        private const string MutexName = "Global\\EasySaveApplicationMutex";
        private static Mutex _mutex = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Activate the distant server in a thread
            var distantServerThread = new Thread(() => _ = new ServerViewModel());
            distantServerThread.Start();
            
            // Load language settings from the config.json file,
            // then set the UI culture and the culture of the application's resources with it 
            var language = LoadLang();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            Lang.Resources.Culture = Thread.CurrentThread.CurrentUICulture;

            // Use a Mutex to ensure mono-instance (after loading the language)
            _mutex = new Mutex(
                true, 
                MutexName, 
                out var createdNew
                );
            
            // Check if another instance of the application is already running
            if (!createdNew)
            {
                MessageBox.Show(
                    Lang.Resources.Message_ApplicationAlreadyRunning, 
                    "EasySave", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Exclamation
                );
                
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private static string LoadLang()
        {
            var model = new ConfigModel();
            return model.Config!.Language.ToString();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Release the Mutex resource upon application closure
            _mutex.ReleaseMutex();
            _mutex.Dispose();
            
            // Exit the application
            Environment.Exit(0);
            base.OnExit(e);
        }
    }
}
