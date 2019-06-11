using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		protected static void QuickSendToAClient<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}

		protected static void QuickSendToTheServer<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}

		protected static void QuickSyncToTheServerAndClients<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSyncToServerAndClients<T>();
		}



		////////////////

		protected abstract void ReceiveOnClient();
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		protected abstract void ReceiveOnServer( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );
		}
	}
}
