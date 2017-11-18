using Mono.Cecil;
using Mono.Cecil.Mdb;
using Mono.Cecil.Pdb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BitStrap
{
	public class AssemblyIO
	{
		public HashSet<string> AssemblyPaths { get; private set; }

		public ReaderParameters ReaderParameters { get; private set; }
		public WriterParameters WriterParameters { get; private set; }

		public AssemblyIO()
		{
			AssemblyPaths = new HashSet<string>();

			ReaderParameters = new ReaderParameters();
			ReaderParameters.AssemblyResolver = CreateAssemblyResolver();
			WriterParameters = new WriterParameters();
		}

		public AssemblyDefinition GetAssemblyDefinitionInPath( string assemblyPath )
		{
			string mdbPath = assemblyPath + ".mdb";
			string pdbPath = assemblyPath.Substring( 0, assemblyPath.Length - 3 ) + "pdb";

			if( File.Exists( pdbPath ) )
			{
				ReaderParameters.ReadSymbols = true;
				ReaderParameters.SymbolReaderProvider = new PdbReaderProvider();
				WriterParameters.WriteSymbols = true;
				// pdb written out as mdb, as mono can't work with pdbs
				WriterParameters.SymbolWriterProvider = new MdbWriterProvider();
			}
			else if( File.Exists( mdbPath ) )
			{
				ReaderParameters.ReadSymbols = true;
				ReaderParameters.SymbolReaderProvider = new MdbReaderProvider();
				WriterParameters.WriteSymbols = true;
				WriterParameters.SymbolWriterProvider = new MdbWriterProvider();
			}
			else
			{
				ReaderParameters.ReadSymbols = false;
				ReaderParameters.SymbolReaderProvider = null;
				WriterParameters.WriteSymbols = false;
				WriterParameters.SymbolWriterProvider = null;
			}

			return AssemblyDefinition.ReadAssembly( assemblyPath, ReaderParameters );
		}

		private DefaultAssemblyResolver CreateAssemblyResolver()
		{
			var assemblyResolver = new DefaultAssemblyResolver();

			foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
			{
				if( assembly.Location.Replace( '\\', '/' ).StartsWith( Application.dataPath.Substring( 0, Application.dataPath.Length - 7 ) ) )
					AssemblyPaths.Add( assembly.Location );

				assemblyResolver.AddSearchDirectory( Path.GetDirectoryName( assembly.Location ) );
			}

			return assemblyResolver;
		}
	}
}
