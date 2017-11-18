using BitStrap;
using Supyrb.Common;
using UnityEngine.Events;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;

	public class TimedUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent activationEvent = null;

		[SerializeField]
		private bool activateSelf = false;

		[SerializeField]
		private MonoBehaviourActivationType activationType = MonoBehaviourActivationType.OnEnable;

		[SerializeField]
		private UpdateMode updateMode = UpdateMode.ScaledTime;

		[SerializeField]
		private float timeTillActivation = 0.3f;

		[SerializeField, ReadOnly]
		private bool running = false;

		private WaitForSeconds waitTillActivationScaled;
		private WaitForSecondsCustomRealtime waitTillActivationUnscaled;
		// REFACTOR somehow storing the variable doesn't work
		//private IEnumerator activationCoroutine;

		void Awake()
		{
			SetWaitingTime();
			if (activateSelf && activationType == MonoBehaviourActivationType.Awake)
			{
				TriggerTimer();
			}
		}

		void OnEnable()
		{
			if (activateSelf && activationType == MonoBehaviourActivationType.OnEnable)
			{
				TriggerTimer();
			}
		}

		void Start()
		{
			if (activateSelf && activationType == MonoBehaviourActivationType.Start)
			{
				TriggerTimer();
			}
		}

		[Button]
		public void TriggerTimer()
		{
			CancelTimer();
			if (timeTillActivation <= 0.001f)
			{
				TriggerEvent();
			}
			else
			{
				StartCoroutine(ActivationCoroutine());
			}
		}

		private IEnumerator ActivationCoroutine()
		{
			running = true;
			if (updateMode == UpdateMode.UnscaledTime)
			{
				waitTillActivationUnscaled.UpdateEndTime();
				yield return waitTillActivationUnscaled;
			}
			else
			{
				yield return waitTillActivationScaled;
			}
			TriggerEvent();
			running = false;
		}

		private void TriggerEvent()
		{
			activationEvent.Invoke();
		}

		public void CancelTimer()
		{
			if (running)
			{
				StopCoroutine(ActivationCoroutine());
				running = false;
			}
		}

		private void SetWaitingTime()
		{
			if (updateMode == UpdateMode.UnscaledTime)
			{
				waitTillActivationUnscaled = new WaitForSecondsCustomRealtime(timeTillActivation);
			}
			else
			{
				waitTillActivationScaled = new WaitForSeconds(timeTillActivation);
			}
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (Application.isPlaying)
			{
				SetWaitingTime();
			}
		}
#endif
	}
}