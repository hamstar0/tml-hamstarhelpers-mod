using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.Graphics.Capture;
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

		public static void ExitToDesktop( bool save=true ) {
			if( save ) {
				Main.SaveSettings();
			}

			SocialAPI.Shutdown();

			Environment.Exit( 0 );

			//if( Main.netMode != 0 ) {
			//	Netplay.disconnect = true;
			//}
			//Main.instance.Exit();
		}

		public static void ExitToMenu( bool save=true ) {
			IngameOptions.Close();
			Main.menuMode = 10;
			if( save ) {
				WorldGen.SaveAndQuit( (Action)null );
			} else {
				ThreadPool.QueueUserWorkItem( new WaitCallback( delegate ( object state ) {
					Main.invasionProgress = 0;
					Main.invasionProgressDisplayLeft = 0;
					Main.invasionProgressAlpha = 0f;
					Main.gameMenu = true;
					Main.StopTrackedSounds();
					CaptureInterface.ResetFocus();
					Main.ActivePlayerFileData.StopPlayTimer();
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
	}
}
