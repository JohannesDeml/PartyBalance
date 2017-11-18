// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarLever.cs" company="Johannes Deml">
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
	
	public class BarLever : MonoBehaviour
	{
		[SerializeField]
		private Animator anim;

		[SerializeField]
		private AnimParameter activateLeverParameter = new AnimParameter("ActivateLever");

		private void OnTriggerEnter2D(Collider2D collision)
		{
			anim.SetTrigger(activateLeverParameter.Hash);
		}

		public void ActivateBar()
		{
			ActivityTracker.Instance.TriggerActivateBar(true);
		}

#if UNITY_EDITOR

		void Reset()
		{
			if (anim == null)
			{
				anim = GetComponent<Animator>();
			}
		}
#endif
	}
}

