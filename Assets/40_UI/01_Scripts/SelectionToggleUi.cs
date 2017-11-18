// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionToggleUi.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using TMPro;
using UnityEngine.UI;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;
	
	public class SelectionToggleUi : MonoBehaviour
	{
		public delegate void SelectionToggleDelegate(SelectionToggleUi selectionToggle);

		public event SelectionToggleDelegate OnItemSelected;

		[SerializeField]
		private Toggle toggle;

		[SerializeField]
		private TextMeshProUGUI text;

		public string TextValue
		{
			get { return text.text; }
		}

		private bool initialized;

		void Awake()
		{
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		public void Initialize(ToggleGroup toggleGroup, string descriptionText, bool isOn)
		{
			initialized = false;
			toggle.group = toggleGroup;
			text.text = descriptionText;
			toggle.isOn = isOn;
			gameObject.SetActive(true);
			initialized = true;
		}

		private void OnValueChanged(bool isOn)
		{
			if (!initialized || !isOn)
			{
				return;
			}
			if (OnItemSelected != null)
			{
				OnItemSelected(this);
			}
		}

#if UNITY_EDITOR

		void Reset()
		{
			if (toggle == null)
			{
				toggle = GetComponentInChildren<Toggle>();
			}

			if (text == null)
			{
				text = GetComponentInChildren<TextMeshProUGUI>();
			}
		}
#endif
	}
}

