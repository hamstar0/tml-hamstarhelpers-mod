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
				ServerPacketHandlers.ReceiveRequestModSettingsOnServer( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestModData:
				ServerPacketHandlers.ReceiveRequestModDataOnServer( mymod, reader, player_who );
				break;
			case NetProtocolTypes.RequestPlayerPermaDeath:
				ServerPacketHandlers.ReceivePlayerPermaDeathOnServer( mymod, reader, player_who );
				break;
			case NetProtocolTypes.UploadPlayerData:
				ServerPacketHandlers.ReceivePlayerDataOnServer( mymod, reader, player_who );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( HamstarHelpersMod mymod, int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( to_who, ignore_who );
		}

		public static void SendModDataFromServer( HamstarHelpersMod mymod, int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendModData );
			packet.Write( (int)modworld.Logic.HalfDaysElapsed );

			packet.Send( to_who, ignore_who );
		}

		public static void SendPlayerPermaDeathFromServer( HamstarHelpersMod mymod, int to_who, int ignore_who, int dead_player_who, string msg ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendPlayerPermaDeath );
			packet.Write( (int)dead_player_who );
			packet.Write( (string)msg );

			packet.Send( to_who, ignore_who );
		}
		
		public static void SendPlayerDataFromServer( HamstarHelpersMod mymod, int to_who, int ignore_who, int data_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			var myplayer = Main.player[data_who].GetModPlayer<HamstarHelpersPlayer>();
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.SendPlayerData );
			packet.Write( (int)data_who );
			myplayer.NetSend( packet, false );

			packet.Send( to_who, ignore_who );
		}
		
		/*public static void SendSetAdminFromServer( HamstarHelpersMod mymod, int to_who, int ignore_who, int target_who, bool is_set ) {
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

		private static void ReceiveRequestModSettingsOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			ServerPacketHandlers.SendModSettingsFromServer( mymod, player_who, -1 );
		}

		private static void ReceiveRequestModDataOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }
			
			ServerPacketHandlers.SendModDataFromServer( mymod, player_who, -1 );
		}

		private static void ReceivePlayerPermaDeathOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			string msg = reader.ReadString();

			Main.player[player_who].difficulty = 2;

			ServerPacketHandlers.SendPlayerPermaDeathFromServer( mymod, -1, -1, player_who, msg );
		}
		
		private static void ReceivePlayerDataOnServer( HamstarHelpersMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Server only" ); }

			Player player = Main.player[ player_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();

			int to_who = reader.ReadInt32();
			int ignore_who = to_who == -1 ? player_who : -1;

			myplayer.NetReceive( reader );
			
			ServerPacketHandlers.SendPlayerDataFromServer( mymod, to_who, ignore_who, player_who );
		}
	}
}
