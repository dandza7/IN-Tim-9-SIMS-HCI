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

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveMainPage.xaml
    /// </summary>
    public partial class ExecutiveMainPage : Page
    {
        public ExecutiveMainPage()
        {
            InitializeComponent();
            this.DataContext = this;
            ExecutivePagesFrame.Content = new ExecutiveRoomPages();
        }

        private void DrugsButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveDrugsPages();
            Storyboard sb1 = FindResource("myStoryboard") as Storyboard;
            sb1.Begin();
        }

        private void RoomsButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveRoomPages();
            Storyboard sb1 = FindResource("myStoryboard") as Storyboard;
            sb1.Begin();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveInventoryPages();
            Storyboard sb1 = FindResource("myStoryboard") as Storyboard;
            sb1.Begin();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveStatisticsPages();
            Storyboard sb1 = FindResource("myStoryboard") as Storyboard;
            sb1.Begin();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
