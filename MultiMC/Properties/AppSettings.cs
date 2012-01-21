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

namespace MultiMC
{
	public class AppSettings
	{
		public AppSettings()
			: base()
		{
			dict = new Dictionary<string, string>();
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
					inst = new AppSettings();
					inst.Load();
					return inst;
				}
			}
		}
		
		public void Save(string path = "multimc.cfg")
		{
			string[] line = new string[dict.Count]; int i = 0;
			foreach (KeyValuePair<string, string> kv in dict)
				line[i++] = kv.Key + "=" + kv.Value;
			File.WriteAllLines(path, line);
		}
		
		public void Load(string path = "multimc.cfg")
		{
			try
			{
				dict.Clear();
				string[] lines = File.ReadAllLines(path);
				foreach (string line in lines)
					dict.Add(line.Split('=')[0], line.Split('=')[1]);
			} catch (IndexOutOfRangeException e)
			{
				Console.WriteLine(e.ToString());
			} catch (FileNotFoundException) {}
		}
		
		protected Dictionary<string, string> dict;
		
		/// <summary>
		/// Gets the setting with the specified key.
		/// </summary>
		/// <param name='key'>
		/// The setting's key
		/// </param>
		/// <param name='def'>
		/// The default value
		/// </param>
		private string this[string key, string def]
		{
			get
			{
				if (dict.ContainsKey(key))
					return dict[key];
				else
					return def;
			}
		}
		
		private string this[string key]
		{
			get { return dict[key]; }
			set { dict[key] = value; }
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
			get { return this["LauncherFilename", "launcher.jar"].ToString(); }
			set { this["LauncherFilename"] = value; }
		}
		
		public string JavaPath
		{
			get { return this["JavaPath", "java"].ToString(); }
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

