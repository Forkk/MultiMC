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
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

using MultiMC.Tasks;

using Ionic.Zip;

namespace MultiMC.Tasks
{
	class Modder : Task
	{
		#region Properties

		public Instance Target
		{
			get { return _Target; }
		}

		Instance _Target;

		//public override bool Running
		//{
		//    get { return running; }
		//} bool running;

		private int TaskStep
		{
			get { return step; }
			set
			{
				step = value;
				int p = (int)(((float)TaskStep / (float)totalSteps) * 100);
				Progress = p;
			}
		}

		int step;
		const int totalSteps = 5;

		#endregion

		public Modder(Instance target)
		{
			this._Target = target;
		}

		protected override void TaskStart()
		{
			// Ignore if there's nothing to install
			
			try
			{
				InstallMods();
			} catch (IOException e)
			{
				OnErrorMessage("Failed to install mods. An error occurred:\n" + e.ToString());
				Cancel();
			}
		}

		/// <summary>
		/// Installs mods from instMods into minecraft.jar
		/// </summary>
		private void InstallMods()
		{
			OnStart();
			modFileIndices = new Hashtable();
			foreach (string modFile in Target.InstMods)
			{
				modFileIndices[modFile] = Target.InstMods[modFile];
			}

			string mcBin = Target.BinDir;
			string mcJar = Path.Combine(mcBin, "minecraft.jar");
			string mcBackup = Path.Combine(mcBin, "mcbackup.jar");

			Status = "Installing mods - Backing up minecraft.jar...";
			if (!File.Exists(mcBackup))
			{
				File.Copy(mcJar, mcBackup);
			}

			TaskStep++; // STEP
			Status = "Installing mods - Adding class files...";
			try
			{
				File.Delete(mcJar);
			} catch (IOException e)
			{
				if (e.Message.Contains("being used"))
				{
					OnErrorMessage("Can't install mods because minecraft.jar is being used " +
						"by another process. If you have minecraft.jar open in 7-zip, WinRAR " +
						"or any other program, please close it and then try again.");
					Cancel();
				}
			}
			File.Copy(mcBackup, mcJar);
			using (ZipFile jarFile = new ZipFile(mcJar))
			{
				if (!Directory.Exists(Target.InstModsDir))
					Directory.CreateDirectory(Target.InstModsDir);

				AddToJar(Target.InstModsDir, jarFile);

				TaskStep++; // STEP
				Status = "Installing mods - Removing META-INF...";
				string metaInfRegex = Path.Combine("META-INF", "*");
				if (jarFile.SelectEntries(metaInfRegex) != null)
				{
					DebugUtils.Print("Removing META-INF");
					jarFile.RemoveEntries(jarFile.SelectEntries(metaInfRegex));
				}

				TaskStep++; // STEP
				Status = "Installing mods - Saving minecraft.jar...";
				jarFile.Save(mcJar);
			}

			TaskStep++; // STEP
			Status = "Installing mods - Removing temporary files...";
			if (Directory.Exists(Path.Combine(Target.RootDir, MODTEMP_DIR_NAME)))
			{
				Directory.Delete(Path.Combine(Target.RootDir, MODTEMP_DIR_NAME), true);
			}

			TaskStep++; // STEP
			Status = "Installing mods - Done.";
			//running = false;
			OnComplete();
		}

		const string MODTEMP_DIR_NAME = "modTemp";
		Hashtable modFileIndices;

