// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFlagAttribute.cs" company="Supyrb">
//   Copyright (c) 2016 Supyrb. All rights reserved.
// </copyright>
// <author>
//   http://wiki.unity3d.com/index.php/EnumFlagPropertyDrawer
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	using UnityEngine;

	public class EnumFlagAttribute : PropertyAttribute
	{
		public string enumName;

		public EnumFlagAttribute()
		{
		}

		public EnumFlagAttribute(string name)
		{
			enumName = name;
		}
	}
}