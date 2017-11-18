// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiBoolAnd.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using System;
	using UnityEngine;

	[Serializable]
	public class MultiBoolAnd
	{
		[SerializeField]
		private bool originalValue;

		public bool OriginalValue
		{
			get { return originalValue; }
		}

		public bool ModifiedValue { get; private set; }
		public int MaxModifiers { get; private set; }
		public bool[] Modifiers { get; private set; }
		public bool[] UsedModifierSlots { get; private set; }
		private int openModifierSlotPointer;

		public MultiBoolAnd(int maxModifiers)
		{
			originalValue = true;
			ModifiedValue = originalValue;
			this.MaxModifiers = maxModifiers;
			Modifiers = new bool[maxModifiers];
			UsedModifierSlots = new bool[maxModifiers];
			openModifierSlotPointer = 0;
		}

		public int SetModifier(bool modifierValue)
		{
			for (int i = openModifierSlotPointer; i < MaxModifiers + openModifierSlotPointer; i++)
			{
				var index = i%MaxModifiers;
				if (!UsedModifierSlots[index])
				{
					SetModifier(index, modifierValue);
					openModifierSlotPointer = index + 1;
					return index;
				}
			}
			throw new IndexOutOfRangeException("Trying to set a new modifier value, but all modifiers are set.");
		}

		public void SetModifier(int index, bool modifierValue)
		{
			if (UsedModifierSlots[index] && Modifiers[index] == modifierValue)
			{
				return;
			}
			Modifiers[index] = modifierValue;
			UsedModifierSlots[index] = true;
			UpdateModifiedValue();
		}

		public void RemoveModifier(int index)
		{
			if (!UsedModifierSlots[index])
			{
				return;
			}
			UsedModifierSlots[index] = false;
			UpdateModifiedValue();
		}

		private void UpdateModifiedValue()
		{
			ModifiedValue = originalValue;
			for (int i = 0; i < MaxModifiers; i++)
			{
				if (UsedModifierSlots[i])
				{
					var value = Modifiers[i];
					if (value == false)
					{
						ModifiedValue = false;
						return;
					}
				}
			}
		}

		public void InitializeAfterDeserialization()
		{
			UpdateModifiedValue();
		}

		public void ResetModifiers()
		{
			for (int i = 0; i < MaxModifiers; i++)
			{
				UsedModifierSlots[i] = false;
			}
			ModifiedValue = originalValue;
		}
	}
}