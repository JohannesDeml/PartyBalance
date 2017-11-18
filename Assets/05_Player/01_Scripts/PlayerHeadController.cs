// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerHeadController.cs" company="Johannes Deml">
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
	
	public class PlayerHeadController : MonoBehaviour
	{
		[SerializeField]
		private Animator anim;

		[SerializeField]
		private TriggerAnimationParameter happyParameter;

		void Start()
		{
			ChallengeManager.Instance.ProgressChanged += OnProgressChanged;
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			ChallengeManager.Instance.ProgressChanged -= OnProgressChanged;
		}

		private void OnProgressChanged(float progress)
		{
			anim.SetTrigger(happyParameter.Index);
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

