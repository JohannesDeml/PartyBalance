// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenshotManager.cs" company="Supyrb">
//   Copyright (c) 2015 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using System;
	using UnityEngine;
	using System.Collections;
	using System.IO;
	using BitStrap;

	/// <summary>
	/// Takes screenshots with the double resolution
	/// </summary>
	public class ScreenshotManager : MonoBehaviour
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		[Tooltip("This is the path relative to the project folder, so Application.dataPath + /..")]
		[SerializeField, FolderPath()]
		private string relativeFilePath = "/Data/Screenshots/";

		[SerializeField]
		private int superSize = 2;

		[SerializeField]
		private KeyCode shortcut = KeyCode.F9;

		private string filePath;

		private void Start()
		{
			filePath = Application.dataPath + "/.." + relativeFilePath;
		}

		/// <summary>
		/// Listens for and takes screenshots
		/// </summary>
		private void Update()
		{
			if (Input.GetKeyDown(shortcut))
			{
				TakeScreenshot();
			}
		}

		[Button]
		public void TakeScreenshot()
		{
			// If the screenshot folder does not exist yet, create it
			if (!Directory.Exists(filePath))
			{
				Debug.LogFormat(gameObject, "The folder {0} did not exist, but was created now", filePath);
				Directory.CreateDirectory(filePath);
			}

			// Save the screenshot
			string newFile = filePath + "Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd--H-mm-ss") + ".png";
			Debug.Log("Taking screenshot " + newFile, gameObject);
			Application.CaptureScreenshot(newFile, superSize);
		}
#endif

#if UNITY_EDITOR
		[Button]
		public void OpenScreenshotFolder()
		{
			DirectoryInfo projectPath = new DirectoryInfo(Application.dataPath).Parent;
			var path = projectPath.FullName + relativeFilePath.Replace('/', Path.DirectorySeparatorChar);
			UnityEditor.EditorUtility.RevealInFinder(path);
		}
#endif
	}
}