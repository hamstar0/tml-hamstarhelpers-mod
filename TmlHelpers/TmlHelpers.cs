using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;

namespace HamstarHelpers.TmlHelpers {
	public static class TmlHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();


		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( TmlHelpers.ModIds.ContainsKey(mod) ) { return TmlHelpers.ModIds[mod]; }
			TmlHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return TmlHelpers.ModIds[mod];
		}


		////////////////

		public static void Exit( bool save=true, bool to_main_menu=true ) {
			if( save ) { WorldFile.saveWorld(); }
			Netplay.disconnect = true;
			if( to_main_menu ) {
				SocialAPI.Shutdown();
			} else {
				Main.instance.Exit();
			}
		}
	}
}
