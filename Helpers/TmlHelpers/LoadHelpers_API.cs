using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Helpers.TmlHelpers {
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
			if( !myworld.HasObsoletedID ) { return false; }

			return true;
		}


		public static bool IsWorldBeingPlayed() {
			var mymod = HamstarHelpersMod.Instance;

			if( Main.netMode != 2 && !Main.dedServ ) {
				if( !mymod.LoadHelpers.IsClientPlaying_Hackish ) {
					return false;
				}

				var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();
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
			return HamstarHelpersMod.Instance.LoadHelpers.StartupDelay >= ( 60 * 2 );
		}
	}
}
