using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.ViewModel.Secretary
{
    public class RoomCountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private RoomController _roomController;
        private ObservableCollection<Room> _rooms;
        public string _type;
        public int _num;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public int Number
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
                OnPropertyChanged("Number");

            }
        }
        public RoomCountViewModel(string type, int num)
        {
            this.Type = type;
            this.Number = num;
        }
    
    }
}
