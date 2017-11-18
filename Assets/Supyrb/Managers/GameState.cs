// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameState.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	[System.Flags]
	public enum GameState
	{
		None = 1 << 0,
		Menu = 1 << 1,
		Game = 1 << 2,
		TestChamber = 1 << 3,
	}
}