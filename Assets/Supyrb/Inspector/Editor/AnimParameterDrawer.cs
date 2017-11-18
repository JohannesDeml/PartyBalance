// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimParameterDrawer.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEditor;

namespace Supyrb.Marbloid
{
	using UnityEngine;
	using System.Collections;

    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(AnimParameter))]
    public class AnimParameterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                var nameProperty = property.FindPropertyRelative("name");
                var hashPorperty = property.FindPropertyRelative("hash");

                // Don't make child fields be indented
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                EditorGUI.BeginChangeCheck();
                {
                    nameProperty.stringValue = EditorGUI.TextField(position, label, nameProperty.stringValue);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    hashPorperty.intValue = Animator.StringToHash(nameProperty.stringValue);
                }
                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}

