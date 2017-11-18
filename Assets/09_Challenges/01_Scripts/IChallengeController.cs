// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChallengeController.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	public interface IChallengeController
	{
		void Initialize(IFinishable challenge);
		void StartChallenge();
		void FinishChallenge();
	}
}