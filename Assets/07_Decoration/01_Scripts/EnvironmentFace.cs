// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentFace.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class EnvironmentFace : MonoBehaviour
	{
		[Tooltip("If not set it will set the player as target")]
		[SerializeField]
		private Transform target = null;

		[SerializeField]
		private float maxMovement = 0.3f;

		[SerializeField, Range(0f, 1f)]
		private float distanceMultiplicator = 0.1f;

		[SerializeField]
		private float lerpSpeed = 7f;

		[SerializeField]
		private bool changesPosition = false;

		private Vector3 centerPositionLocal;
		private Vector3 centerPosition;
		private Vector3 toTarget;
		private Vector3 currentPositionLocal;

		void Start()
		{
			if (target == null)
			{
				target = GlobalData.Instance.PointOfInterestPlayer;
			}
			centerPositionLocal = transform.localPosition;
			centerPosition = transform.position;
			currentPositionLocal = transform.localPosition;
		}

		void Update()
		{
			if (changesPosition)
			{
				centerPosition = transform.position - transform.localPosition + centerPositionLocal;
			}
			toTarget = target.position - centerPosition;
			float distance = Mathf.Min(toTarget.magnitude * distanceMultiplicator, maxMovement);
			var targetPositionLocal = centerPositionLocal + toTarget.normalized * distance;
			currentPositionLocal = Vector3.Lerp(currentPositionLocal, targetPositionLocal, lerpSpeed * GameTime.DeltaRaceTime);
			transform.localPosition = currentPositionLocal;
		}

		public void SetChangePosition(bool isChangingPosition)
		{
			changesPosition = isChangingPosition;
		}

#if UNITY_EDITOR

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.gray;
			if (Application.isPlaying)
			{
				Gizmos.DrawWireSphere(centerPosition, maxMovement);
			}
			else
			{
				Gizmos.DrawWireSphere(transform.position, maxMovement);
			}
		}
#endif
	}
}

