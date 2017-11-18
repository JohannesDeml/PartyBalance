// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiIntAdd.cs" company="Supyrb">
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
	public class MultiIntAdd
	{
		[SerializeField]
		private int originalValue;

		public int OriginalValue
		{
			get { return originalValue; }
			set
			{
				if (originalValue == value)
				{
					return;
				}
				var difference = value - originalValue;
				originalValue = value;
				UpdateModifiedValue(difference);
			}
		}

		public int ModifiedValue { get; private set; }
		public int MaxModifiers { get; private set; }
		public int[] Modifiers { get; private set; }
		public bool[] UsedModifierSlots { get; private set; }
		private int openModifierSlotPointer;

		public MultiIntAdd(int originalValue, int maxModifiers)
		{
			OriginalValue = originalValue;
			ModifiedValue = OriginalValue;
			MaxModifiers = maxModifiers;
			Modifiers = new int[maxModifiers];
			UsedModifierSlots = new bool[maxModifiers];
			openModifierSlotPointer = 0;
		}

		public int SetModifier(int modifierValue)
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

		public void AddValue(int index, int difference)
		{
			if (difference == 0)
			{
				return;
			}
			if (!UsedModifierSlots[index])
			{
				Modifiers[index] = 0;
				UsedModifierSlots[index] = true;
			}
			Modifiers[index] += difference;
			UpdateModifiedValue(difference);
		}

		public void SetModifier(int index, int modifierValue)
		{
			var difference = 0;
			if (UsedModifierSlots[index])
			{
				var oldValue = Modifiers[index];
				if (modifierValue == oldValue)
				{
					return;
				}
				difference = modifierValue - oldValue;
			}
			else
			{
				UsedModifierSlots[index] = true;
				difference = modifierValue;
			}
			Modifiers[index] = modifierValue;
			UpdateModifiedValue(difference);
		}

		public void RemoveModifier(int index)
		{
			if (!UsedModifierSlots[index])
			{
				return;
			}
			var difference = -Modifiers[index];
			UsedModifierSlots[index] = false;
			UpdateModifiedValue(difference);
		}

		private void UpdateModifiedValue(int difference)
		{
			ModifiedValue += difference;
		}

		public void InitializeAfterDeserialization()
		{
			ModifiedValue = OriginalValue;
			for (int i = 0; i < MaxModifiers; i++)
			{
				if (UsedModifierSlots[i])
				{
					var value = Modifiers[i];
					ModifiedValue += value;
				}
			}
		}

		public void ResetModifiers()
		{
			for (int i = 0; i < MaxModifiers; i++)
			{
				UsedModifierSlots[i] = false;
			}
			ModifiedValue = OriginalValue;
		}
	}
}