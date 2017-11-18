// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameManager.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using BitStrap;

namespace Supyrb
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// The game manager is used as a singleton and streamlines the general changes of game states for easy access
	/// of important changes in the game.
	/// The game manager further logs all events that happened in the past to easily see if everything works correctly.
	/// It also streamlines the information if the application is quitting, so other scripts don't have to listen 
	/// for that to determine if OnDisable or OnDestroy was called due to the termination of the game.
	/// </summary>
	public class GameManager : Singleton<GameManager>
	{
		public static List<Light> ActiveLights = new List<Light>();

		public delegate void GameInformationDelegate();

		/// <summary>
		/// Event chain that is triggered before the run is initialized. The big advantage of this chain
		/// is that user interaction can easily be integrated with this interface. Elements with higher priority are invoked first.
		/// </summary>
		public PriorityChain PrepareRun = new PriorityChain();

		/// <summary>
		/// Event that is triggered just before the game starts. At a restart of the game this event will too be thrown
		/// This event does have a priority in order to control the flow of invoking the events
		/// </summary>
		public PriorityEventList InitializeRun = new PriorityEventList();

		/// <summary>
		/// Event that is triggered once the game starts. At a restart of the game this event will too be thrown
		/// </summary>
		public event GameInformationDelegate StartGame;

		/// <summary>
		/// This event is thrown if the game ends. It should be triggered at any kind of restart 
		/// before the StartGame event is triggered.
		/// </summary>
		public event GameInformationDelegate GameOver;

		/// <summary>
		/// This event is triggered when the application switches to the main game scene
		/// </summary>
		public event GameInformationDelegate SwitchToGame;

		/// <summary>
		/// This event is triggered when the application is switching to the main menu of the. Don't trigger this event 
		/// if an overlay menu is drawn in the game.
		/// </summary>
		public event GameInformationDelegate SwitchToMenu;

		/// <summary>
		/// This event is triggered when the game switches to test chamber
		/// </summary>
		public event GameInformationDelegate SwitchToTestChamber;


		public event GameInformationDelegate RunEnded;

		public event GameInformationDelegate ShowRunEndUI;

		public event GameInformationDelegate PostProcessRun;

		/// <summary>
		/// Indicating the current focus of the application.
		/// Useful at the start of the application to give information to objects that were not initialized
		/// when the first event was fired, but need to know what the current state is.
		/// </summary>
		public static GameState GameState { get; private set; }

		/// <summary>
		/// Is true when the player lost the game and is automatically set to true when the game over event is triggered
		/// With the GameStart event the bool is set to false again.
		/// </summary>
		public static bool IsGameOver { get; private set; }

		/// <summary>
		/// This bool is true when the game is quitting and is especially interesting in OnDisable or OnDestroy functions
		/// </summary>
		public static bool ApplicationIsQuitting { get; private set; }

		/// <summary>
		/// A list of all game events that happened so far.
		/// </summary>
		public static List<GameEventInformation> GameEvents { get; private set; }

		/// <summary>
		/// The URL the webplayer will switch to if the application is "Quit"
		/// </summary>
#if UNITY_WEBPLAYER
        public static string webplayerQuitURL = "http://supyrb.com";
#endif

		/// <summary>
		/// guarantee this will be always a singleton only - can't use the constructor!
		/// Handled by the Singleton class it is inheriting from
		/// </summary>
		protected GameManager()
		{
			IsGameOver = true;
			ApplicationIsQuitting = false;
			GameState = GameState.None;
			GameEvents = new List<GameEventInformation>();
			PrepareRun.Add(int.MinValue, InitializeAndStartGame);
		}

		void Awake()
		{
			ForceSingletonInstance();
		}

		public void TriggerExitReached()
		{
			// Trigger run ended next frame
			Invoke("TriggerRunEnded", 0.01f);
		}

		public void TriggerRunEnded()
		{
#if UNITY_EDITOR
			LogEvent("RunEnded");
#endif
			GameTime.EndRace();
			if (RunEnded != null)
			{
				RunEnded();
			}

			if (PostProcessRun != null)
			{
				PostProcessRun();
			}

			if (ShowRunEndUI != null)
			{
				ShowRunEndUI();
			}

			TriggerGameOver();
		}

		/// <summary>
		/// Start a new game and send an event to all scripts interested in the event
		/// </summary>
		public void TriggerStartNewRun()
		{
#if UNITY_EDITOR
			LogEvent("StartNewRun");
#endif
			if (!IsGameOver)
			{
				Debug.LogWarning("Trying to start the game even though no game over was fired before." +
								"This Request will be ignored to avoid strange behavior.");
				return;
			}
			IsGameOver = false;

			// Will call Initialize and Start Game in the end
			PrepareRun.Execute();
		}

		private void InitializeAndStartGame(PriorityChainNode chainNode)
		{
			InitializeRun.Invoke();
#if UNITY_EDITOR
			LogEvent("StartGame");
#endif
			GameTime.StartRace();
			if (StartGame != null)
			{
				StartGame();
			}
			chainNode.ExecuteNext();
		}

		/// <summary>
		/// End the game and switch to the menu. Send events to all scripts interested in the event
		/// </summary>
		public void TriggerEndGameAndSwitchToMenu()
		{
			TriggerGameOver();
			TriggerSwitchToMenu();
		}

		/// <summary>
		/// Switch to the menu and send an event to all scripts interested in the event
		/// </summary>
		public void TriggerSwitchToMenu()
		{
#if UNITY_EDITOR
			LogEvent("SwitchToMenu");
#endif
			GameState = GameState.Menu;
			if (SwitchToMenu != null)
			{
				SwitchToMenu();
			}
		}

		/// <summary>
		/// Switch to the game and start it afterwards. Send events to all scripts interested in the event
		/// </summary>
		public void TriggerSwitchToGameAndStartGame()
		{
			TriggerSwitchToGame();
			TriggerStartNewRun();
		}

		/// <summary>
		/// Switch to the game and send an event to all scripts interested in the event
		/// </summary>
		public void TriggerSwitchToGame()
		{
#if UNITY_EDITOR
			LogEvent("SwitchToGame");
#endif
			GameState = GameState.Game;
			if (SwitchToGame != null)
			{
				SwitchToGame();
			}
		}

		/// <summary>
		/// Switch to the test chamber and send an event to all scripts interested in the event
		/// </summary>
		public void TriggerSwitchToTestChamber()
		{
#if UNITY_EDITOR
			LogEvent("SwitchToTestChamber");
#endif
			GameState = GameState.TestChamber;
			if (SwitchToTestChamber != null)
			{
				SwitchToTestChamber();
			}
		}

		/// <summary>
		/// Trigger the game over event and send the event to all scripts interested in the event
		/// </summary>
		public void TriggerGameOver()
		{
#if UNITY_EDITOR
			LogEvent("GameOver");
#endif
			GameTime.EndRace();
			IsGameOver = true;
			if (GameOver != null)
			{
				GameOver();
			}
		}

		/// <summary>
		/// Trigger the GameOver event and the StartGame event afterwards
		/// </summary>
		public void TriggerRestartGame()
		{
			TriggerGameOver();
			TriggerStartNewRun();
		}

		/// <summary>
		/// This method works like <see cref="Application.Quit"/> , but also supports quitting the editor and web player
		/// </summary>
		public static void QuitApplication()
		{
			ApplicationIsQuitting = true;
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
		}

		private void LogEvent(string eventName)
		{
			GameEvents.Add(new GameEventInformation(GameTime.TimeSinceStartup, eventName));
		}

		protected override void OnApplicationQuit()
		{
			base.OnApplicationQuit();
			ApplicationIsQuitting = true;
		}
	}
}