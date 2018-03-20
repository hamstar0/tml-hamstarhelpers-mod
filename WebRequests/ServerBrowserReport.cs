using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.MiscHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Network;
using HamstarHelpers.Utilities.Timers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	class ServerBrowserEntry {
		public bool IsClient = false;
		public string ServerIP;
		public int Port;
		public string Motd;
		public string WorldName;
		public string WorldProgress;
		public string WorldEvent;
		public long Created;
		public int MaxPlayerCount;
		public int PlayerCount;
		public int PlayerPvpCount;
		public int TeamsCount;
		public IDictionary<string, string> Mods = new Dictionary<string, string>();
	}


	public class ServerBrowserClientData {
		public bool IsClient = true;
		public string SteamID;
		public string ClientIP;
		public string ServerIP;
		public int Port;
		public string WorldName;
		public int Ping;
		public bool IsPassworded;
		public string HelpersVersion;
	}




	class ServerBrowserReport {
		private readonly static string URL =
			"https://script.google.com/macros/s/AKfycbzQl2JmJzdEHguVI011Hk1KuLktYJPDzpWA_tDbyU_Pk02fILUw/exec";

		private static long LastSendTimestamp;


		////////////////

		public static bool CanAddToBrowser() {
			//Netplay.UseUPNP
			//return Netplay.ServerPassword == "";
			if( Main.netMode == 0 ) {
				//throw new Exception("Cannot add single player games to server browser.");
				return false;
			}
			if( HamstarHelpersMod.Instance.Config.IsServerHiddenFromBrowser ) {
				return false;
			}
			if( (SystemHelpers.TimeStampInSeconds() - ServerBrowserReport.LastSendTimestamp) <= 3 ) {
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
			} catch( Exception _ ) {
				LogHelpers.Log( "CanAddToBrowser - Invalid public IP" );
				return false;
			}

			if( ip == "127.0.0.1" || ip.Substring(0, 3) == "10." ) {
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

		public static void AnnounceServer() {
			int pvp = 0;
			bool[] team_checks = new bool[10];

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
				server_data.ServerIP = NetHelpers.NetHelpers.GetPublicIP(); //	Netplay.ServerIP.ToString();	//Main.recentIP[0];
				server_data.Port = Netplay.ListenPort;
				server_data.Motd = Main.motd;
				server_data.WorldName = Main.worldName;
				server_data.WorldProgress = InfoHelpers.GetVanillaProgress();
				server_data.WorldEvent = NPCInvasionHelpers.GetCurrentInvasionType().ToString();
				server_data.Created = SystemHelpers.TimeStampInSeconds() - WorldHelpers.WorldHelpers.GetElapsedPlayTime();
				server_data.MaxPlayerCount = Main.maxNetPlayers;
				server_data.PlayerCount = Main.ActivePlayersCount;
				server_data.PlayerPvpCount = pvp;
				server_data.TeamsCount = team_count;
				server_data.Mods = new Dictionary<string, string>();

				foreach( Mod mod in ModLoader.LoadedMods ) {
					if( mod.File == null ) { continue; }
					server_data.Mods[ mod.DisplayName ] = mod.Version.ToString();
				}
			
				string json_str = JsonConvert.SerializeObject( server_data, Formatting.None );
				byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );
			
				NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
					LogHelpers.Log( "Server data added to browser. " + output );
				}, delegate( Exception e, string output ) {
					LogHelpers.Log( "Server browser returned error: " + e.ToString() );
				} );

				ServerBrowserReport.LastSendTimestamp = SystemHelpers.TimeStampInSeconds();
			} catch( Exception e ) {
				LogHelpers.Log( "AnnounceServer - " + e.ToString() );
				return;
			}
		}


		public static void AnnounceServerConnect() {
			try {
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
			}
		}



		////////////////

		//private bool IsSendingUpdates = true;


		////////////////

		internal ServerBrowserReport() {
			TmlLoadHelpers.AddWorldLoadPromise( delegate {
				if( Main.netMode != 2 ) { return; }
				if( !ServerBrowserReport.CanAddToBrowser() ) { return; }

				if( ServerBrowserReport.CanPromptForBrowserAdd() ) {
					Timers.SetTimer( "server_browser_intro", 60 * 3, delegate {
						string msg = "Hamstar's Helpers would like to list your servers in the Server Browser mod. Type '/hhprivateserver' in the chat or server console to cancel this. Otherwise, do nothing for 60 seconds.";

						Main.NewText( msg, Color.Yellow );
						Console.WriteLine( msg );
						return false;
					} );

					int seconds = HamstarHelpersMod.Instance.Config.ServerBrowserAutoRefreshSeconds;
					seconds = seconds > 10 ? seconds : 10;

					Func<bool> repeats = delegate () {
						if( ServerBrowserReport.CanAddToBrowser() ) {
							ServerBrowserReport.AnnounceServer();
						}
						return true;
					};
					Func<bool> initial = delegate () {
						if( ServerBrowserReport.CanAddToBrowser() ) {
							ServerBrowserReport.EndPrompts();
							ServerBrowserReport.AnnounceServer();
						}
						Timers.SetTimer( "server_browser_report", seconds * 60, repeats );	// 10 minutes by default
						return false;
					};

					Timers.SetTimer( "server_browser_report", 60 * 60, initial );	// 1 minute
				} else {
					ServerBrowserReport.AnnounceServer();
				}
			} );
		}


		internal void StopUpdates() {
			Timers.UnsetTimer( "server_browser_report" );
			//this.IsSendingUpdates = false;
		}
	}
}
