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
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TasksListWindow.xaml
    /// </summary>
    public partial class TasksListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.Task> TasksList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register("TasksList", typeof(IEnumerable<BO.Task>), typeof(TasksListWindow), new PropertyMetadata(null));

  

        

        public TasksListWindow()
        {
            InitializeComponent();
            TasksList = s_bl.Task.ReadAll();
        }
    }
}
