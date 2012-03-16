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
					return OSEnum.Linux;

				default:
					return OSEnum.Other;
				}
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
	}

	public enum OSEnum
	{
		Windows,
		Linux,
		OSX,
		Other,
	}
}
