using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfApp1.View.Model
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public PatientView()
        {
            InitializeComponent();
            DataContext = this;
        }
        private int _id;
        private string _firstName;
        private string _surname;
        private string _jmbg;
        private string _username;
        private string _password;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }

        public string Jmbg
        {
            get => _jmbg;
            set
            {
                if (_jmbg != value)
                {
                    _jmbg = value;
                    OnPropertyChanged("Jmbg");
                }
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

    }
}
