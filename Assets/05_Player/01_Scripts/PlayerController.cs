// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using BitStrap;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody2D rigidBody;

		[SerializeField]
		private Transform bodyTransform = null;

		[SerializeField]
		private Animator anim;

		[SerializeField]
		private FloatAnimationParameter animatorHorizontalSpeed;

		[SerializeField]
		private FloatAnimationParameter animatorVerticalSpeed;

		[SerializeField]
		private BoxCollider2D movementBoxLeft;

		[SerializeField]
		private BoxCollider2D movementBoxRight;

		[SerializeField]
		private LayerMask movementCollisionLayerMask = new LayerMask();

		[SerializeField]
		private float movementSpeed = 3f;

		[SerializeField]
		private float inputLerpSpeed = 5f;

		private float horizontalInput;
		private float horizontalVelocity;
		private Vector2 velocity = Vector2.zero;
		private RaycastHit2D[] hits;
		private Vector3 bodyScale;

		void Awake()
		{
			hits = new RaycastHit2D[1];
			bodyScale = bodyTransform.localScale;
			GlobalData.Instance.PointOfInterestPlayer = transform;
		}

		void Update()
		{
			UpdateAnimations();
		}

		private void UpdateAnimations()
		{
			var horizontalSpeed = rigidBody.velocity.x;
			var absHorizontalSpeed = Math.Abs(horizontalSpeed);
			bodyScale = bodyTransform.localScale;
			if (horizontalSpeed < 0f)
			{
				bodyScale.x = -1f;
			}
			else
			{
				bodyScale.x = 1f;
			}
			bodyTransform.localScale = bodyScale;
			anim.SetFloat(animatorHorizontalSpeed.Index, absHorizontalSpeed);
			anim.SetFloat(animatorVerticalSpeed.Index, rigidBody.velocity.y);
		}

		void FixedUpdate()
		{
			UpdateInput();
			horizontalVelocity = horizontalInput * movementSpeed;
			UpdateMovementCollisions();
			velocity.x = horizontalVelocity;
			velocity.y = rigidBody.velocity.y;
			rigidBody.velocity = velocity;
		}

		private void UpdateMovementCollisions()
		{
			if (Mathf.Abs(horizontalVelocity) < 0.05f)
			{
				return;
			}

			if (horizontalVelocity < 0)
			{
				int numHits = Physics2D.BoxCastNonAlloc(movementBoxLeft.GetOrigin(),
					movementBoxLeft.size,
					movementBoxLeft.transform.rotation.z,
					Vector2.zero,
					hits,
					100f,
					movementCollisionLayerMask);
				if (numHits > 0)
				{
					horizontalVelocity = 0f;
				}
			}
			else
			{
				int numHits = Physics2D.BoxCastNonAlloc(movementBoxRight.GetOrigin(),
					movementBoxRight.size,
					movementBoxRight.transform.rotation.z,
					Vector2.zero,
					hits,
					100f,
					movementCollisionLayerMask);
				if (numHits > 0)
				{
					horizontalVelocity = 0f;
				}
			}
		}

		private void UpdateInput()
		{
			float input = Input.GetAxis("Horizontal");
			horizontalInput = Mathf.Lerp(horizontalInput, input, GameTime.FixedDeltaRaceTime * inputLerpSpeed);
		}


#if UNITY_EDITOR

		void Reset()
		{
			if (rigidBody == null)
			{
				rigidBody = GetComponent<Rigidbody2D>();
			}
		}
#endif
	}
}

