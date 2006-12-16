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
using Varozhka.Storage;

namespace Varozhka.TrainingData
{
    #region Streamers

    /// <summary>
    /// Streamer for Int16
    /// </summary>
    public sealed class Int16Streamer : IStreamer<Int16>
    {
        public void Write(short value, BinaryWriter writer)
        {
            writer.Write(value);
        }

        public short Read(BinaryReader reader)
        {
            return reader.ReadInt16();
        }

        public short[] ReadBlock(BinaryReader reader, int length)
        {
            byte[] bytes = reader.ReadBytes(length * sizeof(short));
            
            short[] shorts = new short[length];
            Buffer.BlockCopy(bytes, 0, shorts, 0, length * sizeof(short));
            
            return shorts;
        }
    }
    
    /// <summary>
    /// Streamer for Int32
    /// </summary>
    public sealed class Int32Streamer : IStreamer<Int32>
    {
        public void Write(int value, BinaryWriter writer)
        {
            writer.Write(value);
        }

        public int Read(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public int[] ReadBlock(BinaryReader reader, int length)
        {
            byte[] bytes = reader.ReadBytes(length * sizeof(int));

            int[] ints = new int[length];
            Buffer.BlockCopy(bytes, 0, ints, 0, length * sizeof(int));

            return ints;
        }
    }

    #endregion
    
    /// <summary>
    /// Class to access indexed Netflix dataset.
    /// </summary>
    public class NetflixData
    {
        /// <summary>
        /// Quick and dirty implementation of loading progress.
        /// </summary>
        public enum LoadingStatus
        {
            Created,
            LoadingMoviesIndex,
            LoadingCustomersIndex,
            Ready
        }
        
        public delegate void PercentageDelegate(int percent);
        public delegate void StatusChangedDelegate(LoadingStatus status);

        /// <summary>
        /// Raised on percentage change
        /// </summary>
        public event PercentageDelegate Percentage;
        
        /// <summary>
        /// Raised if status of the index was changed.
        /// </summary>
        public event StatusChangedDelegate StatusChanged;
        
        #region Private variables

        private MemoryStorage<short, Int16Streamer> _userToMovies;
        private MemoryStorage<int, Int32Streamer> _movieToUserRating;

        #endregion

        public NetflixFiles NetflixFiles
        {
            get { return _netflixFiles; }
        }
        private NetflixFiles _netflixFiles;

        /// <summary>
        /// Current status of the index set.
        /// </summary>
        public LoadingStatus DataStatus
        {
            get { return _status; }
        }
        private LoadingStatus _status = LoadingStatus.Created;

        // progress data
        private int _total;
        private int _currPercent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NetflixData"/> class.
        /// </summary>
        /// <param name="netflixFiles">Information about Netflix files with the dataset</param>
        public NetflixData(NetflixFiles netflixFiles)
        {
            _netflixFiles = netflixFiles;
        }

        /// <summary>
        /// Set status of the index set.
        /// </summary>
        /// <param name="status">New status</param>
        private void SetStatus(LoadingStatus status)
        {
            if (status != _status)
            {
                _status = status;
                StatusChanged(status);
            }
        }

        /// <summary>
        /// Load indexes.
        /// </summary>
        public void Load()
        {
            SetStatus(LoadingStatus.LoadingCustomersIndex);
            _userToMovies = MemoryStorage<Int16, Int16Streamer>.Load(NetflixFiles.UsersToMoviesIndex, OnTotal, OnCount);

            SetStatus(LoadingStatus.LoadingMoviesIndex);
            _movieToUserRating = MemoryStorage<Int32, Int32Streamer>.Load(NetflixFiles.MoviesToUsersIndex, OnTotal, OnCount);
            _movieToUserRating.Comparer = new PackedInt();
            
            SetStatus(LoadingStatus.Ready);
        }

        /// <summary>
        /// Get movies rated by the customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>Array of movies rated by the customer.</returns>
        public short[] GetMoviesByCustomer(int customerId)
        {
            return _userToMovies[customerId];
        }
        
        /// <summary>
        /// Get customers watched the movie.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <returns>Array of customers</returns>
        /// <remarks>
        /// In the current implementation it's better 
        /// to use GetPacksByMovie() function.
        /// </remarks>
        public int[] GetCustomersByMovie(int movieId)
        {
            int[] data = _movieToUserRating[movieId].Clone() as int[];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = PackedInt.GetCustomerId(data[i]);
            }

            return data;
        }

        /// <summary>
        /// Get customer/rating packs for the given movie
        /// </summary>
        /// <param name="movieId">ID of the movie</param>
        /// <returns>Array of packs</returns>
        public int[] GetPacksByMovie(int movieId)
        {
            return _movieToUserRating[movieId];
        }
        
        /// <summary>
        /// How the customer rated the movie.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="customerId">ID of the customer.</param>
        /// <returns>Rating.</returns>
        public byte GetRating(int movieId, int customerId)
        {
            int packed = _movieToUserRating.GetValue(movieId, customerId);

            return PackedInt.GetRating(packed);
        }

        private void OnCount(int current)
        {
            int percent = 100 * current / _total;
            if (percent != _currPercent)
            {
                Percentage(percent);
                _currPercent = percent;
            }
        }

        private void OnTotal(int total)
        {
            _currPercent = 0;
            _total = total;
        }

        
    }
}
