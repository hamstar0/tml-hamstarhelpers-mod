using System;
using System.Threading;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.IO;
using Terraria.Social;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Libraries.TModLoader {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tModLoader.
	/// </summary>
	public static partial class TmlLibraries {
		/// <summary>
		/// Path to config files.
		/// </summary>
		public static string ConfigRelativeFolder => "Mod Configs";


		////////////////

		private static object MyLock = new object();



		////////////////

		/// <summary>
		/// Exits the game to desktop.
		/// </summary>
		/// <param name="save">Saves settings or world state.</param>
		public static void ExitToDesktop( bool save = true ) {
			LogLibraries.Log( "Exiting to desktop " + ( save ? "with save..." : "..." ) );

			if( Main.netMode == NetmodeID.SinglePlayer ) {
				if( save ) { Main.SaveSettings(); }
				SocialAPI.Shutdown();
				Main.instance.Exit();
			} else {
				if( save ) { WorldFile.saveWorld(); }
				Netplay.disconnect = true;
				if( Main.netMode == NetmodeID.MultiplayerClient ) { SocialAPI.Shutdown(); }
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
					if( Main.netMode != NetmodeID.SinglePlayer ) {
						Netplay.disconnect = true;
						Main.netMode = NetmodeID.SinglePlayer;
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
	}
}
