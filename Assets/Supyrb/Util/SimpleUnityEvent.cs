// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleUnityEvent.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine.Events;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class SimpleUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private bool invokeOnAwake = false;

		[SerializeField]
		private bool invokeOnStart = false;

		[SerializeField]
		private bool invokeOnEnable = false;

		[SerializeField]
		private bool invokeOnDisable = false;

		[SerializeField]
		private bool invokeOnDestroy = false;

		[SerializeField]
		private bool invokeOnApplicationPause = false;

		[SerializeField]
		private bool invokeOnApplicationResume = false;

		public UnityEvent Event;

		void Awake()
		{
			if (invokeOnAwake)
			{
				InvokeEvent();
			}
		}

		void OnEnable()
		{
			if (invokeOnEnable)
			{
				InvokeEvent();
			}
		}

		void Start()
		{
			if (invokeOnStart)
			{
				InvokeEvent();
			}
		}

		void OnDisable()
		{
			if (invokeOnDisable)
			{
				InvokeEvent();
			}
		}

		void OnDestroy()
		{
			if (invokeOnDestroy)
			{
				InvokeEvent();
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause && invokeOnApplicationPause)
			{
				InvokeEvent();
				return;
			}

			if (!pause && invokeOnApplicationResume)
			{
				InvokeEvent();
			}
		}

		public virtual void InvokeEvent()
		{
			Event.Invoke();
		}
	}
}