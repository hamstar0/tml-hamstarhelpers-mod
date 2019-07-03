using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	/** <summary>Assorted static "helper" functions pertaining to mod list building.</summary> */
	public partial class ModListHelpers {
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

		public static IDictionary<Services.Tml.BuildPropertiesViewer, Mod> GetLoadedModsByBuildInfo() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.ModsByBuildProps != null ) {
				return mymod.ModListHelpers.ModsByBuildProps;
			}

			mymod.ModListHelpers.ModsByBuildProps = mymod.ModListHelpers.GetModsByBuildProps();
			return mymod.ModListHelpers.ModsByBuildProps;
		}

		////////////////

		public static IDictionary<string, Services.Tml.BuildPropertiesViewer> GetLoadedModNamesWithBuildProps() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.BuildPropsByModNames != null ) {
				return mymod.ModListHelpers.BuildPropsByModNames;
			}

			mymod.ModListHelpers.BuildPropsByModNames = mymod.ModListHelpers.GetBuildPropsByModName();
			return mymod.ModListHelpers.BuildPropsByModNames;
		}

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
