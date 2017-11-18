using System.Collections;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( TweenShader.ShaderProperty ) )]
    public class TweenShaderPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            SerializedProperty nameProperty = property.GetMemberProperty<TweenShader.ShaderProperty>( p => p.name );
            SerializedProperty typeProperty = property.GetMemberProperty<TweenShader.ShaderProperty>( p => p.type );
            SerializedProperty fromProperty = property.GetMemberProperty<TweenShader.ShaderProperty>( p => p.from );
            SerializedProperty toProperty = property.GetMemberProperty<TweenShader.ShaderProperty>( p => p.to );

            Rect nameRect = new Rect( position );
            Rect typeRect = new Rect( nameRect );
            Rect fromRect = new Rect( position );

            nameRect.width = EditorGUIUtility.labelWidth;
            typeRect.x += nameRect.width;
            typeRect.width = 48.0f;

            fromRect.xMin = typeRect.xMax;
            fromRect.width *= 0.5f;

            Rect toRect = new Rect( fromRect );
            toRect.x = fromRect.xMax;

            nameProperty.stringValue = EditorGUI.TextField( nameRect, nameProperty.stringValue );
            typeProperty.enumValueIndex = EditorGUI.Popup( typeRect, typeProperty.enumValueIndex, typeProperty.enumDisplayNames );

            EditorGUIUtility.labelWidth = 32.0f;

            TweenShader.ShaderProperty.Type type = ( TweenShader.ShaderProperty.Type ) typeProperty.enumValueIndex;
            if( type == TweenShader.ShaderProperty.Type.Color )
            {
                fromProperty.colorValue = EditorGUI.ColorField( fromRect, fromProperty.displayName, fromProperty.colorValue );
                toProperty.colorValue = EditorGUI.ColorField( toRect, toProperty.displayName, toProperty.colorValue );
            }
            else if( type == TweenShader.ShaderProperty.Type.Float )
            {
                Color fromColor = fromProperty.colorValue;
                Color toColor = toProperty.colorValue;

                fromColor.a = EditorGUI.FloatField( fromRect, fromProperty.displayName, fromColor.a );
                toColor.a = EditorGUI.FloatField( toRect, toProperty.displayName, toColor.a );

                fromProperty.colorValue = fromColor;
                toProperty.colorValue = toColor;
            }
        }
    }
}
