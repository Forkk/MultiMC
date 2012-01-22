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
				int p = (int) Environment.OSVersion.Platform;
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
	}
}
