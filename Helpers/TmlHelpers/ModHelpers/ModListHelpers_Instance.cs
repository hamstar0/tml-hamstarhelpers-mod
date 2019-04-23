using HamstarHelpers.Components.DataStructures;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public partial class ModListHelpers {
		private IDictionary<Services.Tml.BuildPropertiesEditor, Mod> ModsByBuildProps = new Dictionary<Services.Tml.BuildPropertiesEditor, Mod>();
		private IDictionary<string, ISet<Mod>> ModsByAuthor = new Dictionary<string, ISet<Mod>>();
		private IDictionary<string, Services.Tml.BuildPropertiesEditor> BuildPropsByModNames = new Dictionary<string, Services.Tml.BuildPropertiesEditor>();



		////////////////

		private IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetModsByBuildProps() {
			var mods = this.ModsByBuildProps;

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

		////

		private IDictionary<string, ISet<Mod>> GetModsByAuthor() {
			var mods = new Dictionary<string, ISet<Mod>>();

			foreach( var kv in ModListHelpers.GetLoadedModsByBuildInfo() ) {
				Services.Tml.BuildPropertiesEditor editor = kv.Key;
				Mod mod = kv.Value;

				foreach( string author in editor.Author.Split(',').Select(a=>a.Trim()) ) {
					mods.Append2D( author, mod );
				}
			}

			return mods;
		}

		private IDictionary<string, Services.Tml.BuildPropertiesEditor> GetBuildPropsByModName() {
			return ModListHelpers.GetLoadedModsByBuildInfo()
				.ToDictionary( kv=>kv.Value.Name, kv=>kv.Key );
		}
	}
}
