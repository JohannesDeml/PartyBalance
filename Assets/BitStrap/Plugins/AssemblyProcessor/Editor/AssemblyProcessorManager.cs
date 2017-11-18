using Mono.Cecil;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	public static class AssemblyProcessorManager
	{
		private static EditorPrefBool enabled = new EditorPrefBool( "AssemblyProcessorManager_Enabled" );

		public static bool Enabled
		{
			get { return enabled.Value; }
			set { enabled.Value = value; }
		}

		public static bool LockAndProcessAssemblies()
		{
			bool anyProcessed = false;

			try
			{
				EditorApplication.LockReloadAssemblies();
				anyProcessed = ProcessAssemblies();
			}
			catch( System.Exception e )
			{
				Debug.LogException( e );
			}
			finally
			{
				EditorApplication.UnlockReloadAssemblies();
			}

			return anyProcessed;
		}

		private static bool ProcessAssemblies()
		{
			var assemblyIO = new AssemblyIO();
			bool anyProcessed = false;

			foreach( string assemblyPath in assemblyIO.AssemblyPaths )
			{
				bool processed = ProcessAssemblyInPath( assemblyPath, assemblyIO );
				anyProcessed = anyProcessed || processed;
			}

			return anyProcessed;
		}

		private static bool ProcessAssemblyInPath( string assemblyPath, AssemblyIO assemblyIO )
		{
			AssemblyDefinition assemblyDefinition = assemblyIO.GetAssemblyDefinitionInPath( assemblyPath );

			if( ProcessAssembly( assemblyDefinition ) )
			{
				assemblyDefinition.Write( assemblyPath );
				return true;
			}

			return false;
		}

		private static bool ProcessAssembly( AssemblyDefinition assemblyDefinition )
		{
			bool processed = false;

			List<AssemblyProcessor> processors = AssemblyProcessor.FindAll();

			foreach( ModuleDefinition moduleDefinition in assemblyDefinition.Modules )
			{
				foreach( TypeDefinition typeDefinition in moduleDefinition.Types )
				{
					foreach( MethodDefinition methodDefinition in typeDefinition.Methods )
					{
						foreach( var processor in processors )
						{
							bool methodProcessed = processor.ProcessMethod( moduleDefinition, typeDefinition, methodDefinition );
							processed = processed || methodProcessed;
						}
					}
				}
			}

			return processed;
		}
	}
}
