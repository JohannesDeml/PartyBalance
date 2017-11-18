// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public static class ListExtensions
	{
		/// <summary>
		/// Create a string of the values in a list
		/// </summary>
		/// <typeparam name="T">Type of the list</typeparam>
		/// <param name="list">The list from which the string is generated from</param>
		/// <param name="start">string that is added to the front of the returned string</param>
		/// <param name="separator">A separator string in between all values</param>
		/// <param name="end">string that is added to the back of the returned string</param>
		/// <returns>A string which shows information about all values in a string</returns>
		public static string ToFormattedString<T>(this List<T> list,
			string start = "[", string separator = ", ", string end = "]")
		{
			string formattedString = start;
			for (int i = 0; i < list.Count - 1; i++)
			{
				formattedString += list[i].ToString() + separator;
			}
			if (list.Count != 0)
			{
				formattedString += list[list.Count - 1];
			}
			formattedString += end;
			return formattedString;
		}

		/// <summary>
		/// Create a string of the values in the array
		/// </summary>
		/// <typeparam name="T">Type of the array</typeparam>
		/// <param name="array">The array from which the string is generated from</param>
		/// <param name="start">string that is added to the front of the returned string</param>
		/// <param name="separator">A separator string in between all values</param>
		/// <param name="end">string that is added to the back of the returned string</param>
		/// <returns>A string which shows information about all values in a string</returns>
		public static string ToFormattedString<T>(this T[] array,
			string start = "[", string separator = ", ", string end = "]")
		{
			string formattedString = start;
			for (int i = 0; i < array.Length - 1; i++)
			{
				formattedString += array[i].ToString() + separator;
			}
			if (array.Length != 0)
			{
				formattedString += array[array.Length - 1];
			}
			formattedString += end;
			return formattedString;
		}

		/// <summary>
		/// Get an array index within the clamped range of the array
		/// </summary>
		/// <typeparam name="T">Type of the array</typeparam>
		/// <param name="array">The array to get the index from, can not be null</param>
		/// <param name="index">Index value, will be clamped within the array range</param>
		/// <returns></returns>
		public static T GetIndexSecure<T>(this T[] array,
			int index)
		{
			if (array == null)
			{
				Debug.LogErrorFormat("Trying to get index from array of type {0}, but the array is null", typeof(T));
				return default(T);
			}
			return array[Mathf.Clamp(index, 0, array.Length - 1)];
		}

		public static int GetFirstIndex<T>(this T[] array, T element, int startIndex = 0)
		{
			if (array == null)
			{
				throw new NullReferenceException();
			}
			for (int i = startIndex; i < array.Length; i++)
			{
				if (Equals(array[i], element))
				{
					return i;
				}
			}
			return -1;
		}

		public static T GetRandom<T>(this T[] array)
		{
			if (array == null || array.Length == 0)
			{
				return default(T);
			}
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static T LastEntry<T>(this T[] array)
		{
			if (array == null || array.Length == 0)
			{
				return default(T);
			}
			var length = array.Length;
			return array[length - 1];
		}

        public static T LastEntry<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }
            var length = list.Count;
            return list[length - 1];
        }
    }
}