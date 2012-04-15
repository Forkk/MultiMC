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
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MultiMC
{
	public static class OSUtils
	{
		public static OSEnum OS
		{
			get
			{
				switch ((int)Environment.OSVersion.Platform)
				{
				case (int)PlatformID.Win32NT:
				case (int)PlatformID.Win32Windows:
					return OSEnum.Windows;

				case (int)PlatformID.MacOSX:
					return OSEnum.OSX;

				case 4:
				case 128:
					var kernel = DetectUnixKernel();					
					return (kernel == "Darwin") ? OSEnum.OSX : OSEnum.Linux;

				default:
					return OSEnum.Other;
				}
			}
		}

		public static Runtime Runtime
		{
			get
			{
				// If Mono.Runtime is defined, then we're running in Mono.
				bool monoRuntime = Type.GetType("Mono.Runtime") != null;

				if (monoRuntime)
					return Runtime.Mono;
				else
					return Runtime.DotNet;
			}
		}

		[Obsolete("Use the OS property instead.")]
		public static bool Windows
		{
			get { return OS == OSEnum.Windows; }
		}

		[Obsolete("Use the OS property instead.")]
		public static bool Linux
		{
			get { return OS == OSEnum.Linux; }
		}

		[Obsolete("Use the OS property instead.")]
		public static bool MacOSX
		{
			get { return OS == OSEnum.OSX; }
		}

		/// <summary>
		/// Gets path1 relative to path2
		/// </summary>
		/// <returns>
		/// The relative path.
		/// </returns>
		public static string GetRelativePath(string startPath, string fullDestPath)
		{
			List<string> pathParts = new List<string>(Path.GetFullPath(startPath).
				Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar));
			string[] destParts = fullDestPath.Trim(Path.DirectorySeparatorChar).
				Split(Path.DirectorySeparatorChar);

			//			DebugUtils.Print("Path Parts: {0}\nDest Parts: {1}", 
			//			                 DataUtils.ArrayToString(pathParts),
			//			                 DataUtils.ArrayToString(destParts));

			foreach (string part in destParts)
			{
				if (part.Equals(pathParts[0]))
					pathParts.RemoveAt(0);
				else
					break;
			}

			StringBuilder sb = new StringBuilder();
			foreach (string part in pathParts)
			{
				sb.Append(part + Path.DirectorySeparatorChar);
			}
			sb.Length--;
			return sb.ToString();
		}

		public static string OSName
		{
			get { return OS.ToString(); }
		}

		[DllImport("uxtheme.dll")]
		public extern static int SetWindowTheme(
			IntPtr hwnd,
			[MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName,
			[MarshalAs(UnmanagedType.LPWStr)] string pszSubIdList);
		
		#region private static string DetectUnixKernel
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct utsname
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string sysname;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string nodename;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string release;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string version;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string machine;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string extraJustInCase;

        }
		
		/// <summary>
        /// Detects the unix kernel by p/invoking uname (libc).
        /// </summary>
        /// <returns></returns>
        private static string DetectUnixKernel()
        {
            Debug.Print("Size: {0}", Marshal.SizeOf(typeof(utsname)).ToString());
            Debug.Flush();
            utsname uts = new utsname();
            uname(out uts);

            Debug.WriteLine("System:");
            Debug.Indent();
            Debug.WriteLine(uts.sysname);
            Debug.WriteLine(uts.nodename);
            Debug.WriteLine(uts.release);
            Debug.WriteLine(uts.version);
            Debug.WriteLine(uts.machine);
            Debug.Unindent();

            return uts.sysname.ToString();
        }

        [DllImport("libc")]
        private static extern void uname(out utsname uname_struct);
		#endregion
	}

	public enum OSEnum
	{
		Windows,
		Linux,
		OSX,
		Other,
	}

	/// <summary>
	/// An enumeration of the different runtimes MultiMC supports.
	/// </summary>
	public enum Runtime
	{
		DotNet,
		Mono,
	}
}
