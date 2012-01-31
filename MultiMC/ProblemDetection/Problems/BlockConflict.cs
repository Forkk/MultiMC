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
	public class BlockConflict : RegexProblem
	{
		public BlockConflict() : base(@"java\.lang\.IllegalArgumentException: Slot (\d+) is already occupied by (.)@[0-9a-f]+ when adding (.)@[0-9a-f]+",
			     "MultiMC has detected a block conflict on slot {0} between {1} and {2}.", true, 1)
		{
		}
		
		public override string GetErrorMessage(string mcOutput)
		{
			Match match = indicators[0].Match(mcOutput);
			return String.Format(errorMessage, match.Groups[0], match.Groups[1], match.Groups[2]);
		}
	}
}

