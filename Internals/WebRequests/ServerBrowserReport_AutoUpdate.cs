using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Internals.WebRequests {
	partial class ServerBrowserReporter {
		public static bool CanPromptForBrowserAdd() {
			return ModHelpersMod.Instance.Config.IsServerPromptingUsersBeforeListingOnBrowser
				&& !ModHelpersMod.Instance.Config.IsServerHiddenFromBrowser;
		}


		public static void EndPrompts() {
			var mymod = ModHelpersMod.Instance;

			mymod.Config.IsServerPromptingUsersBeforeListingOnBrowser = false;
			mymod.ConfigJson.SaveFile();

			if( Main.netMode == 2 ) {
				PacketProtocol.QuickSendToClient<ModSettingsProtocol>( -1, -1 );
			}
		}



		////////////////
		
		private void InitializeLoopingServerAnnounce() {
			if( Main.netMode == 0 ) { return; }
				
			Action alert_privacy = delegate {
				string msg = "Mod Helpers would like to list your servers in the Server Browser mod. Type '/mh-private-server' in the chat or server console to cancel this. Otherwise, do nothing for 60 seconds.";

				Main.NewText( msg, Color.Yellow );
				Console.WriteLine( msg );
			};

			if( Main.netMode == 1 ) {
				if( ServerBrowserReporter.CanPromptForBrowserAdd() ) {
					//	3 seconds
					Timers.SetTimer( "ModHelpersServerBrowserIntro", 60 * 3, delegate {
						alert_privacy();
						return false;
					} );
				}
			}

			if( ServerBrowserReporter.CanAnnounceServer() ) {
				if( ServerBrowserReporter.CanPromptForBrowserAdd() ) {
					Timers.SetTimer( "ModHelpersServerBrowserReport", 60 * 60, delegate {   // 1 minute, no repeat
						if( ServerBrowserReporter.CanPromptForBrowserAdd() ) {
							ServerBrowserReporter.EndPrompts();
						}

						try {
							this.BeginLoopingServerAnnounce();
						} catch { }

						return false;
					} );
				} else {
					this.BeginLoopingServerAnnounce();
				}
			}
		}

		
		private void BeginLoopingServerAnnounce() {
			if( Main.netMode != 2 ) { return; }

			// First time no timer
			if( ServerBrowserReporter.CanAnnounceServer() ) {
				ServerBrowserReporter.AnnounceServer();
			}
			
			Timers.SetTimer( "ModHelpersServerBrowserReport", (60 * 10) * 60, delegate {  // 10 minutes
				if( ServerBrowserReporter.CanAnnounceServer() ) {
					if( !ServerBrowserReporter.IsHammering() ) {
						ServerBrowserReporter.AnnounceServer();
					}
				}
				return true;
			} );
		}


		////////////////

		internal void StopLoopingServerAnnounce() {
			Timers.UnsetTimer( "server_browser_report" );
			//this.IsSendingUpdates = false;
		}
	}
}
