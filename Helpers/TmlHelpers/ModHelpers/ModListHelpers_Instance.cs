using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public partial class ModListHelpers {
		private IDictionary<Services.Tml.BuildPropertiesEditor, Mod> ModsByBuildProps = new Dictionary<Services.Tml.BuildPropertiesEditor, Mod>();



		////////////////

		private IDictionary<Services.Tml.BuildPropertiesEditor, Mod> CacheModsByBuildProps() {
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
	}
}
