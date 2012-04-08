using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Gtk;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class GTKImageList : IImageList
	{
		string userImageDir;

		Dictionary<string, Image> defaultImages;

		/// <summary>
		/// Maps an image's key to its filename.
		/// </summary>
		Dictionary<string, string> keyFileDict;

		Image defImage;

		public GTKImageList(string userImageDir, Dictionary<string, Image> defImages,
			Image defaultImage)
		{
			keyFileDict = new Dictionary<string, string>();

			this.defImage = defaultImage;
			this.defaultImages = defImages;
			this.ImgList = new Dictionary<string, Image>();

			this.userImageDir = userImageDir;

			LoadImages();
		}

		public void LoadImages()
		{
			ImgList.Clear();
			keyFileDict.Clear();

			ImgList.Add("default", defImage);

			foreach (KeyValuePair<string, Image> image in defaultImages)
			{
				ImgList.Add(image.Key, image.Value);
			}

			if (Directory.Exists("icons"))
			{
				foreach (string f in Directory.GetFiles("icons"))
				{
					using (Image img = new Image(f))
					{
						string imgKey = Path.GetFileNameWithoutExtension(f);
						if (ImgList.ContainsKey(imgKey))
						{
							ImgList.Remove(imgKey);
						}
						keyFileDict[imgKey] = f;
						ImgList.Add(imgKey, img);
					}
				}
			}
		}

		public Dictionary<string, Image> ImgList
		{
			get;
			protected set;
		}

		public void DeleteImage(string key)
		{
			ImgList.Remove(key);
			File.Delete(keyFileDict[key]);
		}

		public bool WasLoadedFromFile(string key)
		{
			return keyFileDict.ContainsKey(key);
		}
	}
}
