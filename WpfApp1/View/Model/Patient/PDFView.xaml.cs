using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.ViewModel;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PDFView.xaml
    /// </summary>
    public partial class PDFView : Window
    {
        public PDFView()
        {
            InitializeComponent();
            DataContext = new PDFViewModel();
        }

        public void PrintDocument(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintVisual(printGrid, "Patient Appointment Reports");
        }
    }
}
