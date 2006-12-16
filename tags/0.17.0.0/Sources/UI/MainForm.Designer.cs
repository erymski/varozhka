namespace Varozhka.UI
{
    partial class MainForm
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
            this._btnPredict = new System.Windows.Forms.Button();
            this._btnCheckRmse = new System.Windows.Forms.Button();
            this._lblRmse = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._dlgChooseDir = new System.Windows.Forms.FolderBrowserDialog();
            this._worker = new System.ComponentModel.BackgroundWorker();
            this._btnSettings = new System.Windows.Forms.Button();
            this._btnExit = new System.Windows.Forms.Button();
            this._strip = new System.Windows.Forms.StatusStrip();
            this._lblStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this._progress = new System.Windows.Forms.ToolStripProgressBar();
            this._hlink = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnPredict
            // 
            this._btnPredict.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._btnPredict.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnPredict.Location = new System.Drawing.Point(12, 67);
            this._btnPredict.Name = "_btnPredict";
            this._btnPredict.Size = new System.Drawing.Size(138, 23);
            this._btnPredict.TabIndex = 0;
            this._btnPredict.Text = "Generate prediction set";
            this._btnPredict.UseMnemonic = false;
            this._btnPredict.UseVisualStyleBackColor = true;
            this._btnPredict.Click += new System.EventHandler(this._btnPredict_Click);
            // 
            // _btnCheckRmse
            // 
            this._btnCheckRmse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._btnCheckRmse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCheckRmse.Location = new System.Drawing.Point(6, 19);
            this._btnCheckRmse.Name = "_btnCheckRmse";
            this._btnCheckRmse.Size = new System.Drawing.Size(137, 23);
            this._btnCheckRmse.TabIndex = 0;
            this._btnCheckRmse.Text = "Check RMSE";
            this._btnCheckRmse.UseMnemonic = false;
            this._btnCheckRmse.UseVisualStyleBackColor = true;
            this._btnCheckRmse.Click += new System.EventHandler(this._btnCheckRmse_Click);
            // 
            // _lblRmse
            // 
            this._lblRmse.AutoSize = true;
            this._lblRmse.Location = new System.Drawing.Point(189, 19);
            this._lblRmse.Name = "_lblRmse";
            this._lblRmse.Size = new System.Drawing.Size(0, 13);
            this._lblRmse.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(414, 61);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._btnCheckRmse);
            this.groupBox1.Controls.Add(this._lblRmse);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RMSE checker";
            // 
            // _worker
            // 
            this._worker.WorkerReportsProgress = true;
            this._worker.WorkerSupportsCancellation = true;
            // 
            // _btnSettings
            // 
            this._btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSettings.Location = new System.Drawing.Point(261, 67);
            this._btnSettings.Name = "_btnSettings";
            this._btnSettings.Size = new System.Drawing.Size(75, 23);
            this._btnSettings.TabIndex = 0;
            this._btnSettings.Text = "Settings...";
            this._btnSettings.UseVisualStyleBackColor = true;
            this._btnSettings.Click += new System.EventHandler(this._btnSettings_Click);
            // 
            // _btnExit
            // 
            this._btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnExit.Location = new System.Drawing.Point(342, 67);
            this._btnExit.Name = "_btnExit";
            this._btnExit.Size = new System.Drawing.Size(75, 23);
            this._btnExit.TabIndex = 2;
            this._btnExit.Text = "Exit";
            this._btnExit.UseVisualStyleBackColor = true;
            this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
            // 
            // _strip
            // 
            this._strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblStatusText,
            this._progress,
            this._hlink});
            this._strip.Location = new System.Drawing.Point(0, 97);
            this._strip.Name = "_strip";
            this._strip.Size = new System.Drawing.Size(420, 22);
            this._strip.SizingGrip = false;
            this._strip.TabIndex = 9;
            // 
            // _lblStatusText
            // 
            this._lblStatusText.AutoSize = false;
            this._lblStatusText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lblStatusText.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
            this._lblStatusText.Name = "_lblStatusText";
            this._lblStatusText.Size = new System.Drawing.Size(150, 17);
            this._lblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _progress
            // 
            this._progress.AutoSize = false;
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(200, 16);
            this._progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // _hlink
            // 
            this._hlink.AutoSize = false;
            this._hlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._hlink.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._hlink.IsLink = true;
            this._hlink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this._hlink.Name = "_hlink";
            this._hlink.Size = new System.Drawing.Size(58, 17);
            this._hlink.Text = "Varozhka";
            this._hlink.ToolTipText = "Visit Varozhka\'s homepage";
            this._hlink.VisitedLinkColor = System.Drawing.Color.Blue;
            this._hlink.Click += new System.EventHandler(this._hlink_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 119);
            this.Controls.Add(this._btnPredict);
            this.Controls.Add(this._strip);
            this.Controls.Add(this._btnExit);
            this.Controls.Add(this._btnSettings);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Processing panel - Varozhka";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._strip.ResumeLayout(false);
            this._strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnPredict;
        private System.Windows.Forms.Button _btnCheckRmse;
        private System.Windows.Forms.Label _lblRmse;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FolderBrowserDialog _dlgChooseDir;
        private System.ComponentModel.BackgroundWorker _worker;
        private System.Windows.Forms.Button _btnSettings;
        private System.Windows.Forms.Button _btnExit;
        private System.Windows.Forms.StatusStrip _strip;
        private System.Windows.Forms.ToolStripStatusLabel _lblStatusText;
        private System.Windows.Forms.ToolStripProgressBar _progress;
        private System.Windows.Forms.ToolStripStatusLabel _hlink;
    }
}

