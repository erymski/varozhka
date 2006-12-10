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
using NUnit.Framework;
using Varozhka.Storage;
using Varozhka.TrainingData;

namespace Varozhka.UnitTests
{
    [TestFixture]
    public class IndexTests
    {
        
        [Test]
        public void LoadIndex()
        {
            NetflixFiles files = new NetflixFiles(@"K:\netflix");
            NetflixData data = new NetflixData(files);

            SparseIdTranslator userTranslator;
            userTranslator = SparseIdTranslator.Load(files.UserIdIndex);
            
            LinearIdTranslator movieTranslator;
            movieTranslator = new LinearIdTranslator(HardCode.FirstMovieId);

            // several random checks
            byte rating = data.GetRating(movieTranslator.RealToPacked(1), userTranslator.RealToPacked(2442));
            Assert.AreEqual(rating, 3);
            
            rating = data.GetRating(movieTranslator.RealToPacked(1203), userTranslator.RealToPacked(1326195));
            Assert.AreEqual(rating, 1);
            
            rating = data.GetRating(movieTranslator.RealToPacked(4282), userTranslator.RealToPacked(1696063));
            Assert.AreEqual(rating, 5);
        }
        
        //[Test]
        //public void RowTests()
        //{
            
        //}
        
        [Test]
        public void TraverseWholeIndex()
        {
            
        }
    }
}
