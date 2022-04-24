using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1.View.Dialog.PatientDialog
{
    public partial class PatientHelp : Form
    {
        static PatientHelp patientHelp;

        public PatientHelp(string content)
        {
            InitializeComponent();
            
            HelpContent.Text = content;
        }

        public static void Show(string content)
        {
            patientHelp = new PatientHelp(content);
            patientHelp.ShowDialog();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            patientHelp.Dispose();
        }
    }
}
