﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryAddNewDynamicEquipmentDialog.xaml
    /// </summary>
    public partial class SecretaryAddNewDynamicEquipmentDialog : Window
    {
        public SecretaryAddNewDynamicEquipmentDialog()
        {
            InitializeComponent();
        }
        private void Add_DynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            DynamicEquipmentRequestController _dynamicReqController = app.DynamicEquipmentReqeustController;

            DynamicEquipmentRequest der = new DynamicEquipmentRequest(nameTB.Text, int.Parse(amountTB.Text), DateTime.Now.AddSeconds(10));
            _dynamicReqController.Create(der);

            Close();
        }
    }
}
