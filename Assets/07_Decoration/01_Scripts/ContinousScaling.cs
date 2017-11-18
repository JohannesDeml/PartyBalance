// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContinousScaling.cs" company="Johannes Deml">
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
	
	public class ContinousScaling : MonoBehaviour
	{
		[SerializeField, Unit("seconds")]
		private float cycleDuration = 7f;

		[SerializeField]
		private LeanTweenType easingType = LeanTweenType.easeInOutBack;

		[SerializeField]
		private Vector3 minScale = Vector3.one * 1.5f;

		[SerializeField]
		private Vector3 maxScale = Vector3.one * 3f;

		void OnEnable()
		{
			transform.localScale = minScale;
			var tween = LeanTween.scale(gameObject, maxScale, cycleDuration * 0.5f);
			tween.setEase(easingType)
				.setLoopPingPong();
		}

		void OnDisable()
		{
			LeanTween.cancel(gameObject);
		}
	}
}

