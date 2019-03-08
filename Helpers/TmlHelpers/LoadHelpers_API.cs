using HamstarHelpers.Helpers.DebugHelpers;
using System;
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
			if( !myworld.HasObsoleteId ) { return false; }

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
			var mymod = ModHelpersMod.Instance;
			bool notSafelyPlayed = mymod.LoadHelpers.StartupDelay >= ( 60 * 2 );

			if( mymod.Config.DebugModeHelpersInfo && !notSafelyPlayed ) {
				if( Main.netMode != 2 && !Main.dedServ ) {
					var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();
					LogHelpers.WarnOnce( "StartupDelay: "+mymod.LoadHelpers.StartupDelay+" ("+(60 * 2)+"?)"
						+ ", IsClientPlaying_Hackish: "+mymod.LoadHelpers.IsClientPlaying_Hackish+" (true?)"
						+ ", IsSynced: "+myplayer.Logic.IsSynced+" (true?)" );
				} else {
					var myworld = mymod.GetModWorld<ModHelpersWorld>();
					LogHelpers.WarnOnce( "StartupDelay: "+mymod.LoadHelpers.StartupDelay
						+ ", IsModLoaded(): "+LoadHelpers.IsModLoaded()+" (true?)"
						+ ", HasObsoleteId: "+myworld.HasObsoleteId+" (false?)"
						+ ", HasServerBegunHavingPlayers_Hackish: " + mymod.LoadHelpers.HasServerBegunHavingPlayers_Hackish+" (true?)"
						+ ", HasSetupContent: "+mymod.HasSetupContent+" (true?)"
						+ ", HasAddedRecipeGroups: "+mymod.HasAddedRecipeGroups+" (true?)"
						+ ", HasAddedRecipes: "+mymod.HasAddedRecipes+" (true?)" );
				}
			}
			return notSafelyPlayed;
		}


		[Obsolete( "`PlayerIdentityHelpers.GetProperUniqueId(player) != null` might work better", true )]
		public static bool IsPlayerLoaded( Player player ) {
			var mymod = ModHelpersMod.Instance;
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			return myplayer.Logic.HasLoadedOldUID;
		}
	}
}
