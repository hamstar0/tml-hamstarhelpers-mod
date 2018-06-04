using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.TmlHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.WebRequests {
	class ServerBrowserEntry {
		public string ServerIP;
		public int Port;
		public string WorldName;

		public bool IsPassworded;

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
		public long Version;
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
		public string WorldName;

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

		public static bool IsHammering() {
			return ( SystemHelpers.TimeStampInSeconds() - ServerBrowserReporter.LastSendTimestamp ) <= 10;
		}



		////////////////

		//private bool IsSendingUpdates = true;
		public int AveragePing { get; internal set; }


		////////////////

		internal ServerBrowserReporter() {
			this.AveragePing = -1;

			TmlLoadHelpers.AddWorldLoadEachPromise( delegate {
				this.InitializeLoopingServerAnnounce();
			} );
		}

		internal void OnWorldExit() {
			this.StopLoopingServerAnnounce();
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
