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

using System.Diagnostics;
using Varozhka.Processing;
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.UI
{
    /// <summary>
    /// Singleton class to hold all data.
    /// </summary>
    internal class DataManager
    {
        #region Properties

        public NetflixFiles NetflixFiles
        {
            get { return _netflixFiles; }
        }
        NetflixFiles _netflixFiles;

        protected string EstimatorFileName
        {
            get { return _estimatorFileName; }
        }

        private readonly string _estimatorFileName;

        public SparseIdTranslator UserIdTranslator
        {
            get { return _userIdTranslator; }
        }
        SparseIdTranslator _userIdTranslator;

        public LinearIdTranslator MovieIdTranslator
        {
            get { return _movieIdTranslator; }
        }
        LinearIdTranslator _movieIdTranslator;
        
        /// <summary>
        /// Indexed Netflix dataset
        /// </summary>
        protected NetflixData NetflixData
        {
            get
            {
                Debug.Assert(null != _netflixData, "Indexes are not loaded!");
                return _netflixData;
            }
        }
        private NetflixData _netflixData;


        /// <summary>
        /// Instance of estimator. No caching here (Loads a new one on each call).
        /// </summary>
        public IEstimator Estimator
        {
            get
            {
                return EstimatorWrapper.LoadLocally(NetflixData, _estimatorFileName);
            }
        }        

        #endregion

        /// <summary>
        /// Instance of the cruncher.
        /// </summary>
        /// <remarks>No locks here!!! The caller should do all sync operations.</remarks>
        public static DataManager Instance
        {
            get
            {
                Debug.Assert(null != _instance, "DataManager is not initialized.");
                return _instance;
            }
        }
        private static DataManager _instance;
        private static MainForm.SetStatusMessageDelegate _statusMessageDelegate;

        #region Constructor

        private DataManager(NetflixFiles netflixFiles, string estimatorFileName)
        {
            Debug.Assert(null != netflixFiles);
            
            _netflixFiles = netflixFiles;
            _estimatorFileName = estimatorFileName;
            _userIdTranslator = SparseIdTranslator.Load(netflixFiles.UserIdIndex);
            _movieIdTranslator = new LinearIdTranslator(HardCode.FirstMovieId);
        }

        #endregion
        
        /// <summary>
        /// Initialize cruncher
        /// </summary>
        /// <param name="netflixFiles"></param>
        /// <param name="estimatorFileName"></param>
        public static void Init(NetflixFiles netflixFiles, string estimatorFileName,
            NetflixData.PercentageDelegate percentageDelegate, MainForm.SetStatusMessageDelegate statusMessageDelegate)
        {
            _statusMessageDelegate = statusMessageDelegate;
            
            _instance = new DataManager(netflixFiles, estimatorFileName);
            _instance.GetReady(percentageDelegate);
        }

        #region Processing methods
        
        /// <summary>
        /// Load all the stuff required for processing.
        /// </summary>
        private void GetReady(NetflixData.PercentageDelegate percentageDelegate)
        {
            _netflixData = new NetflixData(NetflixFiles);
            _netflixData.Percentage += percentageDelegate;
            _netflixData.StatusChanged += OnLoadingStatusChanged;

            _netflixData.Load();
        }

        // TODO: ER: that's ugly
        private void OnLoadingStatusChanged(NetflixData.LoadingStatus status)
        {
            switch (status)
            {
                case NetflixData.LoadingStatus.Created:
                    break;
                case NetflixData.LoadingStatus.LoadingMoviesIndex:
                    _statusMessageDelegate("Loading movies index");
                    break;
                case NetflixData.LoadingStatus.LoadingCustomersIndex:
                    _statusMessageDelegate("Loading customers index");
                    break;
                case NetflixData.LoadingStatus.Ready:
                    break;
            }
            
        }

        #endregion
    }
}
