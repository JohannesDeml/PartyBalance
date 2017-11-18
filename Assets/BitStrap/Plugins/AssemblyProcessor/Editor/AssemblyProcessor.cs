using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BitStrap
{
	public abstract class AssemblyProcessor
	{
		protected virtual System.Type MethodFilterAttribute { get { return null; } }

		protected ModuleDefinition ModuleDefinition { get; set; }
		protected TypeDefinition TypeDefinition { get; set; }
		protected MethodDefinition MethodDefinition { get; set; }

		protected ILProcessor ILProcessor { get; set; }

		public static List<AssemblyProcessor> FindAll()
		{
			Assembly assembly = Assembly.GetAssembly( typeof( AssemblyProcessor ) );
			var methodProcessors = new List<AssemblyProcessor>();

			foreach( System.Type type in assembly.GetTypes() )
			{
				if( type.IsClass && type.IsSubclassOf( typeof( AssemblyProcessor ) ) )
					methodProcessors.Add( System.Activator.CreateInstance( type ) as AssemblyProcessor );
			}

			return methodProcessors;
		}

		public bool ProcessMethod( ModuleDefinition moduleDefinition, TypeDefinition typeDefinition, MethodDefinition methodDefinition )
		{
			var filterAttributeType = MethodFilterAttribute;
			if( filterAttributeType == null )
				return false;

			string attributeName = filterAttributeType.GetCecilFullname();
			foreach( CustomAttribute customAttribute in methodDefinition.CustomAttributes )
			{
				if( attributeName == customAttribute.AttributeType.FullName )
				{
					ModuleDefinition = moduleDefinition;
					TypeDefinition = typeDefinition;
					MethodDefinition = methodDefinition;

					ILProcessor = MethodDefinition.Body.GetILProcessor();

					OnProcessMethod();

					methodDefinition.CustomAttributes.Remove( customAttribute );

					return true;
				}
			}

			return false;
		}

		protected virtual void OnProcessMethod()
		{
		}

		protected void CallMethod( Expression<System.Func<object>> expression )
		{
			CallMethod( StaticReflectionHelper.GetMethod( expression ) );
		}

		protected void CallMethod<T>( Expression<System.Func<T, object>> expression )
		{
			CallMethod( StaticReflectionHelper.GetMethod( expression ) );
		}

		protected void CallMethod( Expression<System.Action> expression )
		{
			CallMethod( StaticReflectionHelper.GetMethod( expression ) );
		}

		protected void CallMethod<T>( Expression<System.Action<T>> expression )
		{
			CallMethod( StaticReflectionHelper.GetMethod( expression ) );
		}

		protected VariableDefinition CreateLocalVar<T>()
		{
			return new VariableDefinition( GetTypeReference<T>() );
		}

		protected TypeReference GetTypeReference<T>()
		{
			return GetTypeReference( typeof( T ) );
		}

		protected TypeReference GetTypeReference( System.Type type )
		{
			string fullName = type.GetCecilFullname();

			TypeDefinition typeDefinition = ModuleDefinition.GetType( fullName );
			if( typeDefinition != null )
				return typeDefinition;

			TypeReference typeReference;
			if( !ModuleDefinition.TryGetTypeReference( fullName, out typeReference ) )
			{
				typeReference = ModuleDefinition.Import( type );
			}

			return typeReference;
		}

		protected MethodReference GetMethodReference( MethodInfo methodInfo )
		{
			TypeReference typeReference = GetTypeReference( methodInfo.DeclaringType );
			TypeDefinition typeDefinition = typeReference.Resolve();

			if( typeDefinition.Module == ModuleDefinition )
			{
				var methodDefinition = FindMethodDefinitionInType( typeDefinition, methodInfo );
				if( methodDefinition != null )
					return methodDefinition;
			}

			return ModuleDefinition.Import( methodInfo );
		}

		private static MethodDefinition FindMethodDefinitionInType( TypeDefinition typeDefinition, MethodInfo methodInfo )
		{
			foreach( var methodDefinition in typeDefinition.Methods )
			{
				if( methodDefinition.Name != methodInfo.Name )
					continue;
				if( methodDefinition.ReturnType.FullName != methodInfo.ReturnType.GetCecilFullname() )
					continue;

				var defParameters = methodDefinition.Parameters;
				var infParameters = methodInfo.GetParameters();

				if( defParameters.Count != infParameters.Length )
					continue;

				bool anyDifferent = false;
				for( int i = 0; i < infParameters.Length; i++ )
				{
					if( defParameters[i].ParameterType.FullName != infParameters[i].ParameterType.GetCecilFullname() )
					{
						anyDifferent = true;
						break;
					}
				}

				if( anyDifferent )
					continue;

				return methodDefinition;
			}

			return null;
		}

		private void CallMethod( MethodInfo methodInfo )
		{
			var methodReference = GetMethodReference( methodInfo );
			ILProcessor.Emit( OpCodes.Call, methodReference );
		}
	}
}
