﻿using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Provides a set of methods to aux non-layout editor code.
	/// It contains methods that smartly positions Rects in the Inspector.
	/// </summary>
	public static class RectExtensions
	{
		/// <summary>
		/// Given a source big Rect, it returns a sub Rect with same width
		/// but with the height of EditorGUIUtility.singleLineHeight.
		/// Also, its Y position is calculated as if it's the "rownNumber'th"
		/// Rect from top to bottom.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="rownNumber"></param>
		/// <returns></returns>
		public static Rect Row( this Rect position, int rownNumber )
		{
			position.yMin += EditorGUIUtility.singleLineHeight * Mathf.Abs( rownNumber );

			if( rownNumber >= 0 )
			{
				position.height = EditorGUIUtility.singleLineHeight;
			}

			return position;
		}

		/// <summary>
		/// Given a Rect, it returns a left anchored copy with a width of "width".
		/// If a negative number is passed, it behaves as if there is a space on the right with a width of "width".
		/// </summary>
		/// <param name="position"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static Rect Left( this Rect position, float width )
		{
			if( width > 0.0f )
				position.width = width;
			else
				position.width += width;

			return position;
		}

		/// <summary>
		/// Given a Rect, it returns a right anchored copy with a width of "width".
		/// If a negative number is passed, it behaves as if there is a space on the left with a width of "width".
		/// </summary>
		/// <param name="position"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public static Rect Right( this Rect position, float width )
		{
			if( width > 0.0f )
				position.xMin = position.xMax - width;
			else
				position.xMin -= width;

			return position;
		}
	}
}
