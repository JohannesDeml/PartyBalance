// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionStatistics.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Supyrb.Common
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;

    public class SelectionStatistics : MonoBehaviour
    {
        [MenuItem("GameObject/Selection Statistics")]
        public static void ShowSelectionStatistics()
        {
            int objectCounter = 0;
            int staticObjects = 0;
            int rigidBodyCounter = 0;
            int colliderCounter = 0;
            Dictionary<string, int> tags = new Dictionary<string, int>();
            Dictionary<string, int> layers = new Dictionary<string, int>();
            int selectedObjects = Selection.objects.Length;
            Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
            if (transforms == null)
            {
                return;
            }
            foreach (Transform currentTransform in transforms)
            {
                objectCounter++;
                if (EditorUtility.DisplayCancelableProgressBar("Analyzing game objects",
                    "Analyzing object " + objectCounter + " of " + transforms.Length,
                    (float)objectCounter / (float)transforms.Length))
                {
                    Debug.Log("Process canceled");
                    break;
                }
                if (!tags.ContainsKey(currentTransform.tag))
                {
                    tags.Add(currentTransform.tag, 1);
                }
                else
                {
                    tags[currentTransform.tag]++;
                }

                string layerName = LayerMask.LayerToName(currentTransform.gameObject.layer);
                if (!layers.ContainsKey(layerName))
                {
                    layers.Add(layerName, 1);
                }
                else
                {
                    layers[layerName]++;
                }

                if (currentTransform.gameObject.isStatic)
                {
                    staticObjects++;
                }
                if (currentTransform.GetComponent<Rigidbody>() != null)
                {
                    rigidBodyCounter++;
                }
                if (currentTransform.GetComponent<Collider>() != null)
                {
                    colliderCounter++;
                }
            }
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Selection statistics",
                string.Format("--------------------------\n" +
                              "Selected objects: {0} \n" +
                              "Game objects: {1} \n" +
                              "Static objects: {2} \n" +
                              "Rigid bodies: {3} \n" +
                              "Colliders: {4} \n" +
                              "---------------------------\n" +
                              "{5} \n" +
                              "--------------------------\n" +
                              "{6} \n" +
                              "--------------------------\n",
                          string.Format("{0:n0}", selectedObjects),
                          string.Format("{0:n0}", objectCounter),
                          string.Format("{0:n0}", staticObjects),
                          string.Format("{0:n0}", rigidBodyCounter),
                          string.Format("{0:n0}", colliderCounter),
                          tags.ToFormattedString("Tags: \n - ", ",\n - ", ": ", ""),
                          layers.ToFormattedString("Layers: \n - ", ", \n - ", ": ", "")),
                "OK");
        }

        [MenuItem("GameObject/Selection Statistics", true)]
        public static bool ValidateShowSelectionStatistics()
        {
            return Selection.activeTransform != null;
        }

        [MenuItem("Assets/Selection Statistics")]
        public static void ShowAssetSelectionStatistics()
        {
            Dictionary<Type, int> types = new Dictionary<Type, int>();
            List<string> objectNames = new List<string>();
            int objectCounter = 0;
            float memoryFileSize = 0f;
            var objects = Selection.objects;
            if (objects == null)
            {
                return;
            }
            foreach (var current in objects)
            {
                objectCounter++;
                if (EditorUtility.DisplayCancelableProgressBar("Analyzing game objects",
                    "Analyzing object " + objectCounter + " of " + objects.Length,
                    (float)objectCounter / (float)objects.Length))
                {
                    Debug.Log("Process canceled");
                    break;
                }

                // TODO navigate through folders if it is one
                objectNames.Add(current.name);
                memoryFileSize += UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(current);
                types.AddOrIncrement(current.GetType());
            }
            EditorUtility.ClearProgressBar();
            Debug.Log(objectNames.ToFormattedString("", ",\n", ""));
            EditorUtility.DisplayDialog("Selection statistics",
                string.Format("--------------------------\n" +
                              "Selected objects: {0} \n" +
                              "Memory usage: {1} \n" +
                              "---------------------------\n" +
                              "{2} \n" +
                              "--------------------------\n",
                          string.Format("{0:n0}", objectCounter),
                          FileHelper.GetFileSize(memoryFileSize),
                          types.ToFormattedString("Types: \n - ", ", \n - ", ": ", "")),
                "OK");
        }

        [MenuItem("Assets/Selection Statistics", true)]
        public static bool ValidateShowAssetSelectionStatistics()
        {
            return Selection.objects != null;
        }
    }

}
