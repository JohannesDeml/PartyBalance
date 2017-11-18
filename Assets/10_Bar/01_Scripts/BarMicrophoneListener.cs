// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarMicrophoneListener.cs" company="Johannes Deml">
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
	
	public class BarMicrophoneListener : MonoBehaviour 
	{
		[SerializeField]
		private float maxVolume = 0.25f;

		[SerializeField]
		private float minVolume = 0.01f;

		private MicrophoneInput microphone;

		void Start()
		{
			microphone = MicrophoneInput.Instance;
		}

		public void SetMaxVolume(float volume)
		{
			maxVolume = volume;
		}

		public void SetMinVolume(float volume)
		{
			minVolume = volume;
		}

		public float GetPlayerInput()
		{
			return Mathf.Clamp01((microphone.MaxVolume - minVolume) / (maxVolume-minVolume));
		}
	}
}

