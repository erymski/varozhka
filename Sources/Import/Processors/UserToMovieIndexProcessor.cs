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
using System.Diagnostics;
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    internal class UserToMovieIndexProcessor : MovieFilesProcessor
    {
        private UserToMovieIndex _u2mIndex;

        /// <summary>
        /// Translator for customer IDs.
        /// </summary>
        /// <remarks>Cannot be used until processing complete.</remarks>
        protected SparseIdTranslator UserIdTranslator
        {
            get 
            {
                Debug.Assert(null != _userIdTranslator); 
                return _userIdTranslator; 
            }
        }
        private SparseIdTranslator _userIdTranslator;

        /// <summary>
        /// Gets the movie id translator.
        /// </summary>
        protected IIdTranslator<short> MovieIdTranslator
        {
            get { return _movieIdTranslator; }
        }
        private IIdTranslator<short> _movieIdTranslator;


        public UserToMovieIndexProcessor(NetflixFiles netflixFiles, IIdTranslator<short> movieIdTranslator, 
                                         SparseIdTranslator userIdTranslator) : base(netflixFiles)
        {
            _movieIdTranslator = movieIdTranslator;
            _userIdTranslator = userIdTranslator;
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        public override void Init()
        {
            _u2mIndex = new UserToMovieIndex();
        }

        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        public override void ProcessRating(int customerId, short movieId, byte rating)
        {
            _u2mIndex.AddPair(customerId, MovieIdTranslator.RealToPacked(movieId));
        }

        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        public override void Cleanup()
        {
            // generate ID mapper
            //CreateUserIdTranslator();
            
            // save remapped index
            SaveU2MIndex(NetflixFiles, _u2mIndex, _userIdTranslator);
            
            _u2mIndex = null;
        }

        ///// <summary>
        ///// Creates translator from real (sparse) customer ID to "flat" customer ID.
        ///// </summary>
        //private void CreateUserIdTranslator()
        //{
        //    // remap ids
        //    int[] IDs = new int[_u2mIndex._map.Count];
        //    _u2mIndex._map.Keys.CopyTo(IDs, 0);
        //    _userIdTranslator = new SparseIdTranslator(IDs);

        //    _userIdTranslator.Save(NetflixFiles.UserIdIndex);
        //}


        private static void SaveU2MIndex(NetflixFiles netflixFiles, UserToMovieIndex index, IIdTranslator<int> userIdTranslator)
        {
            int usersCount = index._map.Count;

            MemoryStorage<short, Int16Streamer> userToMovies = new MemoryStorage<short, Int16Streamer>(usersCount);
            foreach (KeyValuePair<int, List<short>> pair in index._map)
            {
                int customerId = pair.Key;
                int packedCustomerId = userIdTranslator.RealToPacked(customerId);
                short[] movies = pair.Value.ToArray();

                userToMovies.SetSlot(packedCustomerId, movies);
            }

            userToMovies.Save(netflixFiles.UsersToMoviesIndex);
        }

        /// <summary>
        /// Processing description
        /// </summary>
        public override string Description
        {
            get { return "Generating customers index"; }
        }
    }
}
