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
using System.IO;
using System.Diagnostics;

namespace MultiMC
{
	public class DebugUtils
	{
		/// <summary>
		/// Prints the specified msg and args. This does nothing in the release build.
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public static void Print(string msg, params string[] args)
		{
#if DEBUG
			Console.WriteLine(msg, args);
#endif
		}

		public static void WriteError(string msg, string file = null)
		{
			if (string.IsNullOrEmpty(file))
				file = string.Format("error-{0}.txt", DateTime.Now.ToFileTimeUtc());
			File.WriteAllText(file, msg);
		}

		public static void FatalErrorDialog(string msg)
		{
			switch (OSUtils.OS)
			{
			case OSEnum.Windows:
				break;

			case OSEnum.Linux:
				Console.WriteLine(msg);
				try
				{
					Process.Start("gdialog", string.Format("--msgbox \"{0}\"", msg));
				}
				catch (FileNotFoundException)
				{
				}
				break;
			}
		}
	}
}

