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
using System;

using Gtk;
using Gdk;

namespace MultiMC
{
	/// <summary>
	/// Tools for making Gtk not as ugly on Windows
	/// </summary>
	public class StyleUtils
	{
		/// <summary>
		/// Deuglifies the menu by changing the text color when selected to black
		/// </summary>
		/// <param name='menu'>
		/// The menu to be fixed
		/// </param>
		public static void DeuglifyMenu(Menu menu)
		{
			foreach (MenuItem mi in menu.Children)
			{
				if (mi.Child == null || (mi.Child as Label) == null)
					continue;
				(mi.Child as Label).ModifyFg(StateType.Prelight, new Color(0, 0, 0));
			}
		}
	}
}

