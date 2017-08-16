using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	public enum ServerNetProtocolTypes : byte {
		RequestModData
	}


	static class ServerNetProtocol {
		public static void RoutePacket( HamstarHelpers mymod, BinaryReader reader, int player_who ) {
			ServerNetProtocolTypes protocol = (ServerNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case ServerNetProtocolTypes.RequestModData:
				//if( is_debug ) { DebugHelpers.Log( "Packet RequestModData" ); }
				ServerNetProtocol.ReceiveRequestModDataOnServer( mymod, reader, player_who );
				break;
			default:
				DebugHelpers.DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}
		
		

		////////////////
		// Server Senders
		////////////////
		
		public static void SendModDataFromServer( HamstarHelpers mymod, Player player ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			var modworld = mymod.GetModWorld<MyModWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)ClientNetProtocolTypes.ModData );
			packet.Write( (int)modworld.Logic.HalfDaysElapsed );

			packet.Send( (int)player.whoAmI );
		}


		////////////////
		// Server Receivers
		////////////////
		
		private static void ReceiveRequestModDataOnServer( HamstarHelpers mymod, BinaryReader reader, int player_who ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ServerNetProtocol.SendModDataFromServer( mymod, Main.player[player_who] );
		}
	}
}
