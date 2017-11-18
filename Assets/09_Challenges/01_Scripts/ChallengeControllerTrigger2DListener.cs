// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChallengeControllerTrigger2DListener.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Supyrb
{
	public class ChallengeControllerTrigger2DListener : MonoBehaviour
	{
		[SerializeField]
		private TriggerChallengeController challengeController = null;

		public void OnTriggerEnter2D(Collider2D collision)
		{
			challengeController.OnTriggerEnter2D(collision);
		}
	}
}