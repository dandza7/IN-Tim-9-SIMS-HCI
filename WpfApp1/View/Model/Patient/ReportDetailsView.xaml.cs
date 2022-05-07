using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for ReportDetailsView.xaml
    /// </summary>
    public partial class ReportDetailsView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private DateTime _beginning;
        private DateTime _ending;
        private string _username;
        private string _nametag;
        private string _reportContent;
        public ReportDetailsView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime Beginning
        {
            get
            {
                return _beginning;
            }
            set
            {
                if (value != _beginning)
                {
                    _beginning = value;
                    OnPropertyChanged("Beginning");
                }
            }
        }

        public DateTime Ending
        {
            get
            {
                return _ending;
            }
            set
            {
                if (value != _ending)
                {
                    _ending = value;
                    OnPropertyChanged("Ending");
                }
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public string Nametag
        {
            get
            {
                return _nametag;
            }
            set
            {
                if (value != _nametag)
                {
                    _nametag = value;
                    OnPropertyChanged("Nametag");
                }
            }
        }

        public string ReportContent
        {
            get
            {
                return _reportContent;
            }
            set
            {
                if (value != _reportContent)
                {
                    _reportContent = value;
                    OnPropertyChanged("ReportContent");
                }
            }
        }
    }
}
