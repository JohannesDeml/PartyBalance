using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
    [CustomPropertyDrawer( typeof( AnimationParameter ), true )]
    public class AnimationParameterDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            var component = property.serializedObject.targetObject as Component;
            var animator = component != null ? component.GetComponent<Animator>() : null;
            var nameProperty = property.GetMemberProperty<AnimationParameter>( p => p.name );

			if( animator == null || animator.parameters.Length == 0 )
            {
                EditorGUI.PropertyField( position, nameProperty, label );
            }
            else
            {
                var parameters = FilterParameters( property, animator );
                var popupOptions = parameters.Select( x => new GUIContent( x.name ) ).ToArray();
                int currentIndex = Array.FindIndex( parameters, x => x.name == nameProperty.stringValue );

                EditorGUI.BeginChangeCheck();
                currentIndex = EditorGUI.Popup( position, label, currentIndex, popupOptions );

                if( EditorGUI.EndChangeCheck() )
                {
                    nameProperty.stringValue = parameters[currentIndex].name;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private AnimatorControllerParameter[] FilterParameters( SerializedProperty property, Animator animator )
        {
            var t = property.type;
            if( typeof( BoolAnimationParameter ).Name == t )
                return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Bool ).ToArray();
            if( typeof( IntAnimationParameter ).Name == t )
                return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Int ).ToArray();
            if( typeof( FloatAnimationParameter ).Name == t )
                return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Float ).ToArray();
            if( typeof( TriggerAnimationParameter ).Name == t )
                return animator.parameters.Where( x => x.type == AnimatorControllerParameterType.Trigger ).ToArray();
            return animator.parameters;
        }
    }
}
