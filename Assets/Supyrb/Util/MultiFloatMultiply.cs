// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiFloatMultiply.cs" company="Supyrb">
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
	public class MultiFloatMultiply
	{
		[SerializeField]
		private float originalValue;

		public float OriginalValue
		{
			get { return originalValue; }
			set
			{
				originalValue = value;
				UpdateModifiedValue();
			}
		}

		public float ModifiedValue { get; private set; }
		public int MaxModifiers { get; private set; }
		public float[] Modifiers { get; private set; }
		public bool[] UsedModifierSlots { get; private set; }
		private int openModifierSlotPointer;

		public MultiFloatMultiply(float originalValue, int maxModifiers)
		{
			this.originalValue = originalValue;
			ModifiedValue = this.originalValue;
			MaxModifiers = maxModifiers;
			Modifiers = new float[maxModifiers];
			UsedModifierSlots = new bool[maxModifiers];
			openModifierSlotPointer = 0;
		}

		public int SetModifier(float modifierValue)
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

		public void SetModifier(int index, float modifierValue)
		{
			if (UsedModifierSlots[index] && Mathf.Approximately(Modifiers[index], modifierValue))
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
					if (value == 0f)
					{
						ModifiedValue = 0f;
						return;
					}
					ModifiedValue *= value;
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