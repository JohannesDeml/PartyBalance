// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetPSR.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb.EditorTools
{
	using System;
	using UnityEngine;
	using System.Collections;
	using Supyrb;
	using UnityEditor;

	public class ResetPSR
	{
		[MenuItem("GameObject/Reset position, scale and rotation &r")]
		static void MoveSelectionToOrigin()
		{
			if (SceneObjectSelected())
			{
				ResetSceneSelectionToOrigin();
			}
			else
			{
				ResetAssetsToOrigin();
			}
		}

		private static void ResetSceneSelectionToOrigin()
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);
			ApplyActionOnSelectedTransforms(ResetTransform, transforms);
		}

		private static void ResetAssetsToOrigin()
		{
			var gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets);
			var transforms = new Transform[gameObjects.Length];
			for (int i = 0; i < gameObjects.Length; i++)
			{
				GameObject obj = (GameObject)gameObjects[i];
				transforms[i] = obj.transform;
			}
			ApplyActionOnSelectedTransforms(ResetPosition, transforms);
			AssetDatabase.Refresh();
		}

		[MenuItem("GameObject/Reset position, scale and rotation (no child movement) &#r")]
		static void MoveSelectionToOriginNoChildMovement()
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.Editable);
			ApplyActionOnSelectedTransforms(ResetTransformNoChildMovement, transforms);
		}

		private static void ApplyActionOnSelectedTransforms(Action<Transform> action, Transform[] transforms)
		{
			int numberOfTransforms = transforms.Length;
			bool showProgressBar = (numberOfTransforms > 10);
			try
			{
				for (int i = 0; i < numberOfTransforms; i++)
				{
					if (showProgressBar)
					{
						EditorUtility.DisplayProgressBar("Reset PSR", "Reset position, scale and rotation (" + i + "/" + numberOfTransforms + ")",
							(float)i / (float)numberOfTransforms);
					}
					action(transforms[i]);
				}
			}
			finally
			{
				if (showProgressBar)
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}

		private static void ResetPosition(Transform transformToReset)
		{
			Undo.RecordObject(transformToReset, "Reset PSR");
			transformToReset.localPosition = Vector3.zero;
		}

		private static void ResetTransform(Transform transformToReset)
		{
			Undo.RecordObject(transformToReset, "Reset PSR");
			transformToReset.localPosition = Vector3.zero;
			transformToReset.localScale = Vector3.one;
			transformToReset.localRotation = Quaternion.identity;
		}

		private static void ResetTransformNoChildMovement(Transform transformToReset)
		{
			Undo.RecordObject(transformToReset, "Reset PSR");
			var positionChange = transformToReset.localPosition;
			var rotationChange = transformToReset.localRotation;
			var scaleChange = transformToReset.localScale;
			transformToReset.localPosition = Vector3.zero;
			transformToReset.localScale = Vector3.one;
			transformToReset.localRotation = Quaternion.identity;

			for (int i = 0; i < transformToReset.childCount; i++)
			{
				var child = transformToReset.GetChild(i);
				Undo.RecordObject(child, "Reset PSR");
				child.localPosition = positionChange + Vector3.Scale(rotationChange * child.localPosition, scaleChange);
				child.localRotation *= rotationChange;
				child.localScale = Vector3.Scale(child.localScale, scaleChange);
			}
		}

		[MenuItem("GameObject/Reset position, scale and rotation &r", true)]
		[MenuItem("GameObject/Reset position, scale and rotation (no child movement) &#r", true)]
		static bool ValidateMoveSelectionToOrigin()
		{
			return SceneObjectSelected() || AssetsSelected();
		}

		static bool SceneObjectSelected()
		{
			return Selection.activeTransform != null;
		}

		static bool AssetsSelected()
		{
			return Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets).Length != 0;
		}
	}
}
