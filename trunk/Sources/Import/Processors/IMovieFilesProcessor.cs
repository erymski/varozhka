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

using Varozhka.TrainingData;

namespace Varozhka.Import
{
    /// <summary>
    /// Interface to process movies ratings files
    /// </summary>
    internal interface IMovieFilesProcessor
    {
        /// <summary>
        /// Initialize the processor.
        /// </summary>
        void Init();

        /// <summary>
        /// Ratings file for the movie is going to be processed.
        /// </summary>
        /// <param name="movieId">The movie id.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <remarks>For some processings it's useful to estimate size of the resulting set.</remarks>
        void StartRatingsFile(short movieId, int fileSize);

        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        void ProcessRating(int customerId, short movieId, byte rating);

        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Processing description
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Wrapper for IMovieFilesProcessor.
    /// </summary>
    internal abstract class MovieFilesProcessor : IMovieFilesProcessor
    {
        /// <summary>
        /// Gets the netflix files definition.
        /// </summary>
        /// <value>The netflix files.</value>
        protected NetflixFiles NetflixFiles
        {
            get { return _netflixFiles; }
        }
        private NetflixFiles _netflixFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieFilesProcessor"/> class.
        /// </summary>
        /// <param name="netflixFiles">The netflix files definition.</param>
        public MovieFilesProcessor(NetflixFiles netflixFiles)
        {
            _netflixFiles = netflixFiles;
        }
        
        /// <summary>
        /// Initialize the processor.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Ratings file for the movie is going to be processed.
        /// </summary>
        /// <param name="movieId">The movie id.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <remarks>For some processings it's useful to estimate size of the resulting set.</remarks>
        public virtual void StartRatingsFile(short movieId, int fileSize)
        {
            // ignore
        }
        
        /// <summary>
        /// Process raiting for movie/customer pair.
        /// </summary>
        /// <param name="customerId">ID of the customer.</param>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="rating">Rating.</param>
        public abstract void ProcessRating(int customerId, short movieId, byte rating);
        
        /// <summary>
        /// Cleanup/flush on the end of processing
        /// </summary>
        public abstract void Cleanup();

        /// <summary>
        /// Processing description
        /// </summary>
        public abstract string Description { get; }
    }
}