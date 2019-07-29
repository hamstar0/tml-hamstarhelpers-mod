using HamstarHelpers.Services.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;


namespace HamstarHelpers.Helpers.TModLoader.Mods {
	/// @private
	public partial class ModListHelpers {
		private IDictionary<Services.TML.BuildPropertiesViewer, Mod> ModsByBuildProps = new Dictionary<Services.TML.BuildPropertiesViewer, Mod>();
		private IDictionary<string, ISet<Mod>> ModsByAuthor = new Dictionary<string, ISet<Mod>>();
		private IDictionary<string, Services.TML.BuildPropertiesViewer> BuildPropsByModNames = new Dictionary<string, Services.TML.BuildPropertiesViewer>();



		////////////////

		private IDictionary<Services.TML.BuildPropertiesViewer, Mod> GetModsByBuildProps() {
			var mods = this.ModsByBuildProps;

			foreach( Mod mod in ModLoader.Mods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				
				TmodFile tmod;
				if( !ReflectionHelpers.Get( mod, "modFile", out tmod ) || tmod == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}

				var editor = Services.TML.BuildPropertiesViewer.GetBuildPropertiesForModFile( tmod );

				mods[editor] = mod;
			}

			return mods;
		}

		////

		private IDictionary<string, ISet<Mod>> GetModsByAuthor() {
			var mods = new Dictionary<string, ISet<Mod>>();

			foreach( var kv in ModListHelpers.GetLoadedModsAndBuildInfo() ) {
				Services.TML.BuildPropertiesViewer editor = kv.Key;
				Mod mod = kv.Value;

				foreach( string author in editor.Author.Split(',').Select(a=>a.Trim()) ) {
					mods.Append2D( author, mod );
				}
			}

			return mods;
		}

		private IDictionary<string, Services.TML.BuildPropertiesViewer> GetBuildPropsByModName() {
			return ModListHelpers.GetLoadedModsAndBuildInfo()
				.ToDictionary( kv=>kv.Value.Name, kv=>kv.Key );
		}
	}
}
