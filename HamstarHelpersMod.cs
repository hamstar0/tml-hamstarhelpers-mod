using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		public static HamstarHelpersMod Instance;



		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			try {
				int protocol_code = reader.ReadInt32();

				if( Main.netMode == 1 ) {
					PacketProtocol.HandlePacketOnClient( protocol_code, reader, player_who );
				} else if( Main.netMode == 2 ) {
					PacketProtocol.HandlePacketOnServer( protocol_code, reader, player_who );
				}
			} catch( Exception e ) {
				LogHelpers.Log( "HamstarHelpersMod.HandlePacket - " + e.ToString() );
			}
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
