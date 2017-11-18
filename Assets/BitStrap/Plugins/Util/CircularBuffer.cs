using System.Collections;
using System.Collections.Generic;

namespace BitStrap
{
	/// <summary>
	/// An insert optimized queue that removes the last element if a new one comes when
	/// it reaches its maximum capacity. It does not support removal, though.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CircularBuffer<T> : IEnumerable<T>
	{
		private T[] elements;
		private int tail;

		/// <summary>
		/// Number of elements.
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// First element.
		/// </summary>
		public T Current
		{
			get { return elements[tail]; }
		}

		/// <summary>
		/// Directly access any element like an array.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get { return elements[( tail + index ) % Count]; }
			set { elements[( tail + index ) % Count] = value; }
		}

		/// <summary>
		/// Creates a CircularBuffer given its capacity.
		/// </summary>
		/// <param name="capacity"></param>
		public CircularBuffer( int capacity )
		{
			elements = new T[capacity];
			Count = 0;
			tail = 0;
		}

		/// <summary>
		/// Append an element. It may override the last element if this reaches its maximum element capacity.
		/// </summary>
		/// <param name="element"></param>
		public void Add( T element )
		{
			elements[tail] = element;

			int capacity = elements.Length;
			Count = Count < capacity ? Count + 1 : capacity;
			tail = ( tail + 1 ) % capacity;
		}

		/// <summary>
		/// Clears the buffer.
		/// </summary>
		public void Clear()
		{
			Count = 0;
			tail = 0;
		}

		/// <summary>
		/// Returns an enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			int count = Count;
			for( int i = 0; i < count; i++ )
				yield return this[i];
		}

		/// <summary>
		/// Returns an enumerator.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			int count = Count;
			for( int i = 0; i < count; i++ )
				yield return this[i];
		}
	}
}
