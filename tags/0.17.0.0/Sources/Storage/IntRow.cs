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
using System.Diagnostics;
using System.IO;

namespace Varozhka.Storage
{
    /// <summary>
    /// Byte buffer to be interpreted as other types array
    /// </summary>
    public class Row
    {
        
        protected byte[] _buffer;

        /// <summary>
        /// Saves the data to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(_buffer.Length);
            writer.Write(_buffer, 0, _buffer.Length);
        }

        /// <summary>
        /// Creates instance by loading data from the the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Loaded instance</returns>
        public void Load(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int length = reader.ReadInt32();

            _buffer = new byte[length];
            reader.Read(_buffer, 0, length);
        }
    }
    
    /// <summary>
    /// Keeps a row of integers using underlying array of bytes.
    /// Main purpose of the class is fast read and write from disk.
    /// </summary>
    /// <remarks>ER: Not sure how to do this class as generic. 
    /// Cannot get it compiled for things like 
    ///  T* pT = (T*) pOffset;</remarks>
    public class IntRow : Row
    {
        /// <summary>
        /// Gets the length of the row.
        /// </summary>
        /// <value>The length of the row.</value>
        public int Length
        {
            get { return _buffer.Length / sizeof(int); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IntRow"/> class.
        /// </summary>
        /// <param name="length">The length of the buffer.</param>
        public IntRow(int length)
        {
            Debug.Assert(length > 0);
            _buffer = new byte[length * sizeof(int)];
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Int32"/> at the specified index.
        /// </summary>
        /// <value>Integer value to store</value>
        unsafe public int this[int index]
        {
            get
            {
                Debug.Assert((index >= 0) && (index * sizeof(int) < _buffer.Length));
                fixed (byte* pBuffer = _buffer)
                {
                    int* pOffset = (int*)(pBuffer) + index;
                    return *pOffset;
                }
            }
            
            set
            {
                Debug.Assert((index >= 0) && (index * sizeof(int) < _buffer.Length));
                fixed (byte* pBuffer = _buffer)
                {
                    int* pOffset = (int*)(pBuffer) + index;
                    *pOffset = value;
                }
            }
        }
    }

    /// <summary>
    /// Keeps a row of integers using underlying array of bytes.
    /// Main purpose of the class is fast read and write from disk.
    /// </summary>
    /// <remarks>ER: Not sure how to do this class as generic. 
    /// Cannot get it compiled for things like 
    ///  T* pT = (T*) pOffset;</remarks>
    public class ShortRow : Row
    {
        /// <summary>
        /// Gets the length of the row.
        /// </summary>
        /// <value>The length of the row.</value>
        public int Length
        {
            get { return _buffer.Length / sizeof(short); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ShortRow"/> class.
        /// </summary>
        /// <param name="length">The length of the buffer.</param>
        public ShortRow(int length)
        {
            Debug.Assert(length > 0);
            _buffer = new byte[length * sizeof(short)];
        }

        /// <summary>
        /// Gets or sets the <see cref="T:ShortRow"/> at the specified index.
        /// </summary>
        /// <value>Integer value to store</value>
        unsafe public short this[int index]
        {
            get
            {
                Debug.Assert((index >= 0) && (index * sizeof(short) < _buffer.Length));
                fixed (byte* pBuffer = _buffer)
                {
                    short * pOffset = (short*)(pBuffer) + index;
                    return *pOffset;
                }
            }

            set
            {
                Debug.Assert((index >= 0) && (index * sizeof(short) < _buffer.Length));
                fixed (byte* pBuffer = _buffer)
                {
                    short* pOffset = (short*)(pBuffer) + index;
                    *pOffset = value;
                }
            }
        }
    }
    
    
#if false
    /// <summary>
    /// Max width is int.
    /// </summary>
    class LongRow
    {
        byte[] _buffer;
        private int _width;
        private int _bitsShift;
        private int _clearMask = 0;

        public LongRow(int length, int width)
        {
            _buffer = new byte[length*width];
            _width = width;

            int widthsDiff = sizeof (int) - width;
            _bitsShift = 8*widthsDiff;

            for (; widthsDiff > 0; widthsDiff--)
            {
                _clearMask <<= 8; // useless in the first iteration
                _clearMask |= 0xff;
            }
            _clearMask = ~_clearMask;
        }

        public int this[int index]
        {
            get
            {
                unsafe
                {
                    fixed (byte* pBuffer = _buffer)
                    {
                        int value = *GetOffset(index, pBuffer);
                        return value >> _bitsShift;
                    }
                }
            }
            
            set
            {
                unsafe
                {
                    fixed (byte* pBuffer = _buffer)
                    {
                        int* pOffset = GetOffset(index, pBuffer);
                        *pOffset &= _clearMask; // zeroing the previous value

                        *pOffset |= value << _bitsShift; // set the value
                    }
                }
            }
        }

        /// <summary>
        /// Gets the index offset in the buffer.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="pBuffer">The buffer.</param>
        /// <returns>Offset of the index</returns>
        unsafe private int* GetOffset(int index, byte* pBuffer)
        {
            return (int*)(pBuffer + index*_width);
        }

        /// <summary>
        /// Saves the data to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(_buffer.Length);
            writer.Write(_width);
            writer.Write(_buffer, 0, _buffer.Length);
        }

        /// <summary>
        /// Creates instance by loading data from the the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Loaded instance</returns>
        public static LongRow Load(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int length = reader.ReadInt32();
            int width = reader.ReadInt32();
            
            LongRow row = new LongRow(length, width);
            reader.Read(row._buffer, 0, length);
            
            return row;
        }
    }
#endif
}
