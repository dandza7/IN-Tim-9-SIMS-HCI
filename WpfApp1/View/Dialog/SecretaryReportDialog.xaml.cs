using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfApp1.Controller;
using WpfApp1.View.Model.Secretary;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using WpfApp1.ViewModel.Secretary;
using Syncfusion.UI.Xaml.Grid.Converter;
using System.Windows.Forms;
using System.IO;
using Syncfusion.Windows.PdfViewer;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryReportDialog.xaml
    /// </summary>
    public partial class SecretaryReportDialog : Window
    { 

 
        public SecretaryReportDialog()
        {
            InitializeComponent();
            this.DataContext = new AppointmentReportViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var app = System.Windows.Application.Current as App;
            app.Properties["beginning"] = bgn.Text;
            app.Properties["beginning"] = end.Text;
            PdfExportingOptions options = new PdfExportingOptions();
            options.PageHeaderFooterEventHandler = PdfHeaderFooterEventHandler;
            var document = dataGrid.ExportToPdf(options);
            document.Save("Sample.pdf");
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            PdfViewerControl pdfViewer = new PdfViewerControl();
            pdfViewer.Load(stream);
            Window window = new Window();
            window.Content = pdfViewer;
            window.Show();
        }
            static void PdfHeaderFooterEventHandler(object sender, PdfHeaderFooterEventArgs e)
            {
            var app = System.Windows.Application.Current as App;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Courier, 12, PdfFontStyle.Bold);
                var width = e.PdfPage.GetClientSize().Width;
                PdfPageTemplateElement header = new PdfPageTemplateElement(width, 38);
                string beginning = (string)app.Properties["beginning"];
            string ending = (string)app.Properties["ending"];
            header.Graphics.DrawString("Pregled zakazanih pregleda u periodu od : " + beginning + "do " + ending + ".", font, PdfPens.Black, 25, 3);

            e.PdfDocumentTemplate.Top = header;
            }


        }
    }
