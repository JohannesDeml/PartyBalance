using BitStrap;

namespace Supyrb.Common
{
	using UnityEngine;
	using System.Collections;

	public class GameManagerInterface : MonoBehaviour
	{
		[Button]
		public void TriggerStartGame()
		{
			GameManager.Instance.TriggerStartNewRun();
		}

		[Button]
		public void TriggerGameOver()
		{
			GameManager.Instance.TriggerGameOver();
		}

		[Button]
		public void TriggerRestartGame()
		{
			GameManager.Instance.TriggerRestartGame();
		}

		[Button]
		public void TriggerEndGameAndSwitchToMenu()
		{
			GameManager.Instance.TriggerEndGameAndSwitchToMenu();
		}

		[Button]
		public void TriggerSwitchToMenu()
		{
			GameManager.Instance.TriggerSwitchToMenu();
		}

		[Button]
		public void TriggerSwitchToGameAndStartGame()
		{
			GameManager.Instance.TriggerSwitchToGameAndStartGame();
		}

		[Button]
		public void TriggerSwitchToGame()
		{
			GameManager.Instance.TriggerSwitchToGame();
		}

		[Button]
		public void TriggerSwitchToTestChamber()
		{
			GameManager.Instance.TriggerSwitchToTestChamber();
		}

		[Button]
		public void QuitApplication()
		{
			GameManager.QuitApplication();
		}
	}
}