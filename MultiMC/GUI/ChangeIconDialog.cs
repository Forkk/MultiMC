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

using MultiMC.Data;

using Gtk;
using Gdk;

namespace MultiMC
{
	public partial class ChangeIconDialog : Gtk.Dialog
	{
		ListStore iconList;
		
		public ChangeIconDialog(Instance inst)
		{
			Inst = inst;
			
			this.Build();
			
			iconList = new ListStore(typeof(string), typeof(Pixbuf));
			
			iconView.Model = iconList;
			iconView.TextColumn = 0;
			iconView.PixbufColumn = 1;
			iconView.Cells[0] = new CellRendererText();
			
			CellRendererText textCell = (iconView.Cells[0] as CellRendererText);
			textCell.Editable = true;
			textCell.Width = 64;
			
			LoadIcons();
		}
		
		protected void LoadIcons()
		{
			iconList.Clear();
			foreach (string iKey in Resources.IconKeys)
			{
				iconList.AppendValues(iKey, Resources.GetInstIcon(iKey));
			}
		}

		protected void OnResponse(object o, Gtk.ResponseArgs args)
		{
			if (args.ResponseId == ResponseType.Ok && 
			    !string.IsNullOrEmpty(SelectedIconKey))
			{
				Inst.IconKey = SelectedIconKey;
			}
			
			Destroy();
		}
		
		public string SelectedIconKey
		{
			get
			{
				TreeIter iter;
				if (iconList.GetIter(out iter, iconView.SelectedItems[0]))
				{
					return iconList.GetValue(iter, 0).ToString();
				}
				else
				{
					return null;
				}
			}
		}
		
		public Instance Inst
		{
			get;
			protected set;
		}
	}
}

