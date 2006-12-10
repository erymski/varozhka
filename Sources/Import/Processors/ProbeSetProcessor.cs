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

using System.Collections.Generic;
using System.IO;
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    /// <summary>
    /// Generate extended probe set (original probe set + ratings).
    /// That will simplify RMSE checking for the probe set.
    /// </summary>
    /// <remarks>
    /// It's pretty straightforward:
    /// * find all movie-customer pairs from the probe set
    /// * run through training set and collect ratings for pairs from the probe set
    /// * write extended probe set with ratings
    /// </remarks>
    internal class ProbeSetProcessor : MovieFilesProcessor
    {
        private MovieUserPairs _pairs;

        private Dictionary<int, byte> _userIds = new Dictionary<int, byte>(650000);

        public LinearIdTranslator MovieIdTranslator
        {
            get { return _movieIdTranslator; }
        }
        private LinearIdTranslator _movieIdTranslator;

        public SparseIdTranslator UserIdTranslator
        {
            get { return _userIdTranslator; }
        }
        private SparseIdTranslator _userIdTranslator;

        public ProbeSetProcessor(NetflixFiles netflixFiles, LinearIdTranslator movieIdTranslator)
            : base(netflixFiles)
        {
            _movieIdTranslator = movieIdTranslator;
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        public override void Init()
        {
            short movieId = -1;
            int userId = -1;

            _pairs = new MovieUserPairs(HardCode.MoviesCount);

            string[] lines = File.ReadAllLines(NetflixFiles.ProbeSetFile);
            foreach (string line in lines)
            {
                if (line.EndsWith(":")) // movie
                {
                    movieId = short.Parse(line.Substring(0, line.Length - 1), HardCode.Culture);
                    movieId = MovieIdTranslator.RealToPacked(movieId);
                }
                else
                {
                    userId = int.Parse(line, HardCode.Culture);
                    _pairs.AddPair(movieId, userId);
                }
            }
        }

        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        public override void ProcessRating(int customerId, short movieId, byte rating)
        {
            _userIds[customerId] = 0;

            movieId = MovieIdTranslator.RealToPacked(movieId);
            if (_pairs.HasPair(movieId, customerId))
            {
                _pairs.SetRating(movieId, customerId, rating);
            }
        }

        /// <summary>
        /// Creates translator from real (sparse) customer ID to "flat" customer ID.
        /// </summary>
        private void CreateUserIdTranslator()
        {
            // remap ids
            int[] IDs = new int[_userIds.Count];
            _userIds.Keys.CopyTo(IDs, 0);
            _userIds = null; // free up memory
            
            _userIdTranslator = new SparseIdTranslator(IDs);
            _userIdTranslator.Save(NetflixFiles.UserIdIndex);
        }
        
        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        public override void Cleanup()
        {
            CreateUserIdTranslator();
            
            // order of movies and customer is not important
            using (FileStream stream = new FileStream(NetflixFiles.NormalizedProbeSet, FileMode.Create, 
                                                      FileAccess.Write, FileShare.None))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(_pairs.Movies.Length);
                
                for (int i = 0; i < _pairs.Movies.Length; i++)
                {
                    Dictionary<int, byte> customerAndRatings = _pairs.Movies[i];
                    if (null == customerAndRatings)
                    {
                        writer.Write(0);
                        continue;
                    }
                    else
                    {
                        writer.Write(customerAndRatings.Count);
                    }

                    foreach (KeyValuePair<int, byte> pair in customerAndRatings)
                    {
                        int userId = _userIdTranslator.RealToPacked(pair.Key);
                        writer.Write(userId);
                        
                        writer.Write(pair.Value);
                    }
                }
            }
            
            //int movieId = -1;
            //int customerId = -1;
            
            //// write ratings.. with some overhead
            //using (TextWriter writer = new StreamWriter(NetflixFiles.ProbeSetExtFile))
            //{
            //    foreach (string line in lines)
            //    {
            //        writer.Write(line);
            //        if (line.EndsWith(":")) // it's a movie
            //        {
            //            writer.WriteLine();
            //            movieId = int.Parse(line.Substring(0, line.Length - 1), HardCode.Culture);
            //        }
            //        else
            //        {
            //            customerId = int.Parse(line, HardCode.Culture);

            //            writer.Write(',');
            //            writer.WriteLine(_pairs.GetRating(movieId, customerId));
            //        }
            //    }
            //    writer.Flush();
            //}
            
            _pairs = null;
        }

        /// <summary>
        /// Processing description
        /// </summary>
        public override string Description
        {
            get { return "Processing probe set"; }
        }
    }
}
