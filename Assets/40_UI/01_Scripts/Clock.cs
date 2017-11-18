// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Clock.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine.UI;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class Clock : MonoBehaviour
	{
		[SerializeField]
		private GameObject rootObject = null;

		[SerializeField]
		private RectTransform arrow = null;

		[SerializeField]
		private Image overlayImage = null;

		[SerializeField]
		private float scaleChange = 0.3f;

		[SerializeField]
		private float numberOfBeats = 4f;

		private bool initialized = false;
		private ActivityTracker activityTracker;

		private Vector3 arrowEulerRotation = Vector3.zero;
		private Vector3 rootObjectScale = Vector3.one;

		void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (initialized)
			{
				return;
			}
			rootObject.SetActive(false);
			activityTracker = ActivityTracker.Instance;
			activityTracker.StartTimer += OnStartTimer;
			activityTracker.EndTimer += OnEndTimer;
			initialized = true;
		}

		void OnDestroy()
		{
			Release();
		}

		private void Release()
		{
			if (!initialized)
			{
				return;
			}
			LeanTween.cancel(rootObject);
			activityTracker.StartTimer -= OnStartTimer;
			activityTracker.EndTimer -= OnEndTimer;
			initialized = false;
		}

		private void OnStartTimer(float duration)
		{
			OnUpdateTimer(0f);
			rootObject.SetActive(true);
			LeanTween.cancel(rootObject);
			LeanTween.value(rootObject, OnUpdateTimer, 0f, 1f, duration);
		}

		private void OnUpdateTimer(float progress)
		{
			arrowEulerRotation.z = 360f * progress;
			arrow.rotation = Quaternion.Euler(arrowEulerRotation);

			overlayImage.fillAmount = progress;

			rootObjectScale = Vector3.one * (1f + scaleChange * progress * Mathf.Abs(Mathf.Sin(Mathf.PI * progress * numberOfBeats)));
			rootObject.transform.localScale = rootObjectScale;
		}

		private void OnEndTimer(float duration)
		{
			LeanTween.cancel(rootObject);
			rootObject.SetActive(false);
		}
	}
}

