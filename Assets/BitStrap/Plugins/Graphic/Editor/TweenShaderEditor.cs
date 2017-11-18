using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BitStrap
{
    [CanEditMultipleObjects]
    [CustomEditor( typeof( TweenShader ) )]
    public class TweenShaderEditor : Editor
    {
        private ReorderableList shaderProperiesList;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty targetRendererProperty = serializedObject.FindProperty( "targetRenderer" );
            SerializedProperty curveProperty = serializedObject.FindProperty( "curve" );
            SerializedProperty durationProperty = serializedObject.FindProperty( "duration" );

            EditorGUILayout.PropertyField( targetRendererProperty );
            EditorGUILayout.PropertyField( curveProperty );
            EditorGUILayout.PropertyField( durationProperty );

            shaderProperiesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            shaderProperiesList = new ReorderableList( serializedObject, serializedObject.FindProperty( "shaderProperties" ) );
            shaderProperiesList.drawElementCallback += DrawElement;
            shaderProperiesList.drawHeaderCallback += DrawHeader;
        }

        private void DrawElement( Rect rect, int index, bool isActive, bool isFocused )
        {
            SerializedProperty elementProperty = shaderProperiesList.serializedProperty.GetArrayElementAtIndex( index );
            EditorGUI.PropertyField( rect, elementProperty );
        }

        private void DrawHeader( Rect rect )
        {
            EditorGUI.LabelField( rect, shaderProperiesList.serializedProperty.displayName );
        }
    }
}
