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
			case NetProtocolTypes.SendModData:
				//if( is_debug ) { DebugHelpers.Log( "Packet ModData" ); }
				ClientPacketHandlers.ReceiveModDataOnClient( mymod, reader );
				break;
			case NetProtocolTypes.SendPlayerPermaDeath:
				ClientPacketHandlers.ReceivePlayerPermaDeathOnClient( mymod, reader );
				break;
			default:
				DebugHelpers.DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModDataFromClient( HamstarHelpersMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModData );

			packet.Send();
		}
		public static void SendPermaDeathFromClient( HamstarHelpersMod mymod, string msg ) {
			if( Main.netMode != 1 ) { throw new Exception("Client only"); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestPlayerPermaDeath );
			packet.Write( msg );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModDataOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int half_days = reader.ReadInt32();

			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			modworld.Logic.LoadOnce( half_days );

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<HamstarHelpersPlayer>( mymod );
			modplayer.PostEnterWorld();
		}

		private static void ReceivePlayerPermaDeathOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Client only" ); }

			int player_who = reader.ReadInt32();
			string msg = reader.ReadString();

			Main.LocalPlayer.difficulty = 2;

			if( Main.myPlayer == player_who ) {
				Main.LocalPlayer.KillMe( PlayerDeathReason.ByCustomReason( msg ), 9999, 0 );
			}
		}
	}
}
