// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeBarUi.cs" company="Johannes Deml">
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
	
	public class VolumeBarUi : MonoBehaviour
	{
		[SerializeField, Unit("frames")]
		private int updateCycle = 3;

		[SerializeField]
		private RectTransform volumeBar = null;

		[SerializeField]
		private RectTransform maxVolumeBar = null;

		private Vector2 volumeBarAnchorMax;
		private Vector2 maxVolumeBarAnchorMax;
		private MicrophoneInput microphoneInput;
		private int counter;

		void Start()
		{
			volumeBarAnchorMax = volumeBar.anchorMax;
			maxVolumeBarAnchorMax = maxVolumeBar.anchorMax;
			microphoneInput = MicrophoneInput.Instance;
		}

		void Update()
		{
			counter++;
			if (counter < updateCycle)
			{
				return;
			}
			counter = 0;
			volumeBarAnchorMax.x = microphoneInput.AvgVolume;
			maxVolumeBarAnchorMax.x = microphoneInput.MaxVolume;

			volumeBar.anchorMax = volumeBarAnchorMax;
			maxVolumeBar.anchorMax = maxVolumeBarAnchorMax;
		}
	}
}

