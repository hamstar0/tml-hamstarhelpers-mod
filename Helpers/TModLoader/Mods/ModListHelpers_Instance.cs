using HamstarHelpers.Components.Errors;
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
		private IDictionary<Services.Tml.BuildPropertiesViewer, Mod> ModsByBuildProps = new Dictionary<Services.Tml.BuildPropertiesViewer, Mod>();
		private IDictionary<string, ISet<Mod>> ModsByAuthor = new Dictionary<string, ISet<Mod>>();
		private IDictionary<string, Services.Tml.BuildPropertiesViewer> BuildPropsByModNames = new Dictionary<string, Services.Tml.BuildPropertiesViewer>();



		////////////////

		private IDictionary<Services.Tml.BuildPropertiesViewer, Mod> GetModsByBuildProps() {
			var mods = this.ModsByBuildProps;

			foreach( Mod mod in ModLoader.Mods ) {
				if( mod.Name == "tModLoader" ) { continue; }
				
				TmodFile tmod;
				if( !ReflectionHelpers.Get( mod, "modFile", out tmod ) || tmod == null ) {
					LogHelpers.Warn( "Mod " + mod.DisplayName + " has no file data." );
					continue;
				}

				var editor = Services.Tml.BuildPropertiesViewer.GetBuildPropertiesForModFile( tmod );

				mods[editor] = mod;
			}

			return mods;
		}

		////

		private IDictionary<string, ISet<Mod>> GetModsByAuthor() {
			var mods = new Dictionary<string, ISet<Mod>>();

			foreach( var kv in ModListHelpers.GetLoadedModsByBuildInfo() ) {
				Services.Tml.BuildPropertiesViewer editor = kv.Key;
				Mod mod = kv.Value;

				foreach( string author in editor.Author.Split(',').Select(a=>a.Trim()) ) {
					mods.Append2D( author, mod );
				}
			}

			return mods;
		}

		private IDictionary<string, Services.Tml.BuildPropertiesViewer> GetBuildPropsByModName() {
			return ModListHelpers.GetLoadedModsByBuildInfo()
				.ToDictionary( kv=>kv.Value.Name, kv=>kv.Key );
		}
	}
}
