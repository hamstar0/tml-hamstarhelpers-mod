using HamstarHelpers.Components.Network.Data;
using System;


namespace HamstarHelpers.Components.Network {
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		protected abstract void ReceiveOnClient();
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		protected abstract void ReceiveOnServer( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );
		}

		////////////////
		
		protected PacketProtocolSentToEither( PacketProtocolDataConstructorLock _=null ) { }
	}




	public abstract class PacketProtocolSendToServer : PacketProtocol {
		public static void QuickSend<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}

		////////////////
		
		protected PacketProtocolSendToServer( PacketProtocolDataConstructorLock _=null ) { }

		////////////////

		protected abstract void InitializeClientSendData();
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}
		
		protected abstract void Receive( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.Receive( fromWho );
		}

		////

		protected sealed override void SetServerDefaults( int toWho ) {
			throw new NotImplementedException();
		}
		protected sealed override void ReceiveWithClient() {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithClient() {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new NotImplementedException();
		}
	}




	public abstract class PacketProtocolSendToClient : PacketProtocol {
		public static void QuickSend<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}

		////////////////
		
		protected PacketProtocolSendToClient( PacketProtocolDataConstructorLock _=null ) { }

		////////////////

		protected abstract void InitializeServerSendData( int toWho );
		protected sealed override void SetServerDefaults( int toWho ) {
			this.InitializeServerSendData( toWho );
		}

		protected abstract void Receive();
		protected sealed override void ReceiveWithClient() {
			this.Receive();
		}

		////

		protected sealed override void SetClientDefaults() {
			throw new NotImplementedException();
		}
		protected sealed override void ReceiveWithServer( int fromWho ) {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithClient() {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new NotImplementedException();
		}
	}



	
	public abstract class PacketProtocolRequestToServer : PacketProtocol {
		public static void QuickRequest<T>() where T : PacketProtocolRequestToServer {
			PacketProtocol.QuickRequestToServer<T>();
		}

		////////////////
		
		protected PacketProtocolRequestToServer( PacketProtocolDataConstructorLock _=null ) { }

		////////////////

		protected abstract void InitializeServerSendData( int toWho );
		protected sealed override void SetServerDefaults( int toWho ) {
			this.InitializeServerSendData( toWho );
		}

		protected abstract void ReceiveReply();
		protected sealed override void ReceiveWithClient() {
			this.ReceiveReply();
		}

		////
		
		protected sealed override void SetClientDefaults() {
			throw new NotImplementedException();
		}
		protected sealed override void ReceiveWithServer( int fromWho ) {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithClient() {
			throw new NotImplementedException();
		}
	}


	

	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		public static void QuickRequest<T>( int toWho, int ignoreWho ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( toWho, ignoreWho );
		}

		////////////////
		
		protected PacketProtocolRequestToClient( PacketProtocolDataConstructorLock _=null ) { }

		////

		protected abstract void InitializeClientSendData();
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}

		protected abstract void ReceiveReply( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveReply( fromWho );
		}

		////

		protected sealed override void SetServerDefaults( int toWho ) {
			throw new NotImplementedException();
		}
		protected sealed override void ReceiveWithClient() {
			throw new NotImplementedException();
		}
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new NotImplementedException();
		}
	}
}
