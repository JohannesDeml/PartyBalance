// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEventInformation.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using System;
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// An information wrapper with a time stamp and a name to store information about past events in a simple wrapper
	/// </summary>
	[Serializable]
	public class GameEventInformation
	{
		/// <summary>
		/// Create an event information. The string and time of this object can't be changed after they are set in the constructor
		/// </summary>
		/// <param name="eventTime">Time the event happened</param>
		/// <param name="eventName">Name of the event</param>
		public GameEventInformation(float eventTime, string eventName)
		{
			EventTime = eventTime;
			EventName = eventName;
		}

		public float EventTime { get; private set; }
		public string EventName { get; private set; }

		/// <summary>
		/// Print a formatted version of the game event
		/// </summary>
		/// <returns>A formatted string of the event</returns>
		public override string ToString()
		{
			return string.Format("{0}: {1}", EventTime.ToString("0.000"), EventName);
		}
	}
}