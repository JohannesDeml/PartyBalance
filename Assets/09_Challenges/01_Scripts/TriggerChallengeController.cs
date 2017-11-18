using System;
using UnityEngine;
using UnityEngine.Events;

namespace Supyrb
{
	public class TriggerChallengeController : MonoBehaviour, IChallengeController
	{
		[SerializeField]
		private UnityEvent onStartChallenge = null;

		[SerializeField]
		private UnityEvent onFinishChallenge = null;

		[SerializeField]
		private GameObject triggerObject;

		[SerializeField]
		private float showUpDuration = 0.1f;

		[SerializeField]
		private LeanTweenType showUpEasingType = LeanTweenType.easeOutElastic;

		[SerializeField]
		private float hideDuration = 0.1f;

		[SerializeField]
		private LeanTweenType hideEasingType = LeanTweenType.easeInBack;

		private IFinishable challenge;
		private bool triggered;

		public void Initialize(IFinishable challenge)
		{
			this.challenge = challenge;
		}

		public void StartChallenge()
		{
			triggered = false;
			LeanTween.cancel(triggerObject);
			triggerObject.transform.localScale = Vector3.zero;
			LeanTween.scale(triggerObject, Vector3.one, showUpDuration)
				.setEase(showUpEasingType);
			gameObject.SetActive(true);
			onStartChallenge.Invoke();
		}

		public void FinishChallenge()
		{
			onFinishChallenge.Invoke();
			LeanTween.cancel(triggerObject);
			triggerObject.transform.localScale = Vector3.zero;
			LeanTween.scale(triggerObject, Vector3.zero, hideDuration)
				.setEase(hideEasingType)
				.setOnComplete(OnHideComplete);
		}

		private void OnHideComplete()
		{
			gameObject.SetActive(false);
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (triggered || collision.gameObject.layer != SupyrbLayers.Player)
			{
				return;
			}

			challenge.TriggerFinish(this);
			triggered = true;
		}

#if UNITY_EDITOR

		void Reset()
		{
			if (triggerObject == null)
			{
				triggerObject = gameObject;
			}
		}
#endif
	}
}