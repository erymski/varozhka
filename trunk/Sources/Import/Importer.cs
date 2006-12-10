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
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    class Importer
    {
        #region Constants

        private readonly char[] c_trailMovieId = ":".ToCharArray();
        private static readonly char[] c_sepComma = ",".ToCharArray();
        private const int c_bufferSize = 1024 * 1024; // TODO: need to measure what is the best value (cluster size?)

        #endregion
        
        #region Properties

        protected NetflixFiles NetflixFiles
        {
            get { return _netflixFiles; }
        }
        private NetflixFiles _netflixFiles;
        

        /// <summary>
        /// Gets names of the movies files.
        /// </summary>
        /// <value>The movies files.</value>
        private string[] MoviesFiles
        {
            get
            {
                if (null == _filesMovies)
                {
                    _filesMovies = Directory.GetFiles(NetflixFiles.DatasetDirectory,
                                                      NetflixFiles.MoviesMask, 
                                                      SearchOption.AllDirectories);
                }
                return _filesMovies;
            }
        }
        string[] _filesMovies;
        
        private BackgroundWorker _worker;
        private DoWorkEventArgs _event;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Importer"/> class.
        /// </summary>
        /// <param name="datasetDir">Directory with NetFlix dataset.</param>
        public Importer(string datasetDir, BackgroundWorker worker, DoWorkEventArgs e)
        {
            _worker = worker;
            _netflixFiles = new NetflixFiles(datasetDir);
            _event = e;
        }


        #endregion

        /// <summary>
        /// Starts the import process.
        /// </summary>
        public void StartImport()
        {
            // TODO: most of these processing can be done in one pass... if you have enough memory

            try
            {
                LinearIdTranslator movieIdTranslator = new LinearIdTranslator(HardCode.FirstMovieId);
            
                ProbeSetProcessor probeSetProcessor = new ProbeSetProcessor(NetflixFiles, movieIdTranslator);
                ProcessTrainingSet(probeSetProcessor);
                SparseIdTranslator userIdTranslator = probeSetProcessor.UserIdTranslator;
                probeSetProcessor = null;

                UserToMovieIndexProcessor u2mProcessor = new UserToMovieIndexProcessor(NetflixFiles, movieIdTranslator, userIdTranslator);
                ProcessTrainingSet(u2mProcessor);
                u2mProcessor = null;
            
                MovieToUserIndexProcessor m2uProcessor = new MovieToUserIndexProcessor(NetflixFiles, movieIdTranslator, userIdTranslator);
                ProcessTrainingSet(m2uProcessor);
                m2uProcessor = null;
            }
            catch (ImportException e)
            {
                _event.Cancel = true;
            }
        }

        #region Private functions
        
        private void ProcessTrainingSet(IMovieFilesProcessor processor)
        {
            // initialize processor
            _worker.ReportProgress(0, processor.Description);
            processor.Init();
            
            // process files with movie info
            string[] files = MoviesFiles;
            
            int total = files.Length;
            int gap = total%200;
            int currPercent = 0;
            
            for (int i = 0; i < files.Length; i++)
            {
                ProcessMovieFile(files[i], processor);
                
                // stop processing
                if (_worker.CancellationPending)
                {
                    throw new ImportException();
                }
                
                // progress reporting
                if (0 == (i % gap))
                {
                    int percent = 100 * i / total;
                    if (percent != currPercent)
                    {
                        currPercent = percent;
                        _worker.ReportProgress(currPercent);
                    }
                }
            }
            files = null;
            
            // flush results
            _worker.ReportProgress(100, "Flushing processed data...");
            processor.Cleanup(); // TODO: add stop checking here
        }



        /// <summary>
        /// Processes the movie file. (duplicated code)
        /// </summary>
        /// <param name="fileName">Name of the file with movies.</param>
        /// <param name="processor">The processor.</param>
        private void ProcessMovieFile(string fileName, IMovieFilesProcessor processor)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, c_bufferSize, FileOptions.SequentialScan))
            {
                StreamReader reader = new StreamReader(stream);
                string line = reader.ReadLine(); // read id of the movie
                Debug.Assert(null != line);

                short movieId = short.Parse(line.TrimEnd(c_trailMovieId), HardCode.Culture);
                processor.StartRatingsFile(movieId, Convert.ToInt32(new FileInfo(fileName).Length));

                // process users and ratings
                while (null != (line = reader.ReadLine()))
                {
                    string[] data = line.Split(c_sepComma, 3);

                    int customerId = int.Parse(data[0], HardCode.Culture);
                    byte rating = byte.Parse(data[1], HardCode.Culture);
                    
                    // the date is ignored for now

                    processor.ProcessRating(customerId, movieId, rating);
                }
            }
        }

        #endregion
    }
}
