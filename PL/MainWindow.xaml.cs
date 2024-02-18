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
using System.Windows.Navigation;
using PL.Task;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTasksClick(object sender, RoutedEventArgs e)
        {
            new TasksListWindow().Show();
        }

        private void BtnInitClick(object sender, RoutedEventArgs e)
        {
            //TODO
            //להוסיף הודעה האם הוא בטוח שהוא רוצה איך לעשות את זה בלי לגשת ישירות לMASSEGEBOX
            if (MessageBox.Show("האם אתה בטוח שברצונך לאתחל?", "הודעה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                s_bl.InitializeDB();
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
