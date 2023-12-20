using EasySave.Types;
using EasySaveGraphic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySaveGraphic.Views
{
    /// <summary>
    /// Logique d'interaction pour ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Page
    {
        internal ConfigViewModel ConfigViewModel { get; set; }

        private static ConfigView _instance;

        public static ConfigView Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigView();
                }
                return _instance;
            }
        }

        private ConfigView()
        {
            InitializeComponent();

            ConfigViewModel = new ConfigViewModel();

            this.DataContext = ConfigViewModel.ConfigModel.Config;
            BindBox();
        }

        public void BindBox ()
        {
            try
            {
                ExtensionsTextBox.Text = ConfigViewModel.ConfigModel.Config.ExtensionsToEncrypt != null ? string.Join(",", ConfigViewModel.ConfigModel.Config.ExtensionsToEncrypt) : "";

                JobApplicationTextBox.Text = ConfigViewModel.ConfigModel.Config.JobApplications != null ? string.Join(",", ConfigViewModel.ConfigModel.Config.JobApplications) : "";

                KeyTextBox.Text = ConfigViewModel.ConfigModel.Config.Key;

                LangComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.Language.ToString();
                LangComboBox.ItemsSource = ConfigViewModel.LangTypeComboItem;

                LogComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.LogExtension.ToString();
                LogComboBox.ItemsSource = ConfigViewModel.LogTypeComboItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] extensions = ExtensionsTextBox.Text.Split(',');
                string[] jobApplications = JobApplicationTextBox.Text.Split(',');
                ConfigViewModel.UpdateConfigFile((LogType?)Enum.Parse(typeof(LogType), LogComboBox.Text), (LangType?)Enum.Parse(typeof(LangType), LangComboBox.Text), KeyTextBox.Text, extensions, jobApplications);
                MessageBox.Show(Lang.Resources.Message_SuccessConfig, Lang.Resources.Success, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Resources.Message_ErrorGeneral} {ex.Message}", Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
