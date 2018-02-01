using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	static class ClientPacketHandlers {
		public static void RoutePacket( HamstarHelpersMod mymod, BinaryReader reader ) {
			NetProtocolTypes protocol = (NetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case NetProtocolTypes.SendModSettings:
				ClientPacketHandlers.ReceiveModSettings( mymod, reader );
				break;
			case NetProtocolTypes.SendModData:
				ClientPacketHandlers.ReceiveModData( mymod, reader );
				break;
			case NetProtocolTypes.SendPlayerPermaDeath:
				ClientPacketHandlers.ReceivePlayerPermaDeath( mymod, reader );
				break;
			case NetProtocolTypes.SendPlayerData:
				ClientPacketHandlers.ReceivePlayerData( mymod, reader );
				break;
			//case NetProtocolTypes.SendSetAdmin:
			//	ClientPacketHandlers.ReceiveSetAdminOnClient( mymod, reader );
			//	break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Senders
		////////////////

		public static void SendRequestModSettings( HamstarHelpersMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModSettings );

			packet.Send();
		}

		public static void SendRequestPlayerData( HamstarHelpersMod mymod, int from_who ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestPlayerData );
			packet.Write( (int)from_who );

			packet.Send();
		}

		public static void SendRequestModData( HamstarHelpersMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModData );

			packet.Send();
		}

		public static void SendPermaDeath( HamstarHelpersMod mymod, string msg ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestPlayerPermaDeath );
			packet.Write( msg );

			packet.Send();
		}

		public static void SendPlayerData( HamstarHelpersMod mymod, int to_who ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.UploadPlayerData );
			packet.Write( (int)to_who );
			myplayer.Logic.NetSend( packet, true );

			packet.Send();
		}



		////////////////
		// Receivers
		////////////////

		private static void ReceiveModSettings( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			string json = reader.ReadString();

			mymod.Config.LoadFromNetwork( mymod, json );
		}

		private static void ReceiveModData( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }
			
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.WorldLogic == null ) { throw new Exception( "ReceiveModData - HH logic not initialized." ); }

			int half_days = reader.ReadInt32();
			
			modworld.WorldLogic.LoadFromNetwork( half_days );
		}
		
		private static void ReceivePlayerData( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int from_who = reader.ReadInt32();

			Player of_player = Main.player[from_who];
			var thatplayer = of_player.GetModPlayer<HamstarHelpersPlayer>();
			
			thatplayer.Logic.NetReceive( reader, false );
		}

		private static void ReceivePlayerPermaDeath( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int player_who = reader.ReadInt32();
			string msg = reader.ReadString();
			Player player = Main.player[player_who];
			
			player.difficulty = 2;
			player.KillMe( PlayerDeathReason.ByCustomReason( msg ), 9999, 0 );
		}

		/*private static void ReceiveSetAdmin( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int player_who = reader.ReadInt32();
			bool is_set = reader.ReadBoolean();
			Player player = Main.player[player_who];

			if( UserHelpers.UserHelpers.IsAdmin(player) ) {
				if( !is_set ) {
					UserHelpers.UserHelpers.RemoveAdmin( player );
				}
			} else {
				if( is_set ) {
					UserHelpers.UserHelpers.AddAdmin( player );
				}
			}
		}*/
	}
}
