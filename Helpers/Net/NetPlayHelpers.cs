using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to network play.
	/// </summary>
	public partial class NetPlayHelpers {
		/// <summary>
		/// Gets the internet-facing IP address of the current machine.
		/// </summary>
		/// <returns></returns>
		public static string GetPublicIP() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.NetHelpers.PublicIP == null ) {
				throw new ModHelpersException( "Public IP not yet acquired." );
			}
			return mymod.NetHelpers.PublicIP;
		}


		/// <summary>
		/// Connects the current machine to a server to begin a game. Meant to be called from the main menu.
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public static void JoinServer( string ip, int port ) {
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

		/// <summary>
		/// Gets the most recent gauging of server ping.
		/// </summary>
		/// <returns></returns>
		public static int GetServerPing() {
			if( Main.netMode != 1 ) {
				throw new ModHelpersException( "Only clients can gauge ping." );
			}

			return ModHelpersMod.Instance.NetHelpers.CurrentPing;
		}
	}
}
