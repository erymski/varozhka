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
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using Varozhka.Processing;
using Varozhka.Storage;

namespace Varozhka.UI.Tasks
{
    /// <summary>
    /// Task to generate prediction set.
    /// </summary>
    class GeneratePredictionSetTask : ILongTask
    {
        #region Private variables

        private BackgroundWorker _worker;
        private int _lastPercentValue;

        #endregion

        #region Properties

        internal string OutputFileName
        {
            get { return _outputFileName; }
        }
        private string _outputFileName;

        #endregion
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="outputFileName">File name of the resulting prediction set</param>
        public GeneratePredictionSetTask(string outputFileName)
        {
            _outputFileName = outputFileName;
        }

        /// <summary>
        /// Status message to show during processing
        /// </summary>
        public string ProcessingStatusMessage
        {
            get { return "Generating prediction set..."; }
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
            get { return "Prediction set generation aborted"; }
        }

        /// <summary>
        /// Run the processing.
        /// </summary>
        public void Run(BackgroundWorker worker, DoWorkEventArgs e)
        {
            _worker = worker;
            
            PredictionGenerator processor = 
                new PredictionGenerator(_outputFileName,
                                        DataManager.Instance.Estimator,
                                        DataManager.Instance.MovieIdTranslator,
                                        DataManager.Instance.UserIdTranslator);

            processor.ProcessedMovies += OnProcessedMovie;

            Engine engine = new Engine(DataManager.Instance.NetflixFiles.QualifyingSet, processor);
            engine.Start();

            PackEstimationSet(_outputFileName);
            File.Delete(_outputFileName);
        }

        private void OnProcessedMovie(int moviesCount)
        {
            int percent = 100 * moviesCount / HardCode.MoviesCount;
            if (percent != _lastPercentValue)
            {
                _worker.ReportProgress(percent);
                _lastPercentValue = percent;
            }
        }

        #region Compression

        /// <summary>
        /// Compress the estimation set with GZip and calculate MD5 hash.
        /// </summary>
        /// <param name="outputFilename">The output filename.</param>
        private static void PackEstimationSet(string outputFilename)
        {
            string gzFile = Compress(outputFilename);

            if (File.Exists(gzFile))
            {
                string md5 = PredictionGenerator.CalculateMD5(gzFile);

                string md5File = Path.Combine(Path.GetDirectoryName(outputFilename), "md5.txt");
                File.WriteAllText(md5File, md5);
            }
        }

        /// <summary>
        /// Compresses the file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Name of the compressed file.</returns>
        private static string Compress(string fileName)
        {
            const string c_gzipExtension = ".gz";

            string compressedFileName = fileName + c_gzipExtension;
            using (GZipOutputStream streamGZip = new GZipOutputStream(File.Create(compressedFileName)))
            {
                streamGZip.SetLevel(9);
                using (FileStream inputFile = File.OpenRead(fileName))
                {
                    byte[] data = new byte[inputFile.Length];
                    inputFile.Read(data, 0, data.Length);

                    streamGZip.Write(data, 0, data.Length);
                }
            }

            return compressedFileName;
        }

        #endregion
        
    }
}
