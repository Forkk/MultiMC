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
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	/// <summary>
	/// A Windows form that inherits <c>IWindow</c>
	/// </summary>
	public class WinFormsWindow : Form, IWindow
	{
		public virtual string Title
		{
			get { return Text; }
			set { Text = value; }
		}

		public virtual WindowState State
		{
			get
			{
				switch (base.WindowState)
				{
				case FormWindowState.Maximized:
					return GUI.WindowState.Maximized;

				case FormWindowState.Minimized:
					return GUI.WindowState.Minimized;

				case FormWindowState.Normal:
				default:
					return GUI.WindowState.Normal;
				}
			}
			set
			{
				switch (value)
				{
				case GUI.WindowState.Maximized:
					WindowState = FormWindowState.Maximized;
					break;

				case GUI.WindowState.Minimized:
					WindowState = FormWindowState.Minimized;
					break;

				case GUI.WindowState.Normal:
				default:
					WindowState = FormWindowState.Normal;
					break;
				}
			}
		}

		public virtual DefWindowPosition DefaultPosition
		{
			get
			{
				switch (base.StartPosition)
				{
				case FormStartPosition.CenterParent:
					return DefWindowPosition.CenterParent;

				case FormStartPosition.CenterScreen:
					return DefWindowPosition.CenterScreen;

				default:
					return DefWindowPosition.Manual;
				}
			}
			set
			{
				switch (value)
				{
				case DefWindowPosition.CenterParent:
					base.StartPosition = FormStartPosition.CenterParent;
					break;

				case DefWindowPosition.CenterScreen:
					base.StartPosition = FormStartPosition.CenterScreen;
					break;

				default:
					base.StartPosition = FormStartPosition.Manual;
					break;
				}
			}
		}

		public virtual void MoveToDefPosition()
		{
			switch (DefaultPosition)
			{
			case DefWindowPosition.CenterParent:
				CenterToParent();
				break;

			case DefWindowPosition.CenterScreen:
				CenterToScreen();
				break;
			}
		}

		public virtual new IWindow Parent
		{
			get { return _parent; }
			set
			{
				if (value is Form)
					_parent = value;
				else
					throw new InvalidOperationException("Parent window must be " +
						"an IWin32Window!");
			}
		}

		private IWindow _parent;

		public virtual bool HasParent
		{
			get { return _parent != null; }
		}

		public virtual new void Show()
		{
			if (HasParent)
				base.Show(Parent as IWin32Window);
			else
				base.Show();
		}

		public void Invoke(EventHandler d)
		{
			base.BeginInvoke(d);
		}
	}
}
