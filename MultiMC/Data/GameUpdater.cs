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
using System.Net;
using System.Security.Cryptography;

using MultiMC.Data;

using Ionic.Zip;

namespace MultiMC.Tasks
{
	public class GameUpdater : Task
	{
		string mainGameUrl;
		string latestVersion;
		Uri[] uriList;
		bool forceUpdate;
		bool shouldUpdate;
		int totalDownloadSize;
		int currentDownloadSize;
		
		public GameUpdater(Instance inst, 
		                   string latestVersion, 
		                   string mainGameUrl, 
		                   bool forceUpdate = false)
		{
			this.Inst = inst;
			this.latestVersion = latestVersion;
			this.mainGameUrl = mainGameUrl;
			this.forceUpdate = forceUpdate;
		}
		
		protected override void TaskStart()
		{
			OnStart();
			State = EnumState.CHECKING_CACHE;
			Progress = 5;
			
			// Get a list of URLs to download from
			LoadJarURLs();
			
			// Create the bin directory if it doesn't exist
			if (!Directory.Exists(Inst.BinDir))
				Directory.CreateDirectory(Inst.BinDir);
			
			string binDir = Inst.BinDir;
			if (this.latestVersion != null)
			{
				string versionFile = Path.Combine(binDir, "version");
				bool cacheAvailable = false;
				
				if (!forceUpdate && File.Exists(versionFile) && 
				    (latestVersion.Equals("-1") ||
				 latestVersion.Equals(File.ReadAllText(versionFile))))
				{
					cacheAvailable = true;
					Progress = 90;
				}
				
				if ((forceUpdate) || (!cacheAvailable))
				{
					shouldUpdate = true;
					if (!forceUpdate && File.Exists(versionFile))
						AskToUpdate();
					if (this.shouldUpdate)
					{
						File.WriteAllText(versionFile, "");
						
						DownloadJars();
						ExtractNatives();
						Progress = 100;
					}
				}
			}
			OnComplete();
		}
		
		protected void LoadJarURLs()
		{
			State = EnumState.DETERMINING_PACKAGES;
			string[] jarList = new string[]
			{ 
				"lwjgl.jar", "jinput.jar", "lwjgl_util.jar", this.mainGameUrl
			};
			
			this.uriList = new Uri[jarList.Length + 1];
			Uri baseUri = new Uri(Resources.MinecraftDLUri);
			
			for (int i = 0; i < jarList.Length; i++)
				this.uriList[i] = new Uri(baseUri, jarList[i]);
			
			string nativeJar = string.Empty;
			
			if (OSUtils.Windows)
				nativeJar = "windows_natives.jar";
			else if (OSUtils.Linux)
				nativeJar = "linux_natives.jar";
			else if (OSUtils.MacOSX)
				nativeJar = "macosx_natives.jar";
			else
			{
				OnErrorMessage("Your operating system is not supported.");
				Cancel();
			}
			
			this.uriList[this.uriList.Length - 1] = new Uri(baseUri, nativeJar);
		}
		
		protected void DownloadJars()
		{
			Properties md5s = new Properties();
			if (File.Exists(Path.Combine(Inst.BinDir, "md5s")))
				md5s.Load(Path.Combine(Inst.BinDir, "md5s"));
			State = EnumState.DOWNLOADING;
			
			int[] fileSizes = new int[this.uriList.Length];
			bool[] skip = new bool[this.uriList.Length];
			
			// Get the headers and decide what files to skip downloading
			for (int i = 0; i < uriList.Length; i++)
			{
				Console.WriteLine("Getting header " + uriList[i].ToString());
				
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uriList[i]);
				request.Method = "HEAD";
				
				string etagOnDisk = null;
				if (md5s.ContainsKey(GetFileName(uriList[i])))
					etagOnDisk = md5s[GetFileName(uriList[i])];
				
				if (!forceUpdate && !string.IsNullOrEmpty(etagOnDisk))
					request.Headers[HttpRequestHeader.IfNoneMatch] = etagOnDisk;
				
				HttpWebResponse response = ((HttpWebResponse)request.GetResponse());
				
				int code = (int)response.StatusCode;
				if (code == 300)
					skip[i] = true;
				
				fileSizes[i] = (int)response.ContentLength;
				this.totalDownloadSize += fileSizes[i];
				Console.WriteLine("Got response: " + code + " and file size of " + 
				                  fileSizes[i] + " bytes");
			}
			
			int initialPercentage = Progress = 0;
			
