using System;
using System.IO;
using Terraria;
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
			default:
				DebugHelpers.DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModDataFromClient( HamstarHelpersMod mymod ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)NetProtocolTypes.RequestModData );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModDataOnClient( HamstarHelpersMod mymod, BinaryReader reader ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			int half_days = reader.ReadInt32();

			var modworld = mymod.GetModWorld<MyWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			modworld.Logic.LoadOnce( half_days );

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>( mymod );
			modplayer.PostEnterWorld();
		}
	}
}
