using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface ITextInputDialog : IDialog
	{
		/// <summary>
		/// Gets or sets the message displayed to the user.
		/// </summary>
		string Message
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value in the textbox
		/// </summary>
		string Input
		{
			get;
			set;
		}

		/// <summary>
		/// Selects the text in the text box.
		/// </summary>
		void HighlightText();
	}
}
