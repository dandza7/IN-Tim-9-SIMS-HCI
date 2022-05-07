using System;
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
    /// Interaction logic for SecretaryManageAllergiesDialog.xaml
    /// </summary>
    public partial class SecretaryManageAllergiesDialog : Window
    {
        public SecretaryManageAllergiesDialog(int patientId)
        {
            InitializeComponent();
            DataContext = this;
            patientIdInvisible.Text = patientId.ToString();
        }
        private AllergyController _allergyController;
        private MedicalRecordController _mrController;
        private void AddAllergy_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _allergyController = app.AllergyController;
            _mrController = app.MedicalRecordController;
            MedicalRecord mr = this._mrController.GetByPatientId(Int32.Parse(patientIdInvisible.Text));
            int mrId = mr.Id;
            Allergy allergy = new Allergy(
                mrId,
                allergynameTB.Text
                );

            _allergyController.Create(allergy);
            Close();
        }
    }
}
