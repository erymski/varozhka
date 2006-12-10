using System;
using System.Collections.Generic;

namespace MemoryTest
{
    public class PackedInt : IComparer<int>
    {
        #region Constants

        private const int c_ratingBits = 3;
        private const int c_ratingMask = 0x7;
        private const int c_idMask = ~c_ratingMask;

        #endregion

        #region Static functions

        public static int GetId(int packed)
        {
            return (packed & c_idMask) >> c_ratingBits;
        }
        
        public static byte GetRating(int packed)
        {
            return Convert.ToByte(packed & c_ratingMask);
        }
        
        public static int Pack(int id, int r)
        {
            return (id << c_ratingBits) | r;
        }

        #endregion

        #region IComparer<int> implementation

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        ///    Value                          Condition 
        /// Less than zero                x is less than y.
        /// Zero                            x equals y.
        /// Greater than zero           x is greater than y.
        /// </returns>
        public int Compare(int x, int y)
        {
            return (x >> c_ratingBits) - y;
        }

        #endregion
        
    }
}
