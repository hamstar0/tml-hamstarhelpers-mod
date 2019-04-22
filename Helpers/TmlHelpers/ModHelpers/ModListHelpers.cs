using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public static class ModListHelpers {
		public static IEnumerable<Mod> GetAllLoadedModsPreferredOrder() {
			var mymod = ModHelpersMod.Instance;
			var self = mymod.ModMetaDataMngr;
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

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				if( mod.File == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}
				var editor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );

				mods.Append2D( editor.Author, mod );
			}

			return mods;
		}


		public static IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetLoadedModsByBuildInfo() {
			var mods = new Dictionary<Services.Tml.BuildPropertiesEditor, Mod>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				if( mod.File == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}
				var editor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );

				mods[editor] = mod;
			}

			return mods;
		}
	}
}
