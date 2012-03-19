// 
//  Copyright 2012  Shawn
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
using MultiMC.ProblemDetection;

namespace MultiMC.ProblemDetection
{
	/// <summary>
	/// A simple problem that does relevance detection by simply checking if minecraft's error
	/// string contains a given indicator string
	/// </summary>
	public class BasicProblem : IMinecraftProblem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.BasicProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator string. If mcOutput contains this string,
		/// the problem will be considered relevant
		/// </param>
		/// <param name='errorMessage'>
		/// The error message
		/// </param>
		/// <param name='terminates'>
		/// True if this should cause minecraft's process to be terminated
		/// </param>
		/// <param name='priority'>
		/// The priority of this problem
		/// </param>
		public BasicProblem(string indicator,
		                    string errorMessage,
		                    bool terminates = false,
		                    int priority = 0)
		{
			this.indicators = new string[] { indicator };
			this.errorMessage = errorMessage;
			this.terminates = terminates;
			this.priority = priority;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.BasicProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator strings. If mcOutput contains one of these strings,
		/// the problem will be considered relevant
		/// </param>
		/// <param name='errorMessage'>
		/// The error message
		/// </param>
		/// <param name='terminates'>
		/// True if this should cause minecraft's process to be terminated
		/// </param>
		/// <param name='priority'>
		/// The priority of this problem
		/// </param>
		public BasicProblem(string[] indicators,
		                    string errorMessage,
		                    bool terminates = false,
		                    int priority = 0)
		{
			this.indicators = indicators;
			this.errorMessage = errorMessage;
			this.terminates = terminates;
			this.priority = priority;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.BasicProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator strings. If mcOutput contains one of these strings,
		/// the problem will be considered relevant
		/// </param>
		/// <param name='errorMessage'>
		/// The error message
		/// </param>
		/// <param name='terminates'>
		/// True if this should cause minecraft's process to be terminated
		/// </param>
		/// <param name='priority'>
		/// The priority of this problem
		/// </param>
		public BasicProblem(string errorMessage,
		                    bool terminates = false,
		                    int priority = 0,
		                    params string[] indicators)
		{
			this.indicators = indicators;
			this.errorMessage = errorMessage;
			this.terminates = terminates;
			this.priority = priority;
		}
		
		public virtual string GetErrorMessage(string mcOutput)
		{
			return errorMessage;
		}
		
		public virtual bool ShouldTerminate(string mcOutput)
		{
			return terminates;
		}
		
		public virtual bool IsRelevant(string mcOutput)
		{
			foreach (string indicator in indicators)
			{
				if (mcOutput.Contains(indicator))
					return true;
			}
			return false;
		}
		
		public virtual int GetPriority()
		{
			return priority;
		}
		
		/// <summary>
		/// The indicator strings. If mcOutput contains one of these strings,
		/// the problem will be considered relevant
		/// </summary>
		protected string[] indicators;
		
		/// <summary>
		/// The error message
		/// </summary>
		protected string errorMessage;
		
		/// <summary>
		/// True if this should cause minecraft's process to be terminated
		/// </summary>
		protected bool terminates;
		
		/// <summary>
		/// The priority of this problem
		/// </summary>
		protected int priority;
	}
}

