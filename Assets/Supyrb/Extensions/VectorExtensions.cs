// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorExtensions.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
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

	public static class VectorExtensions
	{
		public static string ToFormattedString(this Vector3 vector)
		{
			return "x: " + vector.x + ", y: " + vector.y + ", z: " + vector.z;
		}

		/// <summary>
		/// Calculates the minimum distance between a point and a plane
		/// Math behind it: http://www.xbdev.net/maths_of_3d/collision_detection/point_to_plane/index.php
		/// </summary>
		/// <param name="point">The point for which the distance should be measured</param>
		/// <param name="pointOnPlane">One point on the plane</param>
		/// <param name="planeNormal">The normal of the plane</param>
		/// <returns>The minimum distance between the point and plane,
		/// this value is negative if the point is behind the plane.</returns>
		public static float DistanceToPlane(this Vector3 point, Vector3 pointOnPlane, Vector3 planeNormal)
		{
			return Vector3.Dot(planeNormal, point - pointOnPlane);
		}

		/// <summary>
		/// Calculates if a point is on or behind a plane. Can be used to do intersection checks
		/// </summary>
		/// <param name="point">The point that is tested</param>
		/// <param name="pointOnPlane">One point on the plane</param>
		/// <param name="planeNormal">The normal of the plane</param>
		/// <returns>True if the point is on or behind (inside) a plane</returns>
		public static bool IsOnOrBehindPlane(this Vector3 point, Vector3 pointOnPlane, Vector3 planeNormal)
		{
			return point.DistanceToPlane(pointOnPlane, planeNormal) <= 0f;
		}

		/// <summary>
		/// Clamps a vector to a certain threshold.
		/// If there is a value smaller than minimumValue it will be set to 0
		/// </summary>
		/// <param name="vector">The vector that this function will be applied to</param>
		/// <param name="minimumValue">the minimum value under which the axis value will be set to zero</param>
		/// <param name="keepMagnitude">If true the vector will have the same magnitude, even if values are clamped</param>
		/// <returns>A clamped vector</returns>
		public static Vector3 MinThreshold(this Vector3 vector, float minimumValue, bool keepMagnitude = false)
		{
			var newVector = new Vector3(
				Mathf.Max(vector.x, minimumValue),
                Mathf.Max(vector.y, minimumValue),
                Mathf.Max(vector.z, minimumValue));
			if (keepMagnitude)
			{
				return newVector.normalized*vector.magnitude;
			}
			return newVector;
		}
		public static Vector3 To(this Vector3 from, Vector3 to)
		{
			return to - from;
		}
	}
}