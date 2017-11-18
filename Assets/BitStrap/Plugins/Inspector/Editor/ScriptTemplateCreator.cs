using System;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.ProjectWindowCallback
{
	internal class CreateScriptAssetAction : EndNameEditAction
	{
		public override void Action( int instanceId, string path, string source )
		{
			string className = Path.GetFileNameWithoutExtension( path );
			source = source.Replace( "#CLASSNAME#", className );
			source = source.Replace( "#SCRIPTNAME#", className );

			int currentYear = DateTime.Today.Year;
			source = source.Replace( "#YEAR#", currentYear.ToString() );

			File.WriteAllText( path, source );

			AssetDatabase.ImportAsset( path );
			Object o = AssetDatabase.LoadAssetAtPath( path, typeof( Object ) );
			ProjectWindowUtil.ShowCreatedAsset( o );
		}
	}
}

namespace BitStrap
{
	public static class ScriptTemplateCreator
	{
		private static Texture2D ScriptIcon
		{
			get { return EditorGUIUtility.IconContent( "cs Script Icon" ).image as Texture2D; }
		}

		[MenuItem( "Assets/Create/C# Supyrb", false, 75 )]
		public static void CreateCSharpScript()
		{
			string path = GetNewScriptPath( "NewMonobehaviour" );
			CreateScript( path, ScriptTemplatePreferences.CSharpScriptDefaultCode );
		}

		[MenuItem( "Assets/Create/C# Editor Script", false, 75 )]
		public static void CreateCSharpEditorScript()
		{
			string path = GetNewScriptPath( "NewEditor" );
			CreateScript( path, ScriptTemplatePreferences.CSharpEditorScriptDefaultCode);
		}

		private static string GetNewScriptPath( string scriptName )
		{
			string path = "Assets";
			if( Selection.activeObject != null )
			{
				path = AssetDatabase.GetAssetPath( Selection.activeObject );
				if( !AssetDatabase.IsValidFolder( path ) )
					path = Path.GetDirectoryName( path );
			}

			return Path.Combine( path, string.Concat( scriptName, ".cs" ) );
		}

		private static void CreateScript( string path, string source )
		{
			var createScriptAssetAction = ScriptableObject.CreateInstance<CreateScriptAssetAction>();
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists( 0, createScriptAssetAction, path, ScriptIcon, source );
		}
	}
}
