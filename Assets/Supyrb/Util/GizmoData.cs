using System;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public enum GizmoType
	{
		Sphere,
		Cube,
		Icon,
		Mesh
	}

	[Serializable]
	public class GizmoData
	{
		public GizmoType GizmoType = GizmoType.Icon;
		public Mesh GizmoMesh = null;
		public string GizmoIconName = "Missing.png";
		public Color GizmoColor = Color.magenta;

		public GizmoData(GizmoType gizmoType, Mesh gizmoMesh, string gizmoIconName, Color gizmoColor)
		{
			GizmoType = gizmoType;
			GizmoMesh = gizmoMesh;
			GizmoIconName = gizmoIconName;
			GizmoColor = gizmoColor;
		}
	}
}