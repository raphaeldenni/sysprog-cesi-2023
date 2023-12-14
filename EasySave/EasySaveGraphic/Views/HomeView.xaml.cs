using EasySave.Models;
using EasySave.Types;
using EasySaveGraphic.ViewModels;
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

namespace EasySaveGraphic.Views
{
    public partial class HomeView : Page
    {
        internal HomeViewModel HomeViewModel { get; set; }
        public List<TaskEntity> Tasks { get; set; }
        public LangType Lang { get; set; }

        public HomeView()
        {
            StartClock();

            HomeViewModel = new HomeViewModel();
            Tasks = HomeViewModel.GetAllTasks(null);
            Lang = HomeViewModel.ConfigModel.Config.Language;
            HomeViewModel.NotifyTaskUpdated += (updatedTask, taskIndex) =>
            {
                UpdateTasksListWhenStart(updatedTask, taskIndex);
            };

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

        private List<TaskEntity> GetCheckedTasks()
        {
            try
            {
                List<TaskEntity> listTasks = taskListView.Items.OfType<TaskEntity>().Where(item => (bool)item.IsChecked).ToList();

                return listTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<TaskEntity>();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            bool result = HomeViewModel.DeleteSelectedTasks(GetCheckedTasks());
            if (result)
            {
                UpdateTasksList(null);
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
                HomeViewModel.StartSelectedTasks(GetCheckedTasks());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Modify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);

                if (GetCheckedTasks().Count != 1)
                {
                    MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorOnlyOneTasks, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (GetCheckedTasks().FirstOrDefault() == null)
                {
                    MessageBox.Show(EasySaveGraphic.Lang.Resources.Message_ErrorSelectATask, EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (navigationService != null)
                {
                    ModifyView modifyView = new ModifyView(GetCheckedTasks().FirstOrDefault());
                    navigationService.Navigate(modifyView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);

                if (navigationService != null)
                {
                    ModifyView modifyView = new ModifyView(null);
                    navigationService.Navigate(modifyView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTasksList(SearchTextBox.Text);
        }

        private void UpdateTasksList(string? search)
        {
            try
            {
                Tasks = HomeViewModel.GetAllTasks(search);
                taskListView.ItemsSource = Tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTasksListWhenStart(TaskEntity task, int taskIndex)
        {
            try
            {
                Tasks[taskIndex] = HomeViewModel.GetAllTasks(task.Name).FirstOrDefault() ?? throw new Exception();
                taskListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{EasySaveGraphic.Lang.Resources.Message_ErrorGeneral} {ex.Message}", EasySaveGraphic.Lang.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_DialogHostSelection_Click(object sender, RoutedEventArgs e)
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
    }
}