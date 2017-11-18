// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMaxRange.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	using System;

	[Serializable]
	public class MinMaxRange
	{
		public float MinLimit = 0f;
		public float MaxLimit = 1f;
		public float MinValue = 0f;
		public float MaxValue = 1f;

		public float ValueRange
		{
			get { return MaxValue - MinValue; }
		}

		public float LimitRange
		{
			get { return MaxLimit - MinLimit; }
		}

		public MinMaxRange(float minLimit, float maxLimit, float minValue, float maxValue)
		{
			MinLimit = minLimit;
			MaxLimit = maxLimit;
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public MinMaxRange() : this(0f, 1f, 0f, 1f)
		{
		}

		public MinMaxRange(float minLimit, float maxLimit) : this(minLimit, maxLimit, minLimit, maxLimit)
		{
		}

		/// <summary>
		/// Creates random value between <see cref="MinValue"/> [Included] and <see cref="MaxValue"/> [Excluded]
		/// </summary>
		/// <returns>A random value between <see cref="MinValue"/> [Included] and <see cref="MaxValue"/> [Excluded]</returns>
		public float GetRandomValue()
		{
			return UnityEngine.Random.Range(MinValue, MaxValue);
		}

		/// <summary>
		/// Creates random value between <see cref="MinValue"/> [Included] and <see cref="MaxValue"/> [Excluded]
		/// </summary>
		/// <returns>A random value between <see cref="MinValue"/> [Included] and <see cref="MaxValue"/> [Excluded]</returns>
		public int GetRandomIntValue()
		{
			return Mathf.RoundToInt(UnityEngine.Random.Range(MinValue, MaxValue));
		}
	}
}