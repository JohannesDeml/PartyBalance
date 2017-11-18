// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityChainNode.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Supyrb
{
	using UnityEngine;
	using System.Collections;


	public class PriorityChainNode
	{
		public PriorityChainNode Next;
		public int CallbackOrder { get; private set; }
		public Action<PriorityChainNode> Processor { get; private set; }

		public PriorityChainNode(int callbackOrder, Action<PriorityChainNode> processor)
		{
			CallbackOrder = callbackOrder;
			this.Processor = processor;
		}

		public void Execute()
		{
			Processor.Invoke(this);
		}

		public void ExecuteNext()
		{
			if (Next == null)
			{
				return;
			}
			Next.Execute();
		}

		public int CompareTo(PriorityChainNode other)
		{
			return CallbackOrder.CompareTo(other.CallbackOrder);
		}
	}

	public class PriorityChainNode<T>
	{
		public PriorityChainNode<T> Next;
		public int CallbackOrder { get; private set; }
		public Action<T, Action<T>> Processor { get; private set; }

		public PriorityChainNode(int callbackOrder, Action<T, Action<T>> processor)
		{
			CallbackOrder = callbackOrder;
			this.Processor = processor;
		}

		public void Execute(T obj)
		{
			Processor.Invoke(obj, ExecuteNext);
		}

		private void ExecuteNext(T obj)
		{
			if (Next == null)
			{
				return;
			}
			Next.Execute(obj);
		}

		public int CompareTo(PriorityChainNode<T> other)
		{
			return CallbackOrder.CompareTo(other.CallbackOrder);
		}
	}
}