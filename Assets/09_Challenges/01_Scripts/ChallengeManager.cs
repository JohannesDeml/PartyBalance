// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChallengeManager.cs" company="Johannes Deml">
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
	
	public class ChallengeManager : Singleton<ChallengeManager>
	{
		public delegate void ProgressUpdateDelegate(float progress);
		public event ProgressUpdateDelegate ProgressChanged;
		public event ProgressUpdateDelegate AllChallengesFinished;

		[SerializeField]
		private Challenges challenges = null;

		[SerializeField]
		private KeyCode finishCurrentChallenge = KeyCode.Alpha3;

		public float CurrentProgress { get; private set; }

		void Start()
		{
			challenges.Initialize(this);
			StartChallenges();
		}

		void Update()
		{
			if (Input.GetKeyDown(finishCurrentChallenge) && challenges.ActiveChallenge != null)
			{
				challenges.ActiveChallenge.TriggerFinish();
			}
		}

		public void StartChallenges()
		{
			CurrentProgress = 0f;
			challenges.StartChallenges();
		}

		public void TriggerProgressChanged(float progress)
		{
			CurrentProgress = progress;
			if (ProgressChanged != null)
			{
				ProgressChanged(progress);
			}
		}

		public void TriggerAllChallengesFinished()
		{
			TriggerProgressChanged(1f);

			if (AllChallengesFinished != null)
			{
				AllChallengesFinished(1f);
			}
		}
	}
}

