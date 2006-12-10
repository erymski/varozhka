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

namespace Varozhka.Processing
{
    /// <summary>
    /// Class to work with preprocessed probe dataset (in binary format).
    /// </summary>
    public class RmseChecker
    {
        public delegate void RmseDelegate(int percent, double rmse);
        
        public event RmseDelegate RMSE;
        
        #region Properties

        protected string FileName
        {
            get { return _fileName; }
        }
        private string _fileName;

        protected IEstimator Estimator
        {
            get { return _estimator; }
        }
        private IEstimator _estimator;
        
        /// <summary>
        /// Current RMSE value
        /// </summary>
        public double Rmse
        {
            get
            {
                return _rmse;
            }
        }
        private double _rmse;
        
        private readonly int _iterations;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="estimator"></param>
        /// <param name="iterations">Iterations between updates.</param>
        public RmseChecker(string fileName, IEstimator estimator, int iterations)
        {
            Debug.Assert(null != estimator);
            Debug.Assert(iterations > 0);
            _estimator = estimator;
            _fileName = fileName;
            _iterations = iterations;
        }

        /// <summary>
        /// Starts the processing.
        /// </summary>
        public double CalculateRMSE()
        {
            _rmse = 0;
            
            // order of movies and customer is not important
            float totalSquaredDelta = 0f;
            int totalRatings = 0;

            byte[] plainData = File.ReadAllBytes(FileName);
            MemoryStream stream = new MemoryStream(plainData);
            //using (FileStream stream = new FileStream(FileName, FileMode.Open,
            //                                          FileAccess.Read, FileShare.Read))
            {
                BinaryReader reader = new BinaryReader(stream);
                int moviesCount = reader.ReadInt32();

                for (int movieId = 0; movieId < moviesCount; movieId++)
                {
                    int ratingsCount = reader.ReadInt32();
                    for (int j = 0; j < ratingsCount; j++)
                    {
                        int userId = reader.ReadInt32();
                        byte realRating = reader.ReadByte();

                        float estimatedRating = Estimator.GetRating(movieId, userId, DateTime.MinValue);

                        // accumulate data
                        float delta = realRating - estimatedRating;
                        totalSquaredDelta += delta * delta;
                        totalRatings++;
                    }
                        
                    // TODO: get rid of event, and use a timer to check current RMSE
                    if (0 == (movieId % _iterations))
                    {
                        _rmse = Math.Sqrt(totalSquaredDelta / totalRatings);
                        RMSE(100 * movieId / moviesCount, _rmse);
                    }
                }
            }

            _rmse = Math.Sqrt(totalSquaredDelta/totalRatings);
            RMSE(100, _rmse);
            return _rmse;
        }
    }
}
