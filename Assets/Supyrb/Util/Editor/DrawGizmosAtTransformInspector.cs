// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawGizmosAtTransformInspector.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Supyrb
{
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(DrawGizmoAtTransform))]
    [CanEditMultipleObjects]
    public class DrawGizmosAtTransformInspector : Editor
    {
        private SerializedProperty type;
        private SerializedProperty color;
        private SerializedProperty sphereSize;
        private SerializedProperty boxSize;
        private SerializedProperty iconName;
        private SerializedProperty mesh;
        private SerializedProperty drawWireRepresentation;
        private SerializedProperty showInEditMode;
        private SerializedProperty showInPlayMode;

        private DrawGizmoAtTransform scriptReference;

        void OnEnable()
        {
            type = serializedObject.FindProperty("Type");
            color = serializedObject.FindProperty("Color");
            sphereSize = serializedObject.FindProperty("SphereSize");
            boxSize = serializedObject.FindProperty("BoxSize");
            iconName = serializedObject.FindProperty("IconName");
            mesh = serializedObject.FindProperty("Mesh");
            drawWireRepresentation = serializedObject.FindProperty("DrawWireRepresentation");
            showInEditMode = serializedObject.FindProperty("ShowInEditMode");
            showInPlayMode = serializedObject.FindProperty("ShowInPlayMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Show "Script" at the top
            EditorGUI.BeginDisabledGroup(true);
            SerializedProperty prop = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
            EditorGUI.EndDisabledGroup();

            scriptReference = (DrawGizmoAtTransform)target;
            EditorGUILayout.PropertyField(type);
            switch (scriptReference.Type)
            {
                case GizmoType.Sphere:
                EditorGUILayout.PropertyField(color);
                EditorGUILayout.PropertyField(sphereSize);
                EditorGUILayout.PropertyField(drawWireRepresentation);
                break;
                case GizmoType.Cube:
                EditorGUILayout.PropertyField(color);
                EditorGUILayout.PropertyField(boxSize);
                EditorGUILayout.PropertyField(drawWireRepresentation);
                break;
                case GizmoType.Icon:
                EditorGUILayout.PropertyField(iconName);
                break;
                case GizmoType.Mesh:
                EditorGUILayout.PropertyField(color);
                EditorGUILayout.PropertyField(mesh);
                EditorGUILayout.PropertyField(drawWireRepresentation);
                break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EditorGUILayout.PropertyField(showInEditMode);
            EditorGUILayout.PropertyField(showInPlayMode);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

