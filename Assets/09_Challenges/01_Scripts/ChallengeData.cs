// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChallengeData.cs" company="Supyrb">
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
	public class ChallengeData : ScriptableObject
	{
		[SerializeField]
		private GameObject challengePrefab;

		public GameObject ChallengePrefab
		{
			get { return challengePrefab; }
		}
	}
}