using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BitStrap
{
	public static class AssemblyProcessorPreferences
	{
		public static void OnPreferencesGUI()
		{
			EditorHelper.BeginBox( "Assembly Processor" );

			bool enabled = AssemblyProcessorManager.Enabled;

			EditorGUI.BeginChangeCheck();
			enabled = EditorGUILayout.Toggle( "Enabled", enabled );
			if( EditorGUI.EndChangeCheck() )
			{
				AssemblyProcessorManager.Enabled = enabled;
			}

			if( GUILayout.Button( "Force Process Assemblies" ) )
			{
				AssemblyProcessorManager.LockAndProcessAssemblies();
				InternalEditorUtility.RequestScriptReload();

				Debug.Log( "Assemblies Processed" );
			}

			EditorHelper.EndBox();
		}
	}
}
