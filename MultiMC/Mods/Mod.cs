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

namespace MultiMC.Mods
{
	public class Mod
	{
		ModInfo modInfo;

		public Mod(string file)
		{
			FileName = file;

			try
			{
				if (File.Exists(file))
				{
					using (ZipFile zipFile = ZipFile.Read(file))
					{
						ZipEntry entry = null;
						try
						{
							entry = zipFile.First(e => 
								Path.GetExtension(e.FileName) == ".minf");
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
									StreamReader reader = new StreamReader(stream);
									modInfo = ModInfo.FromJSON(reader.ReadToEnd());
								}
							}
						}
					}
				}
			}
			catch (ZipException)
			{

			}
			catch (IOException)
			{

			}

			if (modInfo == null)
			{
				Console.WriteLine("No valid mod info found.");
				modInfo = new ModInfo();
				modInfo.Name = Path.GetFileNameWithoutExtension(FileName);
			}
		}

		public string Name
		{
			get { return modInfo.Name; }
		}

		public string FileName
		{
			get;
			private set;
		}

		public string Description
		{
			get { return modInfo.Description; }
		}

		public string ModVersion
		{
			get { return modInfo.ModVersion; }
		}

		public string MCVersion
		{
			get { return modInfo.MCVersion; }
		}
	}
}
