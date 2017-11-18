// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrophoneSelectionUi.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class MicrophoneSelectionUi : MonoBehaviour
	{
		[SerializeField]
		private GameObject selectionTogglePrefab = null;

		[SerializeField]
		private ToggleGroup toggleGroup;

		private List<SelectionToggleUi> selections;
		private string[] microphoneList;
		private MicrophoneInput microphoneInput;

		void Awake()
		{
			selections = new List<SelectionToggleUi>();
			microphoneInput = MicrophoneInput.Instance;
		}

		void OnEnable()
		{
			RefreshList();
		}

		public void RefreshList()
		{
			if (microphoneList != null)
			{
				var microphones = Microphone.devices;
				if (microphones == microphoneList)
				{
					// List already up to date
					return;
				}
			}
			microphoneList = Microphone.devices;
			for (int i = 0; i < microphoneList.Length; i++)
			{
				if (selections.Count > i)
				{
					// UI element already created
					continue;
				}
				var go = Instantiate(selectionTogglePrefab, transform);
				var selectionToggle = go.GetComponent<SelectionToggleUi>();
				selectionToggle.OnItemSelected += OnItemSelected;
				selections.Add(selectionToggle);
			}

			// disable not needed elements
			for (int i = microphoneList.Length; i < selections.Count; i++)
			{
				var selection = selections[i];
				selection.gameObject.SetActive(false);
			}

			for (int i = 0; i < microphoneList.Length; i++)
			{
				var selection = selections[i];
				var microphoneName = microphoneList[i];
				bool isOn = microphoneName == microphoneInput.Device;
				selection.Initialize(toggleGroup, microphoneName, isOn);
			}
		}

		private void OnItemSelected(SelectionToggleUi selectiontoggle)
		{
			Debug.LogFormat("Microphone device {0} selected", selectiontoggle.TextValue);
			microphoneInput.SetDevice(selectiontoggle.TextValue);
		}
	}
}

