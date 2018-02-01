using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			NetProtocolTypes protocol = (NetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case NetProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveRequestModSettings( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestModData:
				ServerPacketHandlers.ReceiveRequestModData( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestPlayerPermaDeath:
				ServerPacketHandlers.ReceivePlayerPermaDeath( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestPlayerData:
				ServerPacketHandlers.ReceiveRequestPlayerData( mymod, reader, player_who );
				break;
			case NetProtocolTypes.UploadPlayerData:
				ServerPacketHandlers.ReceivePlayerData( mymod, reader, player_who );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Server Senders
		////////////////

		public static void SendModSettings( HamstarHelpersMod mymod, int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( to_who, ignore_who );
		}

		public static void SendModData( HamstarHelpersMod mymod, int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.WorldLogic == null ) { throw new Exception( "SendModData - HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendModData );
			packet.Write( (int)modworld.WorldLogic.HalfDaysElapsed );

			packet.Send( to_who, ignore_who );
		}

		public static void SendPlayerPermaDeath( HamstarHelpersMod mymod, int to_who, int ignore_who, int dead_player_who, string msg ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.WorldLogic == null ) { throw new Exception( "SendPlayerPermaDeath - HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendPlayerPermaDeath );
			packet.Write( (int)dead_player_who );
			packet.Write( (string)msg );

			packet.Send( to_who, ignore_who );
		}
		
		public static void SendPlayerData( HamstarHelpersMod mymod, int to_who, int ignore_who, int data_who_specific ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.WorldLogic == null ) { throw new Exception( "SendPlayerData - HH logic not initialized." ); }

			var myplayer = Main.player[data_who_specific].GetModPlayer<HamstarHelpersPlayer>();
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendPlayerData );
			packet.Write( (int)data_who_specific );
			myplayer.Logic.NetSend( packet, false );

			packet.Send( to_who, ignore_who );
		}
		
		/*public static void SendSetAdmin( HamstarHelpersMod mymod, int to_who, int ignore_who, int target_who, bool is_set ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendSetAdmin );
			packet.Write( (int)target_who );
			packet.Write( (bool)is_set );

			packet.Send( to_who, ignore_who );
		}*/

		
		
		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveRequestModSettings( HamstarHelpersMod mymod, BinaryReader reader, int packet_src_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			ServerPacketHandlers.SendModSettings( mymod, packet_src_who, -1 );
		}
		
		private static void ReceiveRequestPlayerData( HamstarHelpersMod mymod, BinaryReader reader, int packet_src_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			int from_who = reader.ReadInt32();
			
			if( from_who == -1 ) {
				for( int i = 0; i < Main.player.Length; i++ ) {
					Player from_plr = Main.player[i];
					if( from_plr == null || !from_plr.active || packet_src_who == i ) { continue; }

					ServerPacketHandlers.SendPlayerData( mymod, packet_src_who, -1, i );
				}

				// If only 1 player is on the server, this bogus packet lets a return signal confirm data request received
				ServerPacketHandlers.SendPlayerData( mymod, packet_src_who, -1, 255 );
			} else {
				ServerPacketHandlers.SendPlayerData( mymod, packet_src_who, -1, from_who );
			}
		}

		private static void ReceiveRequestModData( HamstarHelpersMod mymod, BinaryReader reader, int packet_src_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }
			
			ServerPacketHandlers.SendModData( mymod, packet_src_who, -1 );
		}

		private static void ReceivePlayerPermaDeath( HamstarHelpersMod mymod, BinaryReader reader, int packet_src_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			string msg = reader.ReadString();

			Main.player[packet_src_who].difficulty = 2;

			ServerPacketHandlers.SendPlayerPermaDeath( mymod, -1, -1, packet_src_who, msg );
		}
		
		private static void ReceivePlayerData( HamstarHelpersMod mymod, BinaryReader reader, int packet_src_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			Player player = Main.player[ packet_src_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			int to_who = reader.ReadInt32();
			int ignore_who = to_who == -1 ? packet_src_who : -1;

			myplayer.Logic.NetReceive( reader, true );
			
			ServerPacketHandlers.SendPlayerData( mymod, to_who, ignore_who, packet_src_who );
		}
	}
}
