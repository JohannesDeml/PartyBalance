// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrophoneInput.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
//	 https://forum.unity3d.com/threads/check-current-microphone-input-volume.133501/#post-2067251
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

	public class MicrophoneInput : Singleton<MicrophoneInput>
	{
		[SerializeField]
		private int sampleWindow = 128;

		[SerializeField, ReadOnly]
		private string device;
		[SerializeField, ReadOnly]
		private float maxVolume;
		[SerializeField, ReadOnly]
		private float avgVolume;

		public float MaxVolume
		{
			get { return maxVolume; }
		}

		public float AvgVolume
		{
			get { return avgVolume; }
		}

		public string Device
		{
			get { return device; }
		}

		private AudioClip clipRecord = new AudioClip();
		private bool activeDevice;
		private float[] tempWaveData;

		void Awake()
		{
			tempWaveData = new float[sampleWindow];
		}

		void OnEnable()
		{
			SetDevice(device);
		}

		void OnDisable()
		{
			StopMicrophone();
		}

		//mic initialization
		public void SetDevice(string deviceName)
		{
			if (string.IsNullOrEmpty(deviceName) || !IsValidDeviceName(deviceName))
			{
				if (Microphone.devices.Length == 0)
				{
					Debug.LogWarning("No audio input devices found");
					return;
				}
				deviceName = Microphone.devices[0];
			}

			if (activeDevice)
			{
				if (device == deviceName)
				{
					// Already active and set
					return;
				}
				StopMicrophone();
			}

			device = deviceName;
			clipRecord = Microphone.Start(device, true, 999, 44100);
			activeDevice = true;
		}

		private bool IsValidDeviceName(string deviceName)
		{
			var devices = Microphone.devices;
			for (int i = 0; i < devices.Length; i++)
			{
				if (devices[i] == deviceName)
				{
					return true;
				}
			}
			return false;
		}

		void StopMicrophone()
		{
			Microphone.End(device);
			activeDevice = false;
		}


		//get data from microphone into audioclip
		private void UpdateMicrophoneInformation()
		{
			int micPosition = Microphone.GetPosition(device) - (sampleWindow + 1); // null means the first microphone
			if (micPosition < 0)
			{
				maxVolume = 0;
				avgVolume = 0;
			}
			clipRecord.GetData(tempWaveData, micPosition);

			float max = 0f;
			float sum = 0f;

			for (int i = 0; i < sampleWindow; i++)
			{
				float wavePeak = tempWaveData[i] * tempWaveData[i];
				sum += wavePeak;
				if (max < wavePeak)
				{
					max = wavePeak;
				}
			}
			maxVolume = max;
			avgVolume = sum / sampleWindow;
		}



		void Update()
		{
			if (activeDevice)
			{
				UpdateMicrophoneInformation();
			}
		}

		protected override void OnDestroy()
		{
			StopMicrophone();
		}
		
		// make sure the mic gets started & stopped when application gets focused
		//void OnApplicationFocus(bool focus)
		//{
		//	if (focus)
		//	{
		//		if (!activeDevice)
		//		{
		//			SetDevice(device);
		//		}
		//	}
		//	if (!focus)
		//	{
		//		StopMicrophone();
		//	}
		//}
	}
}

