using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			NetProtocolTypes protocol = (NetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case NetProtocolTypes.RequestModData:
				//if( is_debug ) { DebugHelpers.Log( "Packet RequestModData" ); }
				ServerPacketHandlers.ReceiveRequestModDataOnServer( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestPlayerPermaDeath:
				ServerPacketHandlers.ReceivePlayerPermaDeathOnServer( mymod, reader, player_who );
				break;
			default:
				DebugHelpers.DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}
		
		

		////////////////
		// Server Senders
		////////////////
		
		public static void SendModDataFromServer( HamstarHelpersMod mymod, Player player ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendModData );
			packet.Write( (int)modworld.Logic.HalfDaysElapsed );

			packet.Send( (int)player.whoAmI );
		}

		public static void BroadcastPlayerPermaDeathFromServer( HamstarHelpersMod mymod, int player_who, string msg ) {
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendPlayerPermaDeath );
			packet.Write( (int)player_who );
			packet.Write( (string)msg );

			packet.Send();
		}


		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveRequestModDataOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			ServerPacketHandlers.SendModDataFromServer( mymod, Main.player[player_who] );
		}

		private static void ReceivePlayerPermaDeathOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			string msg = reader.ReadString();

			Main.LocalPlayer.difficulty = 2;

			ServerPacketHandlers.BroadcastPlayerPermaDeathFromServer( mymod, player_who, msg );
		}
	}
}
