using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		private ModPacket GetClientPacket( bool is_request, bool sync_to_all ) {
			if( Main.netMode != 1 ) { throw new Exception("Not a client"); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( is_request );
			packet.Write( sync_to_all );

			return packet;
		}


		private ModPacket GetServerPacket( bool is_request ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not a server" ); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( is_request );

			return packet;
		}
	}
}
