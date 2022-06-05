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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model.Preview;
using WpfApp1.ViewModel;

namespace WpfApp1.View.Model.Executive.ExecutiveStatisticsDialogs
{
    /// <summary>
    /// Interaction logic for DoctorStatistics.xaml
    /// </summary>
    public partial class DoctorStatistics : Page
    {
        public DoctorStatistics(DoctorPreview doc)
        {
            InitializeComponent();
            this.DataContext = new DoctorStatisticsViewModel(doc);
        }
    }
}
