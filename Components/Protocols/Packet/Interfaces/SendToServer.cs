using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	public abstract class PacketProtocolSendToServer : PacketProtocol {
		protected static void QuickSend<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}



		////////////////

		protected abstract void InitializeClientSendData();
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}
		
		protected abstract void Receive( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.Receive( fromWho );
		}


		////////////////

		protected sealed override void SetServerDefaults( int toWho ) {
			throw new HamstarException( "Not implemented" );
		}
		protected sealed override void ReceiveWithClient() {
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
