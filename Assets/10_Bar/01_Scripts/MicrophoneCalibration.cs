// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrophoneCalibration.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using BitStrap;
using UnityEngine.Events;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class MicrophoneCalibration : MonoBehaviour
	{
		public enum State
		{
			Idle,
			CalibratingMaxVolume,
			CalibratingMinVolume
		}

		[Tooltip("The length of volume chunks which average will be used to define the min and max volume")]
		[SerializeField, Unit("frames")]
		private int volumeChunkLength = 6;

		[SerializeField]
		private bool calibrateOnStart = true;

		[SerializeField]
		private string inputButtonName = "Calibration";

		[SerializeField]
		private RectTransform volumeBar = null;

		[SerializeField]
		private BarMicrophoneListener microphoneListener = null;

		[SerializeField, ReadOnly]
		private State state = State.Idle;

		[SerializeField]
		private UnityEvent onStartCalibration = new UnityEvent();

		[SerializeField]
		private UnityEvent onMaxCalibrationStarted = new UnityEvent();

		[SerializeField]
		private UnityEvent onMaxCalibrationFinished = new UnityEvent();

		[SerializeField]
		private UnityEvent onMinCalibrationStarted = new UnityEvent();

		[SerializeField]
		private UnityEvent onMinCalibrationFinished = new UnityEvent();

		[SerializeField]
		private UnityEvent onFinishCalibration = new UnityEvent();

		private MicrophoneInput microphoneInput;
		private MinCalibration minCalibration;
		private MaxCalibration maxCalibration;
		private Calibration calibration;
		private int volumeChunkCounter = 0;
		private float volume;
		private Vector2 volumeBarAnchorMax;

		void Start()
		{
			microphoneInput = MicrophoneInput.Instance;
			minCalibration = new MinCalibration();
			maxCalibration = new MaxCalibration();
			volumeBarAnchorMax = volumeBar.anchorMax;
			if (calibrateOnStart)
			{
				StartCalibrationProcess();
			}
		}

		public void StartCalibrationProcess()
		{
			volumeBar.gameObject.SetActive(true);
			onStartCalibration.Invoke();
			StartMaxVolumeCalibration();
		}

		void Update()
		{
			if (state == State.Idle)
			{
				return;
			}
			if (Input.GetButtonDown(inputButtonName))
			{
				switch (state)
				{
					case State.CalibratingMaxVolume:
						FinishMaxVolumeCalibration();
						break;
					case State.CalibratingMinVolume:
						FinishMinVolumeCalibration();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			volume += microphoneInput.AvgVolume;
			volumeChunkCounter++;
			if (volumeChunkCounter >= volumeChunkLength)
			{
				var avgVolume = volume / volumeChunkCounter;
				bool newValue = calibration.UpdateValue(avgVolume);
				if (newValue)
				{
					volumeBarAnchorMax.y = avgVolume;
					volumeBar.anchorMax = volumeBarAnchorMax;
				}
				volumeChunkCounter = 0;
				volume = 0f;
			}
		}

		private void StartMaxVolumeCalibration()
		{
			ChangeState(State.CalibratingMaxVolume);
			onMaxCalibrationStarted.Invoke();
		}

		private void FinishMaxVolumeCalibration()
		{
			microphoneListener.SetMaxVolume(maxCalibration.Value);
			onMaxCalibrationFinished.Invoke();
			StartMinVolumeCalibration();
		}

		private void StartMinVolumeCalibration()
		{
			ChangeState(State.CalibratingMinVolume);
			onMinCalibrationStarted.Invoke();
		}

		private void FinishMinVolumeCalibration()
		{
			volumeBar.gameObject.SetActive(false);
			microphoneListener.SetMinVolume(minCalibration.Value);
			onMinCalibrationFinished.Invoke();
			FinishCalibrationProcess();
		}

		private void FinishCalibrationProcess()
		{
			ChangeState(State.Idle);
			onFinishCalibration.Invoke();
		}

		private void ChangeState(State newState)
		{
			if (state == newState)
			{
				return;
			}
			state = newState;
			volume = 0f;
			volumeChunkCounter = 0;
			switch (newState)
			{
				case State.Idle:
					calibration = null;
					break;
				case State.CalibratingMaxVolume:
					calibration = maxCalibration;
					calibration.Initialize();
					break;
				case State.CalibratingMinVolume:
					calibration = minCalibration;
					calibration.Initialize();
					break;
				default:
					throw new ArgumentOutOfRangeException("newState", newState, null);
			}
		}
	}

	public class Calibration
	{
		public float Value { get; protected set; }

		public virtual void Initialize()
		{
			Value = 0.5f;
		}

		public virtual bool UpdateValue(float newValue)
		{
			return false;
		}
	}

	public class MinCalibration : Calibration
	{
		public override void Initialize()
		{
			Value = 1.0f;
		}

		public override bool UpdateValue(float newValue)
		{
			if (newValue < Value)
			{
				Value = newValue;
				return true;
			}
			return false;
		}
	}

	public class MaxCalibration : Calibration
	{
		public override void Initialize()
		{
			Value = 0.0f;
		}

		public override bool UpdateValue(float newValue)
		{
			if (newValue > Value)
			{
				Value = newValue;
				return true;
			}
			return false;
		}
	}
}

