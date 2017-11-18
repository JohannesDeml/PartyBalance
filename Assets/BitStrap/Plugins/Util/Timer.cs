using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Timer utility class. Allows you to receive a callback after a certain
	/// amount of time has elapsed.
	/// </summary>
	[System.Serializable]
	public class Timer
	{
		[SerializeField]
		private float length = 1.0f;

		private float counter = -1.0f;
		private SafeAction onTimer = new SafeAction();

		/// <summary>
		/// The timer's length in seconds.
		/// </summary>
		public float Length
		{
			get { return length; }
			set { length = value; }
		}

		/// <summary>
		/// Callback that gets called when "length" seconds has elapsed.
		/// </summary>
		public SafeAction OnTimer
		{
			get { return onTimer; }
		}

		/// <summary>
		/// The countdown time in seconds.
		/// </summary>
		public float RemainingTime
		{
			get { return counter < 0.0f ? 0.0f : Mathf.Clamp( Length - counter, 0.0f, Length ); }
		}

		/// <summary>
		/// Return a 0.0 to 1.0 number where 1.0 means the timer completed and is now stopped.
		/// </summary>
		public float Progress
		{
			get { return counter < 0.0f ? 1.0f : Mathf.InverseLerp( 0.0f, Length, counter ); }
		}

		/// <summary>
		/// Is the timer countdown running?
		/// </summary>
		public bool IsRunning
		{
			get { return counter >= 0.0f; }
		}

		public Timer( float length )
		{
			Length = length;
		}

		/// <summary>
		/// You need to manually call this at your script Update() method
		/// for the timer to work properly.
		/// </summary>
		public void OnUpdate()
		{
			if( counter < 0.0f )
			{
				// Already triggered callback.
			}
			else if( counter < Length )
			{
				counter += Time.deltaTime;
			}
			else
			{
				counter = -1.0f;
				OnTimer.Call();
			}
		}

		/// <summary>
		/// Stop the timer and its counter.
		/// </summary>
		public void Stop()
		{
			counter = -1.0f;
		}

		/// <summary>
		/// Start the timer and play its counter.
		/// </summary>
		public void Start()
		{
			counter = 0.0f;
		}
	}
}
