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

using Ionic.Zip;

namespace MultiMC
{
	public class Mod
	{
		ConfigFile modInfo;

		public Mod(string file)
		{
			FileName = file;
			modInfo = new ConfigFile();

			if (File.Exists(file))
			{
				using (ZipFile zipFile = ZipFile.Read(file))
				{
					ZipEntry entry = null;
					try
					{
						entry = zipFile.First(e => Path.GetExtension(e.FileName) == ".minf");
					}
					catch (InvalidOperationException)
					{

					}

					if (entry != null)
					{
						byte[] data = null;
						using (MemoryStream outStream = new MemoryStream())
						{
							entry.Extract(outStream);
							data = outStream.ToArray();
						}

						if (data != null)
						{
							using (Stream stream = new MemoryStream(data))
							{
								modInfo.Load(stream);
							}
						}
					}
				}
			}
		}

		public string Name
		{
			get { return modInfo["name", FileName]; }
		}

		public string FileName
		{
			get;
			private set;
		}

		public string Description
		{
			get { return modInfo["desc", "No description"]; }
		}

		public string ModVersion
		{
			get { return modInfo["mod-version", ""]; }
		}

		public string MCVersion
		{
			get { return modInfo["mc-version", ""]; }
		}

		public ModType Type
		{
			get { return modInfo.ParseEnumSetting<ModType>("type", ModType.MCJar); }
		}
	}

	public enum ModType
	{
		/// <summary>
		/// The zip file's contents should be extracted and added to minecraft.jar
		/// </summary>
		MCJar,

		/// <summary>
		/// The zip file should be placed in modloader's mods folder (minecraft/mods)
		/// </summary>
		MLMod,

		/// <summary>
		/// The zip file's contents should be *extracted* into 
		/// modloader's mods folder (minecraft/mods)
		/// </summary>
		MLModUnzip,

		/// <summary>
		/// The zip file contains multiple folders, each containing 
		/// files that should go in certain places. For example, 
		/// the zip  file for Equivalent Exchange contains two 
		/// folders, "mods" and "resources". All of the files 
		/// in these two folders would go in the respective 
		/// folders inside the minecraft folder. Including a 
		/// folder in the zip called "jar" will install 
		/// everything in that folder inside minecraft.jar
		/// </summary>
		Multiple,
	}
}
