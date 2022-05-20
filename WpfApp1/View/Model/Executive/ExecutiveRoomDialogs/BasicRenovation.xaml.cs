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
using WpfApp1.Model;

namespace WpfApp1.View.Model.Executive.ExecutiveRoomDialogs
{
    /// <summary>
    /// Interaction logic for BasicRenovation.xaml
    /// </summary>
    
    public partial class BasicRenovation : Page, INotifyPropertyChanged
    {
        //--------------------------------------------------------------------------------------------------------
        //          INotifyPropertyChanged fields:
        //--------------------------------------------------------------------------------------------------------
        #region NotifyProperties
        public String _feedback;
        public string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                if (value != _feedback)
                {
                    _feedback = value;
                    OnPropertyChanged("Feedback");
                }
            }
        }
        public String _selectionProblem;
        public string SelectionProblem
        {
            get
            {
                return _selectionProblem;
            }
            set
            {
                if (value != _selectionProblem)
                {
                    _selectionProblem = value;
                    OnPropertyChanged("SelectionProblem");
                }
            }
        }
        public String _selectedBeginning;
        public string SelectedBeginning
        {
            get
            {
                return _selectedBeginning;
            }
            set
            {
                if (value != _selectedBeginning)
                {
                    _selectedBeginning = value;
                    OnPropertyChanged("SelectedBeginning");
                    FindPotentialEndings(SelectedBeginning);
                }
            }
        }
        private List<string> _endings;
        public List<string> Endings
        {
            get
            {
                return _endings;
            }
            set
            {
                if (value != _endings)
                {
                    _endings = value;
                    OnPropertyChanged("Endings");
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
    
        public ExecutiveRoomPages ParentPage;
        public List<string> Beginnings { get; set; }

        public BasicRenovation(ExecutiveRoomPages parent)
        {
            InitializeComponent();
            ParentPage = parent;
            this.DataContext = this;
            this.Beginnings = ParentPage.Beginnings;
            Type.Text = ParentPage.SelectedType;
            Nametag.Text = ParentPage.SelectedNametag;
        }
        public void ResetFields()
        {
            Nametag.Text = "";
            Type.Text = "";
            Beginning.SelectedValue = "";
            Ending.SelectedValue = "";
            Description.Text = "";
        }

        private void FindPotentialEndings(string beginning)
        {
            if (beginning.Equals(""))
            {
                return;
            }
            this.Endings = ParentPage.RenovationController.GetDaysAvailableForRenovation(new List<int>() { ParentPage.SelectedId }, beginning);
            Ending.IsEnabled = true;

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
            ResetFields();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Beginning.Text.Equals("") || Ending.Text.Equals("") || Description.Text.Equals(""))
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (Description.Text.Contains(";"))
            {
                Feedback = "*you can't put semicolon (;) in description!";
                return;
            }
            
            ParentPage.RenovationController.Create(new Renovation(0, new List<int>() { ParentPage.SelectedId }, Description.Text, DateTime.Parse(Beginning.Text), DateTime.Parse(Ending.Text), "B"));
            ParentPage.CloseFrame.Begin();
            ResetFields();
        }

    }
}
