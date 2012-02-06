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
using System.Reflection;

using Gtk;
using Gdk;

namespace MultiMC
{
	public class Resources
	{
		// Dropbox: http://dl.dropbox.com/u/52412912/
		
		// URLs
//		public const string DotNetZipURL = "http://multimc.tk/MultiMC/DotNetZip.dll";
//		public const string VInfoUrl = "http://multimc.tk/MultiMC/cs-version";
//		public const string LatestVersionURL = "http://multimc.tk/MultiMC/MultiMC.exe";
		
		// Minecraft URLs
		public const string LauncherURL = 
			"https://s3.amazonaws.com/MinecraftDownload/launcher/minecraft.jar";
		public const string MojangMCDLUri = "http://s3.amazonaws.com/MinecraftDownload/";
		public const string ForkkMCDLUri = "http://dl.dropbox.com/u/52412912/LWJGL/";
		
		// Update URLs
		public const string DownloadSiteURL = "http://forkk.net/MultiMC/download/";
		public const string VInfoUrl = DownloadSiteURL + "cs-version";
		public const string DotNetZipURL = DownloadSiteURL + "Ionic.Zip.Reduced.dll";
		public const string OtherUpdateURL = DownloadSiteURL + "MultiMC.exe";
		public const string LinuxUpdateURL = DownloadSiteURL + "Linux/MultiMC";
		
		// Other Stuff
		public const string InstanceXmlFile = "instance.xml";
		public const string InstDir = "instances";
		public const string ConfigFileName = "multimc.cfg";
		public const string LastLoginFileName = "lastlogin";
		public const string LastLoginKey = 
			"Bi[r;Yq'/FKM].@wgZoIBh~bkY}&W,0>)Gz%Jbusexx)&ijhXV}b^8m;&jfL73tx";
		
		public static string LatestVersionURL
		{
			get
			{
				if (OSUtils.Linux)
					return LinuxUpdateURL;
				return OtherUpdateURL;
			}
		}
		
		/// <summary>
		/// Gets the name of the currently running executable file.
		/// </summary>
		/// <value>
		/// The name of the executable file.
		/// </value>
		public static string ExecutableFileName
		{
			get
			{
				string assemblyName = Assembly.GetExecutingAssembly().Location;
				if (OSUtils.Linux)
				{
					return assemblyName.Substring(0, assemblyName.LastIndexOf('.'));
				}
				return assemblyName;
			}
		}
		
		public static string NewVersionFileName
		{
			get
			{
				return OSUtils.Linux ? "update" : "update.exe";
			}
		}
		
		/// <summary>
		/// Gets the version string for displaying to the user.
		/// </summary>
		/// <value>
		/// The version string.
		/// </value>
		public static string VersionString
		{
			get
			{
				Version v = AppUtils.GetVersion();
				return string.Format("{0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build, v.Revision);
			}
		}

		public static Pixbuf GetInstIcon(string key)
		{
			if (iconDict.ContainsKey(key))
				return iconDict[key];
			else
				return iconDict["stone"];
		}
		
		public static string[] IconKeys
		{
			get
			{
				string[] keyArray = new string[iconDict.Keys.Count];
				iconDict.Keys.CopyTo(keyArray, 0);
				return keyArray;
			}
		}
		
		private static Dictionary<string, Pixbuf> iconDict = LoadIcons();
		
		public static Dictionary<string, Pixbuf> LoadIcons()
		{
			Dictionary<string, Pixbuf> pixBufDict = new Dictionary<string, Pixbuf>();
			
			if (Directory.Exists("icons"))
			{
				foreach (string f in Directory.GetFiles("icons"))
				{
					pixBufDict.Add(Path.GetFileNameWithoutExtension(f), new Pixbuf(f));
				}
			}
			
			foreach (string name in 
			        new string[]{ "stone", "brick", 
				"diamond", "dirt", "gold", "grass", "iron", "planks", "tnt" })
			{
				if (!pixBufDict.ContainsKey(name))
					pixBufDict.Add(name, Pixbuf.LoadFromResource("MultiMC.icons." + name));
			}
			
			return pixBufDict;
		}
	}
}

