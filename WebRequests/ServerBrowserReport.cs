using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.MiscHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.Utilities.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	class HHServerUpdateRequestProtocol : PacketProtocol {
		public override void SetServerDefaults() { }
		
		public override bool ReceiveRequestOnServer( int from_who ) {
			if( ServerBrowserReport.CanAnnounce() ) {
				ServerBrowserReport.AnnounceServer();
			}
			return true;
		}
	}



	class ServerBrowserEntry {
		public bool IsClient = false;
		public string ServerIP;
		public int Port;
		public string WorldName;
		public string WorldProgress;
		public string WorldEvent;
		public long Since;
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
	}




	class ServerBrowserReport {
		private readonly static string URL =
			"https://script.google.com/macros/s/AKfycbzQl2JmJzdEHguVI011Hk1KuLktYJPDzpWA_tDbyU_Pk02fILUw/exec";


		public static bool CanAnnounce() {
			//Netplay.UseUPNP
			//return Netplay.ServerPassword == "";
			return !HamstarHelpersMod.Instance.Config.IsServerHiddenFromBrowser;
		}


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

			var server_data = new ServerBrowserEntry();
			server_data.ServerIP = NetHelpers.NetHelpers.GetPublicIP(); //	Netplay.ServerIP.ToString();	//Main.recentIP[0];
			server_data.Port = Netplay.ListenPort;
			server_data.WorldName = Main.worldName;
			server_data.WorldProgress = InfoHelpers.GetVanillaProgress();
			server_data.WorldEvent = NPCInvasionHelpers.GetCurrentInvasionType().ToString();
			server_data.Since = SystemHelpers.TimeStampInSeconds() - WorldHelpers.WorldHelpers.GetElapsedPlayTime();
			server_data.MaxPlayerCount = Main.maxNetPlayers;
			server_data.PlayerCount = Main.ActivePlayersCount;
			server_data.PlayerPvpCount = pvp;
			server_data.TeamsCount = team_count;
			server_data.Mods = new Dictionary<string, string>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				server_data.Mods[ mod.DisplayName ] = mod.Version.ToString();
			}
			
			string json_str = JsonConvert.SerializeObject( server_data, Formatting.None );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );
			
			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server data added to browser. " + output );
			}, delegate( Exception e ) {
				LogHelpers.Log( "Server browser returned error: " + e.ToString() );
			} );
		}


		public static void AnnounceServerConnect() {
			var client_data = new ServerBrowserClientData();
			client_data.SteamID = SteamHelpers.GetSteamID();
			client_data.ClientIP = Netplay.GetLocalIPAddress();
			client_data.ServerIP = Netplay.ServerIP.ToString(); //Main.recentIP[0];
			client_data.WorldName = Main.worldName;
			client_data.Port = Netplay.ListenPort;
			client_data.Ping = NetHelpers.NetHelpers.GetServerPing();
			client_data.IsPassworded = !string.IsNullOrEmpty( Netplay.ServerPassword );
			
			string json_str = JsonConvert.SerializeObject( client_data, Formatting.None );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );
			
			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server connection data added to browser. " + output );
			}, delegate ( Exception e ) {
				LogHelpers.Log( "Server browser returned error for client: " + e.ToString() );
			} );
		}



		////////////////

		private bool IsInWorld = false;


		////////////////

		internal ServerBrowserReport() {
			Main.OnTick += this.Update;
		}

		internal void Unload() {
			Main.OnTick -= this.Update;
		}

		////////////////

		private void Update() {
			var mymod = HamstarHelpersMod.Instance;
			if( mymod == null || mymod.TmlLoadHelpers == null ) { return; }

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				if( !this.IsInWorld ) {
					this.IsInWorld = true;
					if( ServerBrowserReport.CanAnnounce() ) {
						ServerBrowserReport.AnnounceServer();
					}
				}
			} else {
				if( !this.IsInWorld ) {
					this.IsInWorld = false;
				}
			}
		}
	}
}
