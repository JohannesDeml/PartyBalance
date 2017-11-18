// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Challenge.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Supyrb
{
	[Serializable]
	public class Challenge : IFinishable
	{
		[SerializeField]
		private ChallengeData data;

		private Challenges parent;
		private int challengeIndex;
		private GameObject challengeInstance;
		private IChallengeController challengeController;

		public Challenge(ChallengeData data)
		{
			this.data = data;
		}

		public void Initialize(Challenges challenges, int index)
		{
			parent = challenges;
			challengeIndex = index;
			challengeInstance = Object.Instantiate(data.ChallengePrefab);
			challengeInstance.name = "Challenge " + index;
			challengeController = challengeInstance.GetComponent<IChallengeController>();
			challengeController.Initialize(this);
			challengeInstance.SetActive(false);
		}

		public void StartChallenge()
		{
			challengeController.StartChallenge();
		}

		public void TriggerFinish()
		{
			TriggerFinish(challengeController);
		}

		public void TriggerFinish(IChallengeController finishedController)
		{
			parent.ChallengeFinished(challengeIndex);
			finishedController.FinishChallenge();
		}
	}
}