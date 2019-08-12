using HamstarHelpers.Classes.Errors;
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
		/// Gets a class's type by it's proper name from a given assembly.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="namespaceAndClassName"></param>
		/// <returns></returns>
		public static Type GetTypeFromAssembly( string assemblyName, string namespaceAndClassName ) {
			var rh = ModHelpersMod.Instance.ReflectionHelpers;
			string newAssemblyName = namespaceAndClassName + assemblyName.Substring( assemblyName.IndexOf( ',' ) );

			if( rh.TypeMap.ContainsKey( newAssemblyName ) ) {
				return rh.TypeMap[newAssemblyName];
			}

			rh.TypeMap[newAssemblyName] = Type.GetType( newAssemblyName );
			return rh.TypeMap[newAssemblyName];
		}


		////////////////

		/// <summary>
		/// Gets all types from a given assembly using a given class name.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		public static IList<Type> GetTypesFromAssembly( string assemblyName, string className ) {
			Assembly assemblies = ReflectionHelpers.GetAssembly( assemblyName );
			return ReflectionHelpers.GetTypesFromAssembly( assemblies, className );
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
				IList<Type> types = assembly.GetTypes();

				foreach( Type t in types ) {
					if( t.Name == className ) {
						classTypeList.Add( t );
					}
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return new List<Type>();
			}

			rh.AssClassTypeMap.Set2D( assembly.FullName, className, classTypeList );
			return classTypeList;
		}


		/// <summary>
		/// Gets all types from a given assembly using a given class name.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="namespacedType"></param>
		/// <returns></returns>
		public static Type GetTypeFromAssembly( Assembly assembly, string namespacedType ) {
			var rh = ModHelpersMod.Instance.ReflectionHelpers;
			IList<Type> classTypeList;

			if( rh.AssClassTypeMap.TryGetValue2D( assembly.FullName, namespacedType, out classTypeList ) ) {
				return classTypeList.Count == 1 ? classTypeList[0] : null;
			} else {
				classTypeList = new List<Type>();
			}

			try {
				classTypeList = new List<Type> { assembly.GetType( namespacedType ) };
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return null;
			}

			rh.AssClassTypeMap.Set2D( assembly.FullName, namespacedType, classTypeList );
			return classTypeList.Count == 1 ? classTypeList[0] : null;
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
	}
}
