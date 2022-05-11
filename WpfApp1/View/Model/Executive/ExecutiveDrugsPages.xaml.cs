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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Executive.ExecutiveDrugsDialogs;

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveDrugsPages.xaml
    /// </summary>
    public partial class ExecutiveDrugsPages : Page, INotifyPropertyChanged
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

        public String _wrongSelection;
        public string WrongSelection
        {
            get
            {
                return _wrongSelection;
            }
            set
            {
                if (value != _wrongSelection)
                {
                    _wrongSelection = value;
                    OnPropertyChanged("WrongSelection");
                }
            }
        }
        private List<Drug> _validatedDrugs;
        public List<Drug> ValidatedDrugs
        {
            get
            {
                return _validatedDrugs;
            }
            set
            {
                if (value != _validatedDrugs)
                {
                    _validatedDrugs = value;
                    OnPropertyChanged("ValidatedDrugs");
                }
            }
        }
        private List<Drug> _unvalidatedDrugs;
        public List<Drug> UnvalidatedDrugs
        {
            get
            {
                return _unvalidatedDrugs;
            }
            set
            {
                if (value != _unvalidatedDrugs)
                {
                    _unvalidatedDrugs = value;
                    OnPropertyChanged("UnvalidatedDrugs");
                }
            }
        }
        private List<Drug> _rejectedDrugs;
        public List<Drug> RejectedDrugs
        {
            get
            {
                return _rejectedDrugs;
            }
            set
            {
                if (value != _rejectedDrugs)
                {
                    _rejectedDrugs = value;
                    OnPropertyChanged("RejectedDrugs");
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

        private DrugController _drugController;
        public DrugController DrugController
        {
            get { return _drugController; }
        }
        public Storyboard CloseFrame { get; set; }
        public Storyboard OpenFrame { get; set; }
        public Storyboard MR { get; set; }
        public Storyboard MM { get; set; }
        public Storyboard ML { get; set; }
        public int TypeIndicator;
        public Drug SelectedDrug { get; set; }
        public ExecutiveDrugsPages()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _drugController = app.DrugController;
            ValidatedDrugs = new List<Drug>();
            UnvalidatedDrugs = new List<Drug>();
            RejectedDrugs = new List<Drug>();
            TypeIndicator = 0;
            GetDrugs();
            SetAnimations();

        }
        public void GetDrugs()
        {
            List<Drug> drugs = this.DrugController.GetAll().ToList();
            ValidatedDrugs.Clear();
            UnvalidatedDrugs.Clear();
            RejectedDrugs.Clear();
            foreach (Drug drug in drugs)
            {
                if (drug.IsVerified)
                {
                    ValidatedDrugs.Add(drug);
                }
                else if (drug.IsRejected)
                {
                    RejectedDrugs.Add(drug);
                }
                else
                {
                    UnvalidatedDrugs.Add(drug);
                }
            }
        }
        public void SetAnimations()
        {
            MR = FindResource("MoveButtonRight") as Storyboard;
            MM = FindResource("MoveButtonMiddle") as Storyboard;
            ML = FindResource("MoveButtonLeft") as Storyboard;
            CloseFrame = FindResource("CloseFrame") as Storyboard;
            OpenFrame = FindResource("FormFrameAnimation") as Storyboard;
        }

        private void ShowMoreInfoButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDrug = (Drug)DrugsDG.SelectedItems[0];
            FormFrame.Content = new DrugsInfo(this);
        }

        private void CloseFrame_Completed(object sender, EventArgs e)
        {
            FormFrame.Content = null;
            FormFrame.Opacity = 1;
            SelectedDrug = null;
        }

        private void AddNewDrugButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditRejectedDrugButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrugsDG.SelectedItems.Count == 0)
            {
                WrongSelection = "You must select rejected drug for editing first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            SelectedDrug = (Drug)DrugsDG.SelectedItems[0];
            if (!SelectedDrug.IsRejected)
            {
                WrongSelection = "You can only edit rejected drugs!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
        }

        private void WrongSelectionOK_Click(object sender, RoutedEventArgs e)
        {
            WrongSelectionContainer.Visibility = Visibility.Collapsed;
        }

        private void ChangeShowTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if(TypeIndicator == 0)
            {
                MM.Begin();
                TypeIndicator = 1;
                ChangeShowTypeButton.Content = "Unvalidated";
                DrugsDG.ItemsSource = UnvalidatedDrugs;
                return;
            } else if(TypeIndicator == 1)
            {
                MR.Begin();
                TypeIndicator = 2;
                ChangeShowTypeButton.Content = "Rejected";
                DrugsDG.ItemsSource = RejectedDrugs;
                return;
            } else
            {

                ML.Begin();
                TypeIndicator = 0;
                ChangeShowTypeButton.Content = "Validated";
                DrugsDG.ItemsSource = ValidatedDrugs;
                return;
            }
        }

    }
}
