using HamstarHelpers.Components.Network.Data;
using System;


namespace HamstarHelpers.Components.Network {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		protected PacketProtocolSentToEither( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////
		
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );
		}

		protected abstract void ReceiveOnClient();
		protected abstract void ReceiveOnServer( int fromWho );
	}




	public abstract class PacketProtocolSendToServer : PacketProtocol {
		protected PacketProtocolSendToServer( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		public static void QuickSend<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}

		////////////////
		
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.Receive( fromWho );
		}

		protected abstract void Receive( int fromWho );
	}




	public abstract class PacketProtocolSendToClient : PacketProtocol {
		protected PacketProtocolSendToClient( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		public static void QuickSend<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}

		////////////////
		
		protected sealed override void ReceiveWithClient() {
			this.Receive();
		}

		protected abstract void Receive();
	}



	
	public abstract class PacketProtocolRequestToServer : PacketProtocol {
		protected PacketProtocolRequestToServer( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		public static void QuickRequest<T>() where T : PacketProtocolRequestToServer {
			PacketProtocol.QuickRequestToServer<T>();
		}

		////////////////
		
		protected sealed override void ReceiveWithClient() {
			this.ReceiveReply();
		}

		protected abstract void ReceiveReply();
	}


	

	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		protected PacketProtocolRequestToClient( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////

		public static void QuickRequest<T>( int toWho, int ignoreWho ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( toWho, ignoreWho );
		}

		////////////////
		
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveReply( fromWho );
		}

		protected abstract void ReceiveReply( int fromWho );
	}
}
