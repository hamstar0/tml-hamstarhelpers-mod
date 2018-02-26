using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public class TmlWorldHelpers {
		public static bool IsWorldLoaded() {
			if( !TmlLoadHelpers.IsLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !myworld.HasCorrectID ) { return false; }

			return true;
		}


		public static bool IsGameLoaded() {
//var mymod = HamstarHelpersMod.Instance;
//var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
//var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
//LogHelpers.Log( "HasSetupContent: "+ mymod.HasSetupContent + ", HasCorrectID: "+ modworld.HasCorrectID+ ", who: "+myplayer.player.whoAmI+", HasSyncedModSettings: "+ myplayer.Logic.HasSyncedModSettings + ", HasSyncedModData: " + myplayer.Logic.HasSyncedModData );
			if( !TmlWorldHelpers.IsWorldLoaded() ) { return false; }

			if( Main.netMode == 0 || Main.netMode == 1 ) {
				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.IsSynced();
			}

			return true;
		}
	}
}
