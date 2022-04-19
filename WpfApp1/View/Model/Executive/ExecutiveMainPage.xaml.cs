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
        }

        private void DrugsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RoomsButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveRoomPages();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            ExecutivePagesFrame.Content = new ExecutiveInventoryPages();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
