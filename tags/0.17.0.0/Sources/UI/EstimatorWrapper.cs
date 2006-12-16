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
using System.Reflection;
using Varozhka.Processing;
using Varozhka.TrainingData;

namespace Varozhka.UI
{
    internal class EstimatorWrapper
    {
        public static IEstimator LoadLocally(NetflixData netflixData, string estimatorFileName)
        {
            return GetEstimator(AppDomain.CurrentDomain, netflixData, estimatorFileName);
        }
        /// <summary>
        /// Load estimator from the given assembly
        /// </summary>
        /// <param name="domain">Application domain.</param>
        /// <param name="estimatorFileName">Name of assembly with estimator implementation.</param>
        /// <returns>Loaded estimator. (null if failed)</returns>
        public static IEstimator GetEstimator(AppDomain domain, NetflixData netflixData, string estimatorFileName)
        {
            IEstimator estimator = null;
            if (!string.IsNullOrEmpty(estimatorFileName))
            {
                try
                {
                    //Assembly assembly = domain.Load(Path.GetFileName(EstimatorFileName));
                    Assembly assembly = Assembly.LoadFrom(estimatorFileName);
                    
                    // run through all types in assembly and find an estimator
                    // TODO: only the first estimator will be loaded. allow to choose that later.
                    Type[] types = assembly.GetTypes();
                    Type processorType = Array.Find(types, delegate(Type type)
                                       {
                                           Type[] arrInterfaces = type.GetInterfaces();
                                           return Array.Exists(arrInterfaces, delegate(Type interfaceType)
                                                          {
                                                              return interfaceType == typeof(IEstimator);
                                                          });
                                       });

                    if (null != processorType)
                    {
                        if (processorType.IsSubclassOf(typeof(BaseEstimator)))
                        {
                            estimator = Activator.CreateInstance(processorType, new object[] { netflixData }) as IEstimator;
                        }
                        else
                        {
                            estimator = Activator.CreateInstance(processorType) as IEstimator;
                        }
                    }
                }
                catch (Exception e)
                {
                    // swallow
                }
            }

            return estimator;
        }

// TODO: load assembly in separate domain
#if false
        #region Unused stuff

        private const string c_domainName = "Processing";

        public string CallProcessor(string processFileName)
        {
            string result = null;

            AppDomain domain = null;
            try
            {
                domain = CreateAppDomain(processFileName);

                object unwrap = domain.CreateInstanceAndUnwrap("Reflection", "Reflection.ProcessorWrapper");

                IProcessor processor = GetEstimator(domain, processFileName);
                if (null != processor)
                {
                    result = processor.GetMessage();
                }

            }
            finally
            {
                if (null != domain)
                {
                    UnloadAssembly(domain);
                }
            }

            return result;
        }

        private static AppDomain CreateAppDomain(string processFileName)
        {
            AppDomainSetup domainSetup = new AppDomainSetup();
            domainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            domainSetup.PrivateBinPath = Path.GetDirectoryName(processFileName);

            AppDomain domain = AppDomain.CreateDomain(c_domainName, null, domainSetup);
            domain.AssemblyResolve += new ResolveEventHandler(domain_AssemblyResolve);

            return domain;
        }

        static Assembly domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(@"D:\Development\Tests\Reflection\FooProcessor\bin\Debug\FooProcessor.dll");
        }

        private static void UnloadAssembly(AppDomain domain)
        {
            AppDomain.Unload(domain);
        }

        #endregion
#endif

    }
}
