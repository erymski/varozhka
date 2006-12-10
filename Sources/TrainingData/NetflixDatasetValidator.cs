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

using System.IO;

namespace Varozhka.TrainingData
{
    /// <summary>
    /// Utility class to check Netflix dataset
    /// </summary>
    public static class NetflixDatasetValidator
    {
        #region Constants

        private const string c_trainingDir = "training_set";
        private const string c_ratingFileMask = "mv_00?????.txt";
        private static readonly string[] c_baseFiles = new string[] { "probe.txt", "qualifying.txt", "movie_titles.txt" };
        private static readonly string[] c_varozhkaIndexes = new string[] { "mapping.dat", "movies2ratings.dat", "norm_probe.dat", "user2movies.dat" };

        #endregion

        /// <summary>
        /// Checks if the directory contains the netflix dataset.
        /// </summary>
        /// <param name="directory">Directory to check</param>
        /// <returns>true if success</returns>
        public static bool Contains(string directory, bool fastCheck)
        {
            bool result = false;
            
            while (! string.IsNullOrEmpty(directory))
            {
                result = Directory.Exists(directory);
                if (! result) break;
                
                // check for base files
                foreach (string file in c_baseFiles)
                {
                    string baseFile = Path.Combine(directory, file);
                    if (! File.Exists(baseFile))
                    {
                        result = false;
                        break;
                    }
                }
                if (! result) break;

                // check for ratings files
                string trainingSetDir = Path.Combine(directory, c_trainingDir);
                result = Directory.Exists(trainingSetDir);
                if (! result) break;
                
                // perform more detailed check
                if (! fastCheck)
                {
                    // check for a random rating file
                    string[] files = Directory.GetFiles(trainingSetDir, c_ratingFileMask, SearchOption.AllDirectories);
                    result = (17770 == files.Length);
                }
                
                break;
            }
            
            return result;
        }
        
        /// <summary>
        /// If the directory contains Varozhka's indexes
        /// </summary>
        /// <param name="directory">Directory to check</param>
        /// <returns>true on success</returns>
        /// <remarks>Doesn't check for Netflix files</remarks>
        public static bool Processed(string directory)
        {
            bool result = false;
            
            if (! string.IsNullOrEmpty(directory) && 
                Directory.Exists(directory))
            {
                foreach (string file in c_varozhkaIndexes)
                {
                    result = File.Exists(Path.Combine(directory, file));

                    if (!result) break;
                }
            }
            
            return result;
        }
    }
}
