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

        public ConfigView()
        {
            InitializeComponent();

            ConfigViewModel = new ConfigViewModel();

            this.DataContext = ConfigViewModel.ConfigModel.Config;
            BindBox();
        }

        public void BindBox ()
        {
            ExtensionsTextBox.Text = ConfigViewModel.ConfigModel.Config.ExtensionsToEncrypt != null ? string.Join(",", ConfigViewModel.ConfigModel.Config.ExtensionsToEncrypt) : "";

            KeyTextBox.Text = ConfigViewModel.ConfigModel.Config.Key;

            LangComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.Language.ToString();
            LangComboBox.ItemsSource = ConfigViewModel.LangTypeComboItem;

            LogComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.LogExtension.ToString();
            LogComboBox.ItemsSource = ConfigViewModel.LogTypeComboItem;
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] extensions = ExtensionsTextBox.Text.Split(',');
                ConfigViewModel.UpdateConfigFile((LogType?)Enum.Parse(typeof(LogType), LogComboBox.Text), (LangType?)Enum.Parse(typeof(LangType), LangComboBox.Text), KeyTextBox.Text, extensions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
