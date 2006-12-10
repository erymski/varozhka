// Copyright (c) 2006, Eugene Rymski
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted 
//  provided that the following conditions are met:
// * Redistributions of source code must retain the above copyright notice, this list of conditions 
//   and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
//   and the following disclaimer in the documentation and/or other materials provided with the distribution.
// * Neither the name of the “Varozhka” nor the names of its contributors may be used to endorse or 
//   promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY 
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    public partial class ImportForm : Form
    {
        #region Private vars

        private DateTime _startTime;

        #endregion

        #region Properties

        /// <summary>
        /// If import should be started automatically
        /// </summary>
        public bool AutoStart
        {
            get { return _autoStart; }
            set { _autoStart = value; }
        }
        private bool _autoStart;

        /// <summary>
        /// Directory with Netflix dataset. 
        /// </summary>
        public string AutoNetflixDir
        {
            get { return _preselectedDir; }
            set { _preselectedDir = value; }
        }
        private string _preselectedDir;

        /// <summary>
        /// If import succeeded.
        /// </summary>
        public bool ImportSucceeded
        {
            get { return _importSucceeded; }
            set { _importSucceeded = value; }
        }
        private bool _importSucceeded = false;

        #endregion

        #region Constructor

        public ImportForm()
        {
            InitializeComponent();
        }

        #endregion

        #region UI handlers

        private void _btnStart_Click(object sender, EventArgs e)
        {
            if (! _worker.CancellationPending)
            {
                if (_worker.IsBusy)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to stop the import process?", Text,
                                                          MessageBoxButtons.YesNo, 
                                                          MessageBoxIcon.Question, 
                                                          MessageBoxDefaultButton.Button2);
                    if (System.Windows.Forms.DialogResult.Yes == result)
                    {
                        _btnStart.Enabled = false;
                        _worker.CancelAsync();
                    }
                }
                else
                {
                    StartAsyncImport(_boxDirectory.Text);
                }
            }
        }

        private void _btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == _dlgFolderBrowser.ShowDialog())
            {
                _boxDirectory.Text = _dlgFolderBrowser.SelectedPath;
            }
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
            if (AutoStart)
            {
                StartAsyncImport(AutoNetflixDir);
            }
        }

        #endregion

        private void SetStatusMessage(string message)
        {
            _lblStatus.Text = message;
        }


        #region Async processing support

        private void StartAsyncImport(string netflixDirectory)
        {
            // validate the selected directory
            Cursor = Cursors.WaitCursor;
            //SetStatusMessage("Validating the selected directory");
            bool valid = NetflixDatasetValidator.Contains(netflixDirectory, true);
            Cursor = Cursors.Default;
            
            if (! valid)
            {
                MessageBox.Show("Given directory does not contains Netflix dataset.", Text);
                return;
            }

            SwitchUI2ProgressMode(netflixDirectory);

            _startTime = DateTime.Now;
            
            ImportSucceeded = false;

            // start async processing
            _worker.DoWork += DoImport;
            _worker.RunWorkerCompleted += ImportCompleted;
            _worker.ProgressChanged += OnProgressChanged;

            _worker.RunWorkerAsync(netflixDirectory);
        }

        /// <summary>
        /// Various UI changes before processing started
        /// </summary>
        /// <param name="netflixDirectory"></param>
        private void SwitchUI2ProgressMode(string netflixDirectory)
        {
            _boxDirectory.Text = netflixDirectory;
            Varozhka.Import.Properties.Settings.Default.Save();
            SetStatusMessage("Processing started");
            _progressBar.Visible = true;
            _btnStart.Text = "Stop";
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // check if user state contains a message
            if (null != e.UserState)
            {
                string statusMessage = e.UserState as string;
                if (! string.IsNullOrEmpty(statusMessage))
                {
                    SetStatusMessage(statusMessage);
                }
            }

            Debug.Assert(e.ProgressPercentage >= 0 && e.ProgressPercentage <= 100);
            _progressBar.Value = e.ProgressPercentage;
        }

        private void ImportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Import cancelled.", Text);
                SetStatusMessage("Ready");
            }
            else
            {
                SetStatusMessage(string.Format("Import completed in {0}", DateTime.Now - _startTime));
                ImportSucceeded = true;

                // import was started by external app - so just close the form
                if (AutoStart)
                {
                    Close();
                }
            }
            
            _progressBar.Visible = false;
            _btnStart.Enabled = true;
            _btnStart.Text = "Start";
        }

        private void DoImport(object sender, DoWorkEventArgs e)
        {
            string netflixDirectory = (string)e.Argument;
            
            Importer importer = new Importer(netflixDirectory, sender as BackgroundWorker, e);
            importer.StartImport();
        }

        #endregion

        private void _hlinkHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.digizzle.com/Projects/Varozka/Default.aspx?from=varozhka");
        }
    }
}