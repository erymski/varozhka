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

namespace Varozhka.Import
{
    /// <summary>
    /// Storage of ratings for movie-customer pairs.
    /// </summary>
    /// <remarks>It's important to preprocess movies ID with translator!</remarks>
    class MovieUserPairs
    {
        // user->rating map
        public Dictionary<int, byte>[] Movies
        {
            get { return _movies; }
        }
        private Dictionary<int, byte>[] _movies;

        public MovieUserPairs(int moviesCount)
        {
            Debug.Assert(moviesCount > 0);
            _movies = new Dictionary<int, byte>[moviesCount];
        }

        public void AddPair(short movieId, int customerId)
        {
            if (null == _movies[movieId])
            {
                _movies[movieId] = new Dictionary<int, byte>(19);
            }
            
            _movies[movieId].Add(customerId, 0xff);
        }

        public bool HasPair(short movieId, int customerId)
        {
            return (null != _movies[movieId]) &&
                   _movies[movieId].ContainsKey(customerId);
        }

        public byte GetRating(short movieId, int customerId)
        {
            return _movies[movieId][customerId];
        }

        public void SetRating(short movieId, int customerId, byte rating)
        {
            _movies[movieId][customerId] = rating;
        }
    }
}
