using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Components.Network {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		protected PacketProtocolSentToEither( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		protected override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}
		protected override void ReceiveWithServer( int from_who ) {
			this.ReceiveOnServer( from_who );
		}

		protected abstract void ReceiveOnClient();
		protected abstract void ReceiveOnServer( int from_who );
	}




	public abstract class PacketProtocolSendToServer : PacketProtocol {
		protected PacketProtocolSendToServer( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		public static void QuickSend<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}
		
		////////////////
		
		protected override void ReceiveWithServer( int from_who ) {
			this.Receive( from_who );
		}

		protected abstract void Receive( int from_who );
	}




	public abstract class PacketProtocolSendToClient : PacketProtocol {
		protected PacketProtocolSendToClient( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		public static void QuickSend<T>( int to_who, int ignore_who ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( to_who, ignore_who );
		}
		
		////////////////
		
		protected override void ReceiveWithClient() {
			this.Receive();
		}

		protected abstract void Receive();
	}



	
	public abstract class PacketProtocolRequestToServer : PacketProtocol {
		protected PacketProtocolRequestToServer( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		public static void QuickRequest<T>() where T : PacketProtocolRequestToServer {
			PacketProtocol.QuickRequestToServer<T>();
		}

		////////////////
		
		protected override void ReceiveWithClient() {
			this.ReceiveReply();
		}

		protected abstract void ReceiveReply();
	}


	

	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		protected PacketProtocolRequestToClient( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		public static void QuickRequest<T>( int to_who, int ignore_who ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( to_who, ignore_who );
		}

		////////////////
		
		protected override void ReceiveWithServer( int from_who ) {
			this.ReceiveReply( from_who );
		}

		protected abstract void ReceiveReply( int from_who );
	}
}
