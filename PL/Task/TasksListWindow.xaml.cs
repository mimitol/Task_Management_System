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



        public ObservableCollection<BO.Task> TasksList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TasksList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register("TasksList", typeof(ObservableCollection<BO.Task>), typeof(TasksListWindow), new PropertyMetadata(null));





        public TasksListWindow()
        {
            InitializeComponent();
            TasksList = new ObservableCollection<BO.Task>(s_bl.Task.ReadAll());
        }
    }
}
