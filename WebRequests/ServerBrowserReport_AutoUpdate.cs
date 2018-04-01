using HamstarHelpers.DebugHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Network;
using HamstarHelpers.Utilities.Timers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.WebRequests {
	partial class ServerBrowserReporter {
		public static bool CanPromptForBrowserAdd() {
			return HamstarHelpersMod.Instance.Config.IsServerPromptingForBrowser;
		}


		public static void EndPrompts() {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Config.IsServerPromptingForBrowser = false;
			mymod.JsonConfig.SaveFile();

			if( Main.netMode == 2 ) {
				PacketProtocol.QuickSendData<HHModSettingsProtocol>( -1, -1, false );
			}
		}



		////////////////

		private void InitializeAutoServerUpdates() {
			Action alert_privacy = delegate {
				string msg = "Hamstar's Helpers would like to list your servers in the Server Browser mod. Type '/hhprivateserver' in the chat or server console to cancel this. Otherwise, do nothing for 60 seconds.";

				Main.NewText( msg, Color.Yellow );
				Console.WriteLine( msg );
			};

			TmlLoadHelpers.AddWorldLoadPromise( delegate {
				if( Main.netMode == 1 ) {
					if( ServerBrowserReporter.CanPromptForBrowserAdd() ) {
						//	3 seconds
						Timers.SetTimer( "server_browser_intro", 60 * 3, delegate {
							alert_privacy();
							return false;
						} );
					}
				}

				if( ServerBrowserReporter.CanAddToBrowser() && ServerBrowserReporter.CanPromptForBrowserAdd() ) {
					// 1 minute
					Timers.SetTimer( "server_browser_report", 60 * 60, delegate {
						this.BeginAutoServerUpdates();
						return false;
					} );
				} else {
					this.BeginAutoServerUpdates();
				}
			} );
		}

		
		private void BeginAutoServerUpdates() {
			int seconds = Math.Max( 10, HamstarHelpersMod.Instance.Config.ServerBrowserAutoRefreshSeconds );

			if( ServerBrowserReporter.CanPromptForBrowserAdd() ) {
				ServerBrowserReporter.EndPrompts();
			}
			if( ServerBrowserReporter.CanAddToBrowser() ) {
				ServerBrowserReporter.AnnounceServer();
			}

			// 10 minutes by default between reports
			Timers.SetTimer( "server_browser_report", seconds * 60, delegate {
				if( ServerBrowserReporter.CanAddToBrowser() ) {
					ServerBrowserReporter.AnnounceServer();
				}
				return true;
			} );
		}


		////////////////

		internal void StopAutoServerUpdates() {
			Timers.UnsetTimer( "server_browser_report" );
			//this.IsSendingUpdates = false;
		}
	}
}
