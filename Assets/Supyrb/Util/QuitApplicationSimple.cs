// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuitApplicationSimple.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb.Common
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// Listens to an input and quits the application if the input is pressed
	/// </summary>
	public class QuitApplicationSimple : MonoBehaviour
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		[SerializeField]
		private KeyCode applicationQuitKeyCode = KeyCode.Escape;

		void Update()
		{
			if (Input.GetKeyDown(applicationQuitKeyCode))
			{
				GameManager.QuitApplication();
			}
		}
#endif
	}
}