// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Marker.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
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

	/// <summary>
	/// Marks an object to have problems of any kind. 
	/// The marker also allows to add information this the Note string
	/// </summary>
	public class Marker : MonoBehaviour
	{
		[TextArea]
		public string Note = "No notes set";

		public Color GizmoColor = new Color(1.0f, 0.0f, 0.0f, 0.7f);

		public void OnDrawGizmos()
		{
			Gizmos.color = GizmoColor;
			Gizmos.DrawCube(transform.position, Vector3.one * 8f);
		}
	}
}