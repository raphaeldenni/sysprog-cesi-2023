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
using System.Windows.Shapes;

namespace EasySaveGraphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HomeView HomeView { get; set; }
        public ConfigView ConfigView { get; set; }
        public MainWindow()
        {
            HomeView = new HomeView();
            ConfigView = new ConfigView();
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToHomeView();
        }

        private void NavigateToHomeView()
        {
            try {
                navframe.Navigate(HomeView);
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
                navframe.Navigate(ConfigView);
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
                navframe.Navigate(HomeView);
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
                string logsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

                if (System.IO.Directory.Exists(logsFolderPath))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = logsFolderPath,
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