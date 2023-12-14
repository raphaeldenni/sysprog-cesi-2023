using System;
using System.Threading;
using System.Windows;
using EasySave.Models;

namespace EasySaveGraphic
{
    public partial class App : Application
    {
        private const string MutexName = "Global\\EasySaveApplicationMutex";
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Use a Mutex to ensure mono-instance
            mutex = new Mutex(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                // Une autre instance de l'application est déjà en cours d'exécution
                MessageBox.Show("Une autre instance de l'application est déjà en cours d'exécution.", "EasySave", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Shutdown();
                return;
            }

            // Charger les paramètres de langue depuis le fichier config.json
            string language = LoadLang();

            // Définir la culture de l'interface utilisateur (UI) en fonction de la langue chargée
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);

            // Définir la culture des ressources de l'application
            EasySaveGraphic.Lang.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            base.OnStartup(e);
        }

        private string LoadLang()
        {
            var model = new ConfigModel();
            return model.Config.Language.ToString();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Libérer la ressource Mutex lors de la fermeture de l'application
            mutex.ReleaseMutex();
            mutex.Dispose();

            base.OnExit(e);
        }
    }
}
