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

namespace WpfApp1.View.Model.Secretary
{
    /// <summary>
    /// Interaction logic for MeetingView.xaml
    /// </summary>
    public partial class MeetingView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string _name;
        private DateTime _beginning;
        public MeetingView()
        {
            InitializeComponent();
            DataContext = this;
        }


        public DateTime Beginning
        {
            get => _beginning;
            set
            {
                if (_beginning != value)
                {
                    _beginning = value;
                    OnPropertyChanged("Beginning");
                }
            }
        }
        public string Nametag
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Nametag");
                }
            }
        }

    }
}