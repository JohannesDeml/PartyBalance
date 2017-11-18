using System;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class DrawGizmoAtTransform : MonoBehaviour
	{
#if UNITY_EDITOR
		public Color Color = Color.yellow;
		public GizmoType Type = GizmoType.Sphere;
		public float SphereSize = 0.07f;
		public Vector3 BoxSize = Vector3.one;
		public Mesh Mesh = null;

		[Tooltip("icon in Assets/Gizmos")]
		public string IconName = "Icon";

		public bool DrawWireRepresentation = false;
		public bool ShowInEditMode = true;
		public bool ShowInPlayMode = true;


		public void OnDrawGizmos()
		{
			if ((Application.isPlaying && ShowInPlayMode) ||
				(!Application.isPlaying && ShowInEditMode))
			{
				Gizmos.color = Color;
				switch (Type)
				{
					case GizmoType.Sphere:
						DrawSphere();
						break;
					case GizmoType.Cube:
						DrawCube();
						break;
					case GizmoType.Icon:
						DrawIcon();
						break;
					case GizmoType.Mesh:
						DrawMesh();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void DrawSphere()
		{
			if (DrawWireRepresentation)
			{
				Gizmos.DrawWireSphere(transform.position, SphereSize);
			}
			else
			{
				Gizmos.DrawSphere(transform.position, SphereSize);
			}
		}

		private void DrawCube()
		{
			if (DrawWireRepresentation)
			{
				Gizmos.DrawWireCube(transform.position, BoxSize);
			}
			else
			{
				Gizmos.DrawCube(transform.position, BoxSize);
			}
		}

		private void DrawIcon()
		{
			Gizmos.DrawIcon(transform.position, IconName);
		}

		private void DrawMesh()
		{
			if (DrawWireRepresentation)
			{
				Gizmos.DrawWireMesh(Mesh, transform.position, transform.rotation);
			}
			else
			{
				Gizmos.DrawMesh(Mesh, transform.position, transform.rotation);
			}
		}
#endif
	}
}