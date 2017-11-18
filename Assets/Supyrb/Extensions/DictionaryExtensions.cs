// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Text;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	public static class DictionaryExtensions
	{
		/// <summary>
		/// Create a string of key-value pairs in a dictionary
		/// </summary>
		/// <typeparam name="TKey">Type of the key</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <param name="dictionary">The dictionary from which the string is generated from</param>
		/// <param name="start">string that is added to the front of the returned string</param>
		/// <param name="separatorEntries">Separator string in between all values</param>
		/// <param name="separatorKeyValue">Separator string between key and value for every entry</param>
		/// <param name="end">string that is added to the back of the returned string</param>
		/// <returns>A string which shows information about all values in a string</returns>
		public static string ToFormattedString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
			string start = "[", string separatorEntries = ", ", string separatorKeyValue = ": ", string end = "]")
		{
			StringBuilder builder = new StringBuilder(start);
			foreach (var pair in dictionary)
			{
				builder.Append(pair.Key.ToString() + separatorKeyValue + pair.Value.ToString());
				builder.Append(separatorEntries);
			}
			builder.Remove(builder.Length - separatorEntries.Length, separatorEntries.Length);
			builder.Append(end);
			return builder.ToString();
		}

		/// <summary>
		/// Creates a new key entry if the key is not yet in the dictionary, otherwise the value in the 
		/// dictionary is increased by value
		/// </summary>
		/// <typeparam name="TKey">Key of the dictionary</typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key">Key for which the value should be set or added</param>
		/// <param name="value">The value that should be set or added</param>
		public static void AddOrIncreaseValue<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int value)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] += value;
			}
			else
			{
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Creates a new key if it doesn't exist in the dictionary with a value of 1, otherwise it increases
		/// the current value by 1
		/// </summary>
		/// <typeparam name="TKey">Key of the dictionary</typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key">Key for which the value should be set or incremented</param>
		public static void AddOrIncrement<TKey>(this IDictionary<TKey, int> dictionary, TKey key)
		{
			dictionary.AddOrIncreaseValue(key, 1);
		}
	}
}