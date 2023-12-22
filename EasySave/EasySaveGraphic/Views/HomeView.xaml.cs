using EasySave.Models;
using EasySave.Types;
using EasySaveGraphic.ViewModels;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace EasySaveGraphic.Views
{
    public partial class HomeView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        internal HomeViewModel HomeViewModel { get; set; }
        public List<TaskEntity> Tasks { get; set; }
        public LangType Lang { get; set; }
        public bool IsModification { get; set; }

        public HomeView()
        {
            StartClock();

            HomeViewModel = new HomeViewModel();
            Tasks = HomeViewModel.GetAllTasks(null);

            foreach (var task in Tasks)
            {
                task.Loading = 0;
                task.LeftNumberPriorityFiles = 0;
                task.State = StateType.Inactive;
            }

            Lang = HomeViewModel.ConfigModel.Config.Language;
            DataContext = this;

            InitializeComponent();
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TickEvent;
            timer.Start();
        }

        private void TickEvent(object sender, EventArgs e)
        {
            DigitalClockHour.Content = DateTime.Now.ToString(@"HH\:mm\:ss");
            DigitalClockDay.Content = DateTime.Now.ToString(@"dd\/MM\/yyyy");
        }

        private bool IsTaskRunning()
        {
            foreach (var task in Tasks)
            {
                if (task.State == StateType.Active || task.State == StateType.Pause)
                {
                    return true;
                }
            }

            return false;
        }

        private List<TaskEntity> GetCheckedTasks()
        {
            try
            {
                List<TaskEntity> listTasks = taskListView.Items.OfType<TaskEntity>().Where(item => (bool)item.IsChecked).ToList();
                foreach (var task in listTasks)
                {
                    task.IsChecked = false;
                }
                return listTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<TaskEntity>();
            }
        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtenez la tâche à partir du paramètre
                TaskEntity selectedTask = (TaskEntity)((Button)sender).CommandParameter;

                // Utilisez la tâche comme vous le souhaitez
                HomeViewModel.IsManualPause[(int)selectedTask.Id] = true;
                HomeViewModel.PauseTask(selectedTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Resume_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskEntity selectedTask = (TaskEntity)((Button)sender).CommandParameter;
                HomeViewModel.IsManualPause[(int)selectedTask.Id] = false;
                HomeViewModel.ResumeTask(selectedTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskEntity selectedTask = (TaskEntity)((Button)sender).CommandParameter;
                HomeViewModel.StopTask(selectedTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var tasks = GetCheckedTasks();

            if (tasks.Count == 0 || tasks == null)
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorSelectATask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (var task in tasks)
            {
                if (task.State != StateType.Inactive)
                {
                    MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorDeleteTask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            bool result = HomeViewModel.DeleteSelectedTasks(tasks);
            if (result)
            {
                Tasks = new List<TaskEntity>(HomeViewModel.GetAllTasks(null));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_SuccessDelete, EasySaveGraphic.Lang.Resources.Success, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorDelete,EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK ,MessageBoxImage.Error);
            }

        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tasks = GetCheckedTasks();

                if (tasks.Count == 0 || tasks == null)
                {
                    MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorSelectATask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                HomeViewModel.StartSelectedTasks(tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Modify_Click(object sender, RoutedEventArgs e)
        {
            if (IsTaskRunning())
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorModifyTaskRunning, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var checkTasks = GetCheckedTasks();

            if (checkTasks.Count == 0 || checkTasks == null)
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorSelectATask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (checkTasks.Count != 1)
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorOnlyOneTasks, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (checkTasks.FirstOrDefault() == null)
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorSelectATask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (checkTasks.FirstOrDefault().State != StateType.Inactive)
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorModifyTask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BindComboBox();
            BindModification(checkTasks.FirstOrDefault());

            TitleModifyDialogHost.Content = EasySaveGraphic.Lang.Resources.Edit;
            IsModification = true;
            ModifyDialogHost.IsOpen = true;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            if (IsTaskRunning())
            {
                MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorCreateTaskRunning, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BindComboBox();
            BindModification(null);
            TitleModifyDialogHost.Content = EasySaveGraphic.Lang.Resources.Creation;
            IsModification = false;
            ModifyDialogHost.IsOpen = true;
        }

        private void Button_Close_DialogHost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogHost.CloseDialogCommand.Execute(null, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Ok_DialogHostSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var id1Bool = int.TryParse(TextBoxID1.Text, out int id1);
                var id2Bool = int.TryParse(TextBoxID2.Text, out int id2);

                if (!id1Bool || !id2Bool)
                {
                    foreach (var task in Tasks)
                    {
                        task.IsChecked = false;
                    }

                    taskListView.ItemsSource = null;
                    taskListView.ItemsSource = Tasks;
                }

                if (id1 >= 1 && id2 <= Tasks.Count())
                {
                    for (int currentId = id1; currentId <= id2; currentId++)
                    {
                        Tasks[currentId - 1].IsChecked = true;
                    }

                    taskListView.ItemsSource = null;
                    taskListView.ItemsSource = Tasks;
                }

                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Section_Click(object sender, RoutedEventArgs e)
        {
            TextBoxID1.Text = null;
            TextBoxID2.Text = null;

            SelectionDialogHost.IsOpen = true;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Tasks = new List<TaskEntity>(HomeViewModel.GetAllTasks(SearchTextBox.Text));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void BindModification(TaskEntity? task)
        {
            try
            {
                if (task == null)
                {
                    NameTextBox.Text = "";
                    SourceTextBox.Text = "";
                    DestTextBox.Text = "";
                    OldNameTextBox.Text = "";
                    TypeComboBox.SelectedItem = BackupType.Complete;
                }
                else
                {
                    NameTextBox.Text = task.Name;
                    SourceTextBox.Text = task.SourcePath;
                    DestTextBox.Text = task.DestPath;
                    OldNameTextBox.Text = task.Name;
                    TypeComboBox.SelectedItem = task.Type;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void BindComboBox()
        {
            try
            {
                TypeComboBox.SelectedItem = BackupType.Complete;
                TypeComboBox.ItemsSource = Enum.GetValues(typeof(BackupType));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Source_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    SourceTextBox.Text = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Dest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    DestTextBox.Text = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsModification)
                {
                    HomeViewModel.UpdateTask(OldNameTextBox.Text, IsModification, NameTextBox.Text, SourceTextBox.Text, DestTextBox.Text, (BackupType?)Enum.Parse(typeof(BackupType), TypeComboBox.Text));
                    Button_Close_DialogHost_Click(sender, e);
                }
                else
                {
                    HomeViewModel.UpdateTask(NameTextBox.Text, IsModification, null, SourceTextBox.Text, DestTextBox.Text, (BackupType?)Enum.Parse(typeof(BackupType), TypeComboBox.Text));
                    Button_Close_DialogHost_Click(sender, e);
                }

                Tasks = HomeViewModel.GetAllTasks(null);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}