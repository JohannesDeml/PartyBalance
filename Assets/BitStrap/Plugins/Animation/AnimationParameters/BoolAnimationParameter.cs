using System;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// A Bool Animation Parameter
	/// </summary>
	[Serializable]
	public class BoolAnimationParameter : AnimationParameter
	{
	    private bool initialized = false;
	    private bool currentValue;

        /// <summary>
		/// Sets the value of the selected parameter, based on value
		/// </summary>
		/// <param name="animator"></param>
		/// <param name="value"></param>
		public void Set( Animator animator, bool value )
		{
		    if (animator.isInitialized)
		    {
		        if (!initialized)
		        {
		            currentValue = value;
		            initialized = true;
                    animator.SetBool(Index, value);
		            return;
		        }
		        if (value != currentValue)
		        {
		            currentValue = value;
                    animator.SetBool(Index, value);
                }
            }
		}
	}
}
