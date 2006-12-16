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
using System.Diagnostics;
using System.IO;

namespace Varozhka.Storage
{
    /// <summary>
    /// Interface to implement read/write operations for the T.
    /// </summary>
    /// <typeparam name="T">Type to read/write from binary stream.</typeparam>
    public interface IStreamer<T>
    {
        /// <summary>
        /// Writes the specified value to the binary writer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="writer">The binary writer.</param>
        void Write(T value, BinaryWriter writer);


        /// <summary>
        /// Reads the specified reader from the binary stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The value</returns>
        T Read(BinaryReader reader);

        T[] ReadBlock(BinaryReader reader, int length);
    }

    /// <summary>
    /// Memory storage for the type.
    /// </summary>
    /// <typeparam name="TYPE">Type to store.</typeparam>
    /// <typeparam name="STREAMER">Streamer of the type</typeparam>
    public class MemoryStorage<TYPE, STREAMER>
        where STREAMER : IStreamer<TYPE>, new() // TODO: get rid of this STREAMER
    {
        public delegate void TotalDelegate(int total);
        public delegate void LoadedCountDelegate(int current);

        public event TotalDelegate Total;
        public event LoadedCountDelegate LoadedCount;
        
        #region Private variables

        private const int c_bufferSize = 50 * 1024 * 1024; // 50MB
        private const string c_signature = "3FB700451C0B495e88CA503F99706029";
        
        private TYPE[][] _block;

        #endregion

        /// <summary>
        /// Gets or sets the comparer for binary search.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<TYPE> Comparer
        {
            get { return _comparer; }
            set { _comparer = value; }
        }
        private IComparer<TYPE> _comparer;
        
        /// <summary>
        /// Current version of the file.
        /// </summary>
        protected virtual int Version
        {
            get { return 1; } // this is default value, can be overridden in descendants
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStorage"/> class.
        /// </summary>
        /// <param name="slotsCount">The slots count.</param>
        public MemoryStorage(int slotsCount)
        {
            _block = new TYPE[slotsCount][];
        }

        #endregion

        #region Public methods

        #region Data access methods

        /// <summary>
        /// Assign slot with the values.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="values">The values.</param>
        /// <param name="length">The length.</param>
        public void SetSlot(int slot, TYPE[] values, int length)
        {
            Debug.Assert(null == _block[slot]);
            Debug.Assert(length >= 0);
            
            _block[slot] = new TYPE[length];
            Array.Copy(values, 0, _block[slot], 0, length);
            
            Array.Sort(_block[slot]);
        }
        /// <summary>
        /// Assign slot with the values.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="values">The values.</param>
        public void SetSlot(int slot, TYPE[] values)
        {
            SetSlot(slot, values, values.Length);
        }

        /// <summary>
        /// Gets the values array at the specified slot.
        /// </summary>
        /// <value></value>
        public TYPE[] this[int slot]
        {
            get
            {
                return _block[slot];
            }
        }

        /// <summary>
        /// Gets the index of the item.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int GetIndex(int slot, TYPE item)
        {
            if (null != Comparer)
            {
                return Array.BinarySearch(_block[slot], item, Comparer);
            }
            else
            {
                return Array.BinarySearch(_block[slot], item);
            }
        }

        public TYPE GetValue(int slot, TYPE item)
        {
            Debug.Assert(null != Comparer);
            int index = Array.BinarySearch(_block[slot], item, Comparer);
            
            Debug.Assert(-1 != index);
            return _block[slot][index];
        }

        #endregion

        #region I/O methods

        /// <summary>
        /// Saves storage to the specified fileName.
        /// </summary>
        /// <param name="fileName">The fileName.</param>
        public void Save(string fileName)
        {
            STREAMER streamer = new STREAMER();
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, 
                                                  FileShare.None, c_bufferSize, FileOptions.SequentialScan))
            {
                BinaryWriter writer = new BinaryWriter(stream);

                // write housekeeping info
                writer.Write(c_signature);
                writer.Write(Version);

                // length of the array of slots
                writer.Write(_block.Length);

                for (int i = 0; i < _block.Length; i++)
                {
                    TYPE[] slot = _block[i];
                    writer.Write(slot.Length);
                    
                    for (int j = 0; j < slot.Length; j++)
                    {
                        TYPE value = slot[j];
                        streamer.Write(value, writer);
                    }

                    // ER: cannot get it compiled!
                    //unsafe
                    //{
                    //    fixed (int* pArr = ints)
                    //    {
                    //        byte* pArr2 = (byte*)pArr;
                    //        byte[] foo = pArr2;
                    //        writer.Write(foo/*pArr2*/, 0, ints.Length * 4);
                    //    }

                    //}
                }
            }
        }

        /// <summary>
        /// Loads memory storage from the specified fileName.
        /// </summary>
        /// <param name="fileName">The fileName.</param>
        /// <returns>Loaded storage</returns>
        public static MemoryStorage<TYPE, STREAMER> Load(string fileName, 
                                                            TotalDelegate delegateTotal, 
                                                            LoadedCountDelegate delegateLoaded)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            if (! File.Exists(fileName))
                throw new ArgumentException("Cannot find the file", "fileName");
            
            MemoryStorage<TYPE, STREAMER> data;
            STREAMER streamer = new STREAMER();
#if true
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                                                  FileShare.Read, c_bufferSize, FileOptions.SequentialScan))
#else
            byte[] plainBytes = File.ReadAllBytes(fileName);
            MemoryStream stream = new MemoryStream(plainBytes);
#endif
            {
                BinaryReader reader = new BinaryReader(stream);

                // check signature
                string signature = reader.ReadString();
                if (signature != c_signature)
                    throw new BrokenIndexException(string.Format("Index file '{0}' is broken, and must be regenerated.", fileName));

                // TODO: ER: it's just a stub now. it cannot be controlled in the static method. think on it
                int version = reader.ReadInt32();
                //if (version != Version)
                //    throw new BrokenIndexException(string.Format("Index file '{0}' is in old format, and must be regenerated.", fileName));

                // read all slots
                int slotsLength = reader.ReadInt32();
                delegateTotal(slotsLength);
                
                data = new MemoryStorage<TYPE, STREAMER>(slotsLength);

                int length;
                for (int slot = 0; slot < slotsLength; slot++)
                {
                    length = reader.ReadInt32();
                    data._block[slot] = streamer.ReadBlock(reader, length);
                    //for (int i = 0; i < length; i++) // TODO: ER: slow
                    //{
                    //    data._block[slot][i] = streamer.Read(reader);
                    //}

                    delegateLoaded(slot);
                }
            }
            
            return data;
        }

        #endregion

        #endregion
    }
}
