using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal static void HandlePacketOnClient( int protocol_code, BinaryReader reader, int player_who ) {
			var mymod = ModHelpersMod.Instance;
			bool is_request;

			try {
				is_request = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocol_code ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Unrecognized packet." );
			}

			Type protocol_type;
			if( !mymod.PacketProtocols.TryGetValue( protocol_code, out protocol_type ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Invalid protocol (hash: " + protocol_code + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRaw( protocol_type );

				if( is_request ) {
					protocol.ReceiveRequestWithClientBase();
				} else {
					protocol.ReceiveWithClientBase( reader, player_who );
				}
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - "+protocol_type.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( int protocol_code, BinaryReader reader, int player_who ) {
			var mymod = ModHelpersMod.Instance;
			bool is_request, is_synced_to_clients;

			try {
				is_request = reader.ReadBoolean();
				is_synced_to_clients = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocol_code ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Unrecognized packet." );
			}

			Type protocol_type;
			if( !mymod.PacketProtocols.TryGetValue( protocol_code, out protocol_type ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Invalid protocol (hash: " + protocol_code + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRaw( protocol_type );

				if( is_request ) {
					protocol.ReceiveRequestWithServerBase( player_who );
				} else {
					protocol.ReceiveWithServerBase( reader, player_who );

					if( is_synced_to_clients ) {
						protocol.SendToClient( -1, player_who );
					}
				}
			} catch( Exception e ) {
				throw new HamstarException( protocol_type.Name + " - " + e.ToString() );
			}
		}
	}
}
