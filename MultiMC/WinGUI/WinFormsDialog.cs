using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public class WinFormsDialog : WinFormsWindow, IDialog
	{
		public WinFormsDialog()
		{

		}

		public WinFormsDialog(IWindow parent)
		{

		}

		public virtual bool IsModal
		{
			get { return _modal; }
			set { _modal = value; }
		}

		bool _modal;

		public virtual void Run()
		{
			if (HasParent && Parent is IWin32Window)
				base.ShowDialog(Parent as IWin32Window);
			else
			{
				Console.WriteLine("No parent");
				base.ShowDialog();
			}
		}

		public override void Show()
		{
			if (!IsModal)
				base.Show();
			else
				Run();
		}

		public event EventHandler<DialogResponseEventArgs> Response;

		protected virtual void OnResponse(DialogResponse response)
		{
			if (Response != null)
				Response(this, new DialogResponseEventArgs(response));
		}
	}
}
