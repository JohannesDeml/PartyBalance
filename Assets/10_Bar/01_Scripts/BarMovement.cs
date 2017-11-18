// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarMovement.cs" company="Johannes Deml">
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
	
	[RequireComponent(typeof(Rigidbody2D))]
	public class BarMovement : MonoBehaviour
	{
		[SerializeField]
		private Transform bottomPosition = null;
		[SerializeField]
		private Transform topPosition = null;
		[SerializeField]
		private BarMicrophoneListener inputListener = null;

		[SerializeField]
		private float smoothTimeUp = 0.15f;

		[SerializeField, Unit("%/s")]
		private float maxMovementSpeedUp = 4f;

		[SerializeField]
		private float smoothTimeDown = 0.3f;

		[SerializeField, Unit("%/s")]
		private float maxMovementSpeedDown = 2f;

		[SerializeField]
		private bool active = true;

		[SerializeField, ReadOnly]
		private Rigidbody2D rigidBody;

		[SerializeField, ReadOnly]
		private float currentPosition = 0f;

		[SerializeField, ReadOnly]
		private float targetPosition = 0f;

		private float minPositionY;
		private float maxPositionY;
		private float currentVelocity;
		private Vector2 tempPosition;

		void Awake()
		{
			minPositionY = bottomPosition.position.y;
			maxPositionY = topPosition.position.y;
			tempPosition = transform.position;
		}

		void Start()
		{
			ActivityTracker.Instance.ActivateBar += SetBarActive;
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			ActivityTracker.Instance.ActivateBar -= SetBarActive;
		}

		private void SetBarActive(bool isOn)
		{
			active = isOn;
		}

		void FixedUpdate()
		{
			if (!active)
			{
				return;
			}
			targetPosition = inputListener.GetPlayerInput();
			if (targetPosition > currentPosition)
			{
				currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref currentVelocity,
					smoothTimeUp, maxMovementSpeedUp);
			}
			else
			{
				currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref currentVelocity,
					smoothTimeDown, maxMovementSpeedDown);
			}
			
			tempPosition.y = Mathf.Lerp(minPositionY, maxPositionY, currentPosition);
			rigidBody.MovePosition(tempPosition);
		}

#if UNITY_EDITOR

		void Reset()
		{
			if (rigidBody == null)
			{
				rigidBody = GetComponent<Rigidbody2D>();
				rigidBody.isKinematic = true;
				rigidBody.gravityScale = 0;
			}
		}

#endif
	}
}

