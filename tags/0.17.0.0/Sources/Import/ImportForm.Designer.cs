namespace Varozhka.Import
{
    partial class ImportForm
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
            this._btnStart = new System.Windows.Forms.Button();
            this._lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._boxDirectory = new System.Windows.Forms.TextBox();
            this._dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this._btnSelectFolder = new System.Windows.Forms.Button();
            this._hlinkHome = new System.Windows.Forms.LinkLabel();
            this._worker = new System.ComponentModel.BackgroundWorker();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // _btnStart
            // 
            this._btnStart.Location = new System.Drawing.Point(5, 44);
            this._btnStart.Name = "_btnStart";
            this._btnStart.Size = new System.Drawing.Size(75, 23);
            this._btnStart.TabIndex = 0;
            this._btnStart.Text = "Start";
            this._btnStart.UseVisualStyleBackColor = true;
            this._btnStart.Click += new System.EventHandler(this._btnStart_Click);
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.Location = new System.Drawing.Point(86, 49);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(38, 13);
            this._lblStatus.TabIndex = 1;
            this._lblStatus.Text = "Ready";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Directory with Netflix dataset:";
            // 
            // _boxDirectory
            // 
            this._boxDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Varozhka.Import.Properties.Settings.Default, "NetflixDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._boxDirectory.Location = new System.Drawing.Point(5, 18);
            this._boxDirectory.Name = "_boxDirectory";
            this._boxDirectory.ReadOnly = true;
            this._boxDirectory.Size = new System.Drawing.Size(391, 20);
            this._boxDirectory.TabIndex = 3;
            this._boxDirectory.Text = global::Varozhka.Import.Properties.Settings.Default.NetflixDir;
            // 
            // _dlgFolderBrowser
            // 
            this._dlgFolderBrowser.Description = "Select directory with extracted NetFlix data";
            this._dlgFolderBrowser.ShowNewFolderButton = false;
            // 
            // _btnSelectFolder
            // 
            this._btnSelectFolder.Location = new System.Drawing.Point(403, 18);
            this._btnSelectFolder.Name = "_btnSelectFolder";
            this._btnSelectFolder.Size = new System.Drawing.Size(27, 19);
            this._btnSelectFolder.TabIndex = 4;
            this._btnSelectFolder.Text = "...";
            this._btnSelectFolder.UseVisualStyleBackColor = true;
            this._btnSelectFolder.Click += new System.EventHandler(this._btnSelectFolder_Click);
            // 
            // _hlinkHome
            // 
            this._hlinkHome.AutoSize = true;
            this._hlinkHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this._hlinkHome.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this._hlinkHome.Location = new System.Drawing.Point(389, 49);
            this._hlinkHome.Name = "_hlinkHome";
            this._hlinkHome.Size = new System.Drawing.Size(41, 13);
            this._hlinkHome.TabIndex = 5;
            this._hlinkHome.TabStop = true;
            this._hlinkHome.Text = "digizzle";
            this._hlinkHome.UseMnemonic = false;
            this._hlinkHome.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._hlinkHome_LinkClicked);
            // 
            // _worker
            // 
            this._worker.WorkerReportsProgress = true;
            this._worker.WorkerSupportsCancellation = true;
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(232, 49);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(151, 18);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressBar.TabIndex = 6;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 74);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._hlinkHome);
            this.Controls.Add(this._btnSelectFolder);
            this.Controls.Add(this._boxDirectory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._lblStatus);
            this.Controls.Add(this._btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImportForm";
            this.Text = "Netflix dataset importer - Varozhka";
            this.Load += new System.EventHandler(this.ImportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnStart;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _boxDirectory;
        private System.Windows.Forms.FolderBrowserDialog _dlgFolderBrowser;
        private System.Windows.Forms.Button _btnSelectFolder;
        private System.Windows.Forms.LinkLabel _hlinkHome;
        private System.ComponentModel.BackgroundWorker _worker;
        private System.Windows.Forms.ProgressBar _progressBar;
    }
}

