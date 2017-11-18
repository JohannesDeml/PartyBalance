// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenAssetListener.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;

namespace Supyrb
{
    using UnityEngine;
	using UnityEditor;
	using UnityEditor.Callbacks;

	public class OpenAssetListener
	{
		// TODO don't hard code this!
		public const string ShaderProgramPath = "C:\\Program Files (x86)\\Notepad++\\notepad++.exe";

		public static object ScriptableObjectInspectorWindow { get; private set; }

		[OnOpenAsset(0)]
        public static bool HandleOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
	        if (obj == null)
	        {
		        return false;
	        }

            Type type = obj.GetType();

#if UNITY_EDITOR_WIN
			if (type == typeof(Shader))
			{
				var assetPath = AssetDatabase.GetAssetPath(obj);
				var filePath = Application.dataPath + assetPath.Substring("Assets".Length);
				return RunApplication(ShaderProgramPath, filePath);
			}

	        if (type == typeof(TextAsset))
	        {
				var assetPath = AssetDatabase.GetAssetPath(obj);
		        if (Path.GetExtension(assetPath) == ".cginc")
		        {
					var filePath = Application.dataPath + assetPath.Substring("Assets".Length);
					return RunApplication(ShaderProgramPath, filePath);
				}
			}
#endif
			return false; // No custom option was hit, leaving opening file to next handler
        }

		private static bool RunApplication(string applicationPath, string filePath)
		{
			Process process = new Process();
			process.StartInfo.FileName = applicationPath;
			process.StartInfo.Arguments = filePath;
			return process.Start();
		}
    }
}

