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

namespace MultiMC.GUI
{
	/// <summary>
	/// A common interface to allow windows from all toolkits to share common methods
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// The title text of the window
		/// </summary>
		string Title
		{
			get;
			set;
		}

		/// <summary>
		/// The window's width
		/// </summary>
		int Width
		{
			get;
			set;
		}

		/// <summary>
		/// The window's height
		/// </summary>
		int Height
		{
			get;
			set;
		}

		/// <summary>
		/// The current state of the window
		/// </summary>
		WindowState State
		{
			get;
			set;
		}

		/// <summary>
		/// True if the window is visible
		/// </summary>
		bool Visible
		{
			get;
			set;
		}

		/// <summary>
		/// True if the window is enabled.
		/// </summary>
		bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// The window's default position. Changing this does not move the window.
		/// You must call <c cref="MoveToDefPosition()">MoveToDefPosition</c>
		/// </summary>
		DefWindowPosition DefaultPosition
		{
			get;
			set;
		}

		/// <summary>
		/// Shows the window. This method does not block like dialog's run method does.
		/// </summary>
		void Show();

		/// <summary>
		/// Moves the window to its default position
		/// </summary>
		void MoveToDefPosition();

		/// <summary>
		/// Gets or sets the parent of this window.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the given window
		/// cannot be set as this window's parent. This is usually because the
		/// window is from a different toolkit.</exception>
		/// <remarks>Setting this value is not always a good
		/// Also note that get returning null does not necessarily mean that
		/// the window has no parent. It could also indicate that the parent
		/// window does not inherit <c cref="IWindow">IWindow</c>. To check
		/// if the window has a parent, use <c cref="HasParent">HasParent</c>
		/// instead.</remarks>
		IWindow Parent
		{
			get;
			set;
		}

		bool HasParent
		{
			get;
		}

		/// <summary>
		/// Closes the window.
		/// </summary>
		void Close();

		void Invoke(EventHandler d);
	}

	/// <summary>
	/// The default window position
	/// </summary>
	public enum DefWindowPosition
	{
		/// <summary>
		/// Centers the window on the parent window
		/// </summary>
		CenterParent,

		/// <summary>
		/// Centers the window on the screen
		/// </summary>
		CenterScreen,

		/// <summary>
		/// The window starts at whatever position the OS chooses.
		/// </summary>
		Manual,
	}

	/// <summary>
	/// Window states such as maximized, minimized, and normal
	/// </summary>
	public enum WindowState
	{
		Maximized,
		Minimized,
		Normal
	}
}