			byte[] buffer = new byte[1024 * 10];
			for (int i = 0; i < this.uriList.Length; i++)
			{
				if (skip[i])
				{
					Progress = (initialPercentage + fileSizes[i] * 45 / this.totalDownloadSize);
				}
				else
				{
					string currentFile = GetFileName(uriList[i]);
					md5s.Remove(currentFile);
					md5s.Save(Path.Combine(Inst.BinDir, "md5s"));
					
					int failedAttempts = 0;
					const int MAX_FAILS = 3;
					bool downloadFile = true;
					
					// Download the files
					while (downloadFile)
					{
						downloadFile = false;
						
						HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uriList[i]);
						request.Headers[HttpRequestHeader.CacheControl] = "no-cache";
						
						Console.WriteLine("Getting response");
						HttpWebResponse response = (HttpWebResponse)request.GetResponse();
						Console.WriteLine("Done");
						
						string etag = response.Headers[HttpResponseHeader.ETag];
						etag = etag.TrimEnd('"').TrimStart('"');
						
						Stream dlStream = response.GetResponseStream();
						FileStream fos = 
							new FileStream(Path.Combine(Inst.BinDir, currentFile), FileMode.Create);
						int fileSize = 0;
						
						MD5 digest = MD5.Create();
						digest.Initialize();
						int readSize;
						while ((readSize = dlStream.Read(buffer, 0, buffer.Length)) > 0)
						{
							Console.WriteLine("Read " + readSize + " bytes");
							fos.Write(buffer, 0, readSize);
							
							this.currentDownloadSize += readSize;
							fileSize += readSize;
							
							digest.TransformBlock(buffer, 0, readSize, null, 0);
							
//							Progress = fileSize / fileSizes[i];
							
							Progress = (initialPercentage + this.currentDownloadSize
							            * 70 / this.totalDownloadSize);
						}
						digest.TransformFinalBlock(new byte[] {}, 0, 0);
						
						dlStream.Close();
						fos.Close();
						
						string md5 = HexEncode(digest.Hash);
						
						bool md5Matches = true;
						if (etag != null)
						{
							md5Matches = md5.Equals(etag);
//							Console.WriteLine(md5 + "\n" + etag);
						}
						
						if (md5Matches && fileSize == fileSizes[i] || fileSizes[i] <= 0)
						{
							md5s[(currentFile.Contains("natives") ?
							      currentFile + ".lzma" : currentFile)] = etag;
							md5s.Save(Path.Combine(Inst.BinDir, "md5s"));
						}
						else
						{
							failedAttempts++;
							if (failedAttempts < MAX_FAILS)
							{
								downloadFile = true;
								this.currentDownloadSize -= fileSize;
							}
							else
							{
								OnErrorMessage("Failed to download " + currentFile +
								               " MD5 sums did not match.");
								Cancel();
							}
						}
					}
				}
			}
		}
		
		protected void ExtractNatives()
		{
			State = EnumState.EXTRACTING_PACKAGES;
			
			string nativesJar = 
				Path.Combine(Inst.BinDir, GetFileName(uriList[uriList.Length - 1]));
			
			if (!Directory.Exists(Path.Combine(Inst.BinDir, "native")))
				Directory.CreateDirectory(Path.Combine(Inst.BinDir, "native"));
			
			ZipFile zf = new ZipFile(nativesJar);
			zf.ExtractAll(Path.Combine(Inst.BinDir, "native"), 
			              ExtractExistingFileAction.OverwriteSilently);
			
			if (Directory.Exists(Path.Combine(Inst.BinDir, "native", "META-INF")))
				Directory.Delete(Path.Combine(Inst.BinDir, "native", "META-INF"), true);
		}
		
		protected void AskToUpdate()
		{
			AskUpdateEventArgs args = new AskUpdateEventArgs("Would you like to update Minecraft?");
			if (AskUpdate != null)
				AskUpdate(this, args);
			this.shouldUpdate = args.ShouldUpdate;
		}
		
		protected string GetFileName(Uri uri)
		{
			return Path.GetFileName(uri.LocalPath);
//			Console.WriteLine("str: " + uri.ToString() + " lp: " + uri.LocalPath);
//			string str = uri.ToString().Substring(uri.ToString().LastIndexOf("/"));
//			if (str.IndexOf("?") > 0)
//				str = str.Substring(0, str.IndexOf("?"));
//			return str;
		}
		
		protected EnumState State
		{
			get { return state; }
			set
			{
				state = value;
				switch (value)
				{
				case EnumState.INIT:
					Status = "Initializing loader";
					break;
				case EnumState.DETERMINING_PACKAGES:
					Status = "Determining packages to load";
					break;
				case EnumState.CHECKING_CACHE:
					Status = "Checking cache for existing files";
					break;
				case EnumState.DOWNLOADING:
					Status = "Downloading packages";
					break;
				case EnumState.EXTRACTING_PACKAGES:
					Status = "Extracting downloaded packages";
					break;
				case EnumState.UPDATING_CLASSPATH:
					Status = "Updating classpath";
					break;
				case EnumState.SWITCHING_APPLET:
					Status = "Switching applet";
					break;
				case EnumState.INITIALIZE_REAL_APPLET:
					Status = "Initializing real applet";
					break;
				case EnumState.START_REAL_APPLET:
					Status = "Starting real applet";
					break;
				case EnumState.DONE:
					Status = "Done loading";
					break;
				}
			}
		}
		
		EnumState state;
		
		public Instance Inst
		{
			get;
			protected set;
		}
		
		private static readonly char[] HexLowerChars = new[] 
		{ 
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
		};

		public static string HexEncode(byte[] rawbytes)
		{
			int length = rawbytes.Length;
			char[] chArray = new char[2 * length];
			int index = 0;
			int num3 = 0;

			while (index < length)
			{
				chArray[num3++] = HexLowerChars[rawbytes[index] >> 4];
				chArray[num3++] = HexLowerChars[rawbytes[index] & 15];
				index++;
			}
			return new string(chArray);
		}
		
		public enum EnumState
		{
			INIT, // 1
			DETERMINING_PACKAGES, // 2
			CHECKING_CACHE, // 3
			DOWNLOADING, // 4
			EXTRACTING_PACKAGES, // 5
			UPDATING_CLASSPATH, // 6
			SWITCHING_APPLET, // 7
			INITIALIZE_REAL_APPLET, // 8
			START_REAL_APPLET, // 9
			DONE, // 10
		}
		
		/// <summary>
		/// Occurs when the task asks the user to update.
		/// </summary>
		public event EventHandler<AskUpdateEventArgs> AskUpdate;
	}
	
	public class AskUpdateEventArgs : EventArgs
	{
		public AskUpdateEventArgs(string message)
		{
			this.Message = message;
			ShouldUpdate = false;
		}
		
		public string Message
		{
			get;
			protected set;
		}
		
		public bool ShouldUpdate
		{
			get;
			set;
		}
	}
}

