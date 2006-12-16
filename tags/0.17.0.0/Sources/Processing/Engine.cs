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
using Varozhka.Storage;

namespace Varozhka.Processing
{
    /// <summary>
    /// Engine to process estimation and probe sets.
    /// </summary>
    public class Engine
    {
        #region Constants

        private readonly char[] c_customerInfoSeparator = ",".ToCharArray();
        private char c_movieIdTrail = ':';

        #endregion

        #region Properties

        protected string FileName
        {
            get { return _fileName; }
        }

        private string _fileName;

        protected IProcessor Processor
        {
            get { return _processor; }
        }

        private IProcessor _processor;

        #endregion

        #region Constructor

        public Engine(string fileName, IProcessor processor)
        {
            if (null == processor)
                throw new ArgumentNullException("processor");

            if (!File.Exists(fileName))
                throw new ArgumentException("File should exist!", "fileName");

            _fileName = fileName;
            _processor = processor;
        }

        #endregion

        /// <summary>
        /// Starts the processing.
        /// </summary>
        public void Start() 
        {
            Processor.Init();
            
            using (TextReader reader = new StreamReader(FileName))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    int pos = line.LastIndexOf(c_movieIdTrail);
                    if (-1 == pos) // it's a customer
                    {
                        string[] parts = line.Split(c_customerInfoSeparator, 2);
                    
                        Processor.OnView(int.Parse(parts[0], HardCode.Culture),
                                         DateTime.Parse(parts[1], HardCode.Culture));
                    }
                    else // it's a movie
                    {
                        int movieID = int.Parse(line.Substring(0, pos), HardCode.Culture);
                        
                        Processor.OnMovie(movieID);
                    }
                }
            }
            
            Processor.Complete();
        }
    }
}
