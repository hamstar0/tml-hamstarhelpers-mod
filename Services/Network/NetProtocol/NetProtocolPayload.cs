namespace HamstarHelpers.Services.Network.NetProtocol {
	/// @private
	public abstract class NetProtocolPayload { }



	/// <summary>
	/// Represents server packets. Be sure to add a Serializable attribute.
	/// </summary>
	public abstract class ServerNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveOnServer( int fromWho );
	}



	/// <summary>
	/// Represents client packets. Be sure to add a Serializable attribute.
	/// </summary>
	public abstract class ClientNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		public abstract void ReceiveOnClient( int fromWho );
	}



	/// <summary>
	/// Represents client packets. Be sure to add a Serializable attribute.
	/// </summary>
	public abstract class BidirectionalNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveOnServer( int fromWho );

		/// <summary></summary>
		public abstract void ReceiveOnClient( int fromWho );
	}



	////

	/// <summary>
	/// Represents broadcast packets (received on server and all clients). Be sure to add a Serializable attribute.
	/// </summary>
	public abstract class BroadcastNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveBroadcastOnServer( int fromWho );

		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveBroadcastOnClient( int fromWho );
	}
}
