using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Protocols.Packet;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public override void HandlePacket( BinaryReader reader, int playerWho ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			if( NetIO.HandlePacket(reader, playerWho) ) {
				return;
			}

			try {
				int protocolCode = reader.ReadInt32();
				
				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					PacketProtocol.HandlePacketOnClient( protocolCode, reader, playerWho );
				} else if( Main.netMode == NetmodeID.Server ) {
					PacketProtocol.HandlePacketOnServer( protocolCode, reader, playerWho );
				}
			} catch( Exception e ) {
				LogHelpers.Alert( e.ToString() );
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}
