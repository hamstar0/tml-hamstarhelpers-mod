using HamstarHelpers.Services.Errors;
using HamstarHelpers.Components.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		private ModPacket GetClientPacket( bool isRequest, bool syncToAll ) {
			if( Main.netMode != 1 ) { throw new ModHelpersException( "Not a client"); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );
			packet.Write( syncToAll );

			return packet;
		}


		private ModPacket GetServerPacket( bool isRequest ) {
			if( Main.netMode != 2 ) { throw new ModHelpersException( "Not a server" ); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );

			return packet;
		}
	}
}
