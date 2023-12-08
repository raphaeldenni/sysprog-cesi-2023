using EasySave.Models;
using EasySave.Types;
using EasySaveGraphic.ViewModels;
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
            InitializeComponent();
            StartClock();

            HomeViewModel = new HomeViewModel();
            Tasks = HomeViewModel.GetAllTasks();
            Lang = HomeViewModel.ConfigModel.Config.Language;

            DataContext = this;
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
            List<TaskEntity> listTasks = taskListView.Items.OfType<TaskEntity>().Where(item => (bool)item.IsChecked).ToList();

            return listTasks;
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            bool result = HomeViewModel.DeleteSelectedTasks(GetCheckedTasks());
            if (result)
            {
                MessageBox.Show("Tasks deleted");
                UpdateTasksList();
            } else
            {
                MessageBox.Show("Neuille");
            }
            
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Modify_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Vérifier si le NavigationService existe (il pourrait être null dans certains cas)
            if (navigationService != null)
            {
                // Naviguer vers la page ModifyView.xaml
                navigationService.Navigate(new Uri("Views/ModifyView.xaml", UriKind.Relative));
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Vérifier si le NavigationService existe (il pourrait être null dans certains cas)
            if (navigationService != null)
            {
                // Naviguer vers la page ModifyView.xaml
                navigationService.Navigate(new Uri("Views/ModifyView.xaml", UriKind.Relative));
            }
        }

        private void UpdateTasksList()
        {
            Tasks = HomeViewModel.GetAllTasks();
            taskListView.ItemsSource = Tasks;
        }
    }
}