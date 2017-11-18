// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvisibleUiImage.cs" company="Supyrb">
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
	using UnityEngine.UI;

	/// From http://answers.unity3d.com/answers/1157876/view.html
	/// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
	/// Useful for providing a raycast target without actually drawing anything.
	public class InvisibleUiImage : Graphic
	{
		public override void SetMaterialDirty()
		{
			return;
		}

		public override void SetVerticesDirty()
		{
			return;
		}

		/// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			return;
		}
	}
}