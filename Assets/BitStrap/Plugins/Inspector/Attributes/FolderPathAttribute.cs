using UnityEngine;

namespace BitStrap
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class FolderPathAttribute : PropertyAttribute
	{
		public bool PathRelativeToProject;

		public FolderPathAttribute(bool pathRelativeToProject = true)
		{
			PathRelativeToProject = pathRelativeToProject;
		}
	}
}