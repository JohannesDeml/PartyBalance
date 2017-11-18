// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuListener.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine.Events;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class GameStateActivationHandler : MonoBehaviour
	{
		[SerializeField, EnumFlag]
		protected GameState gameStates = 0;

		[Tooltip("Those objects will be enabled in the defined game state and disabled otherwise")]
		[SerializeField]
		private GameObject[] connectedObjects = new GameObject[0];

		void Awake()
		{
			SetActive(GameManager.GameState);
		}

		void Start()
		{
			GameManager.Instance.SwitchToMenu += OnSwitchToMenu;
			GameManager.Instance.SwitchToGame += OnSwitchToGame;
			GameManager.Instance.SwitchToTestChamber += OnSwitchToTestChamber;
		}

		void OnDestroy()
		{
			if (GameManager.ApplicationIsQuitting)
			{
				return;
			}
			GameManager.Instance.SwitchToMenu -= OnSwitchToMenu;
			GameManager.Instance.SwitchToGame -= OnSwitchToGame;
			GameManager.Instance.SwitchToTestChamber -= OnSwitchToTestChamber;
		}

		protected virtual void OnSwitchToMenu()
		{
			SetActive(GameState.Menu);
		}

        protected virtual void OnSwitchToGame()
		{
			SetActive(GameState.Game);
		}

        protected virtual void OnSwitchToTestChamber()
		{
			SetActive(GameState.TestChamber);
		}

		private void SetActive(GameState gameState)
		{
			SetActive(connectedObjects, (gameState & gameStates) == gameState);
		}

		private void SetActive(GameObject[] targets, bool active)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				var go = targets[i];
				if (go != null)
				{
					go.SetActive(active);
				}
			}
		}
	}
}