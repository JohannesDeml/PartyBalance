// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformSnapshot.cs" company="Supyrb">
//   Copyright (c) 2015 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using System;
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// A class to easily encapsulate data about a transform at a specific time.
	/// TransformSnapshot does not keep a reference to a transform and therefore does not change with the transform.
	/// </summary>
	[Serializable]
	public struct TransformSnapshot
	{
		/// <summary>
		/// The type of the simple transform is used to. 
		/// This information is stored so it can be used the right way if it is applied to Transforms.
		/// </summary>
		[SerializeField]
		private Space space;

		[SerializeField]
		private Vector3 position;

		[SerializeField]
		private Quaternion rotation;

		[SerializeField]
		private Vector3 scale;

		public Space Space
		{
			get { return space; }
		}

		public Vector3 Position
		{
			get { return position; }
		}

		public Quaternion Rotation
		{
			get { return rotation; }
		}

		public Vector3 Scale
		{
			get { return scale; }
		}

		public Vector3 Forward
		{
			get { return Rotation*Vector3.forward; }
		}

		public Vector3 Right
		{
			get { return Rotation*Vector3.right; }
		}

		public Vector3 Up
		{
			get { return Rotation*Vector3.up; }
		}

		/// <summary>
		/// Encapsulate a transform information with its position, rotation, local scale and type
		/// The scale will always be the local scale to prevent strange effects with 
		/// lossy scale <see cref="Transform.lossyScale"/>
		/// </summary>
		/// <param name="position">Position of the transform</param>
		/// <param name="rotation">Rotation  of the transform</param>
		/// <param name="scale">Scale of the transform</param>
		/// <param name="space">Type of the transform that is stored. This can either be local or world.
		/// This value is important once the SimpleTransofrm is applied to a transform to set the right values.</param>
		public TransformSnapshot(Vector3 position, Quaternion rotation, Vector3 scale, Space space = Space.World)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
			this.space = space;
		}

		/// <summary>
		/// Encapsulate a transform information with its position, rotation, local scale and type
		/// The scale will always be the local scale to prevent strange effects with 
		/// lossy scale <see cref="Transform.lossyScale"/>
		/// </summary>
		/// <param name="transform">The transform from which a snapshot should be taken</param>
		/// <param name="space">Type of the transform that is processed from the transform. This can either be local or world.
		/// This value is important once the SimpleTransofrm is applied to a transform to set the right values.</param>
		public TransformSnapshot(Transform transform, Space space = Space.World)
		{
			this.position = (space == Space.Self) ? transform.localPosition : transform.position;
			this.rotation = (space == Space.Self) ? transform.localRotation : transform.rotation;
			this.scale = transform.localScale;
			this.space = space;
		}

		public override string ToString()
		{
			return string.Format("TransformSnapshot: Type: {0} \nPosition: {1} \nRotation (euler): {2} \nScale: {3}",
				Space.ToString(), Position.ToFormattedString(), Rotation.eulerAngles.ToFormattedString(),
				Scale.ToFormattedString());
		}

		public static float Distance(TransformSnapshot a, TransformSnapshot b)
		{
			return Vector3.Distance(a.Position, b.Position);
		}

		public static float Distance(TransformSnapshot st, Vector3 v)
		{
			return Vector3.Distance(st.Position, v);
		}
	}

	public static class SimpleTransformExtensions
	{
		/// <summary>
		/// Creates a snapshot of the current transform that is immune to changes
		/// The scale stored will always be the local scale to prevent strange effects with 
		/// lossy scale <see cref="Transform.lossyScale"/>
		/// </summary>
		/// <param name="transform">The transform the snapshot should be taken from</param>
		/// <returns>A simple transform that holds the world information of the transform</returns>
		public static TransformSnapshot GetSnapshot(this Transform transform)
		{
			return new TransformSnapshot(transform);
		}

		/// <summary>
		/// Creates a snapshot of the current transform that is immune to changes
		/// The scale stored will always be the local scale to prevent strange effects with 
		/// lossy scale <see cref="Transform.lossyScale"/>
		/// </summary>
		/// <param name="transform">The transform the snapshot should be taken from</param>
		/// <param name="space">Type of the transform that is processed from the transform. This can either be local or world.
		/// This value is important once the SimpleTransofrm is applied to a transform to set the right values.</param>
		/// <returns>A simple transform that holds the information of the given type of the transform</returns>
		public static TransformSnapshot GetSnapshot(this Transform transform, Space space)
		{
			return new TransformSnapshot(transform, space);
		}

		/// <summary>
		/// Applies the position, rotation and local scale of the simple transform to the properties of the transform 
		/// with their respective type. Only with scale their is just the local scale that is stored and will be set.
		/// </summary>
		/// <param name="transform">Transform that will be changed</param>
		/// <param name="snapshot">The simple transform that should be applied on the transform</param>
		/// <returns>Returns the changed transform. The transform is already changed, 
		/// but out of convenience you can just keep on working with the transform</returns>
		public static Transform ApplySnapshot(this Transform transform, TransformSnapshot snapshot)
		{
			switch (snapshot.Space)
			{
				case Space.World:
					transform.position = snapshot.Position;
					transform.rotation = snapshot.Rotation;
					transform.localScale = snapshot.Scale;
					break;
				case Space.Self:
					transform.localPosition = snapshot.Position;
					transform.localRotation = snapshot.Rotation;
					transform.localScale = snapshot.Scale;
					break;
			}
			return transform;
		}
	}
}