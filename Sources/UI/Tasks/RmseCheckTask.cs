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

using System.ComponentModel;
using System.Diagnostics;
using Varozhka.Processing;

namespace Varozhka.UI.Tasks
{
    /// <summary>
    /// Calculate RMSE with the current estimator
    /// </summary>
    class RmseCheckTask : ILongTask
    {
        private BackgroundWorker _worker;

        #region ILongTask overrides

        /// <summary>
        /// Status message to show during processing
        /// </summary>
        public string ProcessingStatusMessage
        {
            get { return "RMSE check in progress..."; }
        }

        /// <summary>
        /// If the task must be run from the begin to the end.
        /// App will exit if it's false.
        /// </summary>
        public bool CanBeStopped
        {
            get { return true; }
        }

        /// <summary>
        /// Message to show on stopped processing.
        /// </summary>
        public string ProcessingStoppedMessage
        {
            get { return "RMSE check aborted"; }
        }

        /// <summary>
        /// Run the processing.
        /// </summary>
        public void Run(BackgroundWorker worker, DoWorkEventArgs e)
        {
            _worker = worker;
            RmseChecker checker = new RmseChecker(DataManager.Instance.NetflixFiles.NormalizedProbeSet, 
                                                  DataManager.Instance.Estimator, 100);
            checker.RMSE += OnRmseReport;
            e.Result = checker.CalculateRMSE();
        }

        private void OnRmseReport(int percent, double rmse)
        {
            Debug.Assert((0 <= percent) && (percent <= 100));
            _worker.ReportProgress(percent, rmse);
        }

        #endregion
    }
}
