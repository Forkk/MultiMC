using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IImageList
	{
		void LoadImages(bool loadCustomIcons = true);

		void DeleteImage(string key);

		bool WasLoadedFromFile(string key);
	}
}
