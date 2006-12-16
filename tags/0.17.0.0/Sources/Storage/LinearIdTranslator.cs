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
namespace Varozhka.Storage
{
    /// <summary>
    /// Class to make sequence of IDs zero-based.
    /// It requires to work with movies IDs (which starts from 1)
    /// </summary>
    public class LinearIdTranslator : IIdTranslator<short>
    {
        private short _difference;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LinearIdTranslator"/> class.
        /// </summary>
        /// <param name="difference">The difference between real ID and zero.</param>
        public LinearIdTranslator(short difference)
        {
            _difference = difference;
        }

        #region IIdTranslator members

        /// <summary>
        /// Converts packed ID to real ID.
        /// </summary>
        /// <param name="value">Packed ID.</param>
        /// <returns>Real ID</returns>
        public short PackedToReal(short value)
        {
            return (short)(value + _difference);
        }

        /// <summary>
        /// Converts real ID to packed ID.
        /// </summary>
        /// <param name="value">Real ID.</param>
        /// <returns>Packed ID</returns>
        public short RealToPacked(short value)
        {
            return (short)(value - _difference);
        }

        #endregion
    }
}