using BitStrap;
using Supyrb.Common;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class ConstantRotation : MonoBehaviour
	{
		[SerializeField]
		private Space space = Space.World;

		[SerializeField]
		private Vector3 rotationAxis = Vector3.up;

		[SerializeField, Unit("seconds")]
		private float degreePerSecond = 90f;

		private Quaternion rotationPerUpdate;

		void Awake()
		{
			CalculatePreFrameRotation();
		}

		void FixedUpdate()
		{
			if (space == Space.World)
			{
				transform.rotation *= rotationPerUpdate;
			}
			else
			{
				transform.localRotation *= rotationPerUpdate;
			}
		}

		private void CalculatePreFrameRotation()
		{
			rotationPerUpdate = Quaternion.AngleAxis(degreePerSecond*Time.fixedDeltaTime, rotationAxis);
		}

		void OnValidate()
		{
			if (Application.isPlaying)
			{
				CalculatePreFrameRotation();
			}
		}
	}
}