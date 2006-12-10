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
using System.Collections.Generic;

namespace Varozhka.TrainingData
{
    public class PackedInt : IComparer<int>
    {
        #region Constants

        private const int c_ratingBits = 3;
        private const int c_ratingMask = 0x7;
        private const int c_idMask = ~c_ratingMask;

        #endregion

        #region Static functions

        public static int GetCustomerId(int packed)
        {
            return (packed & c_idMask) >> c_ratingBits;
        }
        
        public static byte GetRating(int packed)
        {
            return Convert.ToByte(packed & c_ratingMask);
        }
        
        public static int Pack(int customerId, int rating)
        {
            return (customerId << c_ratingBits) | rating;
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
    
    /// <summary>
    /// This is a wrapper to work with packs.
    /// A pack contains customer ID and rating packed in Int32.
    /// </summary>
    public class Pack
    {
        /// <summary>
        /// Packed value
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private int _value;
        
        /// <summary>
        /// ID of the customer
        /// </summary>
        public int CustomerID
        {
            get { return PackedInt.GetCustomerId(_value); }
        }
        
        public byte Rating
        {
            get
            {
                return PackedInt.GetRating(_value);
            }
        }
    }
    
}
