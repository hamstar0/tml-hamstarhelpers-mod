using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	public abstract class PacketProtocolRequestToServer : PacketProtocol {
		public static void QuickRequest<T>( int retries ) where T : PacketProtocolRequestToServer {
			PacketProtocolRequestToServer.QuickRequestToServer<T>( retries );
		}



		////////////////

		protected abstract void InitializeServerSendData( int toWho );
		protected sealed override void SetServerDefaults( int toWho ) {
			this.InitializeServerSendData( toWho );
		}

		protected abstract void ReceiveReply();
		protected sealed override void ReceiveWithClient() {
			this.ReceiveReply();
		}


		////////////////

		protected sealed override void SetClientDefaults() {
			throw new HamstarException( "Not implemented" );
		}
		protected sealed override void ReceiveWithServer( int fromWho ) {
			throw new HamstarException( "Not implemented" );
		}
		protected sealed override bool ReceiveRequestWithClient() {
			throw new HamstarException( "Not implemented" );
		}
	}
}
