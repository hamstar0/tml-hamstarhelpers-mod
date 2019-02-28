using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false, Inherited = true )]
	public class NullableAttribute : Attribute { }





	public partial class ReflectionHelpers {
		public readonly static BindingFlags MostAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;



		////////////////

		public static IList<Type> GetTypesFromAssembly( string assemblyName, string typeName ) {
			var typeList = new List<Type>();

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Func<Assembly, IEnumerable<Type>> selectMany = delegate ( Assembly a ) {
					try {
						return a.GetTypes();
					} catch {
						return new List<Type>();
					}
				};

				foreach( var ass in assemblies ) {
					if( ass.GetName().Name != assemblyName ) { continue; }

					foreach( Type t in selectMany( ass ) ) {
						if( t.Name == typeName ) {
							typeList.Add( t );
						}
					}
					break;
				}

			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}

			return typeList;
		}


		public static IEnumerable<Type> GetAllAvailableSubTypesFromMods( Type parentType ) {
			IEnumerable<Assembly> asses = ModLoader.LoadedMods.Select( mod => mod.GetType().Assembly );
			return ReflectionHelpers.GetAllAvailableSubTypesFromAssemblies( asses, parentType );
		}

		public static IEnumerable<Type> GetAllAvailableSubTypesFromAssemblies( IEnumerable<Assembly> asses, Type parentType ) {
			var subclasses = new List<Type>();

			foreach( Assembly ass in asses ) {
				Type[] myTypes = null;
				try {
					myTypes = ass.GetTypes();
				} catch {
					continue;
				}

				foreach( var myType in myTypes ) {
					if( myType == null || !myType.IsSubclassOf( parentType ) || myType.IsAbstract ) { continue; }
					subclasses.Add( myType );
				}
			}

			return subclasses;
		}

		
		////////////////

		public static Type GetClassFromAssembly( string assemblyName, string namespaceAndClassName ) {
			string newAssemblyName = namespaceAndClassName + assemblyName.Substring( assemblyName.IndexOf( ',' ) );
			return Type.GetType( newAssemblyName );
		}



		////////////////

		



		////////////////

		internal ReflectionHelpers() { }
	}
}
