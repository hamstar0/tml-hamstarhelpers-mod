using HamstarHelpers.Components.Network.Data;
using System;


namespace HamstarHelpers.Components.Network {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		protected PacketProtocolSentToEither( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

		[Obsolete( "use ReceiveOnClient", true )]
		protected override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}
		[Obsolete( "use ReceiveOnServer", true )]
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

		[Obsolete( "use Receive", true )]
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

		[Obsolete( "use Receive", true )]
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

		[Obsolete( "use ReceiveReply", true )]
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
		
		[Obsolete( "use ReceiveReply", true)]
		protected override void ReceiveWithServer( int from_who ) {
			this.ReceiveReply( from_who );
		}

		protected abstract void ReceiveReply( int from_who );
	}
}
