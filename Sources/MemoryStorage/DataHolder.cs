using System;
using System.IO;

namespace MemoryTest
{
    /// <summary>
    /// Stores array of arrays
    /// </summary>
    class DataHolder
    {
        private const int c_bufferSize = 5 * 1024 * 1024; // 5MB
        private int[][] _block;
        private readonly PackedInt _comparer = new PackedInt();

        public DataHolder(int slotsCount)
        {
            _block = new int[slotsCount][];
        }
        
        public void SetSlot(int slot, int[] collection, int length)
        {
            _block[slot] = new int[length];
            Array.Copy(collection, 0, _block[slot], 0, length);
            
            Array.Sort(_block[slot]);
        }
        
        public int[] this[int slot]
        {
            get
            {
                return _block[slot];
            }
        }
        
        public byte GetRating(int slot, int id)
        {
            int index = Array.BinarySearch(_block[slot], id, _comparer);
            
            return index < 0 ? (byte)0 : PackedInt.GetRating(_block[slot][index]);
        }
        
        public void Save(string filename)
        {
            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, 
                                                  FileShare.None, c_bufferSize, FileOptions.SequentialScan))
            {
                BinaryWriter writer = new BinaryWriter(stream);

                // length of the array of slots
                writer.Write(_block.Length);

                foreach (int[] ints in _block)
                {
                    writer.Write(ints.Length);
                    foreach (int i in ints) // ER: I guess it should be deadly slow...
                    {
                        writer.Write(i);
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

        public static DataHolder Load(string filename)
        {
            DataHolder data;
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read,
                                                  FileShare.Read, c_bufferSize, FileOptions.SequentialScan))
            {
                BinaryReader reader = new BinaryReader(stream);

                int slotsLength = reader.ReadInt32();
                data = new DataHolder(slotsLength);

                int length;
                for (int slot = 0; slot < slotsLength; slot++)
                {
                    length = reader.ReadInt32();
                    data._block[slot] = new int[length];
                    
                    for (int i = 0; i < length;i++)
                    {
                        data._block[slot][i] = reader.ReadInt32();
                    }
                }
            }
            
            return data;
        }


        /// <summary>
        /// Generates the dataset with random data in it.
        /// </summary>
        /// <param name="slotSize">Max number of items in the slot.</param>
        /// <param name="slotsCount">The slots count.</param>
        /// <param name="maxValue">The max slot value.</param>
        /// <returns>Generated data set</returns>
        public static DataHolder GenerateRandomDataset(int slotSize, int slotsCount, int maxValue)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            int[] array = new int[slotSize];
            int length;
            DataHolder holder = new DataHolder(slotsCount);
            for (int slot = 0; slot < slotsCount; slot++)
            {
                length = rnd.Next(slotSize);
                for (int j = 0; j < length; j++)
                {
                    int id = rnd.Next(maxValue);
                    int rating = rnd.Next(1, 6);

                    array[j] = PackedInt.Pack(id, rating);
                }

                holder.SetSlot(slot, array, length);
            }

            return holder;
        }
        
    }
}
