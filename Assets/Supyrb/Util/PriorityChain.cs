// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityChain.cs" company="Supyrb">
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

	/// <summary>
	/// A linked list of callbackOrder events that can eat up an execute event or pass it on to the next node if they didn't handle it.
	/// </summary>
	public class PriorityChain
	{
		private PriorityChainNode head;

		public void Execute()
		{
			if (head == null)
			{
				return;
			}
			head.Execute();
		}

		/// <summary>
		/// Adds a new node to the chained list
		/// </summary>
		/// <param name="callbackOrder">The lower the value, the earlier this node will be called</param>
		/// <param name="processor">The method that will be processed</param>
		public void Add(int callbackOrder, Action<PriorityChainNode> processor)
		{
			var newNode = new PriorityChainNode(callbackOrder, processor);
			if (head == null || head.CallbackOrder > callbackOrder)
			{
				newNode.Next = head;
				head = newNode;
				return;
			}

			var predecessor = head;

			while (predecessor.Next != null && predecessor.Next.CallbackOrder <= callbackOrder)
			{
				predecessor = predecessor.Next;
			}
			newNode.Next = predecessor.Next;
			predecessor.Next = newNode;
		}

		public bool Remove(Action<PriorityChainNode> processor)
		{
			if (head == null)
			{
				return false;
			}

			if (head.Processor == processor)
			{
				head = head.Next;
				return true;
			}
			var predecessor = head;
			while (predecessor.Next != null)
			{
				if (predecessor.Next.Processor == processor)
				{
					predecessor.Next = predecessor.Next.Next;
					return true;
				}
				predecessor = predecessor.Next;
			}
			return false;
		}
	}

	/// <summary>
	/// A linked list of callbackOrder events that can eat up an execute event or pass it on to the next node if they didn't handle it.
	/// </summary>
	public class PriorityChain<T>
	{
		private PriorityChainNode<T> head;

		public void Execute(T obj)
		{
			if (head == null)
			{
				return;
			}
			head.Execute(obj);
		}

		/// <summary>
		/// Adds a new node to the chained list
		/// </summary>
		/// <param name="callbackOrder">The lower the value, the earlier this node will be called</param>
		/// <param name="processor">The method that will be processed</param>
		public void Add(int callbackOrder, Action<T, Action<T>> processor)
		{
			var newNode = new PriorityChainNode<T>(callbackOrder, processor);
			if (head == null || head.CallbackOrder > callbackOrder)
			{
				newNode.Next = head;
				head = newNode;
				return;
			}

			var predecessor = head;

			while (predecessor.Next != null && predecessor.Next.CallbackOrder <= callbackOrder)
			{
				predecessor = predecessor.Next;
			}
			newNode.Next = predecessor.Next;
			predecessor.Next = newNode;
		}

		public bool Remove(Action<T, Action<T>> processor)
		{
			if (head == null)
			{
				return false;
			}

			if (head.Processor == processor)
			{
				head = head.Next;
				return true;
			}
			var predecessor = head;
			while (predecessor.Next != null)
			{
				if (predecessor.Next.Processor == processor)
				{
					predecessor.Next = predecessor.Next.Next;
					return true;
				}
				predecessor = predecessor.Next;
			}
			return false;
		}
	}
}