using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Collection of helper methods when coding a PropertyDrawer editor.
	/// </summary>
	public static class PropertyDrawerHelper
	{
		/// <summary>
		/// If the target property has a [Tooltip] attribute, load it into its label.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="label"></param>
		public static void LoadAttributeTooltip( PropertyDrawer self, GUIContent label )
		{
			var tooltipAttribute = self.fieldInfo.GetCustomAttributes( typeof( TooltipAttribute ), true ).FirstOrDefault() as TooltipAttribute;
			if( tooltipAttribute != null )
				label.tooltip = tooltipAttribute.tooltip;
		}
	}
}
