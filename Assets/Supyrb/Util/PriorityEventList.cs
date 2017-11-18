// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityEventList.cs" company="Supyrb">
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

	public enum EventPriority
	{
		ExtremelyHigh,
		VeryHigh,
		High,
		Default,
		Low,
		VeryLow
	}

	public class PriorityEventList
	{
		public delegate void PriorityEventDelegate();

		public event PriorityEventDelegate ExtremelyHighPriority;
		public event PriorityEventDelegate VeryHighPriority;
		public event PriorityEventDelegate HighPriority;
		public event PriorityEventDelegate DefaultPriority;
		public event PriorityEventDelegate LowPriority;
		public event PriorityEventDelegate VeryLowPriority;

		public PriorityEventList()
		{
		}

		public void Add(EventPriority priority, PriorityEventDelegate action)
		{
			switch (priority)
			{
				case EventPriority.ExtremelyHigh:
					ExtremelyHighPriority += action;
					break;
				case EventPriority.VeryHigh:
					VeryHighPriority += action;
					break;
				case EventPriority.High:
					HighPriority += action;
					break;
				case EventPriority.Default:
					DefaultPriority += action;
					break;
				case EventPriority.Low:
					LowPriority += action;
					break;
				case EventPriority.VeryLow:
					VeryLowPriority += action;
					break;
				default:
					throw new ArgumentOutOfRangeException("priority", priority, null);
			}
		}

		public void Remove(EventPriority priority, PriorityEventDelegate action)
		{
			switch (priority)
			{
				case EventPriority.ExtremelyHigh:
					ExtremelyHighPriority -= action;
					break;
				case EventPriority.VeryHigh:
					VeryHighPriority -= action;
					break;
				case EventPriority.High:
					HighPriority -= action;
					break;
				case EventPriority.Default:
					DefaultPriority -= action;
					break;
				case EventPriority.Low:
					LowPriority -= action;
					break;
				case EventPriority.VeryLow:
					VeryLowPriority -= action;
					break;
				default:
					throw new ArgumentOutOfRangeException("priority", priority, null);
			}
		}

		public void Invoke()
		{
			if (ExtremelyHighPriority != null) ExtremelyHighPriority();
			if (VeryHighPriority != null) VeryHighPriority();
			if (HighPriority != null) HighPriority();
			if (DefaultPriority != null) DefaultPriority();
			if (LowPriority != null) LowPriority();
			if (VeryLowPriority != null) VeryLowPriority();
		}
	}
}