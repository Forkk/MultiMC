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
	public class Properties
	{
		public Properties()
		{
			dict = new Dictionary<string, string>();
		}

		public virtual void Save(string path)
		{
			string[] line = new string[dict.Count];
			int i = 0;
			foreach (KeyValuePair<string, string> kv in dict)
				line[i++] = kv.Key + "=" + kv.Value;
			File.WriteAllLines(path, line);
		}
		
		public virtual void Load(string path)
		{
			try
			{
				dict.Clear();
				string[] lines = File.ReadAllLines(path);
				foreach (string line in lines)
					if (!line.StartsWith("#"))
						dict.Add(line.Split('=')[0], line.Split('=')[1]);
			} catch (IndexOutOfRangeException e)
			{
				Console.WriteLine(e.ToString());
			} catch (FileNotFoundException)
			{
			}
		}
		
		protected Dictionary<string, string> dict;
		
		/// <summary>
		/// Gets the property with the specified key.
		/// </summary>
		/// <param name='key'>
		/// The property's key
		/// </param>
		/// <param name='def'>
		/// The default value
		/// </param>
		protected string this[string key, string def]
		{
			get
			{
				if (dict.ContainsKey(key))
					return dict[key];
				else
					return def;
			}
		}
		
		public string this[string key]
		{
			get { return dict[key]; }
			set { dict[key] = value; }
		}
		
		public bool ContainsKey(string key)
		{
			return dict.ContainsKey(key);
		}
		
		public bool Remove(string key)
		{
			return dict.Remove(key);
		}
	}
}

