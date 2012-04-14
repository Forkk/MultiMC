using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class GTKAboutDialog : AboutDialog, IAboutDialog
	{
		public GTKAboutDialog()
			: base()
		{
			Authors = new string[] { "Andrew Okin", "ShaRose" };
			License = Properties.Resources.License;
			Logo = Gdk.Pixbuf.LoadFromResource("MultiMC.Resources.MultiMC32.png");
			this.ProgramName = "MultiMC";
			this.Version = AppUtils.GetVersion().ToString();

			DeleteEvent += WindowDestroyed;
			base.Response += OnResponse;

			base.Shown += (o, args) => MoveToDefPosition();
		}

		void OnResponse(object o, ResponseArgs args)
		{
			DialogResponse response = DialogResponse.Other;

			switch (args.ResponseId)
			{
			case ResponseType.Ok:
				response = DialogResponse.OK;
				break;

			case ResponseType.Cancel:
				response = DialogResponse.Cancel;
				break;

			case ResponseType.Yes:
				response = DialogResponse.Yes;
				break;

			case ResponseType.No:
				response = DialogResponse.No;
				break;
			}

			if (Response != null)
				Response(this, new DialogResponseEventArgs(response));

			Close();
		}

		void WindowDestroyed(object o, EventArgs args)
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
				{
					_parent = value;
					base.ParentWindow = (value as Window).GdkWindow;
				}
				else if (value is Dialog)
				{
					_parent = value;
					base.ParentWindow = (value as Dialog).GdkWindow;
				}
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

		public new void Close()
		{
			base.Destroy();
		}

		public void Invoke(EventHandler d)
		{
			Application.Invoke(d);
		}

		public event EventHandler Closed;

		public bool IsModal
		{
			get { return base.Modal; }
			set { base.Modal = value; }
		}

		public new void Run()
		{
			base.Run();
		}

		public new event EventHandler<DialogResponseEventArgs> Response;

		public event EventHandler ChangelogClicked;


		public bool ShowInTaskbar
		{
			get;
			set;
		}

		public bool AlwaysOnTop
		{
			get;
			set;
		}
	}
}
