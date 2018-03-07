using HamstarHelpers.DebugHelpers;
using HamstarHelpers.DotNetHelpers;
using HamstarHelpers.MiscHelpers;
using HamstarHelpers.NPCHelpers;
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
		public string WorldName;
		public string WorldProgress;
		public string WorldEvent;
		public int Uptime;
		public int PlayerCount;
		public int PlayerPvpCount;
		public int TeamsCount;
		public IDictionary<string, Version> Mods = new Dictionary<string, Version>();
	}


	public class ServerBrowserClientData {
		public bool IsClient = true;
		public string SteamID;
		public string ClientIP;
		public string ServerIP;
		public string WorldName;
		public int Ping;
	}




	class ServerBrowserReport {
		private readonly static string URL = "https://script.google.com/macros/s/AKfycbzQl2JmJzdEHguVI011Hk1KuLktYJPDzpWA_tDbyU_Pk02fILUw/exec";


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
			server_data.ServerIP = Main.recentIP[0];
			server_data.WorldName = Main.worldName;
			server_data.WorldProgress = InfoHelpers.GetVanillaProgress();
			server_data.WorldEvent = NPCInvasionHelpers.GetCurrentInvasionType().ToString();
			server_data.Uptime = WorldHelpers.WorldHelpers.GetElapsedPlayTime();
			server_data.PlayerCount = Main.ActivePlayersCount;
			server_data.PlayerPvpCount = pvp;
			server_data.TeamsCount = team_count;
			server_data.Mods = new Dictionary<string, Version>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				server_data.Mods[ mod.DisplayName ] = mod.Version;
			}
			
			string json_str = JsonConvert.SerializeObject( server_data, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server data added to browser. " + output );
			} );
		}


		public static void AnnounceServerConnect() {
			var client_data = new ServerBrowserClientData();
			client_data.SteamID = SteamHelpers.GetSteamID();
			client_data.ClientIP = Main.getIP;
			client_data.ServerIP = Main.recentIP[0];
			client_data.WorldName = Main.worldName;
			client_data.Ping = NetHelpers.NetHelpers.GetServerPing();
			
			string json_str = JsonConvert.SerializeObject( client_data, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			NetHelpers.NetHelpers.MakePostRequestAsync( ServerBrowserReport.URL, json_bytes, delegate ( string output ) {
				LogHelpers.Log( "Server connection data added to browser. " + output );
			} );
		}
	}
}
