using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.NetProtocol {
	public enum ClientNetProtocolTypes : byte {
		ModData
	}


	static class ClientNetProtocol {
		public static void RoutePacket( HamstarHelpers mymod, BinaryReader reader ) {
			ClientNetProtocolTypes protocol = (ClientNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case ClientNetProtocolTypes.ModData:
				//if( is_debug ) { DebugHelpers.Log( "Packet ModData" ); }
				ClientNetProtocol.ReceiveModDataOnClient( mymod, reader );
				break;
			default:
				DebugHelpers.DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendRequestModDataFromClient( HamstarHelpers mymod ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)ServerNetProtocolTypes.RequestModData );
			packet.Write( (int)Main.myPlayer );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModDataOnClient( HamstarHelpers mymod, BinaryReader reader ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			int half_days = reader.ReadInt32();

			var modworld = mymod.GetModWorld<MyModWorld>();
			if( modworld.Logic == null ) { throw new Exception( "HH logic not initialized." ); }

			modworld.Logic.LoadOnce( half_days );

			var modplayer = Main.player[Main.myPlayer].GetModPlayer<MyModPlayer>( mymod );
			modplayer.PostEnterWorld();
		}
	}
}
