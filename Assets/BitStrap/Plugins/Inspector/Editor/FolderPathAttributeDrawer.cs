using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer(typeof(FolderPathAttribute))]
	public class FolderPathAttributeDrawer : PropertyDrawer
	{
		private const float buttonWidth = 20f;
		private const float padding = 4f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			FolderPathAttribute pathAttribute = attribute as FolderPathAttribute;
			if (property.propertyType != SerializedPropertyType.String)
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}
			string path = property.stringValue;
			position.width -= buttonWidth + padding;
			EditorGUI.PropertyField(position, property, label);
			position.x += position.width + padding;
			position.width = buttonWidth;
			if (GUI.Button(position, "Q"))
			{
				path = EditorUtility.OpenFolderPanel("Select folder", (pathAttribute.PathRelativeToProject)?ToAbsolutePath(path):path, string.Empty);
				if (string.IsNullOrEmpty(path))
				{
					return;
				}
				if (!pathAttribute.PathRelativeToProject)
				{
					property.stringValue = path;
					return;
				}
				property.stringValue = ToRelativePath(path);
			}
		}

		private string ToAbsolutePath(string relativePath)
		{
			return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + relativePath;
		}

		private string ToRelativePath(string absolutePath)
		{
			return absolutePath.Substring(Application.dataPath.Length - "Assets".Length);
		}
	}
}