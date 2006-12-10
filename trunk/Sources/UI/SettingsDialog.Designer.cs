namespace Varozhka.UI
{
    partial class SettingsDialog
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
            this._dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._layoutFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._btnChangeOutputDir = new System.Windows.Forms.Button();
            this._boxOutputDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._btnChangeNetflixDir = new System.Windows.Forms.Button();
            this._boxNexflixDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._btnAssembly = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._boxAssembly = new System.Windows.Forms.TextBox();
            this._checkCheckUpdates = new System.Windows.Forms.CheckBox();
            this._checkAutoSubmit = new System.Windows.Forms.CheckBox();
            this._dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this._layoutFlow.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dlgFolderBrowser
            // 
            this._dlgFolderBrowser.ShowNewFolderButton = false;
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point(3, 165);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(75, 23);
            this._btnOK.TabIndex = 0;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(84, 165);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _layoutFlow
            // 
            this._layoutFlow.Controls.Add(this.groupBox1);
            this._layoutFlow.Controls.Add(this.groupBox2);
            this._layoutFlow.Controls.Add(this._checkCheckUpdates);
            this._layoutFlow.Controls.Add(this._checkAutoSubmit);
            this._layoutFlow.Controls.Add(this._btnOK);
            this._layoutFlow.Controls.Add(this._btnCancel);
            this._layoutFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutFlow.Location = new System.Drawing.Point(0, 0);
            this._layoutFlow.Name = "_layoutFlow";
            this._layoutFlow.Size = new System.Drawing.Size(385, 193);
            this._layoutFlow.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._btnChangeOutputDir);
            this.groupBox1.Controls.Add(this._boxOutputDir);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this._btnChangeNetflixDir);
            this.groupBox1.Controls.Add(this._boxNexflixDir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 85);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Working directories";
            // 
            // _btnChangeOutputDir
            // 
            this._btnChangeOutputDir.Location = new System.Drawing.Point(346, 54);
            this._btnChangeOutputDir.Name = "_btnChangeOutputDir";
            this._btnChangeOutputDir.Size = new System.Drawing.Size(24, 20);
            this._btnChangeOutputDir.TabIndex = 5;
            this._btnChangeOutputDir.Text = "...";
            this._btnChangeOutputDir.UseVisualStyleBackColor = true;
            this._btnChangeOutputDir.Click += new System.EventHandler(this._btnChangeOutputDir_Click);
            // 
            // _boxOutputDir
            // 
            this._boxOutputDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Varozhka.UI.Properties.Settings.Default, "OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._boxOutputDir.Location = new System.Drawing.Point(73, 54);
            this._boxOutputDir.Name = "_boxOutputDir";
            this._boxOutputDir.ReadOnly = true;
            this._boxOutputDir.Size = new System.Drawing.Size(266, 20);
            this._boxOutputDir.TabIndex = 4;
            this._boxOutputDir.Text = global::Varozhka.UI.Properties.Settings.Default.OutputDir;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output:";
            // 
            // _btnChangeNetflixDir
            // 
            this._btnChangeNetflixDir.Location = new System.Drawing.Point(346, 20);
            this._btnChangeNetflixDir.Name = "_btnChangeNetflixDir";
            this._btnChangeNetflixDir.Size = new System.Drawing.Size(24, 20);
            this._btnChangeNetflixDir.TabIndex = 2;
            this._btnChangeNetflixDir.Text = "...";
            this._btnChangeNetflixDir.UseVisualStyleBackColor = true;
            this._btnChangeNetflixDir.Click += new System.EventHandler(this._btnChangeNetflixDir_Click);
            // 
            // _boxNexflixDir
            // 
            this._boxNexflixDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Varozhka.UI.Properties.Settings.Default, "NetflixDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._boxNexflixDir.Location = new System.Drawing.Point(73, 20);
            this._boxNexflixDir.Name = "_boxNexflixDir";
            this._boxNexflixDir.ReadOnly = true;
            this._boxNexflixDir.Size = new System.Drawing.Size(266, 20);
            this._boxNexflixDir.TabIndex = 1;
            this._boxNexflixDir.Text = global::Varozhka.UI.Properties.Settings.Default.NetflixDir;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Netflix files:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._btnAssembly);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this._boxAssembly);
            this.groupBox2.Location = new System.Drawing.Point(3, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 42);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Processing";
            // 
            // _btnAssembly
            // 
            this._btnAssembly.Location = new System.Drawing.Point(346, 13);
            this._btnAssembly.Name = "_btnAssembly";
            this._btnAssembly.Size = new System.Drawing.Size(24, 20);
            this._btnAssembly.TabIndex = 5;
            this._btnAssembly.Text = "...";
            this._btnAssembly.UseVisualStyleBackColor = true;
            this._btnAssembly.Click += new System.EventHandler(this._btnAssembly_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Processing assembly:";
            // 
            // _boxAssembly
            // 
            this._boxAssembly.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Varozhka.UI.Properties.Settings.Default, "ProcessingAssembly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._boxAssembly.Location = new System.Drawing.Point(120, 13);
            this._boxAssembly.Name = "_boxAssembly";
            this._boxAssembly.ReadOnly = true;
            this._boxAssembly.Size = new System.Drawing.Size(219, 20);
            this._boxAssembly.TabIndex = 6;
            this._boxAssembly.Text = global::Varozhka.UI.Properties.Settings.Default.ProcessingAssembly;
            // 
            // _checkCheckUpdates
            // 
            this._checkCheckUpdates.AutoSize = true;
            this._checkCheckUpdates.Checked = global::Varozhka.UI.Properties.Settings.Default.CheckForUpdates;
            this._checkCheckUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkCheckUpdates.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Varozhka.UI.Properties.Settings.Default, "CheckForUpdates", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._checkCheckUpdates.Location = new System.Drawing.Point(3, 142);
            this._checkCheckUpdates.Name = "_checkCheckUpdates";
            this._checkCheckUpdates.Size = new System.Drawing.Size(177, 17);
            this._checkCheckUpdates.TabIndex = 3;
            this._checkCheckUpdates.Text = "Automatically check for updates";
            this._checkCheckUpdates.UseVisualStyleBackColor = true;
            // 
            // _checkAutoSubmit
            // 
            this._checkAutoSubmit.AutoSize = true;
            this._checkAutoSubmit.Checked = global::Varozhka.UI.Properties.Settings.Default.SubmitResults;
            this._checkAutoSubmit.CheckState = System.Windows.Forms.CheckState.Checked;
            this._checkAutoSubmit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Varozhka.UI.Properties.Settings.Default, "SubmitResults", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this._checkAutoSubmit.Location = new System.Drawing.Point(186, 142);
            this._checkAutoSubmit.Name = "_checkAutoSubmit";
            this._checkAutoSubmit.Size = new System.Drawing.Size(154, 17);
            this._checkAutoSubmit.TabIndex = 4;
            this._checkAutoSubmit.Text = "Automatically submit results";
            this._checkAutoSubmit.UseVisualStyleBackColor = true;
            // 
            // _dlgOpenFile
            // 
            this._dlgOpenFile.Filter = "Assemblies|*.dll";
            this._dlgOpenFile.Title = "Select processing assembly";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 193);
            this.Controls.Add(this._layoutFlow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this._layoutFlow.ResumeLayout(false);
            this._layoutFlow.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog _dlgFolderBrowser;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.FlowLayoutPanel _layoutFlow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _boxNexflixDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnChangeNetflixDir;
        private System.Windows.Forms.Button _btnChangeOutputDir;
        private System.Windows.Forms.TextBox _boxOutputDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox _checkCheckUpdates;
        private System.Windows.Forms.CheckBox _checkAutoSubmit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _btnAssembly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _boxAssembly;
        private System.Windows.Forms.OpenFileDialog _dlgOpenFile;
    }
}