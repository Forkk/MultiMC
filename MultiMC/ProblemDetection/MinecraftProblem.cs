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

namespace MultiMC.ProblemDetection
{
	/// <summary>
	/// An interface for detecting what might be wrong with the user's
	/// Minecraft instance and printing an easy to understand error message
	/// explaining how to fix it.
	/// </summary>
	public interface IMinecraftProblem
	{
		/// <summary>
		/// Gets a plain-english, human readable error message explaining 
		/// what might have caused the problem and how to fix it.
		/// This method may return null if the given <c>mcOutput</c>
		/// </summary>
		/// <returns>
		/// A human readable error message explaining how to fix the problem
		/// </returns>
		/// <param name='mcOutput'>
		/// The text minecraft printed to stderr
		/// </param>
		string GetErrorMessage(string mcOutput);
		
		/// <summary>
		/// Returns true if the given output line should cause minecraft to be terminated.
		/// </summary>
		/// <param name='mcOutput'>
		/// The text minecraft printed to stderr
		/// </param>
		bool ShouldTerminate(string mcOutput);
		
		/// <summary>
		/// Determines whether this problem is relevant for the given error.
		/// </summary>
		/// <param name='mcOutput'>
		/// The text minecraft printed to stderr
		/// </param>
		/// <returns>
		/// <c>true</c> if this problem is relevant for the given error; otherwise, <c>false</c>.
		/// </returns>
		bool IsRelevant(string mcOutput);
		
		/// <summary>
		/// Gets the priority of this problem. 
		/// Problems with a higher priority will be considered more relevant than lower priority ones
		/// </summary>
		/// <returns>
		/// The priority of this problem. 
		/// Note that changing the value of this after registering the 
		/// problem will NOT affect anything. 
		/// To change a problem's priority, you must unregister and then re-register it.
		/// </returns>
		int GetPriority();
	}
}
