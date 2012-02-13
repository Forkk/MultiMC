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
using System.Reflection;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace MultiMC
{
	public class HintList
	{
		public static HintList Hints
		{
			get
			{
				if (list == null)
				{
					list = new HintList();
				}
				return list;
			}
		}

		static HintList list;
		const string DisabledHintsFile = "shown-hints";
		const string HintFileResourceID = "MultiMC.Properties.HintStrings";
		Dictionary<string, string> hintDict;
		List<string> disabledHints;

		private HintList()
		{
			hintDict = new Dictionary<string, string>();
			disabledHints = new List<string>();
			string hintFileString = "";
			
			using (Stream resourceStream = 
				Assembly.GetExecutingAssembly().GetManifestResourceStream(HintFileResourceID))
			{
				using (StreamReader reader = new StreamReader(resourceStream))
				{
					hintFileString = reader.ReadToEnd();
				}
			}
			
			XmlDocument hintDoc = new XmlDocument();
			hintDoc.LoadXml(hintFileString);
			foreach (XmlElement element in hintDoc.GetElementsByTagName("hint"))
			{
				if (element.HasAttribute("key") &&
					!string.IsNullOrEmpty(element.Attributes["key"].InnerText))
				{
					hintDict.Add(element.Attributes["key"].InnerText, element.InnerText);
				}
				else
				{
					Console.WriteLine("Invalid hint in hints file. Attribute 'key' is required!");
				}
			}
			
			Load();
		}
		
		public void Load()
		{
			if (File.Exists(DisabledHintsFile))
			{
				foreach (string line in File.ReadAllLines(DisabledHintsFile))
				{
					if (!line.StartsWith("#"))
						disabledHints.Add(line);
				}
			}
		}
		
		public void Save()
		{
			List<string> lines = new List<string>();
			lines.Add("# This is a list of hints that have already been displayed and will not " +
				"be shown again unless the reset hints button is clicked.");
			lines.AddRange(disabledHints);
			File.WriteAllLines(DisabledHintsFile, lines);
		}
		
		public bool ShouldShowHint(Hint id)
		{
			return !disabledHints.Contains(id.ToString());
		}
		
		public void SetShowHint(Hint id, bool show)
		{
			if (show)
				disabledHints.Remove(id.ToString());
			else
			{
				if (!disabledHints.Contains(id.ToString()))
					disabledHints.Add(id.ToString());
			}
				
			Save();
		}
		
		public void ResetAllHints()
		{
			disabledHints.Clear();
			Save();
		}
		
		public string this[Hint id]
		{
			get
			{
				switch (id)
				{
				default:
					if (hintDict.ContainsKey(id.ToString()))
						return hintDict[id.ToString()];
					else
						return id.ToString();
				}
			}
		}
	}

	public enum Hint
	{
		WelcomeHint,
		AddModsHint,
		EditModsHint,
	}
}

