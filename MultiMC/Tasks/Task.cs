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
using System.Threading;
using MultiMC.GUI;

namespace MultiMC.Tasks
{
	/// <summary>
	/// An interface for tasks to be performed in the background such as updating, installing
	/// mods, etc.
	/// </summary>
	public abstract class Task
	{
		#region Progress & Status
		
		/// <summary>
		/// Human-readable status message that indicates what the task is doing
		/// </summary>
		public virtual string Status
		{
			get { return _Status; }
			protected set
			{
				_Status = value;
				OnStatusChange(Status);
			}
		}

		private string _Status;

		/// <summary>
		/// Progress on the current task (in percent).
		/// -1 if progress is unknown or cannot be determined.
		/// </summary>
		public virtual int Progress
		{
			get { return _Progress; }
			protected set
			{
				_Progress = value;
				OnProgressChange(Progress);
			}
		}

		private int _Progress;
		
		public virtual int TaskID
		{
			get;
			set;
		}
		

		/// <summary>
		/// True if the task is running
		/// </summary>
		public virtual bool Running
		{
			get { return running; }
		}

		bool running;

		#endregion

		#region Properties

		/// <summary>
		/// The task's thread (null if not started)
		/// </summary>
		public Thread TaskThread
		{
			get;
			private set;
		}

		#endregion

		/// <summary>
		/// Starts the process.
		/// </summary>
		public void Start()
		{
			TaskThread = new Thread(new ThreadStart(TaskStart));
			TaskThread.Name = ToString() + " " + TaskID;
			TaskThread.Start();
		}

		/// <summary>
		/// Cancels the task
		/// </summary>
		public void Cancel()
		{
			OnCancel();
			OnComplete(true);
			TaskThread.Abort();
		}

		/// <summary>
		/// Called when the task is started.
		/// </summary>
		protected abstract void TaskStart();

		/// <summary>
		/// Called when the task is cancelled.
		/// </summary>
		protected virtual void OnCancel()
		{
		}

		#region Events

		/// <summary>
		/// Called when the task is started
		/// </summary>
		public event EventHandler Started;

		protected virtual void OnStart()
		{
			running = true;
			if (Started != null)
				Started(this, EventArgs.Empty);
		}

		/// <summary>
		/// Called when the task ends
		/// </summary>
		public event EventHandler<TaskCompleteEventArgs> Completed;

		protected virtual void OnComplete(bool cancelled = false)
		{
			running = false;
			if (Completed != null)
				Completed(this, new TaskCompleteEventArgs(this, cancelled));
		}

		/// <summary>
		/// Called when an exception is thrown that the task isn't meant to handle
		/// </summary>
		public event TaskExceptionEventHandler ExceptionThrown;
		public delegate void TaskExceptionEventHandler(object sender,TaskExceptionEventArgs e);

		protected virtual void OnException(Exception e)
		{
			if (ExceptionThrown != null)
				ExceptionThrown(this, new TaskExceptionEventArgs(this, e));
		}

		/// <summary>
		/// Called when the task's progress status changes
		/// </summary>
		public event ProgressChangeEventHandler ProgressChange;
		public delegate void ProgressChangeEventHandler(object sender,ProgressChangeEventArgs e);

		protected virtual void OnProgressChange(int NewValue)
		{
			if (ProgressChange != null)
				ProgressChange(this, new ProgressChangeEventArgs(this, NewValue));
		}

		/// <summary>
		/// Called when the task's status message changes
		/// </summary>
		public event StatusChangeEventHandler StatusChange;
		public delegate void StatusChangeEventHandler(object sender,TaskStatusEventArgs e);

		protected virtual void OnStatusChange(string NewValue)
		{
			if (StatusChange != null)
				StatusChange(this, new TaskStatusEventArgs(this, NewValue));
		}

		/// <summary>
		/// Called when an error occurs to display an error message
		/// </summary>
		public event ErrorMessageEventHandler ErrorMessage;
		public delegate void ErrorMessageEventHandler(object sender,ErrorMessageEventArgs e);

