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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Varozhka.TrainingData;
using Varozhka.UI.Properties;
using Varozhka.UI.Tasks;

namespace Varozhka.UI
{
    public partial class MainForm : Form
    {
        internal delegate void TaskCompleteDelegate(object data);

        #region Constants

        private const string c_submissionFileName = @"output.txt";
        private const string c_urlSubmissions = "http://www.netflixprize.com/submissions";
        private const string c_indexerExecutable = "Varozhka.Import.exe";

        #endregion

        #region Properties

        /// <summary>
        /// Current processing
        /// </summary>
        private ILongTask CurrentTask
        {
            get
            {
                Debug.Assert(null != _currTask, "There is no active processing task.");
                return _currTask;
            }
            set
            {
                Debug.Assert((null == _currTask) || (null == value), "Other task already in progress.");
                
                _currTask = value;
            }
        }
        private ILongTask _currTask;
        
        private TaskCompleteDelegate _onComplete;

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Long processings
        
        private void StartBackgroundTask(ILongTask task)
        {
            StartBackgroundTask(task, null);
        }

        private void StartBackgroundTask(ILongTask task, TaskCompleteDelegate onComplete)
        {
            CurrentTask = task;
            _onComplete = onComplete;
            
            EnableProcessingControls(false);
            SetStatusMessage(_currTask.ProcessingStatusMessage);

            // reset progress bar
            _progress.Value = _progress.Minimum = 0;
            _progress.Maximum = 100;

            // TODO: get rid of BackgroundWorker
            _worker.DoWork += StartProcessing;
            _worker.RunWorkerCompleted += ProcessingCompleted;
            _worker.ProgressChanged += ProgressChanged;

            _worker.RunWorkerAsync();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progress.Value = e.ProgressPercentage;
            if (null != e.UserState)
            {
                _lblRmse.Text = ((double)e.UserState).ToString();
            }
        }

        /// <summary>
        /// Callback for background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartProcessing(object sender, DoWorkEventArgs e)
        {
            CurrentTask.Run(sender as BackgroundWorker, e);
        }

        private void ProcessingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                if (CurrentTask.CanBeStopped)
                {
                    MessageBox.Show(CurrentTask.ProcessingStoppedMessage, Text);
                }
                else
                {
                    ReportError(CurrentTask.ProcessingStoppedMessage);
                }
            }
            else
            {
                _worker.DoWork -= StartProcessing;
                _worker.RunWorkerCompleted -= ProcessingCompleted;

                if (null != _onComplete)
                {
                    _onComplete(CurrentTask);
                }

                ReadyForActions();
            }
            
