// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChallengeListener.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
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
	
	public class ChallengeListener : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent onProgressChanged;

		[SerializeField]
		private UnityEvent onAllChallengesFinished;

		private ChallengeManager challengeManager;

		void Start()
		{
			challengeManager = ChallengeManager.Instance;
			challengeManager.ProgressChanged += OnProgressChanged;
			challengeManager.AllChallengesFinished += OnAllChallengesFinished;
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			challengeManager.ProgressChanged -= OnProgressChanged;
			challengeManager.AllChallengesFinished -= OnAllChallengesFinished;
		}

		private void OnProgressChanged(float progress)
		{
			onProgressChanged.Invoke();
		}

		private void OnAllChallengesFinished(float progress)
		{
			onAllChallengesFinished.Invoke();
		}
	}
}

