using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class GTKWindow : Window, IWindow
	{
		public GTKWindow(string title = "")
			: base(title)
		{
			DestroyEvent += new DestroyEventHandler(WindowDestroyed);
		}

		void WindowDestroyed(object o, DestroyEventArgs args)
		{
			if (Closed != null)
				Closed(this, EventArgs.Empty);
		}

		public int Width
		{
			get
			{
				int w = 0;
				int h = 0;
				base.GetSize(out w, out h);
				return w;
			}
			set
			{
				base.Resize(value, Height);
			}
		}

		public int Height
		{
			get
			{
				int w = 0;
				int h = 0;
				base.GetSize(out w, out h);
				return h;
			}
			set
			{
				base.Resize(Width, value);
			}
		}

		public new WindowState State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
				switch (_state)
				{
				case WindowState.Normal:
					Unmaximize();
					Deiconify();
					break;

				case WindowState.Maximized:
					Deiconify();
					Maximize();
					break;

				case WindowState.Minimized:
					Iconify();
					break;
				}
			}
		}
		WindowState _state = WindowState.Normal;

		public bool Enabled
		{
			get
			{
				return base.Sensitive;
			}
			set
			{
				base.Sensitive = value;
			}
		}

		public DefWindowPosition DefaultPosition
		{
			get
			{
				switch (base.WindowPosition)
				{
				case WindowPosition.Center:
					return DefWindowPosition.CenterScreen;
					
				case WindowPosition.CenterOnParent:
					return DefWindowPosition.CenterParent;

				default:
					return DefWindowPosition.Manual;
				}
			}
			set
			{
				switch (value)
				{
				case DefWindowPosition.CenterScreen:
					base.WindowPosition = WindowPosition.Center;
					break;

				case DefWindowPosition.CenterParent:
					base.WindowPosition = WindowPosition.CenterOnParent;
					break;

				case DefWindowPosition.Manual:
					base.WindowPosition = WindowPosition.None;
					break;
				}
			}
		}

		public void MoveToDefPosition()
		{
			// TODO movetodefposition
		}


		public virtual new IWindow Parent
		{
			get { return _parent; }
			set
			{
				if (value is Window)
					_parent = value;
				else
					throw new InvalidOperationException("Parent window must be " +
						"a GTK window!");
			}
		}

		private IWindow _parent;

		public virtual bool HasParent
		{
			get { return _parent != null; }
		}

		public void Close()
		{
			base.Destroy();
		}

		public void Invoke(EventHandler d)
		{
			Application.Invoke(d);
		}

		public event EventHandler Closed;
	}
}
