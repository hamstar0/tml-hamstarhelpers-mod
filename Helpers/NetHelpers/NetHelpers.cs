using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Timers;
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
			Main.defaultIP = ip;
			if( Netplay.SetRemoteIP( ip ) ) {
				Main.menuMode = 10;
				Netplay.StartTcpClient();
			}
		}



		////////////////

		private string PublicIP = null;


		////////////////

		internal NetHelpers() {
			this.LoadIPAsync();

			int attempts = 3;

			Timers.SetTimer( "retry_ip", 60 * 20, delegate () {
				if( this.PublicIP != null ) { return false; }

				this.LoadIPAsync();
				return attempts-- > 0;
			} );
		}
		

		private void LoadIPAsync() {
			Action<string> on_success = delegate ( string output ) {
				if( this.PublicIP != null ) { return; }

				string[] a = output.Split( ':' );
				string a2 = a[1].Substring( 1 );
				string[] a3 = a2.Split( '<' );
				string a4 = a3[0];

				this.PublicIP = a4;
			};
			Action<Exception, string> on_fail = delegate ( Exception e, string output ) {
				LogHelpers.Log( "Could not acquire IP: " + e.ToString() );
			};

			NetHelpers.MakeGetRequestAsync( "http://checkip.dyndns.org/", on_success, on_fail );
			//NetHelpers.MakeGetRequestAsync( "https://api.ipify.org/", on_success, on_fail );
			//using( WebClient web_client = new WebClient() ) {
			//	this.PublicIP = web_client.DownloadString( "http://ifconfig.me/ip" );
			//}
		}
	}
}
