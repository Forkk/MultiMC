using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface ILoginDialog : IDialog
	{
		string Username
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}

		bool RememberUsername
		{
			get;
			set;
		}

		bool RememberPassword
		{
			get;
			set;
		}

		bool ForceUpdate
		{
			get;
			set;
		}
	}
}
