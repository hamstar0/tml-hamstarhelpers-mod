using HamstarHelpers.Components.Errors;
using HamstarHelpers.DebugHelpers;
using System;
using System.IO;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class OldPacketProtocol {
		internal static void HandlePacketOnClient( BinaryReader reader, int player_who ) {
			var mymod = HamstarHelpersMod.Instance;
			int protocol_hash;
			bool is_request;

			try {
				protocol_hash = reader.ReadInt32();
				is_request = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocol_hash ) ) {
				throw new HamstarException( "Unrecognized packet." );
			}

			Type protocol_type;
			if( !mymod.PacketProtocols.TryGetValue( protocol_hash, out protocol_type ) ) {
				throw new HamstarException( "Invalid protocol (hash: " + protocol_hash + ")" );
			}

			try {
				var protocol = (OldPacketProtocol)Activator.CreateInstance( protocol_type );

				if( is_request ) {
					protocol.ReceiveBaseRequestOnClient();
				} else {
					protocol.ReceiveBaseOnClient( reader, player_who );
				}
			} catch( Exception e ) {
				throw new HamstarException( protocol_type.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( BinaryReader reader, int player_who ) {
			var mymod = HamstarHelpersMod.Instance;
			int protocol_hash;
			bool is_request, is_synced_to_clients;

			try {
				protocol_hash = reader.ReadInt32();
				is_request = reader.ReadBoolean();
				is_synced_to_clients = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocol_hash ) ) {
				throw new HamstarException( "Unrecognized packet." );
			}

			Type protocol_type;
			if( !mymod.PacketProtocols.TryGetValue( protocol_hash, out protocol_type ) ) {
				throw new HamstarException( "Invalid protocol (hash: " + protocol_hash+")" );
			}

			try {
				var protocol = (OldPacketProtocol)Activator.CreateInstance( protocol_type );

				if( is_request ) {
					protocol.ReceiveBaseRequestOnServer( player_who );
				} else {
					protocol.ReceiveBaseOnServer( reader, player_who );

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
