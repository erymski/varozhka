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
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Varozhka.Processing;
using Varozhka.Storage;

namespace Varozhka.UI
{
    /// <summary>
    /// Generator of a prediction set to be submitted.
    /// </summary>
    class PredictionGenerator : IProcessor
    {
        private int _currMovieId;
        private int _currCount;
        
        public delegate void ProcessedMoviesDelegate(int moviesCount);
        public event ProcessedMoviesDelegate ProcessedMovies;
        
        #region Constructor

        public PredictionGenerator(string outputFilename, IEstimator estimator, 
                                   LinearIdTranslator movieIdTranslator, SparseIdTranslator userIdTranslator)
        {
            if (null == estimator)
                throw new ArgumentNullException("estimator");
            
            _filename = outputFilename;
            _estimator = estimator;
            _movieIdTranslator = movieIdTranslator;
            _userIdTranslator = userIdTranslator;
        }

        #endregion

        #region Properties

        protected string FileName
        {
            get { return _filename; }
        }
        private string _filename;

        public IEstimator Estimator
        {
            get { return _estimator; }
        }
        private readonly IEstimator _estimator;

        protected TextWriter Writer
        {
            get { return _writer; }
        }
        private TextWriter _writer;

        private LinearIdTranslator MovieIdTranslator
        {
            get { return _movieIdTranslator; }
        }
        LinearIdTranslator _movieIdTranslator;

        private SparseIdTranslator UserIdTranslator
        {
            get { return _userIdTranslator; }
        }
        private SparseIdTranslator _userIdTranslator;

        #endregion

        #region Overrides for IProcessor

        void IProcessor.OnMovie(int movieId)
        {
            _currMovieId = MovieIdTranslator.PackedToReal((short)movieId);// TODO: check if casting can be removed
            Writer.Write(movieId); 
            Writer.WriteLine(':');

            _currCount++;
            ProcessedMovies(_currCount);
        }

        void IProcessor.OnView(int customerId, DateTime date)
        {
            customerId = UserIdTranslator.RealToPacked(customerId);
            float rating = Estimator.GetRating(_currMovieId, customerId, date);
            Writer.WriteLine(rating.ToString("F2", HardCode.Culture));
        }

        void IProcessor.Init()
        {
            _writer = new StreamWriter(FileName);
            _currCount = 0;
        }

        void IProcessor.Complete()
        {
            Debug.Assert(null != Writer);
            Writer.Flush();
            Writer.Close();

            //string md5string = CalculateMD5(FileName);
            //File.WriteAllText(Path.Combine(Path.GetDirectoryName(FileName), "md5.txt"), md5string);
        }

        #endregion

        #region Utility functions

        /// <summary>
        /// Calculates the MD5 hash for the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>String with MD5 hash</returns>
        public static string CalculateMD5(string fileName)
        {
            string res = string.Empty;
            
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] hash;
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    hash = md5.ComputeHash(stream);
                }

                // ER: maybe it can be done in a better way...
                StringBuilder builder = new StringBuilder(50);
                Array.ForEach(hash, delegate(byte digit)
                                        {
                                            builder.Append(digit.ToString("x2"));

                                        });

                res = builder.ToString();
            }
            
            return res;
        }

        #endregion
    }
}
