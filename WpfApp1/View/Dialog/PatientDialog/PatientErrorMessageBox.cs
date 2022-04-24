﻿using System;
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
    public partial class PatientErrorMessageBox : Form
    {
        static PatientErrorMessageBox messageBox;

        public PatientErrorMessageBox(string content)
        {
            InitializeComponent();

            Content.Text = content;
        }

        public static void Show(string content)
        {
            messageBox = new PatientErrorMessageBox(content);
            messageBox.ShowDialog();
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            messageBox.Dispose();
        }

        private void OkayButton_Enter(object sender, EventArgs e)
        {
            OkayButton.Cursor = Cursors.Hand;
        }

        private void OkayButton_Leave(object sender, EventArgs e)
        {
            OkayButton.Cursor = Cursors.Default;
        }
    }
}
