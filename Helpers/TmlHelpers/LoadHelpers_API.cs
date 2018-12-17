using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public partial class LoadHelpers {
		public static bool IsModLoaded() {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}


		public static bool IsWorldLoaded() {
			if( !LoadHelpers.IsModLoaded() ) { return false; }

			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();
			if( !myworld.HasObsoletedID ) { return false; }

			return true;
		}


		public static bool IsWorldBeingPlayed() {
			var mymod = ModHelpersMod.Instance;

			if( Main.netMode != 2 && !Main.dedServ ) {
				if( !mymod.LoadHelpers.IsClientPlaying_Hackish ) {
					return false;
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();
				return myplayer.Logic.IsSynced;
			} else {
				if( !LoadHelpers.IsWorldLoaded() ) {
					return false;
				}
				if( !mymod.LoadHelpers.HasServerBegunHavingPlayers_Hackish ) {
					return false;
				}

				return true;
			}
		}


		public static bool IsWorldSafelyBeingPlayed() {
			return ModHelpersMod.Instance.LoadHelpers.StartupDelay >= ( 60 * 2 );
		}


		public static bool IsPlayerLoaded( Player player ) {
			var mymod = ModHelpersMod.Instance;
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			return myplayer.Logic.HasLoadedUID;
		}
	}
}