		/// <summary>
		/// Recursively adds files from the given directory into the given jar
		/// </summary>
		/// <param name="dir">directory to copy files from</param>
		/// <param name="jarFile">jar file to add them to</param>
		private void AddToJar(string dir, ZipFile jarFile, string pathInJar = "")
		{
			IEnumerable<string> cFiles = Directory.EnumerateFileSystemEntries(dir);
			foreach (string f in cFiles)
			{
				// For files that are not zip files...
				if (File.Exists(f) && Path.GetExtension(f) != ".zip")
				{
					// Automatically put world edit in the bin folder.
					if (Path.GetFileName(f) == "WorldEdit.jar")
					{
						File.Copy(f, 
							Path.Combine(Target.RootDir, ".minecraft", "bin", "WorldEdit.jar"), true);
						continue;
					}

					string existing = Path.Combine(pathInJar, Path.GetFileName(f));
					int index;
					if (jarFile[existing] != null &&
//						jarFile[existing].CreationTime.CompareTo(File.GetCreationTimeUtc(f)) > 0
					    Int32.TryParse(jarFile[existing].Comment, out index) &&
					    index > (int)modFileIndices[f])
					{
						DebugUtils.Print("File conflict between {0} in jar ({2}) and {1} ({3}) " + 
						    "being added, " + "not overwriting.", existing, f, 
						    jarFile[existing].Comment, 
						    (modFileIndices[f] != null ? modFileIndices[f].ToString() : "none"));
					}
					else
					{
						if (jarFile[existing] != null)
						{
							DebugUtils.Print("File conflict between {0} in jar ({2}) and {1} ({3}) " +
							    "being added, " + " overwriting.", existing, f,
							    jarFile[existing].Comment,
							    (modFileIndices[f] != null ? modFileIndices[f].ToString() : "none"));
						}

						ZipEntry fEntry = jarFile.UpdateFile(f, pathInJar);
						fEntry.SetEntryTimes(File.GetCreationTime(f),
							fEntry.AccessedTime, fEntry.ModifiedTime);
						if (modFileIndices[f] != null)
							fEntry.Comment = modFileIndices[f].ToString();
					}
				}

				// For directories
				else if (Directory.Exists(f))
				{
					DebugUtils.Print("Adding subdirectory " + f + " to " +
						Path.Combine(pathInJar, Path.GetFileName(f)));
					AddToJar(f, jarFile, Path.Combine(pathInJar, Path.GetFileName(f)));
				}

				// For zip files
				else if (File.Exists(f) &&
					(Path.GetExtension(f) == ".zip" || Path.GetExtension(f) == ".jar"))
				{
					string tmpDir = Path.Combine(Target.RootDir, MODTEMP_DIR_NAME,
						Path.GetFileNameWithoutExtension(f));
					DebugUtils.Print("Adding zip file {0}, extracting to {1}...", f, tmpDir);
					//Console.WriteLine("Temp directory for {0}: {1}", f, tmpDir);

					if (Directory.Exists(tmpDir))
					{
						Directory.Delete(tmpDir, true);
						Directory.CreateDirectory(tmpDir);
					}
					else
						Directory.CreateDirectory(tmpDir);

					//Console.WriteLine("Extracting {0} to temp directory...", f);
					using (ZipFile zipFile = new ZipFile(f))
					{
						foreach (ZipEntry entry in zipFile)
						{
							entry.Extract(tmpDir);
							string extractedFile = Path.Combine(tmpDir, 
								entry.FileName.Replace('/', Path.PathSeparator));

							if (modFileIndices[f] == null)
								continue;
							RecursiveSetIndex(extractedFile, (int) modFileIndices[f]);

							// If it's a file
							//						if (File.Exists(extractedFile))
							//							File.SetCreationTime(extractedFile, File.GetCreationTime(f));

							// If it's a directory
							//						else if (Directory.Exists(extractedFile))
							//							Directory.SetCreationTime(extractedFile, File.GetCreationTime(f));

							//Console.WriteLine("{0} create time is {1}", extractedFile, 
							//    File.GetCreationTime(f).ToString());
						}

						//Console.WriteLine("Adding to jar...");
						AddToJar(tmpDir, jarFile, pathInJar);
					}
				}
			}
		}
		
		private void RecursiveSetIndex(string file, int index)
		{
			if (Directory.Exists(file))
			{
				foreach (string subfile in Directory.GetFileSystemEntries(file))
				{
					RecursiveSetIndex(subfile, index);
				}
			}
			else if (File.Exists(file))
			{
				modFileIndices[file] = index;
			}
		}
	}
}


