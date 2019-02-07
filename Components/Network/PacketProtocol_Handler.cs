using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Reflection;
using System.Threading;


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
				var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

				if( isRequest ) {
					protocol.ReceiveRequestWithClientBase();
					protocol.OnClone();
				} else {
					if( protocol.IsAsync ) {
						ThreadPool.QueueUserWorkItem( _ => {
							protocol.HandleClient_Core( reader, playerWho );
						} );
					} else {
						protocol.HandleClient_Core( reader, playerWho );
					}
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
				var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

				if( isRequest ) {
					protocol.ReceiveRequestWithServerBase( playerWho );
					protocol.OnClone();
				} else {
					if( protocol.IsAsync ) {
						ThreadPool.QueueUserWorkItem( _ => {
							protocol.HandleServer_Core( reader, playerWho, isSyncedToClients );
						} );
					} else {
						protocol.HandleServer_Core( reader, playerWho, isSyncedToClients );
					}
				}
			} catch( Exception e ) {
				throw new HamstarException( protocolType.Name + " - " + e.ToString() );
			}
		}


		////////////////

		private void HandleClient_Core( BinaryReader reader, int playerWho ) {
			this.ReceiveWithClientBase( reader, playerWho );
			this.OnClone();
		}

		private void HandleServer_Core( BinaryReader reader, int playerWho, bool isSyncedToClients ) {
			this.ReceiveWithServerBase( reader, playerWho );
			this.OnClone();

			if( isSyncedToClients ) {
				this.SendToClient( -1, playerWho );
			}
		}
	}
}
