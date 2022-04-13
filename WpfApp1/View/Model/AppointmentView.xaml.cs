﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Controller;
using WpfApp1.View.Converter;

namespace WpfApp1.View.Model
{
    /// <summary>
    /// Interaction logic for AppointmentView.xaml
    /// </summary>
    public partial class AppointmentView : UserControl, INotifyPropertyChanged
    {
        public AppointmentView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private int _id;
        private DateTime _beginning;
        private DateTime _ending;

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
        public DateTime Ending
        {
            get => _ending;
            set
            {
                if (_ending != value)
                {
                    _ending = value;
                    OnPropertyChanged("Ending");
                }
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
