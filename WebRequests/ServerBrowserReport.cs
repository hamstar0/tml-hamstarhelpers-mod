using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.WebRequests {
	class ServerBrowserEntry {
		public string ServerIP;
		public int Port;
		public string WorldName;

		public string Motd;
		public string WorldProgress;
		public string WorldEvent;
		//public long Created;
		public int MaxPlayerCount;
		public int PlayerCount;
		public int PlayerPvpCount;
		public int TeamsCount;
		public int AveragePing;
		public IDictionary<string, string> Mods = new Dictionary<string, string>();
	}



	class ServerBrowserWorkAssignment {
		public bool Success;
		public bool ProofOfWorkNeeded;
		public string Hash;
	}


	class ServerBrowserWorkProof {
		public bool IsReply = true;

		public string ServerIP;
		public int Port;
		public string WoldName;

		public string HashBase;
	}



	/*public class ServerBrowserClientData {
		public bool IsClient = true;
		public string SteamID;
		public string ClientIP;
		public string ServerIP;
		public int Port;
		public string WorldName;
		public int Ping;
		public bool IsPassworded;
		public string HelpersVersion;
	}*/




	partial class ServerBrowserReporter {
		private readonly static string URL =
			"https://script.google.com/macros/s/AKfycbzQl2JmJzdEHguVI011Hk1KuLktYJPDzpWA_tDbyU_Pk02fILUw/exec";

		private static long LastSendTimestamp;


		////////////////

		public static bool CanAddToBrowser() {
			//return Netplay.ServerPassword == "";
			if( Main.netMode == 0 ) {
				//throw new Exception("Cannot add single player games to server browser.");
				return false;
			}
			//if( !Netplay.UseUPNP ) {
			//	return false;
			//}
			if( HamstarHelpersMod.Instance.Config.IsServerHiddenFromBrowser ) {
				return false;
			}
			if( (SystemHelpers.TimeStampInSeconds() - ServerBrowserReporter.LastSendTimestamp) <= 3 ) {
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



		////////////////

		//private bool IsSendingUpdates = true;
		public int AveragePing { get; internal set; }


		////////////////

		internal ServerBrowserReporter() {
			this.AveragePing = -1;

			this.InitializeAutoServerUpdates();
		}


		////////////////

		internal void UpdatePingAverage( int ping ) {
			if( this.AveragePing == -1 ) {
				this.AveragePing = ping;
			} else {
				this.AveragePing = (ping + ( this.AveragePing * 2 )) / 3;
			}
		}
	}
}
