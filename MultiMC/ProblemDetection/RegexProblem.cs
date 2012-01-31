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
using System.Text.RegularExpressions;

namespace MultiMC.ProblemDetection
{
	/// <summary>
	/// A slightly more powerful problem that is sililar to BasicProblem, but is powered by Regular expressions.
	/// </summary>
	public class RegexProblem : IMinecraftProblem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.RegexProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator string. Has to be a Regular Expression. Will be relevant if it matches.
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
		public RegexProblem(string indicator,
		                    string errorMessage,
		                    bool terminates = false,
		                    int priority = 0)  : this(new String[]{indicator},errorMessage,terminates,priority)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.RegexProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator strings. All have to be Regular Expressions. Will be relevant if it matches.
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
		public RegexProblem(string[] indicators,
		                    string errorMessage,
		                    bool terminates = false,
		                    int priority = 0)
		{
			this.indicators = new Regex[indicators.Length];
			for (int i = 0; i<indicators.Length; i++)
			{
				this.indicators[i] = new Regex(indicators[i], RegexOptions.Compiled);
			}
			
			this.errorMessage = errorMessage;
			this.terminates = terminates;
			this.priority = priority;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiMC.RegexProblem"/> class.
		/// </summary>
		/// <param name='indicator'>
		/// The indicator strings. All have to be Regular Expressions. Will be relevant if it matches.
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
		public RegexProblem(string errorMessage,
		                    bool terminates = false,
		                    int priority = 0,
		                    params string[] indicators) : this(indicators,errorMessage,terminates,priority)
		{
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
			foreach (Regex regex in indicators)
			{
				if (regex.IsMatch(mcOutput))
					return true;
			}
			return false;
		}
		
		public virtual int GetPriority()
		{
			return priority;
		}
		
		/// <summary>
		/// The registed Regex matches.
		/// </summary>
		protected Regex[] indicators;
		
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

