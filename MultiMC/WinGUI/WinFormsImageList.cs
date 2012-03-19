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

		Dictionary<string, Image> defaultImages;

		/// <summary>
		/// Maps an image's key to its filename.
		/// </summary>
		Dictionary<string, string> keyFileDict;

		Image defImage;

		public WinFormsImageList(string userImageDir, Dictionary<string, Image> defImages,
			Image defaultImage)
		{
			keyFileDict = new Dictionary<string, string>();

			this.defImage = defaultImage;
			this.defaultImages = defImages;

			ImgList = new ImageList();
			ImgList.ImageSize = new Size(32, 32);
			ImgList.ColorDepth = ColorDepth.Depth32Bit;

			this.userImageDir = userImageDir;

			LoadImages();
		}

		public void LoadImages()
		{
			ImgList.Images.Clear();
			keyFileDict.Clear();

			ImgList.Images.Add("default", defImage);

			foreach (KeyValuePair<string, Image> image in defaultImages)
			{
				ImgList.Images.Add(image.Key, image.Value);
			}

			if (Directory.Exists("icons"))
			{
				foreach (string f in Directory.GetFiles("icons"))
				{
					using (Image img = Image.FromFile(f))
					{
						string imgKey = Path.GetFileNameWithoutExtension(f);
						if (ImgList.Images.ContainsKey(imgKey))
						{
							ImgList.Images.RemoveByKey(imgKey);
						}
						keyFileDict[imgKey] = f;
						ImgList.Images.Add(imgKey, img);
					}
				}
			}
		}

		public ImageList ImgList
		{
			get;
			protected set;
		}

		public void DeleteImage(string key)
		{
			ImgList.Images.RemoveByKey(key);
			File.Delete(keyFileDict[key]);
		}

		public bool WasLoadedFromFile(string key)
		{
			return keyFileDict.ContainsKey(key);
		}
	}
}