            CurrentTask = null;
        }

        /// <summary>
        /// Actions to perform after processing completed.
        /// </summary>
        private void ReadyForActions()
        {
            _progress.Value = 0;
            EnableProcessingControls(true);
            SetStatusMessage("Ready");
        }

        /// <summary>
        /// Enable/disable controls affected by long processings
        /// </summary>
        /// <param name="enable"></param>
        private void EnableProcessingControls(bool enable)
        {
            _btnCheckRmse.Enabled = enable;
            _btnPredict.Enabled = enable;
            _btnSettings.Enabled = enable;
        }

        #endregion

        private void ShowSettingsDialog()
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.ShowDialog(this); // all assignments are in the dialog
            
            // TODO: index should be reloaded if the directory changed
        }

        /// <summary>
        /// Show error message and shutdown.
        /// </summary>
        /// <param name="message">The error message</param>
        private void ReportError(string message)
        {
            MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        public delegate void SetStatusMessageDelegate(string message);
        
        /// <summary>
        /// Set the message in status bar
        /// </summary>
        /// <param name="message">Status string</param>
        private void SetStatusMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new SetStatusMessageDelegate(SetStatusMessage), new object[] {message});
            }
            else
            {
                _lblStatusText.Text = message;
            }
        }

        #region UI handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += " [" + Application.ProductVersion + "]";
            
            // TODO: rewrite the logic with exceptions
            
            // check if we have enough info to perform processing
            if (! AreSettingsValid)
            {
                MessageBox.Show("Please, set processing settings", Text, 
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ShowSettingsDialog();
                
                if (Settings.Default.IsEmpty())
                {
                    ReportError("Not enough info to make processings. Quitting now.");
                    return;
                }
            }

            // perform indexing if necessary 
            if (! NetflixDatasetValidator.Processed(Settings.Default.NetflixDir))
            {
                MessageBox.Show(
                    "The selected directory needs to be indexed. This is a long and resource-consuming process, so be patient.",
                    Text, MessageBoxButtons.OK);

                // perform import
                string indexer = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), c_indexerExecutable);
                if (! File.Exists(indexer))
                {
                    ReportError("Cannot find executable with indexer. Quitting now.");
                    return;
                }
                Process process = Process.Start(indexer, 
                                                String.Format("{0}", Settings.Default.NetflixDir));
                process.WaitForExit();
                if (1 == process.ExitCode ||
                    !NetflixDatasetValidator.Processed(Settings.Default.NetflixDir)) // sanity check
                {
                    ReportError("The Netflix dataset was not properly indexed. Quitting now.");
                    return;
                }
            }

            AsyncCheckForUpdates();
            
            // TODO: check if Netflix dir is valid
            //if (string.IsNullOrEmpty(Settings.Default.NetflixDir))
            //{
            //    if (! SelectNetflixDir())
            //    {
            //        ReportError("Cannot find Netflix directory. Shutting down.");
            //    }
            //}
            StartBackgroundTask(new IndexLoadingTask(Settings.Default.NetflixDir, 
                                                     Settings.Default.ProcessingAssembly, 
                                                     new SetStatusMessageDelegate(SetStatusMessage)));
        }

        /// <summary>
        /// Validate settings
        /// </summary>
        private static bool AreSettingsValid
        {
            get
            {
                return ! Settings.Default.IsEmpty() && 
                       Directory.Exists(Settings.Default.NetflixDir);
            }
        }

        private void _btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        private void _btnPredict_Click(object sender, EventArgs e)
        {
            string outputFilename = Path.Combine(Settings.Default.OutputDir, c_submissionFileName);

            StartBackgroundTask(new GeneratePredictionSetTask(outputFilename), PrepareForSubmission);
        }

        /// <summary>
        /// Perform action with generated prediction subset
        /// </summary>
        /// <param name="data"></param>
        private void PrepareForSubmission(object data)
        {
            //GeneretePredictionSetTask task = (GeneretePredictionSetTask)data;

            Process.Start("explorer.exe", "\"" + Settings.Default.OutputDir + "\"");
            Process.Start(c_urlSubmissions);
        }

        private void _btnCheckRmse_Click(object sender, EventArgs e)
        {
            StartBackgroundTask(new RmseCheckTask());
        }

        private void _btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _hlink_Click(object sender, EventArgs e)
        {
            // TODO: add version number in url???
            Process.Start("http://www.digizzle.com/Projects/Varozka/Default.aspx?from=varozhka");
        }
        
        #endregion

        #region Updates checking

        /// <summary>
        /// Asyncronously checks updates.
        /// </summary>
        /// <returns></returns>
        private void AsyncCheckForUpdates()
        {
#if false // debugging - checking in sync mode
            CheckForUpdates(null);
#else
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckForUpdates));
#endif
        }

        private void CheckForUpdates(object state)
        {
            UpdatesChecker checker = new UpdatesChecker();
            bool updateAvailable = checker.IsUpdateAvailable(new Version(Application.ProductVersion), UpdatesUrls.Version);
            if (updateAvailable)
            {
                GoToUpdatesPage(UpdatesUrls.ChangeLog, "Update is available. Do you want to go to downloads page?");
            }
        }

        private delegate void GoToUpdatesPageDelegate(string urlChangeLog, string question);
        private void GoToUpdatesPage(string urlChangeLog, string question)
        {
            if (InvokeRequired)
            {
                Invoke(new GoToUpdatesPageDelegate(GoToUpdatesPage), new object[] { urlChangeLog, question });
            }
            else
            {
                DialogResult result = MessageBox.Show(question, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                      MessageBoxDefaultButton.Button1);
                if (DialogResult.Yes == result)
                {
                    Process.Start(urlChangeLog);
                }
            }
        }

        #endregion
    }
}