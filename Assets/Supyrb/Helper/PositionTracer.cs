// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionTracer.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

#endif

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class PositionTracer : MonoBehaviour
	{
#if UNITY_EDITOR
		[Tooltip("Minimum distance from one point to the next to store it. " +
				"If points should be stored at every interval set the value to -1")]
		[SerializeField]
		private float
			distanceThreshold = 0.1f;

		[SerializeField]
		private bool showTracedData = true;

		[SerializeField]
		private bool tracePosition = true;

		[SerializeField]
		private bool resetDataOnGameStart = true;

		[SerializeField]
		private float updateInterval = 0.01f;

		[SerializeField]
		private Color lineColor = Color.white;

		[Tooltip("Name for the file when it is saved")]
		[SerializeField]
		private string basicFileName = String.Empty;

		[SerializeField]
		private KeyCode saveToObjKey = KeyCode.S;

		[SerializeField]
		private KeyCode saveToScriptableObjectKey = KeyCode.S;

		private List<TransformSnapshot> simpleTransforms = new List<TransformSnapshot>(500);
        private float nextCapture;
		private float lastCapture;

		protected void Start()
		{
			if (tracePosition)
			{
				ResetValues();
			}
		}

		void OnEnable()
		{
			GameManager.Instance.StartGame += ResetValues;
		}

		void OnDisable()
		{
			if (GameManager.Instance == null || GameManager.ApplicationIsQuitting)
			{
				return;
			}

			GameManager.Instance.StartGame -= ResetValues;
		}

		void Update()
		{
			if (!tracePosition)
			{
				return;
			}

			if (GameTime.RaceTime > nextCapture)
			{
				nextCapture += updateInterval;
				lastCapture = GameTime.RaceTime;
				CaptureCurrentPosition();
			}

			if (Input.GetKeyDown(saveToObjKey))
			{
				SaveToObj();
			}

			if (Input.GetKeyDown(saveToScriptableObjectKey))
			{
				SaveToScriptableObject();
			}
		}

		private void CaptureCurrentPosition()
		{
			if (distanceThreshold <= 0 ||
				TransformSnapshot.Distance(simpleTransforms[simpleTransforms.Count - 1], transform.position) >= distanceThreshold)
			{
				simpleTransforms.Add(transform.GetSnapshot(Space.World));
			}
		}

		public void OnDrawGizmos()
		{
			if (tracePosition && showTracedData && simpleTransforms != null)
			{
				Gizmos.color = lineColor;
				for (int i = 1; i < simpleTransforms.Count; i++)
				{
					Gizmos.DrawLine(simpleTransforms[i - 1].Position, simpleTransforms[i].Position);
				}
			}
		}

		protected void ResetValues()
		{
			if (resetDataOnGameStart && tracePosition)
			{
				simpleTransforms.Clear();
			}
			nextCapture = updateInterval;
			lastCapture = 0f;
			simpleTransforms.Add(transform.GetSnapshot(Space.World));
		}

		private void SaveToObj()
		{
			if (simpleTransforms == null || simpleTransforms.Count < 5)
			{
				return;
			}

			string filePath = EditorUtility.SaveFilePanel("Export .obj file", "Data",
				basicFileName + "_" + Mathf.RoundToInt(lastCapture*60f) + " frames_" + DateTime.Now.ToString("yyyy-MM-dd"), "obj");
			if (filePath.Equals(String.Empty))
			{
				Debug.Log("Saving file aborted");
				return;
			}

			var parts = filePath.Split('/');
			var fileName = parts[parts.Length - 1];
			fileName = fileName.Substring(0, fileName.Length - ".obj".Length);

			StringBuilder meshString = new StringBuilder();

			meshString.Append("# ---------------------------"
							+ "\n# Unity automated recording of " + gameObject.name
							+ "\n# Date: " + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
							+ "\n# Length:" + lastCapture.ToString("0.000") + " seconds = "
							+ Mathf.RoundToInt(lastCapture*60f) + " frames"
							+ "\n# ---------------------------"
							+ "\n\n"
							+ "g Recording_" + fileName + "\n");

			for (int i = 0; i < simpleTransforms.Count; i++)
			{
				Vector3 pos = simpleTransforms[i].Position;
				meshString.AppendFormat("v {0} {1} {2}\n", pos.x, pos.y, pos.z);
			}

			// Needed because otherwise the obj file is invalid
			meshString.Append("\n# Simple face to create a valid obj file" +
							"\nf 1 2 3");


			WriteToFile(meshString.ToString(), filePath);
			Debug.Log("Saved file " + filePath);
		}

		private void SaveToScriptableObject()
		{
			TracedData data = ScriptableObject.CreateInstance<TracedData>();
			data.Info =
				"Recording of " + gameObject.name
				+ "\n Date: " + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
				+ "\n Length:" + lastCapture.ToString("0.000") + " seconds = "
				+ Mathf.RoundToInt(lastCapture*60f) + " frames";
			data.Length = lastCapture;
			data.UpdateInterval = updateInterval;
			data.TracedTransforms = simpleTransforms;

			string filePath = EditorUtility.SaveFilePanel("Save as scriptable object", Application.dataPath,
				basicFileName + "_" + Mathf.RoundToInt(lastCapture*60f) + " frames_" + DateTime.Now.ToString("yyyy-MM-dd"), "asset");
			if (filePath.Equals(String.Empty))
			{
				Debug.Log("Saving file aborted");
				return;
			}
			string relativeFilePath = filePath.Substring(Application.dataPath.Length - "Assets".Length);

			AssetDatabase.CreateAsset(data, relativeFilePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = data;
		}

		static void WriteToFile(string s, string filename)
		{
			using (StreamWriter sw = new StreamWriter(filename))
			{
				sw.Write(s);
			}
		}

		void Reset()
		{
			if (string.IsNullOrEmpty(basicFileName))
			{
				basicFileName = gameObject.name;
			}
		}
#endif
	}
}