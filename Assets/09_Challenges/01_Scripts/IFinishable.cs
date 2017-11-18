// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFinishable.cs" company="Johannes Deml">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	public interface IFinishable
	{
		void TriggerFinish(IChallengeController finishedController);
	}
}