using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer(typeof(ExponentInfoAttribute))]
	public class ExponentInfoDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label);
			ExponentInfoAttribute labelAttribute = attribute as ExponentInfoAttribute;
			if (labelAttribute == null)
			{
				return;
			}

			string exp = "-";
			if (property.type.Equals("int"))
			{
				exp = property.intValue.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("long"))
			{
				exp = property.longValue.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("float"))
			{
				exp = property.floatValue.ToString("G2", CultureInfo.InvariantCulture);
			}
			else if (property.type.Equals("double"))
			{
				exp = property.doubleValue.ToString("G2", CultureInfo.InvariantCulture);
			}
			GUI.Label(position.Right(labelAttribute.width + 2f), exp, labelAttribute.labelStyle);
		}
	}
}