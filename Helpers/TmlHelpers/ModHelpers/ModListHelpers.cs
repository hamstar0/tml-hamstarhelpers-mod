using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
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


		public static IDictionary<string, ISet<Mod>> GetLoadedModsByAuthor() {
			var mods = new Dictionary<string, ISet<Mod>>();

			foreach( var kv in ModListHelpers.GetLoadedModsByBuildInfo() ) {
				Services.Tml.BuildPropertiesEditor editor = kv.Key;
				Mod mod = kv.Value;

				mods.Append2D( editor.Author, mod );
			}

			return mods;
		}


		public static IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetLoadedModsByBuildInfo() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.ModListHelpers.ModsByBuildProps != null ) {
				return mymod.ModListHelpers.ModsByBuildProps;
			}

			return mymod.ModListHelpers.CacheModsByBuildProps();
		}
	}
}
