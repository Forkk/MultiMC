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
	/// Detects when modloader fails to load a mod
	/// </summary>
	public class ModLoadFailedProblem : IMinecraftProblem
	{
		public ModLoadFailedProblem()
		{
			
		}
		
		public bool IsRelevant(string mcOutput)
		{
			return mcOutput.Contains("Failed to load mod");
		}
		
		public string GetErrorMessage(string mcOutput)
		{
			int q = mcOutput.IndexOf('"');
			string modName = mcOutput.Substring(q + 1, mcOutput.IndexOf('"', q + 1) - q);
			
			return string.Format(
				"MultiMC detected that one of your mods ({0}) failed to load. " +
				"This could be because the mod was installed incorrectly.\n" +
				"Please make sure you followed the instructions that the creator " +
				"provided for installing the mod. Make sure you have all of the mods " +
				"that this mod requires. If you continue to have problems, contact the " +
				"mod creator.",
				modName);
		}
		
		public bool ShouldTerminate(string mcOutput)
		{
			return true;
		}
		
		public int GetPriority()
		{
			return 0;
		}
	}
}

