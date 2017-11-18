// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mountain.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using BitStrap;
using UnityEngine;

namespace Supyrb
{
	public class Mountain : MonoBehaviour
	{
		[SerializeField]
		private EnvironmentFace face;

		[SerializeField, Unit("seconds")]
		private float duration = 0.5f;

		[SerializeField]
		private LeanTweenType easingType = LeanTweenType.easeOutCirc;

		[SerializeField]
		private float jumpHeight = 3f;

		[SerializeField]
		private int repeat = 2;

		private Vector3 startPosition;

		void Start()
		{
			startPosition = transform.localPosition;
			ChallengeManager.Instance.ProgressChanged += OnProgressChanged;
			ChallengeManager.Instance.AllChallengesFinished += OnAllChallengesFinished;
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			ChallengeManager.Instance.ProgressChanged -= OnProgressChanged;
			ChallengeManager.Instance.AllChallengesFinished -= OnAllChallengesFinished;
		}

		private void OnProgressChanged(float progress)
		{
			PlayAnimation(repeat);
		}

		private void OnAllChallengesFinished(float progress)
		{
			PlayAnimation(-1);
		}

		private void PlayAnimation(int loops)
		{
			LeanTween.cancel(gameObject);
			transform.localPosition = startPosition;
			face.SetChangePosition(true);
			LeanTween.moveLocalY(gameObject, transform.localPosition.y + jumpHeight, duration)
				.setEase(easingType)
				.setLoopPingPong(loops)
				.setOnComplete(() => { face.SetChangePosition(false); });
		}

#if UNITY_EDITOR

		[Button]
		private void PlayAnimation()
		{
			PlayAnimation(repeat);
		}

#endif
	}
}