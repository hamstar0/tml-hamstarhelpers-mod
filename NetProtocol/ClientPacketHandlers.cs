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
				ClientPacketHandlers.ReceiveModSettingsOnClient( mymod, reader );
				break;
			case NetProtocolTypes.SendModData:
				ClientPacketHandlers.ReceiveModDataOnClient( mymod, reader );
				break;
			case NetProtocolTypes.SendPlayerPermaDeath:
				ClientPacketHandlers.ReceivePlayerPermaDeathOnClient( mymod, reader );
				break;
			case NetProtocolTypes.SendPlayerData:
				ClientPacketHandlers.ReceivePlayerDataOnClient( mymod, reader );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModSettingsFromClient( HamstarHelpersMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModSettings );

			packet.Send();
		}

		public static void SendRequestModDataFromClient( HamstarHelpersMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModData );

			packet.Send();
		}

		public static void SendPermaDeathFromClient( HamstarHelpersMod mymod, string msg ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestPlayerPermaDeath );
			packet.Write( msg );

			packet.Send();
		}

		public static void SendPlayerDataFromClient( HamstarHelpersMod mymod, int to_who ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			Player player = Main.LocalPlayer;
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.UploadPlayerData );
			packet.Write( (int)to_who );
			myplayer.NetSend( packet );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>( mymod );
			string json = reader.ReadString();

			mymod.JsonConfig.DeserializeMe( json );

			myplayer.FinishModSettingsSync();
		}

		private static void ReceiveModDataOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>( mymod );
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			int half_days = reader.ReadInt32();
			
			modworld.Logic.LoadFromNetwork( half_days );
		}
		
		private static void ReceivePlayerDataOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int from_who = reader.ReadInt32();
			var myplayer = Main.player[from_who].GetModPlayer<HamstarHelpersPlayer>();

			myplayer.NetReceive( reader, false );

			myplayer.FinishPlayerDataSync();
		}

		private static void ReceivePlayerPermaDeathOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int player_who = reader.ReadInt32();
			string msg = reader.ReadString();
			Player player = Main.player[player_who];
			
			player.difficulty = 2;
			player.KillMe( PlayerDeathReason.ByCustomReason( msg ), 9999, 0 );
		}
	}
}
