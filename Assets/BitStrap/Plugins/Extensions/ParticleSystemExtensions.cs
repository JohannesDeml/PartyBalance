using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Extensions to the ParticleSystem class.
	/// </summary>
	public static class ParticleSystemExtensions
	{
		/// <summary>
		/// Enables/disables the emission module in a ParticleSystem.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="enabled"></param>
		public static void EnableEmission( this ParticleSystem self, bool enabled )
		{
			ParticleSystem.EmissionModule emission = self.emission;
			emission.enabled = enabled;
		}
	}
}
