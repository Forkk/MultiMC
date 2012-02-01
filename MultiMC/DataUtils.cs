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
using System.Text;

namespace MultiMC
{
	/// <summary>
	/// Provides many useful methods for handling / converting data.
	/// </summary>
	public class DataUtils
	{
		private static readonly char[] HexLowerChars = new[] 
		{ 
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
		};
		
		/// <summary>
		/// Gets a string representation of <paramref name="rawbytes"/>
		/// </param>
		public static string HexEncode(byte[] rawbytes)
		{
			int length = rawbytes.Length;
			char[] chArray = new char[2 * length];
			int index = 0;
			int num3 = 0;

			while (index < length)
			{
				chArray[num3++] = HexLowerChars[rawbytes[index] >> 4];
				chArray[num3++] = HexLowerChars[rawbytes[index] & 15];
				index++;
			}
			return new string(chArray);
		}
		
		public static byte[] BytesFromString(string str)
		{
			return new UTF8Encoding().GetBytes(str);
		}
	}
}

