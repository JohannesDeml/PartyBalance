// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMaxRangeDrawer.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(MinMaxRange))]
    public class MinMaxRangeDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                // First line
                var firstLinePosition = EditorGUI.PrefixLabel(position, label);
                EditorGUIUtility.labelWidth = 40f;
                var halfWidthFirstLine = firstLinePosition.width/2f;
                var lineHeight = EditorGUIUtility.singleLineHeight;
                var minValueProperty = property.FindPropertyRelative("MinValue");
                var maxValueProperty = property.FindPropertyRelative("MaxValue");
                var minLimitProperty = property.FindPropertyRelative("MinLimit");
                var maxLimitProperty = property.FindPropertyRelative("MaxLimit");
                var minValuePos = new Rect(firstLinePosition.x, firstLinePosition.y, halfWidthFirstLine, lineHeight);
                var maxValuePos = new Rect(firstLinePosition.x + halfWidthFirstLine, firstLinePosition.y, halfWidthFirstLine, lineHeight);

				// Don't make child fields be indented
				int indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

                EditorGUI.PropertyField(minValuePos, minValueProperty, new GUIContent("Min"));
                EditorGUI.PropertyField(maxValuePos, maxValueProperty, new GUIContent("Max"));
				
                // Second line
	            position.x += indent * GUI.skin.label.CalcSize(new GUIContent("   ")).x;
                position.y += lineHeight;
                var limitFieldWidth = 40f;
                var padding = 2f;
                var minLimitPos = new Rect(position.x, position.y, limitFieldWidth, lineHeight);
                var sliderPos = new Rect(position.x + limitFieldWidth + padding, position.y, position.width - 2* (limitFieldWidth + padding), lineHeight);
                var maxLimitPos = new Rect(position.xMax - limitFieldWidth, position.y, limitFieldWidth, lineHeight);
                
                // Min limit
                EditorGUI.PropertyField(minLimitPos, minLimitProperty, GUIContent.none);
                // Slider
                float minValue = minValueProperty.floatValue;
                float maxValue = maxValueProperty.floatValue;
                EditorGUI.MinMaxSlider(sliderPos, ref minValue, ref maxValue, minLimitProperty.floatValue, maxLimitProperty.floatValue);
                minValueProperty.floatValue = minValue;
                maxValueProperty.floatValue = maxValue;
                // Max limit
                EditorGUI.PropertyField(maxLimitPos, maxLimitProperty, GUIContent.none);

                EditorGUIUtility.labelWidth = 0f;
				EditorGUI.indentLevel = indent;
			}
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2f;
        }
    }
}

