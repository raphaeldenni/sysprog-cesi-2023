using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveGraphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Lorsque la fenêtre est chargée, naviguez vers la page HomeView
            NavigateToHomeView();
        }

        private void NavigateToHomeView()
        {
            // Obtenez le chemin de la vue HomeView.xaml
            Uri homeViewUri = new Uri("Views/HomeView.xaml", UriKind.Relative);

            // Naviguez vers la page HomeView dans le Frame
            navframe.Navigate(homeViewUri);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangePage(Uri pageUri)
        {
            if (navframe != null)
            {
                navframe.Navigate(pageUri);
            }
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            // Changer de page vers la vue ConfigView.xaml
            ChangePage(new Uri("Views/ConfigView.xaml", UriKind.Relative));
        }

        // Exemple de changement de page pour le bouton "Home"
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            // Changer de page vers la vue HomeView.xaml
            ChangePage(new Uri("Views/HomeView.xaml", UriKind.Relative));
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtenez le chemin complet du dossier "logs"
                string logsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

                // Vérifiez si le dossier existe
                if (System.IO.Directory.Exists(logsFolderPath))
                {
                    // Ouvrir l'explorateur de fichiers avec le dossier "logs"
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = logsFolderPath,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal,  // Ou ProcessWindowStyle.Maximized
                    };

                    // Démarrer le processus
                    Process.Start(psi);
                }
                else
                {
                    MessageBox.Show(Lang.Resources.Message_ErrorLog, Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}