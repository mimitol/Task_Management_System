using BlApi;
using BO;
using DO;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        /// Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));



        public ObservableCollection<BO.EngineerInTask> Engineers
        {
            get { return (ObservableCollection<BO.EngineerInTask>)GetValue(EngineersProperty); }
            set { SetValue(EngineersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineersProperty =
            DependencyProperty.Register("Engineers", typeof(ObservableCollection<BO.EngineerInTask>), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.EngineerInTask? SelectedEngineer
        {
            get { return (BO.EngineerInTask)GetValue(SelectedEngineerProperty); }
            set { SetValue(SelectedEngineerProperty, value);}
        }

        // Using a DependencyProperty as the backing store for SelectedEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedEngineerProperty =
            DependencyProperty.Register("SelectedEngineer", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata());




        public bool IsAdd { get; set; }
        public bool IsScheduled { get; set; }
        public TaskWindow(int id=-1)
        {
            if (id == -1)
            {
                Task = new BO.Task();
                IsAdd = true;
                Engineers = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask());
                SelectedEngineer = null;
            }
            else
                try
                {
                    IsAdd = false;
                    Task = s_bl.Task.Read(id);
                    Engineers = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask(E=>E.Level >= Task.ComplexityLevel));
                    SelectedEngineer = Task.Engineer;
                }
                catch (BO.BlDoesNotExistException)
                {
                    MessageBox.Show("Task doesn't exist");
                    this.Close();
                }
            InitializeComponent();
        }

        private void BtnClickUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Engineer = SelectedEngineer;
                s_bl.Task.Update(Task);
                MessageBox.Show("Task update sucssesfully");
                Close();
                TasksListWindow tasksListWindow = new TasksListWindow();
                tasksListWindow.Show();


            }
            catch (BlDoesNotExistException)
            {
                MessageBox.Show($"Task with ID={Task.Id} does Not exist");
            }
        }
        private void BtnClickAddTask(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Create(Task);
                MessageBox.Show("Task created sucssesfully");
                Close();
                TasksListWindow tasksListWindow = new TasksListWindow();
                tasksListWindow.Show();

            }
            
            catch (BlInvalidPropertyException)
            {
                MessageBox.Show("Invalid input!");
                Close();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Engineers = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask(E => E.Level >= Task.ComplexityLevel));
        }
    }
}
