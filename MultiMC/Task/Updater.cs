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

//#define UPDATE_DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.Threading;
using System.IO;

using MultiMC;

namespace MultiMC.Tasks
{
	class Updater : Task
	{
		WebClient UpdateClient;

		#region Properties

		public Version NewVersion
		{
			get { return nVer; }
		}

		Version nVer;

		#endregion

		public Updater()
		{
			UpdateClient = new WebClient();
		}

		protected override void TaskStart()
		{
			CheckUpdate();
		}

		/// <summary>
		/// Checks for updates
		/// </summary>
		private void CheckUpdate()
		{
			OnStart();

			Status = "Checking for updates - Getting latest version info...";
			Version nVersion = GetLatestVersion();
			Progress = 50;

			if (nVersion != null)
			{
				Status = "Version " + nVersion.ToString() + " is available.";
				Console.WriteLine(Status);
			}
			// Step two, send update available event
			Progress = 100;
			nVer = nVersion;
			OnComplete();
		}

		/// <summary>
		/// Returns version info for the latest version of MultiCraft
		/// </summary>
		/// <returns>Version info for the latest version of MultiCraft</returns>
		private Version GetLatestVersion()
		{
			Version nVersion = null;
			try
			{
#if UPDATE_DEBUG
				nVersion = new Version(UpdateClient.DownloadString(Resources.VInfoDebugUrl));
#else
				nVersion = new Version(UpdateClient.DownloadString(Resources.VInfoUrl));
#endif
			} catch (WebException e)
			{
				OnErrorMessage("Update check failed:\n" + e.Message + 
					"\nVisit http://www.tinyurl.com/multiplemc to check for updates manually.");
				Status = "Update check failed:\n" + e.Message;
				return null;
			}
			
			Version cVersion = Assembly.GetExecutingAssembly().GetName().Version;

			if (nVersion.CompareTo(cVersion) > 0)
			{
				return nVersion;
			}
			else
			{
				return null;
			}
		}

		#region Events

		#endregion

		#region Classes

		public class UpdateAvailableEventArgs : EventArgs
		{
			public UpdateAvailableEventArgs(Version ver)
			{
				this.nVersion = ver;
			}

			#region Properties

			public Version NewVersion
			{
				get { return nVersion; }
			}
			Version nVersion;

			#endregion
		}

		#endregion
	}
}

