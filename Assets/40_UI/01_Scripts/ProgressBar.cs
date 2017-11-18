// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressBar.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using TMPro;
using UnityEngine.Events;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class ProgressBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform barRect = null;

		[SerializeField]
		private TextMeshProUGUI progressText = null;

		[SerializeField]
		private RectTransform progressTextRect = null;

		[SerializeField]
		private float targetAge = 60f;

		[SerializeField, Range(0f, 1f)]
		private float animationDuration = 0.1f;

		[SerializeField]
		private LeanTweenType easeType = LeanTweenType.easeInOutBounce;

		[SerializeField]
		private UnityEvent onStartAnimatingProgress;

		private Vector2 anchorMax;
		private float currentProgress;

		void Start()
		{
			anchorMax = barRect.anchorMax;
			ChallengeManager.Instance.ProgressChanged += AnimateProgress;
			ChangeProgress(0f);
			ChangeText(0);
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			ChallengeManager.Instance.ProgressChanged -= AnimateProgress;
		}

		private void AnimateProgress(float targetProgress)
		{
			LeanTween.cancel(barRect.gameObject);
			LeanTween.cancel(progressText.gameObject);
			currentProgress = targetProgress;
			onStartAnimatingProgress.Invoke();
			var seq = LeanTween.sequence();
			seq.append(LeanTween.value(barRect.gameObject, ChangeProgress, anchorMax.x, targetProgress, animationDuration)
				.setEase(easeType));
			seq.append(LeanTween.scale(progressTextRect, Vector3.one * 0.4f, 0.07f).setEase(LeanTweenType.easeInBack));
			seq.append(ChangeText);
			seq.append(LeanTween.scale(progressTextRect, Vector3.one, 0.1f).setEase(LeanTweenType.easeInBack));

		}

		private void ChangeText()
		{
			ChangeText(Mathf.FloorToInt(currentProgress * targetAge));
		}

		private void ChangeText(int age) 
		{
			progressText.text = "Alter: " + age.ToString();
		}

		private void ChangeProgress(float progress)
		{
			anchorMax.x = progress;
			barRect.anchorMax = anchorMax;
		}
	}
}

