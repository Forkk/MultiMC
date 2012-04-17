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
using System.Collections;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace MultiMC
{
	public class AppUtils
	{
		static bool initialized;

		public static void Init()
		{
			if (initialized)
				return;

			initialized = true;
			ServicePointManager.ServerCertificateValidationCallback =
				new RemoteCertificateValidationCallback(CertCheck);

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		}

		static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Console.WriteLine("Unhandled exception occurred!");
				Console.WriteLine(e.ExceptionObject.ToString());

				Exception ex = e.ExceptionObject as Exception;

				if (ex == null)
				{
					Console.WriteLine("ExceptionObject is not an Exception!");
					Environment.Exit(-2);
				}

				Console.WriteLine("Dumping error log...");
				LogError(ex);

				if (ex is StackOverflowException ||
					ex is OutOfMemoryException)
				{
					Console.WriteLine("Fatal exception! Aborting!");
					Environment.Exit(-1);
				}

				if (UnhandledException != null)
					UnhandledException(sender, e);
			}
			catch (Exception)
			{
				Environment.Exit(-3);
			}
		}

		public static event UnhandledExceptionEventHandler UnhandledException;

		static readonly string[] EmbeddedDLLs = new string[]
		{
			"GitSharp.Core",
			"GitSharp",
			"ICSharpCode.SharpZipLib",
			"Ionic.Zip.Reduced",
			"Newtonsoft.Json",
			"nunit.framework",
			"Tamir.SharpSSH",
			"Winterdom.IO.FileMap",
		};

		static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string assemblyName = new AssemblyName(args.Name).Name;
			string resourceName = "MultiMC." + assemblyName;

			if (EmbeddedDLLs.Any(dll => assemblyName.ToLower().Contains(dll)))
			{
				resourceName = "MultiMC.Lib." + assemblyName + ".dll";
			}

			using (Stream stream = Assembly.GetExecutingAssembly().
				GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					Byte[] assemblyData = new Byte[stream.Length];
					stream.Read(assemblyData, 0, assemblyData.Length);

					Console.WriteLine("Loaded {0} from resources.", assemblyName);
					return Assembly.Load(assemblyData);
				}
			}

			return null;
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
			// FIXME Mac doesn't like WebClients :(
			WebClient webClient = new WebClient();
			return webClient.DownloadString((url + "?" + urlParams));

			//HttpWebRequest request = 
			//    HttpWebRequest.Create((url + "?" + urlParams)) as HttpWebRequest;
			//request.Method = "GET";

			//using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			//{
			//    using (Stream responseStream = response.GetResponseStream())
			//    {
			//        using (StreamReader reader = new StreamReader(responseStream))
			//        {
			//            return reader.ReadToEnd();
			//        }
			//    }
			//}
		}

		public static T GetAssemblyAttr<T>(Assembly assembly) where T : Attribute
		{
			if (assembly == null)
				throw new ArgumentNullException("The argument 'assembly' was null");

			object[] attrs = assembly.GetCustomAttributes(typeof(T), true);
			if (attrs == null || attrs.Length <= 0)
				return null;

			return attrs[0] as T;
		}

		public static void LogError(Exception e, string logFileName = null)
		{
			if (string.IsNullOrEmpty(logFileName))
				logFileName = string.Format("error_{0}-{1}-{2}_{3}.{4}.{5}.txt",
					DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year,
					DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.AppendLine(string.Format("Error report for {0}", DateTime.Now));
			sb.AppendLine();
			sb.AppendLine(string.Format("Exception type: {0}", e.GetType()));
			sb.AppendLine(string.Format("MultiMC Version: {0}", AppUtils.GetVersion()));
			sb.AppendLine();
			sb.AppendLine("Computer info:");
			sb.AppendLine(string.Format("\tOperating System: {0}", OSUtils.OSName));
			sb.AppendLine(string.Format("\tRuntime: {0}", OSUtils.Runtime.ToString()));
			sb.AppendLine();
			sb.AppendLine("---------- BEGIN STACK TRACE ----------");
			sb.AppendLine(e.ToString());
			sb.AppendLine("----------- END STACK TRACE -----------");
			
			// Write the data to an error log.
			File.WriteAllText(logFileName, sb.ToString());
		}

		private static bool CertCheck(object sender,
									  X509Certificate cert,
									  X509Chain chain,
									  SslPolicyErrors error)
		{
			switch(error)
			{
			case SslPolicyErrors.None:
				return true;
			case SslPolicyErrors.RemoteCertificateChainErrors:
				// check hash of cert. will eventually fail when it changes
				if("94512DCE63E389DEFC9506F84B272EB8A2A335D5" == cert.GetCertHashString())
					return true;
				return false;
			case SslPolicyErrors.RemoteCertificateNameMismatch:
				// The cert is not for the URL used!
				return false;
			case SslPolicyErrors.RemoteCertificateNotAvailable:
				// Can't get cert. Too bad.
				return false;
			}
			return false;
		}

		/// <summary>
		/// Gets the name of the currently running executable file.
		/// </summary>
		/// <value>
		/// The name of the executable file.
		/// </value>
		public static string ExecutableFileName
		{
			get
			{
				string assemblyName = Assembly.GetExecutingAssembly().Location;
				if (OSUtils.OS == OSEnum.Linux)
				{
					return assemblyName.Substring(0, assemblyName.LastIndexOf('.'));
				}
				return assemblyName;
			}
		}
	}
}

