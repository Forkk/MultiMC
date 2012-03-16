using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public class WinFormsImageList : IImageList
	{
		string userImageDir;

		public WinFormsImageList(string userImageDir)
		{
			ImgList = new ImageList();
			ImgList.ImageSize = new Size(32, 32);
			ImgList.ColorDepth = ColorDepth.Depth32Bit;

			this.userImageDir = userImageDir;

			LoadImages();
		}

		public void LoadImages()
		{
			if (Directory.Exists("icons"))
			{
				foreach (string f in Directory.GetFiles("icons"))
				{
					Image img = Image.FromFile(f);
					ImgList.Images.Add(f, img);
				}
			}
		}

		public ImageList ImgList
		{
			get;
			protected set;
		}
	}
}
