using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using System.Threading.Tasks;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		internal static void HandlePacketOnClient( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest;

			Type protocolType = mymod.PacketProtocolMngr.GetProtocolType( protocolCode );
			if( protocolType == null ) {
				throw new ModHelpersException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				isRequest = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new ModHelpersException( "Could not read data for protocol " + protocolType.Namespace+"."+protocolType.Name+" - "+e.ToString() );
			}
			
			try {
				var protocol = (PacketProtocol)StreamProtocol.CreateInstance( protocolType );
				//var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

				if( isRequest ) {
					protocol.ReceiveRequestWithClientBase();
					protocol.OnClone();
				} else {
					if( protocol.IsAsync ) {
						Task.Run( () => {
							protocol.HandleClient_Core( reader, playerWho );
						} );
					} else {
						protocol.HandleClient_Core( reader, playerWho );
					}
				}
			} catch( Exception e ) {
				throw new ModHelpersException( "Error handling " + protocolType.Namespace + "." + protocolType.Name + " - " + e.ToString() );
			}
		}


		internal static void HandlePacketOnServer( int protocolCode, BinaryReader reader, int playerWho ) {
			var mymod = ModHelpersMod.Instance;
			bool isRequest, isSyncedToClients;

			Type protocolType = mymod.PacketProtocolMngr.GetProtocolType( protocolCode );
			if( protocolType == null ) {
				throw new ModHelpersException( "Invalid protocol (hash: " + protocolCode + ")" );
			}

			try {
				isRequest = reader.ReadBoolean();
				isSyncedToClients = reader.ReadBoolean();
			} catch( Exception e ) {
				throw new ModHelpersException( "Could not read data for protocol " + protocolType.Namespace+"."+protocolType.Name+" - "+e.ToString() );
			}
			
			try {
				var protocol = (PacketProtocol)StreamProtocol.CreateInstance( protocolType );
				//var protocol = (PacketProtocol)Activator.CreateInstance( protocolType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

				if( isRequest ) {
					protocol.ReceiveRequestWithServerBase( playerWho );
					protocol.OnClone();
				} else {
					if( protocol.IsAsync ) {
						Task.Run( () => {
							protocol.HandleServer_Core( reader, playerWho, isSyncedToClients );
						} );
					} else {
						protocol.HandleServer_Core( reader, playerWho, isSyncedToClients );
					}
				}
			} catch( Exception e ) {
				throw new ModHelpersException( "Error handling " + protocolType.Namespace + "." + protocolType.Name + " - " + e.ToString() );
			}
		}


		////////////////

		private void HandleClient_Core( BinaryReader reader, int playerWho ) {
			this.ReceiveWithClientBase( reader, playerWho );
			this.OnClone();
		}

		private void HandleServer_Core( BinaryReader reader, int playerWho, bool isSyncedToClients ) {
			this.ReceiveWithServerBase( reader, playerWho, isSyncedToClients );
			this.OnClone();

			if( isSyncedToClients ) {
				this.SendToClient( -1, playerWho );
			}
		}
	}
}
