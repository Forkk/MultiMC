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
using System.Collections;
using System.Collections.Generic;

namespace MultiMC
{
	/// <summary>
	/// Provides many useful methods for handling / converting data.
	/// </summary>
	public static class DataUtils
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
		
		public static string ArrayToString(IEnumerable array, string separator = ", ")
		{
			StringBuilder builder = new StringBuilder();
			foreach (object obj in array)
				builder.Append(string.Format("{0}{1}", obj.ToString(), separator));
			if (builder.Length > 0)
				builder.Length--;
			return builder.ToString();
		}

		public static string ToString(this IEnumerable array, string separator = ", ")
		{
			return ArrayToString(array, separator);
		}

		// This is why I love C#

		public static T[] WhereTrue<T>(this IEnumerable<T> array, Func<T, bool> func)
		{
			List<T> l = new List<T>();
			foreach (T value in array)
			{
				if (func.Invoke(value))
					l.Add(value);
			}
			return l.ToArray();
		}
	}
}

