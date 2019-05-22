using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.PacketProtocol.Interfaces {
	public abstract class PacketProtocolSendToClient : PacketProtocol {
		public static void QuickSend<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}



		////////////////

		protected abstract void InitializeServerSendData( int toWho );
		protected sealed override void SetServerDefaults( int toWho ) {
			this.InitializeServerSendData( toWho );
		}

		protected abstract void Receive();
		protected sealed override void ReceiveWithClient() {
			this.Receive();
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
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new HamstarException( "Not implemented" );
		}
	}
}
