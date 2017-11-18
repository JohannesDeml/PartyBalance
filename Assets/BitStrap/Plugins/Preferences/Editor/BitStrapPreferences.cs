using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Check the BitStrap's preferences at "Edit/Preferences/BitStrap".
	/// </summary>
	public static class BitStrapPreferences
	{
		[PreferenceItem( "BitStrap" )]
		public static void OnPreferencesGUI()
		{
			AssemblyProcessorPreferences.OnPreferencesGUI();
			ScriptTemplatePreferences.OnPreferencesGUI();
			GUILayout.FlexibleSpace();
		}
	}
}
