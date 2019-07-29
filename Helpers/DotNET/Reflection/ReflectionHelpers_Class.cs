using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Services.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers {
		/// <summary>
		/// Gets all types from a given assembly using a given class name.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static IList<Type> GetTypesFromAssembly( string assemblyName, string className ) {
			IList<Type> classTypeList = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach( var ass in assemblies ) {
				if( ass.GetName().Name != assemblyName ) { continue; }
				classTypeList = ReflectionHelpers.GetTypesFromAssembly( ass, className );
				break;
			}

			return classTypeList;
		}

		/// <summary>
		/// Gets all types from a given assembly using a given class name.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static IList<Type> GetTypesFromAssembly( Assembly assembly, string className ) {
			var rh = ModHelpersMod.Instance.ReflectionHelpers;
			IList<Type> classTypeList;

			if( rh.AssClassTypeMap.TryGetValue2D( assembly.FullName, className, out classTypeList) ) {
				return classTypeList;
			} else {
				classTypeList = new List<Type>();
			}

			try {
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Func<Assembly, IEnumerable<Type>> selectMany = delegate ( Assembly a ) {
					try {
						return a.GetTypes();
					} catch {
						return new List<Type>();
					}
				};

				foreach( Type t in selectMany( assembly ) ) {
					if( t.Name == className ) {
						classTypeList.Add( t );
					}
				}

			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
			}

			rh.AssClassTypeMap.Set2D( assembly.FullName, className, classTypeList );
			return classTypeList;
		}


		/// <summary>
		/// Gets all available sub-types (immedaite sub-classes) of a given parent type from each loaded mod.
		/// </summary>
		/// <param name="parentType"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetAllAvailableSubTypesFromMods( Type parentType ) {
			IEnumerable<Assembly> asses = ModLoader.Mods.SafeSelect( mod => mod.GetType().Assembly );
			return ReflectionHelpers.GetAllAvailableSubTypesFromAssemblies( asses, parentType );
		}

		/// <summary>
		/// Gets all available sub-types (immedaite sub-classes) of a given parent type from each assembly.
		/// </summary>
		/// <param name="asses"></param>
		/// <param name="parentType"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Gets a class's type by it's proper name from a given assembly.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="namespaceAndClassName"></param>
		/// <returns></returns>
		public static Type GetClassFromAssembly( string assemblyName, string namespaceAndClassName ) {
			string newAssemblyName = namespaceAndClassName + assemblyName.Substring( assemblyName.IndexOf( ',' ) );
			return Type.GetType( newAssemblyName );
		}
	}
}
