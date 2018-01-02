using System;
using System.Collections.Generic;
using Terraria;
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

		public static void ExitToDesktop() {
			Main.SaveSettings();
			SocialAPI.Shutdown();
			Main.instance.Exit();
		}

		public static void ExitToMenu() {
			//if( Main.netMode != 1 ) { WorldFile.saveWorld(); }

			IngameOptions.Close();
			Main.menuMode = 10;
			WorldGen.SaveAndQuit( (Action)null );
		}
	}
}
