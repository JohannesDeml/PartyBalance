using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( ReadOnlyAttribute ) )]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginDisabledGroup( true );
            EditorGUI.PropertyField( position, property, label );
            EditorGUI.EndDisabledGroup();
        }
    }
}
