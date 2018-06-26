using HamstarHelpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.TmlHelpers {
	public partial class LoadHelpers {
		public static bool IsModLoaded() {
			var mymod = HamstarHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}


		public static bool IsWorldLoaded() {
			if( !LoadHelpers.IsModLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( !myworld.HasCorrectID ) { return false; }

			return true;
		}


		public static bool IsWorldBeingPlayed() {
			if( !LoadHelpers.IsWorldLoaded() ) { return false; }

			var mymod = HamstarHelpersMod.Instance;

			if( Main.netMode == 0 || Main.netMode == 1 ) {  // Client or single
				if( !mymod.LoadHelpers.IsClientPlaying ) {
					return false;
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
				return myplayer.Logic.IsSynced();
			} else {  // Server
				if( !mymod.LoadHelpers.HasServerBegunHavingPlayers ) {
					return false;
				}

				return true;
			}
		}


		public static bool IsWorldSafelyBeingPlayed() {
			return HamstarHelpersMod.Instance.LoadHelpers.StartupDelay >= ( 60 * 2 );
		}
	}
}
