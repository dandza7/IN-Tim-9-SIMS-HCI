using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
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
using System.Printing;

namespace WpfApp1.View.Model.Executive.ExecutiveRoomDialogs
{
    /// <summary>
    /// Interaction logic for ExportBusynessPDF.xaml
    /// </summary>
    public partial class ExportBusynessPDF : Page, INotifyPropertyChanged
    {
        #region NotifyProperties
        private string _bDate;
        public string BDate
        {
            get
            {
                return _bDate;
            }
            set
            {
                if (value != _bDate)
                {
                    _bDate = value;
                    OnPropertyChanged("BDate");
                    IsBeginningSelected++;
                    FilterList();
                }
            }
        }

        private string _eDate;
        public string EDate
        {
            get
            {
                return _eDate;
            }
            set
            {
                if (value != _eDate)
                {
                    _eDate = value;
                    OnPropertyChanged("EDate");
                    IsEndingSelected++;
                    FilterList();
                }
            }
        }
        private List<BusynessPreview> _listSource;
        public List<BusynessPreview> ListSource
        {
            get
            {
                return _listSource;
            }
            set
            {
                if (value != _listSource)
                {
                    _listSource = value;
                    OnPropertyChanged("ListSource");
                }
            }
        }
        private ObservableCollection<BusynessPreview> _busynessPreviews;
        public ObservableCollection<BusynessPreview> BusynessPreviews
        {
            get
            {
                return _busynessPreviews;
            }
            set
            {
                if (value != _busynessPreviews)
                {
                    _busynessPreviews = value;
                    OnPropertyChanged("BusynessPreviews");
                }
            }
        }
        #endregion
        #region PropertyChangedNotifier
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        public ExecutiveRoomPages ParentPage { get; set; }
        private RenovationController _renovationController;
        private int IsBeginningSelected = 0;
        private int IsEndingSelected = 0;

        public ExportBusynessPDF(ExecutiveRoomPages parent)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ParentPage = parent;
            var app = Application.Current as App;
            this._renovationController = app.RenovationController;
            this.ListSource = _renovationController.GetBusynessPreview();
            BusynessPreviews = new ObservableCollection<BusynessPreview>();
            Beginning.SelectedDate = DateTime.Today.AddDays(-10);
            Ending.SelectedDate = DateTime.Today.AddDays(10);

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
        }
        private void FilterList()
        {
            BusynessPreviews.Clear();
            foreach (BusynessPreview bp in ListSource)
            {
                bool checker = true;
                if (IsBeginningSelected > 1)
                {
                    //Console.WriteLine("{0} : {1} < {2}", DateTime.Parse(BDate), bp.Beginning, DateTime.Parse(BDate) < bp.Beginning);
                    if (DateTime.Parse(BDate) > bp.Beginning)
                        checker = false;
                }
                if (IsEndingSelected > 1)
                {
                    //Console.WriteLine("{0} : {1} > {2}", DateTime.Parse(EDate), bp.Ending, DateTime.Parse(EDate) > bp.Ending);
                    if (DateTime.Parse(EDate) < bp.Ending)
                        checker = false;
                }
                if (checker)
                    BusynessPreviews.Add(bp);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintVisual(toPrint, "Rooms Busyness Report");
            ParentPage.CloseFrame.Begin();
        }
    }  
}
