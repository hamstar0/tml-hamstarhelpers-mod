using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;


namespace HamstarHelpers.Helpers.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tModLoader.
	/// </summary>
	public static partial class TmlHelpers {
		/// <summary>
		/// Exits the game to desktop.
		/// </summary>
		/// <param name="save">Saves settings or world state.</param>
		public static void ExitToDesktop( bool save = true ) {
			LogHelpers.Log( "Exiting to desktop " + ( save ? "with save..." : "..." ) );

			if( Main.netMode == 0 ) {
				if( save ) { Main.SaveSettings(); }
				SocialAPI.Shutdown();
				Main.instance.Exit();
			} else {
				if( save ) { WorldFile.saveWorld(); }
				Netplay.disconnect = true;
				if( Main.netMode == 1 ) { SocialAPI.Shutdown(); }
				Environment.Exit( 0 );
			}
		}

		/// <summary>
		/// Exits to the main menu.
		/// </summary>
		/// <param name="save">Saves settings or world state.</param>
		public static void ExitToMenu( bool save = true ) {
			IngameOptions.Close();
			Main.menuMode = 10;

			if( save ) {
				WorldGen.SaveAndQuit( (Action)null );
			} else {
				ThreadPool.QueueUserWorkItem( new WaitCallback( delegate ( object state ) {
					Main.invasionProgress = 0;
					Main.invasionProgressDisplayLeft = 0;
					Main.invasionProgressAlpha = 0f;

					Main.StopTrackedSounds();
					CaptureInterface.ResetFocus();
					Main.ActivePlayerFileData.StopPlayTimer();

					Main.gameMenu = true;
					if( Main.netMode != 0 ) {
						Netplay.disconnect = true;
						Main.netMode = 0;
					}

					Main.fastForwardTime = false;
					Main.UpdateSundial();

					Main.menuMode = 0;
				} ), (Action)null );
			}
		}


		/*public static string[] AssertCallParams( object[] args, Type[] types, bool[] nullables = null ) {
			if( args.Length != types.Length ) {
				return new string[] { "Mismatched input argument quantity." };
			}

			var errors = new List<string>();

			for( int i = 0; i < types.Length; i++ ) {
				if( args[i] == null ) {
					if( !types[i].IsClass || nullables == null || !nullables[i] ) {
						errors.Add( "Invalid paramater #" + i + ": Expected " + types[i].Name + ", found null" );
					}
				} else if( args[i].GetType() != types[i] ) {
					errors.Add( "Invalid parameter #" + i + ": Expected " + types[i].Name + ", found " + args[i].GetType() );
				}
			}

			return errors.ToArray();
		}*/


		////////////////

		private static void ForceSetupPlayer( Player player ) {
			ModPlayer[] modPlayers;

			if( !ReflectionHelpers.Get( player, "modPlayers", out modPlayers ) || modPlayers.Length == 0 ) {
				MethodInfo setupPlayerMethod = typeof( PlayerHooks ).GetMethod( "SetupPlayer", DotNET.Reflection.ReflectionHelpers.MostAccess );
				if( setupPlayerMethod == null ) {
					throw new HamstarException( "Could not run SetupPlayer for " + ( player?.name ?? "null player" ) );
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
	}
}
