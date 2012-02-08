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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MultiMC
{
	public class OSUtils
	{
		/// <summary>
		/// The path to .minecraft for the current OS
		/// </summary>
		public static string MinecraftDir
		{
			get
			{
				if (Linux)
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
						".minecraft");

				switch (Environment.OSVersion.Platform)
				{
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
					return Path.Combine(Environment.GetEnvironmentVariable("APPDATA"));

				case PlatformID.Unix:
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
						".minecraft");

				case PlatformID.MacOSX:
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
						"Library", "Application Support", ".minecraft");
					
				default:
					throw new PlatformNotSupportedException("Your operating system is not supported.");
				}
			}
		}

		public static bool OSCompatible
		{
			get
			{
				if (Linux)
					return true;
				switch (Environment.OSVersion.Platform)
				{
				case PlatformID.Win32NT:
				case PlatformID.MacOSX:
				case PlatformID.Win32Windows:
					return true;

				default:
					return false;
				}
			}
		}

		public static bool Windows
		{
			get
			{
				switch (Environment.OSVersion.Platform)
				{
				case PlatformID.Win32NT:
				case PlatformID.Win32Windows:
					return true;

				default:
					return false;
				}
			}
		}

		public static bool Linux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 128);
			}
		}

		public static bool MacOSX
		{
			get
			{
				switch (Environment.OSVersion.Platform)
				{
				case PlatformID.MacOSX:
					return true;

				default:
					return false;
				}
			}
		}
		
		public static bool IsOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}
		
		public static string FindJava()
		{
			if (OSUtils.Windows)
			{
				string[] paths = new[]
				{
					@"C:\Program Files\Java\jre6\bin\java.exe",
					@"C:\Program Files\Java\jre7\bin\java.exe",
					@"C:\Program Files (x86)\Java\jre6\bin\java.exe",
					@"C:\Program Files (x86)\Java\jre6\bin\java.exe",
				};
				Console.WriteLine("Detecting Java path for Windows.");
				
				foreach (string path in paths)
					if (File.Exists(path))
						return path;
			}
			// TODO add FindJava for Linux and Mac
			
			return "java";
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
		
//		public static string MakeRelativePath(string path1, string path2)
//		{
//			path1 = path1.EndsWith("" + Path.PathSeparator) ? path1 : path1 + Path.PathSeparator;
//			path2 = path2.EndsWith("" + Path.PathSeparator) ? path2 : path2 + Path.PathSeparator;
//			Uri uri = new Uri(path1, UriKind.RelativeOrAbsolute, true).
//				MakeRelativeUri(new Uri(path2, UriKind.RelativeOrAbsolute, true));
//			return uri.LocalPath;
//		}
	}
}
