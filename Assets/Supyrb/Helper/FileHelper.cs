// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Supyrb">
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

	public static class FileHelper
	{
		public static string GetFileSize(double byteCount)
		{
			string size = "0 Bytes";
			if (byteCount >= 1073741824.0)
				size = String.Format("{0:##.##}", byteCount/1073741824.0) + " GB";
			else if (byteCount >= 1048576.0)
				size = String.Format("{0:##.##}", byteCount/1048576.0) + " MB";
			else if (byteCount >= 1024.0)
				size = String.Format("{0:##.##}", byteCount/1024.0) + " KB";
			else if (byteCount > 0 && byteCount < 1024.0)
				size = byteCount.ToString() + " Bytes";

			return size;
		}
	}
}