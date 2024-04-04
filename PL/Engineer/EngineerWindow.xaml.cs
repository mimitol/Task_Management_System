using BO;
using PL.Task;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    ///
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public bool IsAdd { get; set; }

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        /// Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public EngineerWindow(int id = -1)
        {
            IsAdd = id == -1;
            InitializeComponent();
            if (id == -1)
            {
                Engineer = new BO.Engineer();
                IsAdd = true;
            }
            else
                try
                {
                    IsAdd = false;
                    Engineer = s_bl.Engineer.Read(id);
                }
                catch (BO.BlDoesNotExistException)
                {
                    MessageBox.Show("Engineer doesn't exist");
                    this.Close();
                }
        }

        private void AddEngineerBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Engineer.Create(Engineer);
                MessageBox.Show("Engineer created sucssesfully");
                Close();
                EngineersListWindow engineersListWindow = new EngineersListWindow();
                engineersListWindow.Show();

            }

            catch (BlInvalidPropertyException)
            {
                MessageBox.Show("Invalid input!");
                Close();
            }
        }

        private void UpdateEngineerBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Engineer.Update(Engineer);
                MessageBox.Show("Engineer update sucssesfully");
                Close();
                EngineersListWindow engineersListWindow = new EngineersListWindow();
                engineersListWindow.Show();


            }
            catch (BlDoesNotExistException)
            {
                MessageBox.Show($"Task with ID={Engineer.Id} does Not exist");
            }
        }
    }
}
