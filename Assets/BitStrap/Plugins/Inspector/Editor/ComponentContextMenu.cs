using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Provides cool features to the component context menu such as:
	/// "Fold All", "Move to Top", "Move to Bottom" and "Sort Components".
	/// </summary>
	public class ComponentContextMenu
	{
		private class ComponentComparer : IComparer<Component>
		{
			public static readonly ComponentComparer instance = new ComponentComparer();

			public int Compare( Component a, Component b )
			{
				System.Type monobehaviourType = typeof( MonoBehaviour );
				int monobehaviourOffset = monobehaviourType.IsAssignableFrom( a.GetType() ) ? 999999 : 0;
				monobehaviourOffset -= monobehaviourType.IsAssignableFrom( b.GetType() ) ? 999999 : 0;

				return string.Compare( a.GetType().Name, b.GetType().Name ) + monobehaviourOffset;
			}
		}

		private enum Destination
		{
			Top,
			Bottom
		};

		private const string kComponentArrayName = "m_Component";
		private const int kFirstComponentIndex = 1;

		[MenuItem( "CONTEXT/Component/Fold All" )]
		public static void FoldAll( MenuCommand command )
		{
			ActiveEditorTracker editorTracker = ActiveEditorTracker.sharedTracker;
			Editor[] editors = editorTracker.activeEditors;

			bool areAllFolded = true;
			for( int i = 1; i < editors.Length; i++ )
			{
				if( editorTracker.GetVisible( i ) > 0 )
					areAllFolded = false;
			}

			for( int i = 1; i < editors.Length; i++ )
			{
				if( editorTracker.GetVisible( i ) < 0 )
					continue;

				editorTracker.SetVisible( i, areAllFolded ? 1 : 0 );
				InternalEditorUtility.SetIsInspectorExpanded( editors[i].target, areAllFolded );
			}
		}

		[MenuItem( "CONTEXT/Component/Move to Top" )]
		public static void Top( MenuCommand command )
		{
			Move( ( Component ) command.context, Destination.Top );
		}

		[MenuItem( "CONTEXT/Component/Move to Bottom" )]
		public static void Bottom( MenuCommand command )
		{
			Move( ( Component ) command.context, Destination.Bottom );
		}

		[MenuItem( "CONTEXT/Component/Sort Components" )]
		public static void SortComponents( MenuCommand command )
		{
			Component target = ( Component ) command.context;
			SerializedObject gameObject = new SerializedObject( target.gameObject );
			SerializedProperty componentArray = gameObject.FindProperty( kComponentArrayName );
			int size = componentArray.arraySize;

			Component[] components = new Component[size];
			int[] componentKeys = new int[size];

			for( int i = 0; i < size; ++i )
			{
				SerializedProperty iterator = componentArray.GetArrayElementAtIndex( i );

				iterator.Next( true );
				componentKeys[i] = iterator.intValue;

				iterator.Next( true );
				components[i] = iterator.objectReferenceValue as Component;
			}

			System.Array.Sort( components, componentKeys, kFirstComponentIndex, size - kFirstComponentIndex, ComponentComparer.instance );

			for( int i = kFirstComponentIndex; i < size; ++i )
			{
				SerializedProperty iterator = componentArray.GetArrayElementAtIndex( i );

				iterator.Next( true );
				iterator.intValue = componentKeys[i];

				iterator.Next( true );
				iterator.objectReferenceValue = components[i];
			}

			gameObject.ApplyModifiedProperties();
		}

		private static void Move( Component target, Destination destination )
		{
			SerializedObject gameObject = new SerializedObject( target.gameObject );
			SerializedProperty componentArray = gameObject.FindProperty( kComponentArrayName );
			int size = componentArray.arraySize;

			for( int index = kFirstComponentIndex; index < size; ++index )
			{
				SerializedProperty iterator = componentArray.GetArrayElementAtIndex( index );
				iterator.Next( true );
				iterator.Next( true );

				if( iterator.objectReferenceValue == target )
				{
					componentArray.MoveArrayElement( index, destination == Destination.Top ? kFirstComponentIndex : size - 1 );
					gameObject.ApplyModifiedProperties();

					break;
				}
			}
		}
	}
}
