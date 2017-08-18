using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( HamstarHelpers mymod, BinaryReader reader, int player_who ) {
			NetProtocolTypes protocol = (NetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case NetProtocolTypes.RequestModData:
				//if( is_debug ) { DebugHelpers.Log( "Packet RequestModData" ); }
				ServerPacketHandlers.ReceiveRequestModDataOnServer( mymod, reader, player_who );
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

			packet.Write( (byte)NetProtocolTypes.SendModData );
			packet.Write( (int)modworld.Logic.HalfDaysElapsed );

			packet.Send( (int)player.whoAmI );
		}


		////////////////
		// Server Receivers
		////////////////
		
		private static void ReceiveRequestModDataOnServer( HamstarHelpers mymod, BinaryReader reader, int player_who ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ServerPacketHandlers.SendModDataFromServer( mymod, Main.player[player_who] );
		}
	}
}
