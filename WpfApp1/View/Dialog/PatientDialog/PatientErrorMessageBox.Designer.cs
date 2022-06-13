namespace WpfApp1.View.Dialog.PatientDialog
{
    partial class PatientErrorMessageBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OkayButton = new System.Windows.Forms.Button();
            this.MessageBoxContent = new System.Windows.Forms.Label();
            this.Content = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OkayButton
            // 
            this.OkayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(240)))));
            this.OkayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkayButton.ForeColor = System.Drawing.Color.Snow;
            this.OkayButton.Location = new System.Drawing.Point(358, 137);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(90, 30);
            this.OkayButton.TabIndex = 1;
            this.OkayButton.Text = "Okay";
            this.OkayButton.UseVisualStyleBackColor = false;
            this.OkayButton.Click += new System.EventHandler(this.OkayButton_Click);
            this.OkayButton.Enter += new System.EventHandler(this.OkayButton_Enter);
            this.OkayButton.Leave += new System.EventHandler(this.OkayButton_Leave);
            // 
            // MessageBoxContent
            // 
            this.MessageBoxContent.AutoSize = true;
            this.MessageBoxContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(222)))), ((int)(((byte)(254)))));
            this.MessageBoxContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageBoxContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(49)))), ((int)(((byte)(110)))));
            this.MessageBoxContent.Location = new System.Drawing.Point(57, 86);
            this.MessageBoxContent.Name = "MessageBoxContent";
            this.MessageBoxContent.Size = new System.Drawing.Size(0, 29);
            this.MessageBoxContent.TabIndex = 2;
            this.MessageBoxContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Content
            // 
            this.Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(222)))), ((int)(((byte)(254)))));
            this.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Content.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(49)))), ((int)(((byte)(110)))));
            this.Content.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Content.Location = new System.Drawing.Point(8, 5);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(440, 116);
            this.Content.TabIndex = 3;
            this.Content.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(198)))), ((int)(((byte)(252)))));
            this.panel1.Controls.Add(this.Content);
            this.panel1.Controls.Add(this.OkayButton);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 176);
            this.panel1.TabIndex = 4;
            // 
            // PatientErrorMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(464, 183);
            this.Controls.Add(this.MessageBoxContent);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PatientErrorMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PatientMessageBox";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Label MessageBoxContent;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Panel panel1;
    }
}