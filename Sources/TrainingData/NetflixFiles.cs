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
using System.IO;

namespace Varozhka.TrainingData
{
    /// <summary>
    /// Netflix filenames storage
    /// </summary>
    public class NetflixFiles
    {
        #region Constants

        private const string c_userToMovieFilename = "user2movies.dat";
        private const string c_movieToUserRatingsFilename = "movies2ratings.dat";
        private const string c_customerIdIndexFileName = "mapping.dat";
        private const string c_trainingSetDirName = "training_set";
        private const string c_moviesFileName = "movie_titles.txt";
        private const string c_moviesMask = "mv_*.txt";
        private const string c_probeSetFileName = "probe.txt";
        private const string c_probeSetExtFileName = "probe_ext.txt";
        private const string c_fileQualifyingSet = "qualifying.txt";
        private const string c_normalizedProbeSet = "norm_probe.dat";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dataset directory.
        /// </summary>
        /// <value>The dataset directory.</value>
        public string DatasetDirectory
        {
            get { return _datasetDir; }
        }
        private string _datasetDir;

        public string UserIdIndex
        {
            get { return Path.Combine(DatasetDirectory, c_customerIdIndexFileName); }
        }

        public string MoviesToUsersIndex
        {
            get { return Path.Combine(DatasetDirectory, c_movieToUserRatingsFilename); }
        }

        public string UsersToMoviesIndex
        {
            get { return Path.Combine(DatasetDirectory, c_userToMovieFilename); }
        }
        /// <summary>
        /// Gets the file name with movies titles.
        /// </summary>
        /// <value>The movies file.</value>
        protected string TitlesFile
        {
            get
            {
                return Path.Combine(DatasetDirectory, c_moviesFileName);
            }
        }
        
        /// <summary>
        /// Get file mask for movies files.
        /// </summary>
        public static string MoviesMask
        {
            get { return c_moviesMask; }
        }

        /// <summary>
        /// Gets the name of the file with the probe set.
        /// </summary>
        /// <value>The probe set file.</value>
        public string ProbeSetFile
        {
            get { return Path.Combine(DatasetDirectory, c_probeSetFileName); }
        }
        
        /// <summary>
        /// Gets the name of the file with the probe set.
        /// </summary>
        /// <value>The probe set file.</value>
        public string ProbeSetExtFile
        {
            get { return Path.Combine(DatasetDirectory, c_probeSetExtFileName); }
        }
        
        /// <summary>
        /// Gets the training set directory.
        /// </summary>
        /// <value>The training set directory.</value>
        protected string TrainingSetDirectory
        {
            get
            {
                return Path.Combine(DatasetDirectory, c_trainingSetDirName);
            }
        }
        
        public string QualifyingSet
        {
            get
            {
                return Path.Combine(DatasetDirectory, c_fileQualifyingSet);
            }
        }

        /// <summary>
        /// Normalized probe set in binary format.
        /// </summary>
        public string NormalizedProbeSet
        {
            get
            {
                return Path.Combine(DatasetDirectory, c_normalizedProbeSet);
            }
        }

        #endregion

        #region Constructor

        public NetflixFiles(string datasetDir)
        {
            if (string.IsNullOrEmpty(datasetDir))
                throw new ArgumentNullException("datasetDir");
            
            _datasetDir = datasetDir;
        }

        #endregion
    }
}
