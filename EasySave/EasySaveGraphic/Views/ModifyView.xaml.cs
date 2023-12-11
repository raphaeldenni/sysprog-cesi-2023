using EasySave.Models;
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
    /// Logique d'interaction pour ModifyView.xaml
    /// </summary>
    public partial class ModifyView : Page
    {
        ModifyViewModel ModifyViewModel { get; set; }
        public string CurrentTaskName { get; set; }
        bool IsModification { get; set; }

        public ModifyView(TaskEntity? task)
        {
            InitializeComponent();

            ModifyViewModel = new ModifyViewModel();
            BindComboBox();

            if (task != null)
            {
                CurrentTaskName = task.Name;
                IsModification = true;
                BindModification(task);
            } else
            {
                IsModification = false;
            }
        }

        public void BindModification(TaskEntity task)
        {
            NameTextBox.Text = task.Name;
            SourceTextBox.Text = task.SourcePath;
            DestTextBox.Text = task.DestPath;
            TypeComboBox.SelectedItem = task.Type;
        }

        public void BindComboBox()
        {
            TypeComboBox.SelectedItem = BackupType.Complete;
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(BackupType));
        }

        private void Button_GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Vérifier si le NavigationService existe et si la navigation arrière est possible
            if (navigationService != null && navigationService.CanGoBack)
            {
                // Effectuer la navigation arrière
                navigationService.GoBack();
            }
        }

        private void Button_Source_Click (object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                SourceTextBox.Text = dialog.SelectedPath;
            }
        }

        private void Button_Dest_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                DestTextBox.Text = dialog.SelectedPath;
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (IsModification)
                {
                    ModifyViewModel.UpdateTask(CurrentTaskName, !IsModification, NameTextBox.Text, SourceTextBox.Text, DestTextBox.Text, (BackupType?)Enum.Parse(typeof(BackupType), TypeComboBox.Text));
                } else
                {
                    ModifyViewModel.UpdateTask(NameTextBox.Text, !IsModification, null, SourceTextBox.Text, DestTextBox.Text, (BackupType?)Enum.Parse(typeof(BackupType), TypeComboBox.Text));
                }
                Button_GoBack_Click(sender, e);

            } catch (Exception ex)
            {
                Exception test = ex;
            }
        }
    }
}
