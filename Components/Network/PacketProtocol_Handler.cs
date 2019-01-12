using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal static void HandlePacketOnClient( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest;

			try {
				isRequest = reader.ReadBoolean();
			} catch( Exception e ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnClient - " + e.ToString() );
				throw new HamstarException( e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocolCode ) ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Unrecognized packet." );
				throw new HamstarException( "Unrecognized packet." );
			}

			Type protocolType;
			if( !mymod.PacketProtocols.TryGetValue( protocolCode, out protocolType ) ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Invalid protocol (hash: " + protocolCode + ")" );
				throw new HamstarException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRawUninitialized( protocolType );

				if( isRequest ) {
					protocol.ReceiveRequestWithClientBase();
					protocol.OnInitialize();
				} else {
					protocol.ReceiveWithClientBase( reader, playerWho );
					protocol.OnInitialize();
				}
			} catch( Exception e ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnClient - "+protocolType.Name + " - " + e.ToString() );
				throw new HamstarException( protocolType.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest, isSyncedToClients;

			try {
				isRequest = reader.ReadBoolean();
				isSyncedToClients = reader.ReadBoolean();
			} catch( Exception e ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnServer - " + e.ToString() );
				throw new HamstarException( e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocolCode ) ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Unrecognized packet." );
				throw new HamstarException( "Unrecognized packet." );
			}

			Type protocolType;
			if( !mymod.PacketProtocols.TryGetValue( protocolCode, out protocolType ) ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Invalid protocol (hash: " + protocolCode + ")" );
				throw new HamstarException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRawUninitialized( protocolType );

				if( isRequest ) {
					protocol.ReceiveRequestWithServerBase( playerWho );
					protocol.OnInitialize();
				} else {
					protocol.ReceiveWithServerBase( reader, playerWho );
					protocol.OnInitialize();

					if( isSyncedToClients ) {
						protocol.SendToClient( -1, playerWho );
					}
				}
			} catch( Exception e ) {
				throw new HamstarException( protocolType.Name + " - " + e.ToString() );
			}
		}
	}
}
