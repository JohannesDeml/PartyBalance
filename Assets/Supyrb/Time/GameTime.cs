// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTime.cs" company="Supyrb">
//   Copyright (c) 2015 Supyrb. All rights reserved.
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
	using System;

	/// <summary>
	/// This class is like <see cref="Time"/> but includes information about the ongoing game as well as a 
	/// timescale independent time
	/// Game is always refers to the whole application while race refers to one game session (the game itself)
	/// </summary>
	public class GameTime : Singleton<GameTime>
	{
		[Tooltip("Defines if the game should be started in paused mode. Beware that the timescale is zero in this case")]
		[SerializeField]
		private bool startPaused = false;

		/// <summary>
		/// The time passed since the game started
		/// </summary>
		public static float TimeSinceStartup
		{
			get { return Time.realtimeSinceStartup; }
		}

		/// <summary>
		/// The delta time for updates independent of the timescale
		/// </summary>
		public static float UnscaledDeltaTime
		{
			get { return Time.unscaledDeltaTime; }
		}

		/// <summary>
		/// The current time of the race. If the game is paused, this value does not increase
		/// </summary>
		public static float RaceTime { get; private set; }

		/// <summary>
		/// The delta time for updates dependent of the time scale
		/// </summary>
		public static float DeltaRaceTime
		{
			get { return Time.deltaTime; }
		}

		/// <summary>
		/// The delta time for updates dependent of the time scale. 
		/// Smoothing values as explained here: https://forum.unity3d.com/threads/time-smoothdeltatime.10253/
		/// </summary>
		public static float SmoothDeltaRaceTime
		{
			get { return Time.smoothDeltaTime; }
		}

		/// <summary>
		/// The delta time for fixed updates dependent of the time scale
		/// </summary>
		public static float FixedDeltaRaceTime
		{
			get { return Racing ? Time.fixedDeltaTime : 0f; }
		}

		/// <summary>
		/// Returns whether or not the game is in an active race 
		/// </summary>
		public static bool Racing
		{
			get { return inRaceMode && racing.ModifiedValue; }
		}

		private static bool inRaceMode;

		/// <summary>
		/// The string used for pausing the game. Can be removed if the game is not continued normally.
		/// </summary>
		private static readonly int InternalModifier = 0;

		public static float TimeScale
		{
			get { return timeScale.ModifiedValue; }
		}

		public delegate void GameTimeDelegate();

		public static event GameTimeDelegate OnPauseGame;
		public static event GameTimeDelegate OnContinueGame;

		private static MultiFloatMultiply timeScale = new MultiFloatMultiply(1f, 8);
		private static MultiBoolAnd racing = new MultiBoolAnd(8);

		/// <summary>
		/// Make sure the singleton can't be created by anyone else
		/// </summary>
		protected GameTime()
		{
			timeScale.SetModifier(InternalModifier, 1f);
			racing.SetModifier(InternalModifier, true);
		}

		void Awake()
		{
			ForceSingletonInstance();

			if (startPaused)
			{
				SetTimeScaleModifier(InternalModifier, 0f);
			}
			else
			{
				UpdateTimeScale();
			}
		}

		/// <summary>
		/// Update the game time and race time
		/// </summary>
		private void Update()
		{
			RaceTime += Racing ? Time.deltaTime : 0f;
		}

		public static void StartRace()
		{
			RaceTime = 0f;
			ContinueRace();
			inRaceMode = true;
		}

		public static void TogglePause()
		{
			if (racing.Modifiers[InternalModifier])
			{
				PauseRace();
			}
			else
			{
				ContinueRace();
			}
		}

		public static void PauseRace()
		{
			SetRacingModifier(InternalModifier, false);
			SetTimeScaleModifier(InternalModifier, 0f);
			if (OnPauseGame != null)
			{
				OnPauseGame();
			}
		}

		private static void UpdateTimeScale()
		{
			if (Mathf.Approximately(Time.timeScale, TimeScale))
			{
				return;
			}
			Time.timeScale = TimeScale;
		}

		public static void EndRace()
		{
			inRaceMode = false;
			//SetTimeScaleModifier("EndRace", 0f);
		}

		public static void ContinueRace()
		{
			SetRacingModifier(InternalModifier, true);
			SetTimeScaleModifier(InternalModifier, 1);
			if (OnContinueGame != null)
			{
				OnContinueGame();
			}
		}

		public static int SetTimeScaleModifier(float newValue)
		{
			var index = timeScale.SetModifier(newValue);
			UpdateTimeScale();
			return index;
		}

		public static void SetTimeScaleModifier(int index, float newValue)
		{
			timeScale.SetModifier(index, newValue);
			UpdateTimeScale();
		}

		public static void RemoveTimeScaleModifier(int index)
		{
			timeScale.RemoveModifier(index);
			UpdateTimeScale();
		}

		public static int SetRacingModifier(bool value)
		{
			return racing.SetModifier(value);
		}

		public static void SetRacingModifier(int index, bool value)
		{
			racing.SetModifier(index, value);
		}

		public static void RemoveRacingModifier(int index)
		{
			racing.RemoveModifier(index);
		}

#if UNITY_EDITOR
		public MultiFloatMultiply GetTimeManipulator()
		{
			return timeScale;
		}
#endif
	}
}