using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
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
				if( kv.Key == mymod.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				modSet.Add( kv.Value.Name );
			}

			// Add remaining mods
			foreach( var mod in ModLoader.LoadedMods ) {
				if( modSet.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}

			return mods;
		}


		////////////////

		public static IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetLoadedModsByBuildInfo() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.ModsByBuildProps != null ) {
				return mymod.ModListHelpers.ModsByBuildProps;
			}

			mymod.ModListHelpers.ModsByBuildProps = mymod.ModListHelpers.GetModsByBuildProps();
			return mymod.ModListHelpers.ModsByBuildProps;
		}

		////////////////

		public static IDictionary<string, Services.Tml.BuildPropertiesEditor> GetLoadedModNamesWithBuildProps() {
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
