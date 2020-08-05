using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		private ModPacket GetClientPacket( bool isRequest, bool syncToAll ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not a client"); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );
			packet.Write( syncToAll );

			return packet;
		}


		private ModPacket GetServerPacket( bool isRequest ) {
			if( Main.netMode != NetmodeID.Server ) { throw new ModHelpersException( "Not a server" ); }

			string name = this.GetPacketName();
			var packet = ModHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( isRequest );

			return packet;
		}
	}
}
