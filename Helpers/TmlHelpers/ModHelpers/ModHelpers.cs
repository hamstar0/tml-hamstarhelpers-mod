using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers.ModHelpers {
	public static class ModHelpers {
		public static IEnumerable<Mod> GetAllMods() {
			var self = HamstarHelpersMod.Instance.ModMetaDataManager;
			var mods = new LinkedList<Mod>();
			var mod_set = new HashSet<string>();

			mods.AddLast( HamstarHelpersMod.Instance );
			mod_set.Add( HamstarHelpersMod.Instance.Name );

			foreach( var kv in self.ConfigMods ) {
				if( kv.Key == HamstarHelpersMod.Instance.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				mod_set.Add( kv.Value.Name );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mod_set.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}
			return mods;
		}
	}
}
