// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapToGrid.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnapToGrid : ScriptableObject
{
    [MenuItem("GameObject/Snap to Grid &#g")]
    static void MenuSnapToGrid()
    {
        foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable))
        {
            Undo.RecordObject(t, "Snapping to Grid");
            t.position = new Vector3(
                Mathf.Round(t.position.x / EditorPrefs.GetFloat("MoveSnapX")) * EditorPrefs.GetFloat("MoveSnapX"),
                Mathf.Round(t.position.y / EditorPrefs.GetFloat("MoveSnapY")) * EditorPrefs.GetFloat("MoveSnapY"),
                Mathf.Round(t.position.z / EditorPrefs.GetFloat("MoveSnapZ")) * EditorPrefs.GetFloat("MoveSnapZ")
            );
        }
    }

    [MenuItem("GameObject/Snap to Grid &#g", true)]
    static bool ValidateMenuSnapToGrid()
    {
        return ObjectSelected();
    }

    static bool ObjectSelected()
    {
        return Selection.activeTransform != null;
    }
}