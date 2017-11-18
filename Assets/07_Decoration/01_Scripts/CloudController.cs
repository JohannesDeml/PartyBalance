// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudController.cs" company="Johannes Deml">
//   Copyright (c) 2017 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using BitStrap;
using UnityEngine.SceneManagement;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	[Serializable]
	public class CloudData
	{
		public Transform Transform { get; private set; }
		public SpriteRenderer SpriteRenderer { get; private set; }
		public Vector3 Position;
		public float SpeedPerSecond;

		public CloudData(Transform transform, SpriteRenderer spriteRenderer)
		{
			Transform = transform;
			SpriteRenderer = spriteRenderer;
			Position = transform.localPosition;
		}

		public void Update(float deltaTime)
		{
			Position.x += SpeedPerSecond * deltaTime;
			Transform.localPosition = Position;
		}
	}

	public class CloudController : MonoBehaviour
	{
		public enum Direction
		{
			LeftToRight,
			RightToLeft
		}

		[SerializeField, Range(0, 10)]
		private int numClouds = 5;

		[SerializeField]
		private GameObject cloudPrefab;

		[SerializeField]
		private Sprite[] cloudSprites;

		[SerializeField]
		private Rect cloudBorders = new Rect(Vector2.zero, new Vector2(40f, 10f));

		[SerializeField]
		private MinMaxRange cloudSpeed = new MinMaxRange(0f, 10f, 1f, 3f);

		[SerializeField]
		private Direction direction = Direction.LeftToRight;

		[SerializeField, ReadOnly]
		private float directionMultiplier = 1f;

		private CloudData[] clouds;

		void Start()
		{
#if UNITY_EDITOR
			var currentActiveScene = SceneManager.GetActiveScene();
			var cloudScene = SceneManager.CreateScene("Clouds");
			SceneManager.SetActiveScene(cloudScene);
#endif
			clouds = new CloudData[numClouds];

			for (int i = 0; i < numClouds; i++)
			{
				Vector3 startPosition = Vector3.zero;
				startPosition.x = Random.Range(cloudBorders.xMin, cloudBorders.xMax);
				startPosition.y = Random.Range(cloudBorders.yMin, cloudBorders.yMax);
				var go = Instantiate(cloudPrefab, startPosition, Quaternion.identity);
				var spriteRenderer = go.GetComponent<SpriteRenderer>();
				var cloudData = new CloudData(go.transform, spriteRenderer);
				cloudData.SpriteRenderer.sprite = GetRandomSprite();
				cloudData.SpeedPerSecond = cloudSpeed.GetRandomValue() * directionMultiplier;
				clouds[i] = cloudData;
			}
#if UNITY_EDITOR
			SceneManager.SetActiveScene(currentActiveScene);
#endif
		}

		void Update()
		{
			for (int i = 0; i < numClouds; i++)
			{
				var cloud = clouds[i];
				if (direction == Direction.LeftToRight)
				{
					if (cloud.Position.x > cloudBorders.xMax)
					{
						CloudReachedEnd(cloud);
						continue;
					}
				}
				else
				{
					if (cloud.Position.x < cloudBorders.xMin)
					{
						CloudReachedEnd(cloud);
						continue;
					}
				}
				cloud.Update(GameTime.DeltaRaceTime);
			}
		}

		private void CloudReachedEnd(CloudData cloudData)
		{
			cloudData.SpriteRenderer.sprite = GetRandomSprite();
			cloudData.SpeedPerSecond = cloudSpeed.GetRandomValue() * directionMultiplier;
			cloudData.Position.x = direction == Direction.LeftToRight ? cloudBorders.xMin : cloudBorders.xMax;
			cloudData.Position.y = Random.Range(cloudBorders.yMin, cloudBorders.yMax);
		}

		private Sprite GetRandomSprite()
		{
			return cloudSprites[Random.Range(0, cloudSprites.Length)];
		}

#if UNITY_EDITOR

		void OnValidate()
		{
			directionMultiplier = direction == Direction.LeftToRight ? 1f : -1f;
		}
		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(new Vector3(cloudBorders.xMin, cloudBorders.yMin, 0f),
				new Vector3(cloudBorders.xMin, cloudBorders.yMax, 0f));
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(new Vector3(cloudBorders.xMax, cloudBorders.yMin, 0f),
				new Vector3(cloudBorders.xMax, cloudBorders.yMax, 0f));
		}
#endif
	}
}

