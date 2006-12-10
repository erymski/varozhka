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

namespace Varozhka.Storage
{
    class StorageUtilities
    {

        /// <summary>
        /// Generates the dataset with random data in it.
        /// </summary>
        /// <param name="slotSize">Max number of items in the slot.</param>
        /// <param name="slotsCount">The slots count.</param>
        /// <param name="maxValue">The max slot value.</param>
        /// <returns>Generated data set</returns>
        //public static MemoryStorage GenerateRandomDataset(int slotSize, int slotsCount, int maxValue)
        //{
        //    Random rnd = new Random(DateTime.Now.Millisecond);

        //    int[] array = new int[slotSize];
        //    int length;
        //    MemoryStorage holder = new MemoryStorage(slotsCount);
        //    for (int slot = 0; slot < slotsCount; slot++)
        //    {
        //        length = rnd.Next(slotSize);
        //        for (int j = 0; j < length; j++)
        //        {
        //            int id = rnd.Next(maxValue);
        //            int rating = rnd.Next(1, 6);

        //            array[j] = PackedInt.Pack(id, rating);
        //        }

        //        holder.SetSlot(slot, array, length);
        //    }

        //    return holder;
        //}
        
    }
}
