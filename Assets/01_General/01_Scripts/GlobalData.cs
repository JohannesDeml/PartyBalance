// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalData.cs" company="Johannes Deml">
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
	
	public class GlobalData : Singleton<GlobalData>
	{
		[ReadOnly]
		public Transform PointOfInterestPlayer;
	}
}

