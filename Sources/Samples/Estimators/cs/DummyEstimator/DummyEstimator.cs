using System;
using Varozhka.Processing;

namespace SampleEstimator
{
    /// <summary>
    /// Plain estimator, which returns constant value for all movies and customers.
    /// </summary>
    class DummyEstimator : IEstimator
    {
        private const float c_constRating = 3.55f;

        public float GetRating(int movieId, int customerId, DateTime date)
        {
            return c_constRating;
        }
    }
}
