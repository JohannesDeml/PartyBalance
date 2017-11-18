// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimedListChallengeController.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using BitStrap;
using UnityEngine;

namespace Supyrb
{
	public class TimedListChallengeController : ListChallengeController
	{
		[SerializeField, Unit("seconds")]
		private float time = 10f;

		private ActivityTracker activityTracker;
		private WaitForSeconds waitingTime;
		private bool timerRunning = false;

		public override void Initialize(IFinishable challenge)
		{
			activityTracker = ActivityTracker.Instance;
			waitingTime = new WaitForSeconds(time);
			base.Initialize(challenge);
		}

		public override void TriggerFinish(IChallengeController subChallenge)
		{
			if (!timerRunning)
			{
				StartCoroutine(RunTimer());
			}
			base.TriggerFinish(subChallenge);
		}

		public override void FinishChallenge()
		{
			StopCoroutine(RunTimer());
			TimeUp();
			base.FinishChallenge();
		}

		private IEnumerator RunTimer()
		{
			timerRunning = true;
			activityTracker.TriggerStartTimer(time);
			yield return waitingTime;
			TimeUp();
		}

		private void TimeUp()
		{
			timerRunning = false;
			activityTracker.TriggerEndTimer(time);
			StartChallenge();
		}

#if UNITY_EDITOR

		void OnValidate()
		{
			if (Application.isPlaying && waitingTime != null)
			{
				waitingTime = new WaitForSeconds(time);
			}
		}

#endif
	}
}