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

using MultiMC.Tasks;

namespace MultiMC.GUI
{
	/// <summary>
	/// An interface for the main form
	/// </summary>
	public interface IMainWindow : IWindow
	{
		void LoadInstances();

		/// <summary>
		/// Opens a file selection dialog allowing the user to pick a 
		/// .minecraft folder to import.
		/// </summary>
		/// <returns>The path to the .minecraft folder the user picked.</returns>
		string ImportInstance();

		event EventHandler<AddInstEventArgs> AddInstClicked;
		event EventHandler ViewFolderClicked;
		event EventHandler CentralModsFolderClicked;
		event EventHandler RefreshClicked;

		event EventHandler SettingsClicked;
		event EventHandler CheckUpdatesClicked;

		event EventHandler HelpClicked;
		event EventHandler AboutClicked;


		event EventHandler<InstActionEventArgs> InstanceLaunched;

		event EventHandler<InstActionEventArgs> ChangeIconClicked;
		event EventHandler<InstActionEventArgs> EditNotesClicked;

		event EventHandler<InstActionEventArgs> ManageSavesClicked;
		event EventHandler<InstActionEventArgs> EditModsClicked;
		event EventHandler<InstActionEventArgs> RebuildJarClicked;
		event EventHandler<InstActionEventArgs> ViewInstFolderClicked;

		event EventHandler<InstActionEventArgs> DeleteInstClicked;
		event EventHandler<InstActionEventArgs> RemoveOpenALClicked;

		event EventHandler EscPressed;

		IList<Instance> InstanceList
		{
			get;
		}

		IList<Task> TaskList
		{
			get;
		}

		IImageList ImageList
		{
			get;
			set;
		}

		Instance SelectedInst
		{
			get;
		}

		string StatusText
		{
			get;
			set;
		}

		Task GetTaskByID(int taskID);
		bool IsTaskIDTaken(int taskID);
		int GetAvailableTaskID();
	}

	public class InstActionEventArgs : EventArgs
	{
		public InstActionEventArgs(Instance inst)
		{
			this.Inst = inst;
		}

		public Instance Inst
		{
			get;
			protected set;
		}
	}

	public class AddInstEventArgs : EventArgs
	{
		public AddInstEventArgs(AddInstAction action)
		{
			this.Action = action;
		}

		public AddInstAction Action
		{
			get;
			protected set;
		}
	}

	public enum AddInstAction
	{
		CreateNew,
		CopyExisting,
		ImportExisting,
	}
}
