using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.PacketProtocol.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.PacketProtocol {
	public abstract partial class PacketProtocol : PacketProtocolData {
		private ModPacket GetClientPacket( bool isRequest, bool syncToAll ) {
			if( Main.netMode != 1 ) { throw new HamstarException( "Not a client"); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );
			packet.Write( syncToAll );

			return packet;
		}


		private ModPacket GetServerPacket( bool isRequest ) {
			if( Main.netMode != 2 ) { throw new HamstarException( "Not a server" ); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );

			return packet;
		}
	}
}
