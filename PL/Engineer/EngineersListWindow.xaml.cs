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
    /// Interaction logic for EngineersListWindow.xaml
    /// </summary>
    public partial class EngineersListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ObservableCollection<BO.EngineerInTask> EngineersList
        {
            get { return (ObservableCollection<BO.EngineerInTask>)GetValue(EngineersListProperty); }
            set { SetValue(EngineersListProperty, value); }
        }

        public static readonly DependencyProperty EngineersListProperty =
            DependencyProperty.Register("EngineersList", typeof(ObservableCollection<BO.EngineerInTask>), typeof(EngineersListWindow), new PropertyMetadata(null));



        public EngineerExperience? SelectedLevel
        {
            get { return (EngineerExperience?)GetValue(SelectedLevelProperty); }
            set { SetValue(SelectedLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLevelProperty =
            DependencyProperty.Register("SelectedLevel", typeof(EngineerExperience?), typeof(EngineersListWindow), new PropertyMetadata(null));





        public BO.EngineerInTask SelectedEngineerInList { get; set; }
        public EngineersListWindow()
        {
            InitializeComponent();
            EngineersList = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask());
            SelectedLevel = null;

        }

       
        private void ByLevel(object sender, SelectionChangedEventArgs e)
        {
            EngineersList = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask(e => e.Level == SelectedLevel));
        }
        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            SelectedLevel = null;
            EngineersList = new ObservableCollection<BO.EngineerInTask>(s_bl.Engineer.ReadAllEngineerInTask());
        }

        private void SelectedEngineerToUpdate(object sender, SelectionChangedEventArgs e)
        {
            EngineerWindow engineerWindow= new EngineerWindow(SelectedEngineerInList.Id);
            Close();
            engineerWindow.Show();
        }

        private void BtnAddEngineerClick(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }
    }
}
