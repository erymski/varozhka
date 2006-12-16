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

namespace Varozhka.Storage
{
    /// <summary>
    /// Customer IDs are sparse. The class packs them in the mapping array:
    /// index of item - is packed value, the item is the real value.
    /// </summary>
    public class SparseIdTranslator : IIdTranslator<int>
    {
        private int[] _map;

        /// <summary>
        /// It keeps the passed array! IOW - the array is not copied.
        /// </summary>
        /// <param name="map"></param>
        public SparseIdTranslator(int[] map)
        {
            _map = map;
            Array.Sort(_map);
        }

        /// <summary>
        /// Converts packed ID to real ID.
        /// </summary>
        /// <param name="value">Packed ID.</param>
        /// <returns>Real ID.</returns>
        public int PackedToReal(int value)
        {
            return _map[value];
        }

        /// <summary>
        /// Converts real ID to packed ID.
        /// </summary>
        /// <param name="value">Real ID.</param>
        /// <returns>Packed ID.</returns>
        public int RealToPacked(int value)
        {
            // TODO: place to speed up 
            return Array.BinarySearch(_map, value);
        }

        /// <summary>
        /// Saves the sparse translator data to the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <remarks>The old file will be overwritten if exists.</remarks>
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                
                writer.Write(_map.Length);
                // TODO: place to optimize
                foreach (int i in _map)
                {
                    writer.Write(i);
                }
            }
        }

        /// <summary>
        /// Loads sparse translator from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Instance of the sparse translator.</returns>
        public static SparseIdTranslator Load(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader reader = new BinaryReader(stream);

                int length = reader.ReadInt32();
                int[] map = new int[length];
                // TODO: place to optimize
                for (int i = 0; i < map.Length; i++)
                {
                    map[i] =  reader.ReadInt32();
                }

                return new SparseIdTranslator(map);
            }
        }
    }
}
