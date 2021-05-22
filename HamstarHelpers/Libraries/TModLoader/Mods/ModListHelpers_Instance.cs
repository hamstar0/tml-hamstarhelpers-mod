using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Libraries.DotNET.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;


namespace HamstarHelpers.Libraries.TModLoader.Mods {
	/// @private
	public partial class ModListLibraries {
		private IDictionary<Services.TML.BuildPropertiesViewer, Mod> ModsByBuildProps = new Dictionary<Services.TML.BuildPropertiesViewer, Mod>();
		private IDictionary<string, ISet<Mod>> ModsByAuthor = new Dictionary<string, ISet<Mod>>();
		private IDictionary<string, Services.TML.BuildPropertiesViewer> BuildPropsByModNames = new Dictionary<string, Services.TML.BuildPropertiesViewer>();



		////////////////

		private IDictionary<Services.TML.BuildPropertiesViewer, Mod> GetModsByBuildProps() {
			var mods = this.ModsByBuildProps;

			foreach( Mod mod in ModLoader.Mods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				
				TmodFile tmod;
				if( !ReflectionLibraries.Get( mod, "modFile", out tmod ) || tmod == null ) {
					LogLibraries.Warn( "Mod " + mod.DisplayName + " has no file data." );
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

			foreach( var kv in ModListLibraries.GetLoadedModsAndBuildInfo() ) {
				Services.TML.BuildPropertiesViewer editor = kv.Key;
				Mod mod = kv.Value;

				foreach( string author in editor.Author.Split(',').Select(a=>a.Trim()) ) {
					mods.Append2D( author, mod );
				}
			}

			return mods;
		}

		private IDictionary<string, Services.TML.BuildPropertiesViewer> GetBuildPropsByModName() {
			return ModListLibraries.GetLoadedModsAndBuildInfo()
				.ToDictionary( kv=>kv.Value.Name, kv=>kv.Key );
		}
	}
}
