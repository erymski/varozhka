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
using Varozhka.TrainingData;

namespace Varozhka.Import
{
    internal class UserToMovieIndex
    {
        public Dictionary<int, List<short>> _map;

        public UserToMovieIndex()
        {
            _map = new Dictionary<int, List<short>>(600000);
        }

        public void Init()
        {

        }

        public void AddPair(int userId, short movieId)
        {
            List<short> movies;
            if (!_map.TryGetValue(userId, out movies))
            {
                movies = new List<short>(300);
                _map[userId] = movies;
            }

            movies.Add(movieId);
        }
    }
    
    internal class MovieToUserIndex
    {
        public List<int>[] Bucket
        {
            get
            {
                return _bucket;
            }
        }
        private readonly List<int>[] _bucket;
        
        /// <summary>
        /// Size of the index
        /// </summary>
        public int Size
        {
            get { Debug.Assert(null != _bucket); return _bucket.Length;  }
        }
        
        public MovieToUserIndex(int size)
        {
            _bucket = new List<int>[size];
        }

        public void CreateRow(short movieId, int estimatedSize)
        {
            Debug.Assert(null == _bucket[movieId]);
            _bucket[movieId] = new List<int>(estimatedSize);
        }

        public void AddPair(short movieId, int customerId, byte rating)
        {
            List<int> row = _bucket[movieId];
            Debug.Assert(null != row);

            int packed = PackedInt.Pack(customerId, rating);
            row.Add(packed);
        }
        
    }
}
