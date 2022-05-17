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

namespace WpfApp1.View.Model.Executive.ExecutiveDrugsDialogs
{
    /// <summary>
    /// Interaction logic for EditDrug.xaml
    /// </summary>
    public partial class EditDrug : Page, INotifyPropertyChanged
    {
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
        public ExecutiveDrugsPages ParentPage { get; set; }

        public EditDrug(ExecutiveDrugsPages parent)
        {
            InitializeComponent();
            this.ParentPage = parent;
            this.DataContext = this;
            Feedback = "";
            DrugName.Text = ParentPage.SelectedDrug.Name;
            DrugInfo.Text = ParentPage.SelectedDrug.Info;
            DrugComment.Text = ParentPage.SelectedDrug.Comment;
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
        }

        private void EditDrug_Click(object sender, RoutedEventArgs e)
        {
            if (DrugName.Text == "" || DrugInfo.Text == "")
            {
                Feedback = "You have to fill all fields!";
                return;
            }
            if (DrugName.Text.Contains(";") || DrugInfo.Text.Contains(";"))
            {
                Feedback = "You can't use semicolon (;) in fields!";
                return;
            }
            ParentPage.DrugController.Update(new Drug(ParentPage.SelectedDrug.Id, DrugName.Text, DrugInfo.Text, false, false, ""));
            ParentPage.GetDrugs();
            ParentPage.CloseFrame.Begin();
        }
    }
}
