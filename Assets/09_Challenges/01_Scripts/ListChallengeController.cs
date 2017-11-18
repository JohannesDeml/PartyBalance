using System.Collections;
using BitStrap;
using UnityEngine;

namespace Supyrb
{
	public class ListChallengeController : MonoBehaviour, IChallengeController, IFinishable
	{
		[SerializeField]
		private GameObject prefab;

		[SerializeField, Unit("seconds")]
		private float deactivationDelayAfterFinish = 0.75f;

		[SerializeField]
		private Transform[] positions;

		private IChallengeController[] subChallenges;
		
		private IFinishable challenge;
		private WaitForSecondsCustomRealtime waitForSecondsRealtime;
		private int subChallengeFinishedCounter;

		public virtual void Initialize(IFinishable challenge)
		{
			this.challenge = challenge;
			waitForSecondsRealtime = new WaitForSecondsCustomRealtime(deactivationDelayAfterFinish);
			subChallenges = new IChallengeController[positions.Length];
			for (int i = 0; i < positions.Length; i++)
			{
				var position = positions[i];
				var go = Instantiate(prefab, position.localPosition, position.localRotation, transform);
				var subChallenge = go.GetComponent<IChallengeController>();
				subChallenge.Initialize(this);
				go.SetActive(false);
				subChallenges[i] = subChallenge;
			}
		}

		public virtual void StartChallenge()
		{
			gameObject.SetActive(true);
			
			if (subChallenges.Length == 0)
			{
				challenge.TriggerFinish(this);
				return;
			}
			subChallengeFinishedCounter = 0;
			for (int i = 0; i < subChallenges.Length; i++)
			{
				var subChallenge = subChallenges[i];
				subChallenge.StartChallenge();
			}
		}

		public virtual void FinishChallenge()
		{
			for (int i = 0; i < subChallenges.Length; i++)
			{
				subChallenges[i].FinishChallenge();
			}
			StartCoroutine(DisableGameObject());
		}

		private IEnumerator DisableGameObject()
		{
			waitForSecondsRealtime.UpdateEndTime();
			yield return waitForSecondsRealtime;
			gameObject.SetActive(false);
		}

		public virtual void TriggerFinish(IChallengeController subChallenge)
		{
			subChallenge.FinishChallenge();
			subChallengeFinishedCounter++;
			if (subChallengeFinishedCounter >= subChallenges.Length)
			{
				challenge.TriggerFinish(this);
				return;
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (positions == null)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			for (int i = 0; i < positions.Length; i++)
			{
				var position = positions[i];
				if (position == null)
				{
					continue;
				}
				Gizmos.DrawWireSphere(position.position, 0.25f);
			}
		}
#endif
	}
}