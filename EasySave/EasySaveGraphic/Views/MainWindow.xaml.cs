using EasySaveGraphic.Views;
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
using System.IO;

namespace EasySaveGraphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomeView HomeViewInstance;
        private ConfigView ConfigViewInstance;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToHomeView();
        }

        private void NavigateToHomeView()
        {
            try
            {
                if (HomeViewInstance == null)
                {
                    HomeViewInstance = new HomeView();
                }

                navframe.Navigate(HomeViewInstance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigateToConfigView()
        {
            try
            {
                if (ConfigViewInstance == null)
                {
                    ConfigViewInstance = new ConfigView();
                }

                navframe.Navigate(ConfigViewInstance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            try {
                NavigateToConfigView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigateToHomeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var easySaveFolderPath = Path.Combine(appDataPath, "EasySave", "Logs");

                if (System.IO.Directory.Exists(easySaveFolderPath))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = easySaveFolderPath,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal,
                    };

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