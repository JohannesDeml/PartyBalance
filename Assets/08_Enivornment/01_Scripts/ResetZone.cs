// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetZone.cs" company="Johannes Deml">
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
	
	public class ResetZone : MonoBehaviour
	{
		[SerializeField]
		private Transform resetPosition = null;
		[SerializeField]
		private bool resetXPosition = true;

		[SerializeField]
		private bool resetSpeed = false;

		[SerializeField]
		private float maxSpeed = 20f;

		private Vector3 resetPositionValue;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == SupyrbLayers.Player)
			{
				var playerTransform = collision.transform;
				resetPositionValue = resetPosition.position;
				if (!resetXPosition)
				{
					resetPositionValue.x = playerTransform.position.x;
				}
				playerTransform.position = resetPositionValue;

				var rb = collision.attachedRigidbody;
				if (resetSpeed)
				{
					rb.velocity = Vector2.zero;
				}
				else
				{
					rb.velocity = rb.velocity.normalized * maxSpeed;
				}
			}
		}
	}
}

