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
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    class MovieToUserIndexProcessor : MovieFilesProcessor
    {
        /// <summary>
        /// Gets the customer ID translator.
        /// </summary>
        protected IIdTranslator<int> UserIdTranslator
        {
            get { return _userIdTranslator; }
        }
        private IIdTranslator<int> _userIdTranslator;


        /// <summary>
        /// Gets the movie id translator.
        /// </summary>
        protected IIdTranslator<short> MovieIdTranslator
        {
            get { return _movieIdTranslator; }
        }
        private IIdTranslator<short> _movieIdTranslator;


        private MovieToUserIndex _m2uIndex;


        public MovieToUserIndexProcessor(NetflixFiles netflixFiles, IIdTranslator<short> movieIdTranslator, IIdTranslator<int> userIdTranslator) : base(netflixFiles)
        {
            _userIdTranslator = userIdTranslator;
            _movieIdTranslator = movieIdTranslator;
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        public override void Init()
        {
            _m2uIndex = new MovieToUserIndex(HardCode.MoviesCount);
        }

        public override void StartRatingsFile(short movieId, int fileSize)
        {
            _m2uIndex.CreateRow(MovieIdTranslator.RealToPacked(movieId), fileSize / HardCode.AverageTextLine);
        }

        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        public override void ProcessRating(int customerId, short movieId, byte rating)
        {
            _m2uIndex.AddPair(MovieIdTranslator.RealToPacked(movieId), customerId, rating);
        }

        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        public override void Cleanup()
        {
            SaveM2UIndex(_m2uIndex, NetflixFiles, UserIdTranslator);
            _m2uIndex = null;
        }

        private static void SaveM2UIndex(MovieToUserIndex index, NetflixFiles netflixFiles, IIdTranslator<int> userIdTranslator)
        {
            string indexFileName = netflixFiles.MoviesToUsersIndex;

            MemoryStorage<int, Int32Streamer> moviesToUsers = new MemoryStorage<int, Int32Streamer>(index.Size);
            for (int i = 0; i < index.Bucket.Length; i++) 
            {
                // map customer IDs
                int[] data = index.Bucket[i].ToArray();
                index.Bucket[i] = null; // free up some memory
                for (int j = 0; j < data.Length; j++)
                {
                    int packed = data[j];

                    // unpack data
                    int customerId = PackedInt.GetCustomerId(packed);
                    byte rating = PackedInt.GetRating(packed);
                    Debug.Assert((rating > 0) && (rating < 6));

                    // remap customer ID
                    int mappedCustomerId = userIdTranslator.RealToPacked(customerId);

                    data[j] = PackedInt.Pack(mappedCustomerId, rating);
                }
                moviesToUsers.SetSlot(i, data);
            }

            moviesToUsers.Save(indexFileName);
        }

        /// <summary>
        /// Processing description
        /// </summary>
        public override string Description
        {
            get { return "Generating movies index"; }
        }
    }
}
