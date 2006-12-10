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
using System.Net;

namespace Varozhka.UI
{
    /// <summary>
    /// Class with static information about update urls.
    /// </summary>
    public static class UpdatesUrls
    {
        /// <summary>
        /// URL of file with the latest version.
        /// </summary>
        public static string Version
        {
            get
            {
                //return @"http://localhost:2789/digizzle.com/Projects/Varozhka/current_version.txt";
                return @"http://www.digizzle.com/Projects/Varozhka/current_version.txt";
            }
        }
        
        /// <summary>
        /// URL of page with change log.
        /// </summary>
        public static string ChangeLog
        {
            get
            {
                return @"http://www.digizzle.com/Projects/Varozhka/ChangeLog.aspx";
                //return @"http://localhost:2789/digizzle.com/Projects/Varozhka/ChangeLog.aspx";
            }
        }
    }
    
    /// <summary>
    /// Straightforward checker for updates.
    /// </summary>
    public class UpdatesChecker
    {
        /// <summary>
        /// Checks if update is available
        /// </summary>
        /// <param name="currentVersion">Current version of an application</param>
        /// <param name="urlVersionFile">URL of file with the latest version of the app.</param>
        /// <returns>true if update is available</returns>
        public bool IsUpdateAvailable(Version currentVersion, string urlVersionFile)
        {
            bool result = false;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string versionString = client.DownloadString(urlVersionFile);

                    if (! string.IsNullOrEmpty(versionString))
                    {
                        Version latestVersion = new Version(versionString);
                        result = latestVersion > currentVersion;
                    }
                }
            }
            catch (Exception e) // swallow the exception for now
            {
                // TODO: add some logic here
            }            
            return result;
        }
    }
}
