using UnityEngine;

namespace BitStrap
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class ExponentInfoAttribute : PropertyAttribute
	{
		public GUIStyle labelStyle;
		public float width;

		public ExponentInfoAttribute()
		{
			labelStyle = GUI.skin.GetStyle("miniLabel");
			width = labelStyle.CalcSize(new GUIContent("0.0E+00")).x;
		}
	}
}