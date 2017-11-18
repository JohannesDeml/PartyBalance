// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Challenges.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using BitStrap;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Supyrb
{
	public class Challenges : ScriptableObject
	{
		[SerializeField]
		private List<Challenge> challenges = null;

		private ChallengeManager manager;
		private int challengePointer = 0;
		public Challenge ActiveChallenge { get; private set; }

		public void Initialize(ChallengeManager challengeManager)
		{
			manager = challengeManager;
#if UNITY_EDITOR
			var activeScene = SceneManager.GetActiveScene();
			var challengeScene = SceneManager.CreateScene("Challenges");
			SceneManager.SetActiveScene(challengeScene);
#endif

			for (int i = 0; i < challenges.Count; i++)
			{
				var challenge = challenges[i];
				challenge.Initialize(this, i);
			}

#if UNITY_EDITOR
			SceneManager.SetActiveScene(activeScene);
#endif
		}

		public void StartChallenges()
		{
			StartChallenge(0);
		}

		private void StartChallenge(int index)
		{
			challengePointer = index;
			ActiveChallenge = challenges[index];
			ActiveChallenge.StartChallenge();
		}

		public void ChallengeFinished(int challengeIndex)
		{
			Assert.AreEqual(challengeIndex, challengePointer);
			challengePointer++;
			if (challengePointer >= challenges.Count)
			{
				ActiveChallenge = null;
				manager.TriggerAllChallengesFinished();
				return;
			}
			manager.TriggerProgressChanged((float)(challengeIndex + 1)/ challenges.Count);
			StartChallenge(challengePointer);
		}

#if UNITY_EDITOR

		[SerializeField, FolderPath]
		private string challengesFolder = "Assets/";

		[Button]
		private void UpdateChallenges()
		{
			if (string.IsNullOrEmpty(challengesFolder))
			{
				Debug.Log("Set the folder path first");
				return;
			}

			var dataInFolder = new List<ChallengeData>();

			var guids = UnityEditor.AssetDatabase.FindAssets("t:ChallengeData", new string[] { challengesFolder });
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
				var data = UnityEditor.AssetDatabase.LoadAssetAtPath<ChallengeData>(assetPath);
				dataInFolder.Add(data);
			}
			challenges.Clear();
			for (int i = 0; i < dataInFolder.Count; i++)
			{
				challenges.Add(new Challenge(dataInFolder[i]));
			}
		}

#endif
	}
}