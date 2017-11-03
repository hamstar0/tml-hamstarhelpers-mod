using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	public static class TmlHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();


		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( TmlHelpers.ModIds.ContainsKey(mod) ) { return TmlHelpers.ModIds[mod]; }
			TmlHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return TmlHelpers.ModIds[mod];
		}
	}
}
