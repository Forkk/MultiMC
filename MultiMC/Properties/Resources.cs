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
using System.Collections.Generic;

using Gtk;
using Gdk;

namespace MultiMC
{
	public class Resources
	{
		public const string InstanceXmlFile = "instance.xml";
		public const string VInfoUrl = "http://multimc.tk/MultiMC/cs-version";
		public const string InstDir = "instances";
		public const string NewVersionFileName = "update.exe";
		public const string LatestVersionURL = "http://multimc.tk/MultiMC/MultiMC.exe";
		public const string LauncherURL = 
			"https://s3.amazonaws.com/MinecraftDownload/launcher/minecraft.jar";
		public const string DotNetZipURL = "http://multimc.tk/MultiMC/DotNetZip.dll";
		public const string ConfigFileName = "multimc.cfg";
		
		public static Pixbuf GetInstIcon(string key)
		{
			if (iconDict.ContainsKey(key))
				return iconDict[key];
			else
				return iconDict["stone"];
		}
		
		private static Dictionary<string, Pixbuf> iconDict = LoadIcons();
		
		public static Dictionary<string, Pixbuf> LoadIcons()
		{
			Dictionary<string, Pixbuf> pixBufDict = new Dictionary<string, Pixbuf>();
			
			foreach (string f in Directory.GetFiles("icons"))
			{
				pixBufDict.Add(Path.GetFileNameWithoutExtension(f), new Pixbuf(f));
			}
			
			foreach (string name in 
			        new string[]{ "stone", "brick", 
				"diamond", "dirt", "gold", "grass", "iron", "planks", "tnt" })
			{
				if (!pixBufDict.ContainsKey(name))
					pixBufDict.Add(name, Pixbuf.LoadFromResource("icons/" + name + ".png"));
			}
			
			return pixBufDict;
		}
	}
}

