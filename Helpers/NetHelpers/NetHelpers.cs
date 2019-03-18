using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.NetHelpers {
	public partial class NetHelpers {
		public static string GetPublicIP() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.NetHelpers.PublicIP == null ) {
				throw new HamstarException( "Public IP not yet acquired." );
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

		public static int GetServerPing() {
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Only clients can gauge ping." );
			}

			return ModHelpersMod.Instance.NetHelpers.CurrentPing;
		}
	}
}
