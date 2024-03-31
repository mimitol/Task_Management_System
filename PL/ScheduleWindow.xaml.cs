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

namespace PL
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            MinStartDate = DateTime.Now;
            InitializeComponent();
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime MinStartDate { get; set; }

        private void BtnClickSchedule(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Milestone.SetStartDate(StartDate);
                s_bl.Milestone.SetEndDate(EndDate);
                s_bl.Milestone.CreateProjectSchedule();
                MessageBox.Show("Schedule Succeded");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schedule failed. Check if you have some circles in the dependencies");
            }
        }
    }
}
