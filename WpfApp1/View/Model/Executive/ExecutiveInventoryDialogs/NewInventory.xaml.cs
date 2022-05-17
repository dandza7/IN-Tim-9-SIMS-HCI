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

namespace WpfApp1.View.Model.Executive.ExecutiveInventoryDialogs
{
    /// <summary>
    /// Interaction logic for NewInventory.xaml
    /// </summary>
    
    public partial class NewInventory : Page, INotifyPropertyChanged
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
        public ExecutiveInventoryPages ParentPage { get; set; }
        public List<string> SOPRooms { get; set; }
        public NewInventory(ExecutiveInventoryPages parent)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ParentPage = parent;
            this.SOPRooms = parent.InventoryController.GetSOPRooms();
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddRooms.Text == "" || AddName.Text == "")
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (AddName.Text.Contains(";"))
            {
                Feedback = "*you can't use semicolon (;) in name!";
                return;
            }
            ParentPage.InventoryController.Create(new Inventory(0, 0, AddName.Text, "S", 1), AddRooms.Text);
            ParentPage.CloseFrame.Begin();
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
            ParentPage.Inventory = ParentPage.InventoryController.GetPreviews(); 
        }
    }
}
