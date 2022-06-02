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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.ViewModel;


namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveStatisticsPage.xaml
    /// </summary>
    public partial class ExecutiveStatisticsPages : Page
    {
        public Storyboard sb1 { get; set; }
        public ExecutiveStatisticsPages()
        {
            InitializeComponent();
            this.sb1 = FindResource("myStoryboard") as Storyboard;
            this.DataContext = new StatisticsViewModel(this);


        }
    }
}
