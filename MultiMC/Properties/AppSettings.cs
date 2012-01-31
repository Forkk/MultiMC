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
using System.Configuration;
using System.Collections.Generic;
using System.IO;

using MultiMC.Data;

namespace MultiMC
{
	public class AppSettings : Properties
	{
		public AppSettings()
			: base()
		{
		}
		
		private static AppSettings inst;
		
		/// <summary>
		/// The main settings provider.
		/// </summary>
		/// <value>
		/// The main settings provider.
		/// </value>
		public static AppSettings Main
		{
			get
			{
				if (inst != null)
				{
					return inst;
				}
				else
				{
					Console.WriteLine("Loading settings...");
					inst = new AppSettings();
					inst.Load(Resources.ConfigFileName);
					return inst;
				}
			}
		}
		
		public override void Load(string path = Resources.ConfigFileName)
		{
			base.Load(path);
		}
		
		public override void Save(string path = Resources.ConfigFileName)
		{
			base.Save(path);
		}
		
		public int InitialMemoryAlloc
		{
			get { return Int32.Parse(this["InitialMemoryAlloc", "512"]); }
			set { this["InitialMemoryAlloc"] = value.ToString(); }
		}
		
		public int MaxMemoryAlloc
		{
			get { return Int32.Parse(this["MaxMemoryAlloc", "1024"]); }
			set { this["MaxMemoryAlloc"] = value.ToString(); }
		}
		
		public string LauncherPath
		{
			get { return this["LauncherFilename", "launcher.jar"]; }
			set { this["LauncherFilename"] = value; }
		}
		
		public string JavaPath
		{
			get { return this["JavaPath", "java"]; }
			set { this["JavaPath"] = value; }
		}
		
		public bool ShowConsole
		{
			get { return bool.Parse(this["ShowConsole", "false"]); }
			set { this["ShowConsole"] = value.ToString(); }
		}
		
		public bool AutoCloseConsole
		{
			get { return bool.Parse(this["AutoCloseConsole", "false"]); }
			set { this["AutoCloseConsole"] = value.ToString(); }
		}
		
		public bool AutoUpdate
		{
			get { return bool.Parse(this["AutoUpdate", "true"]); }
			set { this["AutoUpdate"] = value.ToString(); }
		}
	}
}

