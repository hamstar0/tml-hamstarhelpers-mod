using HamstarHelpers.DebugHelpers;
using System;
using Terraria;

namespace HamstarHelpers.NetHelpers {
	public partial class NetHelpers {
		public static string GetPublicIP() {
			var mymod = HamstarHelpersMod.Instance;
			if( mymod.NetHelpers.PublicIP == null ) {
				throw new Exception( "Public IP not yet acquired." );
			}
			return mymod.NetHelpers.PublicIP;
		}


		public static void JoinServer( string ip, int port ) {	// Currently only meant for use in main menu only
			Main.autoPass = false;
			Netplay.ListenPort = port;
			Main.getIP = ip;
			if( Netplay.SetRemoteIP( ip ) ) {
				Main.menuMode = 10;
				Netplay.StartTcpClient();
			}
		}



		////////////////

		private string PublicIP = null;
		private int IPLoadRetryTimer = 0;


		////////////////

		internal NetHelpers() {
			Main.OnTick += this.RetryLoadIP;
		}

		internal void Unload() {
			Main.OnTick -= this.RetryLoadIP;
		}

		private void RetryLoadIP() {
			if( this.PublicIP == null ) {
				if( this.IPLoadRetryTimer >= ( 60 * 60 ) ) {
					this.IPLoadRetryTimer = 0;
					this.LoadIPAsync();
				}
				this.IPLoadRetryTimer++;
			}
		}
		

		internal void LoadIPAsync() {
			Action<string> on_success = delegate ( string output ) {
				this.PublicIP = output;
			};
			Action<Exception> on_fail = delegate ( Exception e ) {
				LogHelpers.Log( "Could not acquire IP: " + e.ToString() );
			};

			NetHelpers.MakeGetRequestAsync( "https://api.ipify.org/", on_success, on_fail );
			//using( WebClient web_client = new WebClient() ) {
			//	this.PublicIP = web_client.DownloadString( "http://ifconfig.me/ip" );
			//}
		}
	}
}
