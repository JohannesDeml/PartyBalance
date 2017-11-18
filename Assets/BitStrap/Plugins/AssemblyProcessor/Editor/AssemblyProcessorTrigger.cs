using UnityEditor;
using UnityEditorInternal;

namespace BitStrap
{
	[InitializeOnLoad]
	public static class AssemblyProcessorTrigger
	{
		static AssemblyProcessorTrigger()
		{
			if( !AssemblyProcessorManager.Enabled )
				return;

			var isReloading = new EditorPrefBool( "AssemblyProcessorManager_IsReloadingScripts" );
			bool wasReloadingScripts = isReloading.Value;
			isReloading.Value = false;

			if( !wasReloadingScripts && AssemblyProcessorManager.LockAndProcessAssemblies() )
			{
				isReloading.Value = true;
				InternalEditorUtility.RequestScriptReload();
			}
		}
	}
}
