using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending requests to clients.
	/// </summary>
	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		protected static void QuickRequest<T>( int toWho, int ignoreWho, int retries ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( toWho, ignoreWho, retries );
		}



		////////////////
		
		protected abstract void InitializeClientSendData();
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}

		protected abstract void ReceiveReply( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveReply( fromWho );
		}


		////////////////

		protected sealed override void SetServerDefaults( int toWho ) {
			throw new HamstarException( "Not implemented" );
		}
		protected sealed override void ReceiveWithClient() {
			throw new HamstarException( "Not implemented" );
		}
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new HamstarException( "Not implemented" );
		}
	}
}
