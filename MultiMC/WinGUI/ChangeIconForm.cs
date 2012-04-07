using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class ChangeIconForm : WinFormsDialog, IChangeIconDialog
	{
		public ChangeIconForm()
		{
			InitializeComponent();

			if (OSUtils.OS == OSEnum.Windows)
			{
				OSUtils.SetWindowTheme(iconView.Handle, "explorer", null);
			}
		}

		public string ChosenIconKey
		{
			get { return SelectedImageKey; }
		}

		public IImageList ImageList
		{
			get { return _imageList; }
			set
			{
				if (value is WinFormsImageList)
				{
					_imageList = value as WinFormsImageList;
					iconView.LargeImageList = _imageList.ImgList;

					LoadList();
				}
				else if (value != null)
					throw new InvalidOperationException("WinForms needs a WinFormsImageList.");
				else
					throw new ArgumentNullException("value");
			}
		}
		WinFormsImageList _imageList;

		void LoadList()
		{
			iconView.Items.Clear();
			for (int i = 0; i < _imageList.ImgList.Images.Count; i++)
			{
				string key = iconView.LargeImageList.Images.Keys[i];
				iconView.Items.Add(key, key);
			}
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (iconView.SelectedItems.Count > 0)
				OnResponse(DialogResponse.OK);
			else
				OnResponse(DialogResponse.Cancel);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}

		private void iconView_ItemActivate(object sender, EventArgs e)
		{
			buttonOk.PerformClick();
		}

		static readonly string[] allowedExtensions = new string[] 
			{ ".png", ".jpg", ".jpeg", ".bmp" };

		private bool IsDragDataValid(IDataObject data)
		{
			if (!data.GetDataPresent(DataFormats.FileDrop))
			{
				return false;
			}

			string[] files = (string[])data.GetData(DataFormats.FileDrop);
			foreach (string file in files)
			{
				if (!File.Exists(file) || 
					!allowedExtensions.Contains(Path.GetExtension(file)))
				{
					Console.WriteLine("File {0} not found or invalid.", file);
					return false;
				}
			}
			return true;
		}

		private void iconView_DragDrop(object sender, DragEventArgs e)
		{
			if (IsDragDataValid(e.Data))
			{
				e.Effect = DragDropEffects.Copy;

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string file in files)
				{
					Console.WriteLine("Copying file: " + file);
					if (!Directory.Exists(Properties.Resources.UserIconDir))
						Directory.CreateDirectory(Properties.Resources.UserIconDir);

					File.Copy(file, Path.Combine(Properties.Resources.UserIconDir, 
						Path.GetFileName(file)), true);
				}
				ImageList.LoadImages();
				LoadList();
			}
		}

		private void iconView_DragOver(object sender, DragEventArgs e)
		{
			if (IsDragDataValid(e.Data))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void iconView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete && 
				iconView.SelectedItems.Count > 0 &&
				ImageList.WasLoadedFromFile(SelectedImageKey))
			{
				ImageList.DeleteImage(SelectedImageKey);
				ImageList.LoadImages();
				LoadList();
			}
		}

		private string SelectedImageKey
		{
			get { return iconView.SelectedItems[0].ImageKey; }
		}
	}
}
