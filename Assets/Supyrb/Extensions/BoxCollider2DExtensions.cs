// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxCollider2DExtensions.cs" company="Johannes Deml">
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
	
	public static class BoxCollider2DExtensions 
	{
		public static Vector2 GetOrigin(this BoxCollider2D collider)
		{
			Vector2 origin = collider.offset;
			origin.x += collider.transform.position.x;
			origin.y += collider.transform.position.y;
			return origin;
		}
	}
}

