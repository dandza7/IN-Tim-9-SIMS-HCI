using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ObservableCollection<Request> DeclinedRequests;
        public int userId = -1;

        public DoctorRequestsPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            userId = int.Parse(app.Properties["userId"].ToString());
            _requestController = app.RequestController;
            DeclinedRequests=Requests = new ObservableCollection<Request>();
            Requests = (ObservableCollection<Request>)_requestController.GetAllByDoctorId(userId);
            foreach (Request r in Requests) if (r.Status == Request.RequestStatusType.Declined) DeclinedRequests.Add(r);
            RequestViewGrid.ItemsSource = Requests;
            DeclinedRequestsGrid.ItemsSource = DeclinedRequests;
            this.DataContext = this;
        }

        private void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDateTime(BeginningDTP.Text).Date <= DateTime.Today.Add(new TimeSpan(TimeSpan.TicksPerDay * 2))
            ||_requestController.Create(
                new Request(
                    Convert.ToDateTime(BeginningDTP.Text),
                    Convert.ToDateTime(EndingDTP.Text),
                    Request.RequestStatusType.Pending,
                    userId,
                    TitleTB.Text,
                    ContentTB.Text,
                    (bool)UrgentCBX.IsChecked,
                    ""
                    )) == null
                    ) exceptionLabel.Visibility = Visibility.Visible;
            else exceptionLabel.Visibility = Visibility.Hidden;

            Debug.WriteLine(UrgentCBX.IsChecked);

            BeginningDTP.Text = "";
            EndingDTP.Text = "";
            TitleTB.Clear();
            ContentTB.Clear();


        }

        private void DeclinedRequestsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommentTB.Text = ((Request)DeclinedRequestsGrid.SelectedItems[0]).Comment;

        }
    }
}
