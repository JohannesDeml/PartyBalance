// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitForSecondsCustomRealtime.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
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
	
	public class WaitForSecondsCustomRealtime : CustomYieldInstruction
	{
		private readonly float duration;
		private float endTime;

		public override bool keepWaiting
		{
			get { return endTime > Time.realtimeSinceStartup; }
		}

		public WaitForSecondsCustomRealtime(float seconds)
		{
			duration = seconds;
			UpdateEndTime();
		}

		public void UpdateEndTime()
		{
			endTime = duration + Time.realtimeSinceStartup;
		}
	}
}

