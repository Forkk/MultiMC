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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace MultiMC.Tasks
{
	/// <summary>
	/// A wrapper around the file downloader class that interfaces it to the BackgroundTask class
	/// </summary>
	class Downloader : Task
	{
		#region Properties

		public string Message
		{
			get { return Status; }
			set { Status = value; }
		}

		//public override bool Running
		//{
		//    get { return FDownloader.Downloading; }
		//}

		public string DownloadUrl
		{
			get { return downloadUrl; }
		}

		string downloadUrl;

		public string TargetFile
		{
			get { return targFile; }
		}

		string targFile;

		public int Timeout
		{
			get { return timeout; }
			set
			{
				timeout = value;
			}
		}

		int timeout;

		#endregion

		int lastProgressTime;
		string tmpFile;
		WebClient webClient;

		public Downloader(string targetFile, string downloadUrl, 
		                  string message = null, int timeout = 180)
			: base()
		{
			this.targFile = targetFile;
			this.downloadUrl = downloadUrl;
			Message = message;
			Timeout = timeout;
		}

		protected override void TaskStart()
		{
			OnStart();
			Status = Message;
			Console.WriteLine("Starting download");
			tmpFile = Path.GetTempFileName();
			
			Console.WriteLine("Creating target directory");
			string tempDir = Directory.GetParent(tmpFile).FullName;
			if (!string.IsNullOrEmpty(tempDir) && Directory.Exists(tempDir))
				Directory.CreateDirectory(tempDir);
			
			Console.WriteLine("Creating web client");
			webClient = new WebClient();
			webClient.DownloadProgressChanged += 
				new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
			webClient.DownloadFileCompleted += 
				new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
			
			webClient.DownloadFileAsync(new Uri(downloadUrl),
			                            tmpFile);
			
			lastProgressTime = DateTime.Now.Second;
			
			while (Running)
			{
				if (DateTime.Now.Second >= lastProgressTime + Timeout)
				{
					OnErrorMessage(string.Format("No progress for {0} seconds, download timed out.", 
					                             DateTime.Now.Second - lastProgressTime));
					Cancel();
				}
			}
		}

		void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			Console.WriteLine("Download complete.");
			if (!e.Cancelled && e.Error == null)
			{
				File.Copy(tmpFile, TargetFile, true);
			}
			else if (e.Error != null)
			{
				OnException(e.Error);
			}
			
			if (e.Cancelled || e.Error != null)
				Console.WriteLine("Download failed.");
			
			if (File.Exists(tmpFile))
				File.Delete(tmpFile);

			OnComplete();
		}

		protected override void OnCancel()
		{
			base.OnCancel();
			webClient.CancelAsync();
		}

		void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			OnProgressChange(e.ProgressPercentage);
			lastProgressTime = DateTime.Now.Second;
		}

		void DownloadComplete(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
				OnException(e.Error);
			OnComplete();
		}
	}
}
