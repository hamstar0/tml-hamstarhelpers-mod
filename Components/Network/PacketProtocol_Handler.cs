﻿using HamstarHelpers.Components.Errors;
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
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocolCode ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Unrecognized packet." );
			}

			Type protocolType;
			if( !mymod.PacketProtocols.TryGetValue( protocolCode, out protocolType ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRaw( protocolType );

				if( isRequest ) {
					protocol.ReceiveRequestWithClientBase();
				} else {
					protocol.ReceiveWithClientBase( reader, playerWho );
				}
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnClient - "+protocolType.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest, isSyncedToClients;

			try {
				isRequest = reader.ReadBoolean();
				isSyncedToClients = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - " + e.ToString() );
			}

			if( !mymod.PacketProtocols.ContainsKey( protocolCode ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Unrecognized packet." );
			}

			Type protocolType;
			if( !mymod.PacketProtocols.TryGetValue( protocolCode, out protocolType ) ) {
				throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateRaw( protocolType );

				if( isRequest ) {
					protocol.ReceiveRequestWithServerBase( playerWho );
				} else {
					protocol.ReceiveWithServerBase( reader, playerWho );

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
