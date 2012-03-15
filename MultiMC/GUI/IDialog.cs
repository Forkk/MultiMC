using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	/// <summary>
	/// A common interface to allow dialogs from all toolkits to share common methods
	/// </summary>
	public interface IDialog : IWindow
	{
		/// <summary>
		/// If true, the dialog will stay on top of the parent window and block its execution.
		/// </summary>
		bool IsModal
		{
			get;
			set;
		}

		/// <summary>
		/// Shows a modal dialog and doesn't return until the window closes
		/// </summary>
		void Run();

		/// <summary>
		/// Called when a button is clicked in a dialog.
		/// </summary>
		event EventHandler<DialogResponseEventArgs> Response;
	}

	public enum DialogResponse
	{
		OK,
		Cancel,
		Yes,
		No,
		Abort,
		Other,
	}

	public class DialogResponseEventArgs : EventArgs
	{
		public DialogResponseEventArgs(DialogResponse response)
		{
			this.Response = response;
		}

		public readonly DialogResponse Response;
	}
}
