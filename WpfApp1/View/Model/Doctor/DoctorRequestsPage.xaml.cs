﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorRequestsPage.xaml
    /// </summary>
    public partial class DoctorRequestsPage : Page
    {
        RequestController _requestController;
        public ObservableCollection<Request> Requests;

    public DoctorRequestsPage()
    {
            InitializeComponent();
            var app = Application.Current as App;
            _requestController = app.RequestController;
            Requests = new ObservableCollection<Request>();
            Requests = (ObservableCollection<Request>)_requestController.GetAllByDoctorId(1);//DODATI ID ULOGOVANOG
            RequestViewGrid.ItemsSource = Requests;
            this.DataContext = this;
        }

        private void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            if (_requestController.Create(
                new Request(
                    Convert.ToDateTime(BeginningDTP.Text),
                    Convert.ToDateTime(EndingDTP.Text),
                    Request.RequestStatusType.Pending,
                    1,//DODATI ID ULOGOVANOG
                    TitleTB.Text,
                    ContentTB.Text
                    )) == null) exceptionLabel.Visibility = Visibility.Visible;
            else exceptionLabel.Visibility = Visibility.Hidden;

            BeginningDTP.Text="";
            EndingDTP.Text = "";
            TitleTB.Clear();
            ContentTB.Clear();


        }
    }
}