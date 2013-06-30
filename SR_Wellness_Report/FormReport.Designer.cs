namespace SR_Wellness_Report
{
    partial class FormReport
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
            this.labelFile = new System.Windows.Forms.Label();
            this.openReportDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.comboBoxProperty = new System.Windows.Forms.ComboBox();
            this.groupBoxOrder = new System.Windows.Forms.GroupBox();
            this.radioBtnDesc = new System.Windows.Forms.RadioButton();
            this.radioBtnAsc = new System.Windows.Forms.RadioButton();
            this.checkBoxImportance = new System.Windows.Forms.CheckBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.labelMailAddr = new System.Windows.Forms.Label();
            this.textBoxMailAddr = new System.Windows.Forms.TextBox();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.backgroundParser = new System.ComponentModel.BackgroundWorker();
            this.groupBoxOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFile.Location = new System.Drawing.Point(31, 35);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(85, 17);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "Reports(.xml)";
            // 
            // openReportDialog
            // 
            this.openReportDialog.FileName = "openReportDialog";
            this.openReportDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openReportDialog_FileOk);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(138, 35);
            this.textBoxPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(312, 23);
            this.textBoxPath.TabIndex = 1;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenFile.Location = new System.Drawing.Point(479, 33);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(83, 25);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Choose...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // comboBoxProperty
            // 
            this.comboBoxProperty.FormattingEnabled = true;
            this.comboBoxProperty.Items.AddRange(new object[] {
            "MinIdleTime(dd:hh)",
            "MinTotalLabor(t1:t2)"});
            this.comboBoxProperty.Location = new System.Drawing.Point(34, 90);
            this.comboBoxProperty.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxProperty.Name = "comboBoxProperty";
            this.comboBoxProperty.Size = new System.Drawing.Size(152, 25);
            this.comboBoxProperty.TabIndex = 3;
            this.comboBoxProperty.Text = "MinIdleTime(dd:hh)";
            this.comboBoxProperty.SelectedIndexChanged += new System.EventHandler(this.comboBoxProperty_SelectedIndexChanged);
            // 
            // groupBoxOrder
            // 
            this.groupBoxOrder.Controls.Add(this.radioBtnDesc);
            this.groupBoxOrder.Controls.Add(this.radioBtnAsc);
            this.groupBoxOrder.Location = new System.Drawing.Point(297, 75);
            this.groupBoxOrder.Name = "groupBoxOrder";
            this.groupBoxOrder.Size = new System.Drawing.Size(119, 40);
            this.groupBoxOrder.TabIndex = 6;
            this.groupBoxOrder.TabStop = false;
            this.groupBoxOrder.Text = "Order";
            this.groupBoxOrder.Enter += new System.EventHandler(this.groupBoxOrder_Enter);
            // 
            // radioBtnDesc
            // 
            this.radioBtnDesc.AutoSize = true;
            this.radioBtnDesc.Location = new System.Drawing.Point(58, 15);
            this.radioBtnDesc.Name = "radioBtnDesc";
            this.radioBtnDesc.Size = new System.Drawing.Size(54, 21);
            this.radioBtnDesc.TabIndex = 1;
            this.radioBtnDesc.Text = "Desc";
            this.radioBtnDesc.UseVisualStyleBackColor = true;
            // 
            // radioBtnAsc
            // 
            this.radioBtnAsc.AutoSize = true;
            this.radioBtnAsc.Checked = true;
            this.radioBtnAsc.Location = new System.Drawing.Point(6, 15);
            this.radioBtnAsc.Name = "radioBtnAsc";
            this.radioBtnAsc.Size = new System.Drawing.Size(46, 21);
            this.radioBtnAsc.TabIndex = 0;
            this.radioBtnAsc.TabStop = true;
            this.radioBtnAsc.Text = "Asc";
            this.radioBtnAsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxImportance
            // 
            this.checkBoxImportance.AutoSize = true;
            this.checkBoxImportance.Location = new System.Drawing.Point(437, 92);
            this.checkBoxImportance.Name = "checkBoxImportance";
            this.checkBoxImportance.Size = new System.Drawing.Size(125, 21);
            this.checkBoxImportance.TabIndex = 7;
            this.checkBoxImportance.Text = "High Importance";
            this.checkBoxImportance.UseVisualStyleBackColor = true;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(34, 195);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(528, 113);
            this.textBoxLog.TabIndex = 8;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(367, 151);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(83, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(479, 151);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(83, 23);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labelMailAddr
            // 
            this.labelMailAddr.AutoSize = true;
            this.labelMailAddr.Location = new System.Drawing.Point(31, 154);
            this.labelMailAddr.Name = "labelMailAddr";
            this.labelMailAddr.Size = new System.Drawing.Size(116, 17);
            this.labelMailAddr.TabIndex = 12;
            this.labelMailAddr.Text = "Team E-mail Addr.";
            // 
            // textBoxMailAddr
            // 
            this.textBoxMailAddr.Location = new System.Drawing.Point(153, 151);
            this.textBoxMailAddr.Name = "textBoxMailAddr";
            this.textBoxMailAddr.Size = new System.Drawing.Size(173, 23);
            this.textBoxMailAddr.TabIndex = 13;
            // 
            // textBoxTime
            // 
            this.textBoxTime.Font = new System.Drawing.Font("Microsoft YaHei", 10F);
            this.textBoxTime.Location = new System.Drawing.Point(192, 90);
            this.textBoxTime.MaxLength = 100;
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(83, 25);
            this.textBoxTime.TabIndex = 4;
            this.textBoxTime.Text = "07:00";
            this.textBoxTime.WordWrap = false;
            this.textBoxTime.Enter += new System.EventHandler(this.textBoxTime_Enter);
            this.textBoxTime.Validated += new System.EventHandler(this.textBoxTime_Validated);
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 334);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.textBoxMailAddr);
            this.Controls.Add(this.labelMailAddr);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.checkBoxImportance);
            this.Controls.Add(this.groupBoxOrder);
            this.Controls.Add(this.comboBoxProperty);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.labelFile);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormReport";
            this.Text = "SR Wellness Report";
            this.Load += new System.EventHandler(this.FormReport_Load);
            this.groupBoxOrder.ResumeLayout(false);
            this.groupBoxOrder.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.OpenFileDialog openReportDialog;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.ComboBox comboBoxProperty;
        private System.Windows.Forms.GroupBox groupBoxOrder;
        private System.Windows.Forms.CheckBox checkBoxImportance;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label labelMailAddr;
        private System.Windows.Forms.TextBox textBoxMailAddr;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.RadioButton radioBtnDesc;
        private System.Windows.Forms.RadioButton radioBtnAsc;
        private System.ComponentModel.BackgroundWorker backgroundParser;
    }
}