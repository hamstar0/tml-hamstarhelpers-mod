using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tModLoader.
	/// </summary>
	public static partial class TmlHelpers {
		private static UnifiedRandom BackupRand = null;



		////////////////

		private static void ForceSetupPlayer( Player player ) {
			ModPlayer[] modPlayers;

			if( !ReflectionHelpers.Get( player, "modPlayers", out modPlayers ) || modPlayers.Length == 0 ) {
				MethodInfo setupPlayerMethod = typeof( PlayerHooks ).GetMethod( "SetupPlayer", DotNET.Reflection.ReflectionHelpers.MostAccess );
				if( setupPlayerMethod == null ) {
					throw new ModHelpersException( "Could not run SetupPlayer for " + ( player?.name ?? "null player" ) );
				}

				setupPlayerMethod.Invoke( null, new object[] { player } );
			}
		}

		////

		/// <summary>
		/// Provides an alternative to `Player.GetModPlayer(...)` to ensure the given player is properly loaded. Addresses some
		/// confusing types of errors.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="mod"></param>
		/// <param name="modPlayerName"></param>
		/// <returns></returns>
		public static ModPlayer SafelyGetModPlayer( Player player, Mod mod, string modPlayerName ) {    // Solely for Main.LocalPlayer?
			TmlHelpers.ForceSetupPlayer( player );
			return player.GetModPlayer( mod, modPlayerName );
		}

		/// <summary>
		/// Provides an alternative to `Player.GetModPlayer(...)` to ensure the given player is properly loaded. Addresses some
		/// confusing types of errors.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="player"></param>
		/// <returns></returns>
		public static T SafelyGetModPlayer<T>( Player player ) where T : ModPlayer {
			TmlHelpers.ForceSetupPlayer( player );
			return player.GetModPlayer<T>();
		}


		////////////////

		/// <summary>
		/// Ensures a valid randomizer. Typically returns `Main.rand`.
		/// </summary>
		/// <returns></returns>
		public static UnifiedRandom SafelyGetRand() {
			if( Main.rand != null ) {
				return Main.rand;
			}
			if( TmlHelpers.BackupRand == null ) {
				TmlHelpers.BackupRand = new UnifiedRandom( (int)DateTime.UtcNow.ToFileTime() );
			}
			return TmlHelpers.BackupRand;
		}
	}
}
