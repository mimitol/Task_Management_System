using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TasksListWindow.xaml
    /// </summary>
    public partial class TasksListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //private ObservableCollection<BO.Task> tasksList;
        //public ObservableCollection<BO.Task> TasksList
        //{
        //    get { return tasksList; }
        //    set { tasksList= value; }
        //}



        public ObservableCollection<BO.TaskInList> TasksList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TasksList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register("TasksList", typeof(ObservableCollection<BO.TaskInList>), typeof(TasksListWindow), new PropertyMetadata(null));




        public Status? SelectedStatus
        {
            get { return (Status?)GetValue(SelectedStatusProperty); }
            set { SetValue(SelectedStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStatusProperty =
            DependencyProperty.Register("SelectedStatus", typeof(Status?), typeof(TasksListWindow), new PropertyMetadata(null));

        public TaskInList SelectedTaskInList { get; set; }


        public bool IsScheduled { get; set; }

        public TasksListWindow()
        {
            IsScheduled = s_bl.Task.IsScheduled();
            InitializeComponent();
            TasksList = new ObservableCollection<BO.TaskInList>(s_bl.Task.ReadAllTaskInList());
            SelectedStatus = null;
        }

        private void ByStatus(object sender, SelectionChangedEventArgs e)
        {
            TasksList = new ObservableCollection<BO.TaskInList>(s_bl.Task.ReadAllTaskInList(t=>t.Status==SelectedStatus));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedStatus = null;
            TasksList = new ObservableCollection<BO.TaskInList>(s_bl.Task.ReadAllTaskInList());

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskWindow taskWindow = new TaskWindow(SelectedTaskInList.Id);
            Close();
            taskWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new TaskWindow();
            Close();
            taskWindow.Show();
        }
    }
}
