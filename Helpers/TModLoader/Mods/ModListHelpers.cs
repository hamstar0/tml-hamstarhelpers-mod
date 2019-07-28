using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.TML;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	/// <summary>
	/// Assorted static "helper" functions pertaining to mod list building.
	/// </summary>
	public partial class ModListHelpers {
		/// <summary>
		/// Gets the "preferred" order of all loaded mods for listing use (subjective; not relevant to internal load order).
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Mod> GetAllLoadedModsPreferredOrder() {
			var mymod = ModHelpersMod.Instance;
			var self = mymod.ModFeaturesHelpers;
			var mods = new LinkedList<Mod>();
			var modSet = new HashSet<string>();

			// Set Mod Helpers to front of list
			mods.AddLast( mymod );
			modSet.Add( mymod.Name );

			// Order mods with configs first
			foreach( var kv in self.ConfigMods ) {
				if( kv.Key == mymod.Name ) { continue; }
				TmodFile modFile;
				if( !ReflectionHelpers.Get( kv.Value, "File", out modFile) || modFile == null ) {
					LogHelpers.Warn( "Could not get mod file from mod "+kv.Key );
					continue;
				}

				mods.AddLast( kv.Value );
				modSet.Add( kv.Value.Name );
			}

			// Add remaining mods
			foreach( var mod in ModLoader.Mods ) {
				if( modSet.Contains( mod.Name ) ) { continue; }
				TmodFile modFile;
				if( !ReflectionHelpers.Get(mod, "File", out modFile) || modFile == null ) {
					LogHelpers.Warn( "Could not get mod file from mod " + mod.Name );
					continue;
				}

				mods.AddLast( mod );
			}

			return mods;
		}


		////////////////

		/// <summary>
		/// Gets a map of loaded mods with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<BuildPropertiesViewer, Mod> GetLoadedModsAndBuildInfo() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.ModsByBuildProps != null ) {
				return mymod.ModListHelpers.ModsByBuildProps;
			}

			mymod.ModListHelpers.ModsByBuildProps = mymod.ModListHelpers.GetModsByBuildProps();
			return mymod.ModListHelpers.ModsByBuildProps;
		}

		////////////////

		/// <summary>
		/// Gets a map of all loaded mods by name with their build information.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, BuildPropertiesViewer> GetLoadedModNamesWithBuildProps() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.BuildPropsByModNames != null ) {
				return mymod.ModListHelpers.BuildPropsByModNames;
			}

			mymod.ModListHelpers.BuildPropsByModNames = mymod.ModListHelpers.GetBuildPropsByModName();
			return mymod.ModListHelpers.BuildPropsByModNames;
		}

		/// <summary>
		/// Gets a map of loaded mods with their authors.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<string, ISet<Mod>> GetLoadedModsByAuthor() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.ModsByBuildProps != null ) {
				return mymod.ModListHelpers.ModsByAuthor;
			}

			mymod.ModListHelpers.ModsByAuthor = mymod.ModListHelpers.GetModsByAuthor();
			return mymod.ModListHelpers.ModsByAuthor;
		}
	}
}
