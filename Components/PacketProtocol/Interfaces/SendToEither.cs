using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.PacketProtocol.Interfaces {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		public static void QuickSendToAClient<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}

		public static void QuickSendToTheServer<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}

		public static void QuickSyncToTheServerAndClients<T>() where T : PacketProtocolSendToServer {
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
