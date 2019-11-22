using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the state of the game.
	/// </summary>
	public partial class LoadHelpers {
		/// <summary>
		/// Indicates if mods Mod Helpers is fully loaded (recipes, content, etc.).
		/// </summary>
		/// <returns></returns>
		public static bool IsModLoaded() {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.HasSetupContent ) { return false; }
			if( !mymod.HasAddedRecipeGroups ) { return false; }
			if( !mymod.HasAddedRecipes ) { return false; }

			return true;
		}

		
		/// <summary>
		/// Indicates if the player is playing a game.
		/// </summary>
		/// <returns></returns>
		public static bool IsCurrentPlayerInGame() {
			bool isTimerActive;
			ReflectionHelpers.Get( Main.ActivePlayerFileData, "_isTimerActive", out isTimerActive );

			return !Main.gameMenu && isTimerActive;
		}


		/// <summary>
		/// Indicates if the current world has finished loading, and is ready for play.
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldLoaded() {
			if( !LoadHelpers.IsModLoaded() ) { return false; }

			var myworld = ModContent.GetInstance<ModHelpersWorld>();
			if( !myworld.HasObsoleteId ) { return false; }

			return true;
		}


		/// <summary>
		/// Indicates if a given world is being played at present (at least 1 active player).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldBeingPlayed() {
			var mymod = ModHelpersMod.Instance;

			if( Main.netMode != 2 && !Main.dedServ ) {
				if( !mymod.LoadHelpers.IsLocalPlayerInGame_Hackish ) {
					return false;
				}

				var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer(
					Main.LocalPlayer,
					ModHelpersMod.Instance,
					"ModHelpersPlayer"
				);
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


		/// <summary>
		/// Indicates if a given world is being played (at least 1 active player), and that player has finished all of their
		/// own in-game "loading" stuff (attempts to account for any Terraria/mod hidden loading behaviors).
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldSafelyBeingPlayed() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.LoadHelpers == null ) {
				return false;
			}

			bool notSafelyPlayed = mymod.LoadHelpers.WorldStartupDelay >= ( 60 * 2 );

			if( ModHelpersConfig.Instance.DebugModeHelpersInfo && !notSafelyPlayed ) {
				if( Main.netMode != 2 && !Main.dedServ ) {
					var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "ModHelpersPlayer" );

					LogHelpers.LogOnce( DebugHelpers.GetCurrentContext( 2 ) + " - IsWorldSafelyBeingPlayed - "
						+ "StartupDelay: "+!(mymod.LoadHelpers.WorldStartupDelay < (60 * 2))
						+ ", IsLocalPlayerInGame_Hackish: " + mymod.LoadHelpers.IsLocalPlayerInGame_Hackish+" (true?)"
						+ ", IsSynced: "+(myplayer?.Logic.IsSynced.ToString() ?? "null")+" (true?) = "
						+ " HasSyncedWorldData: "+(myplayer?.Logic.HasSyncedWorldData.ToString() ?? "null")+" &&"
						+ " HasLoadedOldUID: " + (myplayer?.Logic.HasLoadedOldUID.ToString() ?? "null")
					);
				} else {
					var myworld = ModContent.GetInstance<ModHelpersWorld>();
					LogHelpers.LogOnce( DebugHelpers.GetCurrentContext( 2 ) + " - IsWorldSafelyBeingPlayed - "
						+ "StartupDelay: "+!(mymod.LoadHelpers.WorldStartupDelay < (60 * 2))
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
	}
}
