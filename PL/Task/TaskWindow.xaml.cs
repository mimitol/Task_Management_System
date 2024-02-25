using BlApi;
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

        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public bool IsAdd { get; set; }

        public TaskWindow(int id=-1)
        {
            if (id == -1)
            {
                Task = new BO.Task();
                IsAdd = true;
            }
            else
                try
                {
                    IsAdd = false;
                    Task = s_bl.Task.Read(id);
                }
                catch (BO.BlDoesNotExistException)
                {
                    MessageBox.Show("Task doesn't exist");
                    this.Close();
                }
            InitializeComponent();
        }
    }
}
