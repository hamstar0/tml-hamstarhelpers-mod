using System;


namespace HamstarHelpers.Components.Network {
	public abstract class PacketProtocolSendToServer : PacketProtocol {
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
		public static void QuickRequest<T>() where T : PacketProtocolRequestToServer {
			PacketProtocol.QuickRequestToServer<T>();
		}

		////////////////

		protected override void ReceiveWithClient() {
			this.Receive();
		}

		protected abstract void Receive();
	}


	

	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		public static void QuickRequest<T>( int to_who, int ignore_who ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( to_who, ignore_who );
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			this.Receive( from_who );
		}

		protected abstract void Receive( int from_who );
	}
}
