using System;
using UnityEditor;

namespace Supyrb
{
    using UnityEngine;
    using System.Collections;

    [CustomPropertyDrawer(typeof (GizmoData))]
    public class GizmoDataCustomInspector : PropertyDrawer
    {
        public Color backgroundColor = new Color(37f / 255f, 37f / 255f, 37f / 255f);
        private float paddingTop = 2f;
        private float paddingBottom = 2f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.DrawRect(position, backgroundColor);
            position.y += paddingTop;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoType"));
            position.y += EditorGUIUtility.singleLineHeight;

            GizmoType type =
                (GizmoType) property.FindPropertyRelative("GizmoType").enumValueIndex;
            switch (type)
            {
                case GizmoType.Sphere:
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoColor"));
                    position.y += EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Cube:
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoColor"));
                    position.y += EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Icon:
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoIconName"));
                    position.y += EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Mesh:
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoMesh"));
                    position.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("GizmoColor"));
                    position.y += EditorGUIUtility.singleLineHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;
            GizmoType type =
                (GizmoType) property.FindPropertyRelative("GizmoType").enumValueIndex;

            switch (type)
            {
                case GizmoType.Sphere:
                    height += 2 *EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Cube:
                    height += 2 * EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Icon:
                    height += 2 * EditorGUIUtility.singleLineHeight;
                    break;
                case GizmoType.Mesh:
                    height += 3 * EditorGUIUtility.singleLineHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            height += paddingTop + paddingBottom;
            return height;
        }
    }
}