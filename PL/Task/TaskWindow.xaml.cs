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
            set { SetValue(SelectedEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedEngineerProperty =
            DependencyProperty.Register("SelectedEngineer", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata());



        public bool IsAdd { get; set; }
        public bool IsScheduled { get; set; }
        public TaskWindow(int id = -1)
        {
            IsAdd = id == -1;
            IsScheduled = s_bl.Task.IsScheduled();
            InitializeComponent();
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
                    Engineers = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask(E => E.Level >= Task.ComplexityLevel));
                    SelectedEngineer = Engineers.First(e => e.Id == Task.Engineer.Id);
                }
                catch (BO.BlDoesNotExistException)
                {
                    MessageBox.Show("Task doesn't exist");
                    this.Close();
                }

            if (Task.Dependencies == null)
            {
                DependenciesToAdd = new(s_bl.Task.ReadAllTaskInList());
                DependenciesToRemove = null;
            }
            else
            {
                DependenciesToAdd = new(s_bl.Task.ReadAllTaskInList(t => !Task.Dependencies!.Select(d => d.Id).Contains(t.Id)));
                DependenciesToRemove = new(Task.Dependencies);
            }
        }

        private void BtnClickUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Engineer = SelectedEngineer;
                Task.Dependencies = DependenciesToRemove;
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
                Task.Engineer = SelectedEngineer;
                Task.Dependencies = DependenciesToRemove;
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



        public ObservableCollection<TaskInList?> DependenciesToAdd
        {
            get { return (ObservableCollection<TaskInList>)GetValue(DependenciesToAddProperty); }
            set { SetValue(DependenciesToAddProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesToAdd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesToAddProperty =
            DependencyProperty.Register("DependenciesToAdd", typeof(ObservableCollection<TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));



        public TaskInList SelectedDependencyToAdd
        {
            get { return (TaskInList)GetValue(SelectedDependencyToAddProperty); }
            set { SetValue(SelectedDependencyToAddProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDependencyToAdd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDependencyToAddProperty =
            DependencyProperty.Register("SelectedDependencyToAdd", typeof(TaskInList), typeof(TaskWindow), new PropertyMetadata(null));



        public ObservableCollection<TaskInList> DependenciesToRemove
        {
            get { return (ObservableCollection<TaskInList>)GetValue(DependenciesToRemoveProperty); }
            set { SetValue(DependenciesToRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesToRemoveProperty =
            DependencyProperty.Register("DependenciesToRemove", typeof(ObservableCollection<TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));



        public TaskInList SelectedDependencyToRemove
        {
            get { return (TaskInList)GetValue(SelectedDependencyToRemoveProperty); }
            set { SetValue(SelectedDependencyToRemoveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDependencyToRemove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDependencyToRemoveProperty =
            DependencyProperty.Register("SelectedDependencyToRemove", typeof(TaskInList), typeof(TaskWindow), new PropertyMetadata(null));


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Engineers = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask(E => E.Level >= Task.ComplexityLevel));
        }

        private void BtnClickAddDependency(object sender, RoutedEventArgs e)
        {
            if (DependenciesToRemove == null)
                DependenciesToRemove = new () { SelectedDependencyToAdd };
            else
                DependenciesToRemove.Add(SelectedDependencyToAdd);
            DependenciesToAdd.Remove(SelectedDependencyToAdd);
        }

        private void BtnClickRemoveDependency(object sender, RoutedEventArgs e)
        {
            DependenciesToRemove.Remove(DependenciesToRemove.First(d => d.Id == SelectedDependencyToRemove.Id));
            DependenciesToAdd.Add(SelectedDependencyToRemove);
        }
    }
}
