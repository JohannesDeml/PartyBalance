// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFlagAttributeDrawer.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   http://wiki.unity3d.com/index.php/EnumFlagPropertyDrawer
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumFlagAttribute flagSettings = (EnumFlagAttribute)attribute;

            string propName = flagSettings.enumName;
            if (string.IsNullOrEmpty(propName))
            {
                propName = label.text;
            }

            EditorGUI.BeginProperty(position, label, property);
            property.intValue  = EditorGUI.MaskField(position, propName, property.intValue, property.enumNames);

            EditorGUI.EndProperty();
        }

        static T GetBaseProperty<T>(SerializedProperty prop)
        {
            // Separate the steps it takes to get to this property
            string[] separatedPaths = prop.propertyPath.Split('.');

            // Go down to the root of this serialized property
            System.Object reflectionTarget = prop.serializedObject.targetObject as object;
            // Walk down the path to get the target object
            foreach (var path in separatedPaths)
            {
                FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }
            return (T)reflectionTarget;
        }
    }
}