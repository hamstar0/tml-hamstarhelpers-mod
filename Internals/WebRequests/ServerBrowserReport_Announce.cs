using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers;
using HamstarHelpers.MiscHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.WebRequests {
	partial class ServerBrowserReporter {
		public static bool CanAnnounceServer() {
			//return Netplay.ServerPassword == "";
			if( Main.netMode == 0 ) {
				//throw new Exception("Cannot add single player games to server browser.");
				return false;
			}

			if( HamstarHelpersMod.Instance.Config.IsServerHiddenFromBrowser ) {
				return false;
			}
			if( HamstarHelpersMod.Instance.Config.IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP && !Netplay.UseUPNP ) {
				return false;
			}

			if( Main.netMode == 1 ) {
				if( NetHelpers.NetHelpers.GetServerPing() == -1 ) {
					return false;
				}
			}

			string ip;
			try {
				ip = NetHelpers.NetHelpers.GetPublicIP();
			} catch( Exception ) {
				LogHelpers.Log( "CanAddToBrowser - Invalid public IP" );
				return false;
			}

			if( ip == "127.0.0.1" || ip.Substring( 0, 3 ) == "10." ) {
				return false;
			}
			switch( ip.Substring( 0, 7 ) ) {
			case "192.168":
			case "172.16.":
			case "172.17.":
			case "172.18.":
			case "172.19.":
			case "172.20.":
			case "172.21.":
			case "172.22.":
			case "172.23.":
			case "172.24.":
			case "172.25.":
			case "172.26.":
			case "172.27.":
			case "172.28.":
			case "172.29.":
			case "172.30.":
			case "172.31.":
			case "172.32.":
				return false;
			}

			return true;
		}
		

		////////////////

		public static void AnnounceServer() {
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			Version vers = mymod.Version;

			int port = Netplay.ListenPort;
			if( mymod.Config.ServerBrowserCustomPort != -1 ) {
				port = mymod.Config.ServerBrowserCustomPort;
			}

			int pvp = 0;
			bool[] team_checks = new bool[10];
			ServerBrowserReporter server_browser = mymod.ServerBrowser;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( player.hostile ) {
					pvp++;
				}
				team_checks[ player.team ] = true;
			}

			int team_count = 0;
			for( int i=1; i<team_checks.Length; i++ ) {
				if( team_checks[i] ) { team_count++; }
			}

			try {
				var server_data = new ServerBrowserEntry();
				server_data.ServerIP = NetHelpers.NetHelpers.GetPublicIP();
				server_data.Port = port;
				server_data.IsPassworded = Netplay.ServerPassword != "";
				server_data.Motd = Main.motd;
				server_data.WorldName = Main.worldName;
				server_data.WorldProgress = InfoHelpers.GetVanillaProgress();
				server_data.WorldEvent = string.Join( ", ", InfoHelpers.GetCurrentVanillaEvents().ToArray() );
				server_data.MaxPlayerCount = Main.maxNetPlayers;
				server_data.PlayerCount = Main.ActivePlayersCount;
				server_data.PlayerPvpCount = pvp;
				server_data.TeamsCount = team_count;
				server_data.AveragePing = mymod.ServerBrowser.AveragePing;
				server_data.Mods = new Dictionary<string, string>();
				server_data.Version = ( vers.Major * 1000000 ) + ( vers.Minor * 10000 ) + ( vers.Build * 100 ) + vers.Revision;

				foreach( Mod mod in ModLoader.LoadedMods ) {
					if( mod.File == null ) { continue; }
					server_data.Mods[ mod.DisplayName ] = mod.Version.ToString();
				}
				
				string json_str = JsonConvert.SerializeObject( server_data, Formatting.None );
				byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

				Action<string> on_response = delegate ( string output ) {
					ServerBrowserReporter.HandleServerAnnounceOutputAsync( server_data, output );
				};
				Action<Exception, string> on_error = delegate ( Exception e, string output ) {
					LogHelpers.Log( "Server browser returned error: " + e.ToString() );
				};

				NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReporter.URL, json_bytes, on_response, on_error );

				ServerBrowserReporter.LastSendTimestamp = SystemHelpers.TimeStampInSeconds();
			} catch( Exception e ) {
				LogHelpers.Log( "AnnounceServer - " + e.ToString() );
				return;
			}
		}

		private static void HandleServerAnnounceOutputAsync( ServerBrowserEntry server_data, string output ) {
			try {
				var reply = JsonConvert.DeserializeObject<ServerBrowserWorkAssignment>( output );

				if( reply.ProofOfWorkNeeded ) {
					ServerBrowserReporter.DoWorkToValidateServer( server_data, reply.Hash );

					LogHelpers.Log( "Server data added to browser? " + reply.Success + " - Beginning validation..." );
				} else {
					LogHelpers.Log( "Server updated on browser? " + reply.Success );
				}
			} catch( Exception e ) {
				if( !( e is JsonReaderException ) ) {
					LogHelpers.Log( "HandleServerAnnounceOutput - " + e.ToString() + " - " + ( output.Length > 256 ? output.Substring( 0, 256 ) : output ) );
				}
			}
		}


		////////////////

		public static void AnnounceServerConnect() {
			throw new NotImplementedException();
			/*try {
				var client_data = new ServerBrowserClientData();
				client_data.SteamID = SteamHelpers.GetSteamID();
				client_data.ClientIP = NetHelpers.NetHelpers.GetPublicIP();	// Netplay.GetLocalIPAddress();
				client_data.ServerIP = Netplay.ServerIP.ToString(); //Main.recentIP[0];
				client_data.WorldName = Main.worldName;
				client_data.Port = Netplay.ListenPort;
				client_data.Ping = NetHelpers.NetHelpers.GetServerPing();
				client_data.IsPassworded = !string.IsNullOrEmpty( Netplay.ServerPassword );
				client_data.HelpersVersion = HamstarHelpersMod.Instance.Version.ToString();
			
				string json_str = JsonConvert.SerializeObject( client_data, Formatting.None );
				byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );
			
				NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
					LogHelpers.Log( "Server connection data added to browser. " + output );
				}, delegate ( Exception e, string output ) {
					LogHelpers.Log( "Server browser returned error for client: " + e.ToString() );
				} );

				ServerBrowserReport.LastSendTimestamp = SystemHelpers.TimeStampInSeconds();
			} catch( Exception e ) {
				LogHelpers.Log( "AnnounceServerConnect - " + e.ToString() );
				return;
			}*/
		}
	}
}
