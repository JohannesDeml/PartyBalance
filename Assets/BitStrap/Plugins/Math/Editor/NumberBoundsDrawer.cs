﻿using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( IntBounds ) )]
    [CustomPropertyDrawer( typeof( FloatBounds ) )]
    public class NumberBoundsDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            // Bugged Unity... hacks :(
            if( !property.type.EndsWith( "Bounds" ) )
                return;

            Rect labelPosition = new Rect( position );
            Rect minPosition = new Rect( position );
            Rect maxPosition = new Rect( position );

            labelPosition.width = EditorGUIUtility.labelWidth;
            minPosition.x = labelPosition.xMax;
            minPosition.width = ( minPosition.width - labelPosition.width ) * 0.5f;
            maxPosition.x = labelPosition.xMax + minPosition.width;
            maxPosition.width = minPosition.width;

            EditorGUI.LabelField( labelPosition, label );

            SerializedProperty max = property.GetMemberProperty<IntBounds>( b => b.Max );
            SerializedProperty min = property.GetMemberProperty<IntBounds>( b => b.Min );

            EditorHelper.BeginChangeLabelWidth( 32.0f );
            EditorHelper.BeginChangeIndentLevel( 0 );

            EditorGUI.BeginChangeCheck();
            DelayedPropertyField( minPosition, min );
            DelayedPropertyField( maxPosition, max );
            if( EditorGUI.EndChangeCheck() )
            {
                min.serializedObject.ApplyModifiedProperties();
                max.serializedObject.ApplyModifiedProperties();

                var validatable = SerializedPropertyHelper.GetValue( fieldInfo, property ) as IValidatable;
                validatable.Validate();

                min.serializedObject.Update();
                max.serializedObject.Update();
            }

            EditorHelper.EndChangeIndentLevel();
            EditorHelper.EndChangeLabelWidth();
        }

        private void DelayedPropertyField( Rect position, SerializedProperty property )
        {
            switch( property.propertyType )
            {
            case SerializedPropertyType.Integer:
                property.intValue = EditorGUI.DelayedIntField( position, property.displayName, property.intValue );
                break;

            case SerializedPropertyType.Float:
                property.floatValue = EditorGUI.DelayedFloatField( position, property.displayName, property.floatValue );
                break;

            default:
                break;
            }
        }
    }
}
