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
            BindComboBox();
        }

        public void BindComboBox ()
        {
            LangComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.Language.ToString();
            LangComboBox.ItemsSource = ConfigViewModel.LangTypeComboItem;

            LogComboBox.SelectedItem = ConfigViewModel.ConfigModel.Config.LogExtension.ToString();
            LogComboBox.ItemsSource = ConfigViewModel.LogTypeComboItem;
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigViewModel.UpdateConfigFile((LogType?)Enum.Parse(typeof(LogType), LogComboBox.Text), (LangType?)Enum.Parse(typeof(LangType), LangComboBox.Text));
                MessageBox.Show("Config updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
