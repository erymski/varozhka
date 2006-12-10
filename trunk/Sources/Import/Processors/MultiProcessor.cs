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

namespace Varozhka.Import
{
    /// <summary>
    /// Run several processors (synchronously)
    /// </summary>
    class MultiProcessor : IMovieFilesProcessor
    {
        private List<IMovieFilesProcessor> _processors = new List<IMovieFilesProcessor>(5);

        public void AddProcessor(IMovieFilesProcessor processor)
        {
            _processors.Add(processor);
        }

        
        #region IMovieFilesProcessor implementation

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        public void Init()
        {
            // initialize all processors
            _processors.ForEach(delegate(IMovieFilesProcessor processor) { processor.Init(); });
        }

        /// <summary>
        /// Ratings file for the movie is going to be processed.
        /// </summary>
        /// <param name="movieId">The movie id.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <remarks>For some processings it's useful to estimate size of the resulting set.</remarks>
        public void StartRatingsFile(short movieId, int fileSize)
        {
            _processors.ForEach(delegate(IMovieFilesProcessor processor)
                                   {
                                       processor.StartRatingsFile(movieId, fileSize);
                                   });
        }

        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        public void ProcessRating(int customerId, short movieId, byte rating)
        {
            _processors.ForEach(delegate(IMovieFilesProcessor processor)
                                   {
                                       processor.ProcessRating(customerId, movieId, rating);
                                   });
        }

        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        public void Cleanup()
        {
            _processors.ForEach(delegate(IMovieFilesProcessor processor) { processor.Cleanup(); });
        }

        public string Description
        {
            get { return "Run multiple processors"; }
        }

        #endregion
    }
}
