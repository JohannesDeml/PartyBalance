// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupObjects.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace Supyrb.EditorTools
{
	public class GroupObjects
	{
		[MenuItem("GameObject/Group Objects &g")]
		static void GroupSelectedObjects()
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);
			GroupSelectedTransforms(transforms);
		}

		private static void GroupSelectedTransforms(Transform[] transforms)
		{
			int numberOfTransforms = transforms.Length;
			bool showProgressBar = (numberOfTransforms > 20);
			try
			{
				if (showProgressBar)
				{
					EditorUtility.DisplayProgressBar("Group Objects", "Calculate parent position", 0f);
				}
				var medianPoint = Vector3.zero;
				for (int i = 0; i < numberOfTransforms; i++)
				{
					var transform = transforms[i];
					medianPoint += transform.position;
				}
				medianPoint /= (float) numberOfTransforms;
				var parent = transforms[0].parent;
				var layer = transforms[0].gameObject.layer;

				GameObject groupObject = new GameObject("Group");
				groupObject.layer = layer;
				Undo.RegisterCreatedObjectUndo(groupObject, "Group objects");
				if (parent != null)
				{
					groupObject.transform.parent = parent;
				}
				groupObject.transform.position = medianPoint;

                if (numberOfTransforms == 1)
                {
                    groupObject.transform.rotation = transforms[0].rotation;
                }

                for (int i = 0; i < numberOfTransforms; i++)
				{
					if (showProgressBar)
					{
						EditorUtility.DisplayProgressBar("Group Objects", "Move selected objects to group (" + i + "/" + numberOfTransforms + ")",
							(float) i/(float) numberOfTransforms);
					}
					var transform = transforms[i];
					Undo.SetTransformParent(transform, groupObject.transform, "Group objects");
				}
				Selection.activeGameObject = groupObject;

				// Unfold object in hierarchy sadly not working
				//var hierarchy = GetFocusedWindow("Hierarchy");
				//var unfoldEvent = new Event { keyCode = KeyCode.RightArrow, type = EventType.KeyDown};
				//hierarchy.SendEvent(unfoldEvent);
			}
			finally
			{
				if (showProgressBar)
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}

		[MenuItem("GameObject/Group Objects &g", true)]
		static bool ValidateGroupSelectedObjects()
		{
			return SceneObjectSelected();
		}

		static bool SceneObjectSelected()
		{
			return Selection.activeTransform != null;
		}

		private static EditorWindow GetFocusedWindow(string window)
		{
			FocusOnWindow(window);
			return EditorWindow.focusedWindow;
		}

		private static void FocusOnWindow(string window)
		{
			EditorApplication.ExecuteMenuItem("Window/" + window);
		}
	}
}