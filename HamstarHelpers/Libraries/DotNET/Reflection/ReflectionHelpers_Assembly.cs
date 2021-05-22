using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using System;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Libraries.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionLibraries {
		/// <summary>
		/// Returns the "main" assembly of tModLoader/Terraria.
		/// </summary>
		/// <returns></returns>
		public static Assembly GetMainAssembly() {
			var rh = ModHelpersMod.Instance.ReflectionHelpers;

			if( rh.AssMap.ContainsKey( "___" ) ) {
				return rh.AssMap["___"];
			}

			rh.AssMap[ "___" ] = Assembly.GetAssembly( typeof(ModLoader) );
			return rh.AssMap["___"];
		}


		/// <summary>
		/// Gets an assembly by the given name. Caches result for quicker future retrievals.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public static Assembly GetAssembly( string assemblyName ) {
			var rh = ModHelpersMod.Instance.ReflectionHelpers;

			if( rh.AssMap.ContainsKey(assemblyName) ) {
				return rh.AssMap[assemblyName];
			}

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach( Assembly ass in assemblies ) {
				if( ass.GetName().Name != assemblyName ) { continue; }

				rh.AssMap[assemblyName] = ass;
				return ass;
			}

			return null;
		}
	}
}
