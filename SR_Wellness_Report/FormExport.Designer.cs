namespace SR_Wellness_Report
{
    partial class FormExport
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
            this.addrComboBox = new System.Windows.Forms.ComboBox();
            this.exportBtn = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.backgroundSearcher = new System.ComponentModel.BackgroundWorker();
            this.nextBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addrComboBox
            // 
            this.addrComboBox.FormattingEnabled = true;
            this.addrComboBox.Location = new System.Drawing.Point(26, 26);
            this.addrComboBox.Name = "addrComboBox";
            this.addrComboBox.Size = new System.Drawing.Size(318, 21);
            this.addrComboBox.TabIndex = 0;
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(350, 26);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(75, 21);
            this.exportBtn.TabIndex = 1;
            this.exportBtn.Text = "Export";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(26, 67);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(398, 100);
            this.logTextBox.TabIndex = 2;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(26, 183);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(318, 24);
            this.webBrowser.TabIndex = 5;
            this.webBrowser.Visible = false;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // backgroundSearcher
            // 
            this.backgroundSearcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundSearcher_DoWork);
            this.backgroundSearcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundSearcher_RunWorkerCompleted);
            // 
            // nextBtn
            // 
            this.nextBtn.Location = new System.Drawing.Point(350, 183);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(74, 23);
            this.nextBtn.TabIndex = 6;
            this.nextBtn.Text = "Next";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 225);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.addrComboBox);
            this.Name = "FormExport";
            this.Text = "Export";
            this.Load += new System.EventHandler(this.exportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox addrComboBox;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.TextBox logTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.ComponentModel.BackgroundWorker backgroundSearcher;
        private System.Windows.Forms.Button nextBtn;
    }
}