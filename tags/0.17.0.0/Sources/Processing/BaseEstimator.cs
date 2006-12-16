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
using Varozhka.TrainingData;

namespace Varozhka.Processing
{
    /// <summary>
    /// Base class for estimators.
    /// Provides access to Netflix dataset.
    /// </summary>
    public abstract class BaseEstimator : IEstimator
    {
        #region Netflix indexes

        /// <summary>
        /// Indexed Netflix data
        /// </summary>
        protected NetflixData NetflixData
        {
            get { return _netflixData; }
        }
        private NetflixData _netflixData;

        #endregion

        #region Constructor

        public BaseEstimator(NetflixData netflixData)
        {
            _netflixData = netflixData;
        }

        #endregion
        
        /// <summary>
        /// Estimates moview rating by the specified customer on the specified date.
        /// </summary>
        /// <param name="movieId">The movie id.</param>
        /// <param name="customerId">The customer id.</param>
        /// <param name="date">The view date.</param>
        /// <returns>Rating of the movie</returns>
        public abstract float GetRating(int movieId, int customerId, DateTime date);
    }
}
