// 
//  Copyright 2012  Andrew Okin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

//#define BYPASS_SSL_CHECK
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace MultiMC
{
	public class AppUtils
	{
		static bool initialized;
		
		private static void Init()
		{
			if (!initialized)
			{
				initialized = true;
//				ServicePointManager.ServerCertificateValidationCallback = 
//					new RemoteCertificateValidationCallback(CertCheck);
			}
		}
		
		/// <summary>
		/// Gets the version of the program.
		/// </summary>
		/// <returns>
		/// The version of the current program.
		/// </returns>
		public static Version GetVersion()
		{
			Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().
					GetName().Version;
			return appVersion;
		}
		
		public static string ExecutePost(string url, string urlParams)
		{
			Init();
			WebClient webClient = new WebClient();
			return webClient.DownloadString(new Uri(url + "?" + urlParams));
		}
		
		private static bool CertCheck(object sender, 
		                              X509Certificate cert, 
		                              X509Chain chain, 
		                              SslPolicyErrors error)
		{
			if (cert == null)
			{
				Console.WriteLine("Warning: Certificate is null!");
				return false;
			}
#if BYPASS_SSL_CHECK
			else
				return true;
#else
			using (Stream stream = 
				Assembly.GetExecutingAssembly().GetManifestResourceStream("sslcert"))
			{
				if (stream == null)
				{
					Console.WriteLine("Warning: Public key resource is null!");
					return false;
				}
				byte[] bytes = new byte[stream.Length];
				stream.Read(bytes, 0, bytes.Length);
			
				if (bytes.Length < cert.GetPublicKey().Length)
					return false;
			
				for (int i = 0; i < bytes.Length; i++)
				{
					if (bytes[i] != cert.GetPublicKey()[i])
					{
						return false;
					}
				}
				return true;
			}
#endif
		}
	}
}

