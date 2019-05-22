using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.PacketProtocol.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Reflection;
using System.Threading;


namespace HamstarHelpers.Components.PacketProtocol {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal static void HandlePacketOnClient( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest;

			Type protocolType = mymod.PacketProtocolMngr.GetProtocolType( protocolCode );
			if( protocolType == null ) {
				throw new HamstarException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				isRequest = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "Could not read data for protocol " + protocolType.Namespace+"."+protocolType.Name+" - "+e.ToString() );
			}
			
			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateInstance( protocolType );
				//var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

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
				throw new HamstarException( "Error handling " + protocolType.Namespace + "." + protocolType.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest, isSyncedToClients;

			Type protocolType = mymod.PacketProtocolMngr.GetProtocolType( protocolCode );
			if( protocolType == null ) {
				throw new HamstarException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				isRequest = reader.ReadBoolean();
				isSyncedToClients = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new HamstarException( "Could not read data for protocol " + protocolType.Namespace+"."+protocolType.Name+" - "+e.ToString() );
			}
			
			try {
				var protocol = (PacketProtocol)PacketProtocolData.CreateInstance( protocolType );
				//var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

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
				throw new HamstarException( "Error handling " + protocolType.Namespace + "." + protocolType.Name + " - " + e.ToString() );
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
