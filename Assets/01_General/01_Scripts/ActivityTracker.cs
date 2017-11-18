// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivityTracker.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using BitStrap;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class ActivityTracker : Singleton<ActivityTracker>
	{
		public delegate void ActivationDelegate(bool active);

		public event ActivationDelegate ActivateBar;

		public void TriggerActivateBar(bool activate)
		{
			if (ActivateBar != null)
			{
				ActivateBar(activate);
			}
		}

		public delegate void TimerDelegate(float duration);

		public event TimerDelegate StartTimer;
		public event TimerDelegate EndTimer;

		public void TriggerStartTimer(float duration)
		{
			if (StartTimer != null)
			{
				StartTimer(duration);
			}
		}

		public void TriggerEndTimer(float duration)
		{
			if (EndTimer != null)
			{
				EndTimer(duration);
			}
		}
	}
}

