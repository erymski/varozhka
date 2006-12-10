using System;
using System.Collections.Generic;
using System.Diagnostics;
using Varozhka.Processing;
using Varozhka.TrainingData;

namespace SampleEstimator
{
    /// <summary>
    /// Calculates average rating by customer.
    /// </summary>
    public class AverageRatingEstimator : BaseEstimator
    {
        public AverageRatingEstimator(NetflixData netflixData)
            : base(netflixData)
        {
        }
        
        // cache
        private Dictionary<int, float> _mapAverageRating = new Dictionary<int, float>(600000);

        /// <summary>
        /// Estimates moview rating by the specified customer on the specified date.
        /// </summary>
        /// <param name="movieId">The movie id.</param>
        /// <param name="customerId">The customer id.</param>
        /// <param name="date">The view date.</param>
        /// <returns>Rating of the movie</returns>
        public override float GetRating(int movieId, int customerId, DateTime date)
        {
            // check if rating was estimated already 
            if (_mapAverageRating.ContainsKey(customerId))
            {
                return _mapAverageRating[customerId];
            }
            else
            {
                float averageRating = GetAverageRating(customerId, movieId);

                // cache the result
                _mapAverageRating[customerId] = averageRating;
                
                return averageRating;
            }
        }

        /// <summary>
        /// Calculate average rating by the customer
        /// </summary>
        /// <param name="customerId">Customer</param>
        /// <param name="movieId">Movie</param>
        /// <returns>Calculated rating</returns>
        private float GetAverageRating(int customerId, int movieId)
        {
            // get all movies watched by customer
            short[] allMovies = NetflixData.GetMoviesByCustomer(customerId);

            // calculate average rating for the movies
            int count = 0;
            int sumRatings = 0;
            foreach (short movie in allMovies)
            {
                if (movie == movieId) continue; // ignore the movie itself

                sumRatings += NetflixData.GetRating(movie, customerId);
                count++;
            }

            Debug.Assert(count > 0);
            return (float)sumRatings / (float)count;
        }
    }
}