		protected virtual void OnErrorMessage(string message)
		{
			if (ErrorMessage != null)
				ErrorMessage(this, new ErrorMessageEventArgs(this, message));
		}


		/// <summary>
		/// Called when the task wants to show a dialog for some reason.
		/// (such as to ask the user a question)
		/// </summary>
		public event EventHandler<ShowDialogTaskEventArgs> ShowDialog;

		/// <summary>
		/// Shows a dialog with the given message, title, and buttons.
		/// </summary>
		/// <param name="message">The message shown on the dialog.</param>
		/// <param name="title">The dialog's title.</param>
		/// <param name="buttons">The buttons shown on the dialog.</param>
		/// <param name="defResponse">The default dialog response to be
		/// returned if nothing handles the event and sets the response value.</param>
		/// <returns>The response from the dialog.</returns>
		protected virtual DialogResponse ShowMessageDialog(
			string message, string title, 
			MessageButtons buttons = MessageButtons.Ok,
			DialogResponse defResponse = DialogResponse.Other)
		{
			ShowDialogTaskEventArgs args = new ShowDialogTaskEventArgs(
				this, message, title, buttons, defResponse);

			if (ShowDialog != null)
				ShowDialog(this, args);

			return args.Response;
		}

		#endregion

		#region Classes

		public class TaskEventArgs : EventArgs
		{
			public TaskEventArgs(Task task)
			{
				this.TaskID = task.TaskID;
			}

			public int TaskID
			{
				get;
				protected set;
			}
		}
		
		// Event Args Classes
		public class TaskExceptionEventArgs : TaskEventArgs
		{
			public TaskExceptionEventArgs(Task t, Exception e)
				: base(t)
			{
				this.exception = e;
			}

			#region Properties

			/// <summary>
			/// The exception that was thrown
			/// </summary>
			public Exception ThrownException
			{
				get { return exception; }
			}

			Exception exception;

			/// <summary>
			/// If true, the task will be cancelled
			/// </summary>
			public bool CancelTask
			{
				get { return cancel; }
				set { cancel = value; }
			}

			bool cancel;

			#endregion
		}

		public class ProgressChangeEventArgs : TaskEventArgs
		{
			public ProgressChangeEventArgs(Task t, int progress)
				: base(t)
			{
				this.progress = progress;
			}

			#region Properties

			public int Progress
			{
				get { return progress; }
			}

			int progress;

			#endregion
		}

		public class TaskStatusEventArgs : TaskEventArgs
		{
			public TaskStatusEventArgs(Task t, string status)
				: base(t)
			{
				this.status = status;
			}

			#region Properties

			public string Status
			{
				get { return status; }
			}

			string status;

			#endregion
		}

		/// <summary>
		/// Event arguments for error messages.
		/// </summary>
		public class ErrorMessageEventArgs : TaskEventArgs
		{
			public ErrorMessageEventArgs(Task t, string message)
				: base(t)
			{
				this.message = message;
			}

			#region Properties

			/// <summary>
			/// The error message
			/// </summary>
			public string Message
			{
				get { return message; }
			}

			string message;

			#endregion
		}
		
		public class TaskCompleteEventArgs : TaskEventArgs
		{
			public TaskCompleteEventArgs(Task t, bool cancelled = false)
				: base(t)
			{
				this.Cancelled = cancelled;
			}
			
			public bool Cancelled
			{
				get;
				protected set;
			}
		}

		public class ShowDialogTaskEventArgs : TaskEventArgs
		{
			public ShowDialogTaskEventArgs(Task t, 
				string message, string title, MessageButtons buttons,
				DialogResponse defResponse = DialogResponse.Other)
				: base(t)
			{
				this.Message = message;
				this.Title = title;
				this.Buttons = buttons;
				this.Response = defResponse;
			}

			public string Message
			{
				get;
				protected set;
			}

			public string Title
			{
				get;
				protected set;
			}

			public MessageButtons Buttons
			{
				get;
				protected set;
			}

			public DialogResponse Response
			{
				get;
				set;
			}
		}

		#endregion
	}
}
