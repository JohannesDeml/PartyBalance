// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TracedData.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class TracedData : ScriptableObject
	{
		public List<TransformSnapshot> TracedTransforms;

		[TextArea(6, 10)]
		public string Info;

		public float Length;
		public float UpdateInterval;
	}
}