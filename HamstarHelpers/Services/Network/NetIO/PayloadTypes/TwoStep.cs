using System;


namespace HamstarHelpers.Services.Network.NetIO.PayloadTypes {
	/// <summary>
	/// Represents broadcast packets (received on server and then all clients).
	/// </summary>
	public abstract class NetProtocolBroadcastPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveOnServerBeforeRebroadcast( int fromWho );

		/// <summary></summary>
		public abstract void ReceiveBroadcastOnClient();
	}




	////////////////

	/// @private
	public abstract class NetProtocolRequest : NetProtocolPayload {
	}




	////////////////

	/// <summary>
	/// Represents a request from a client to the server.
	/// </summary>
	public abstract class NetProtocolRequestClient<T> : NetProtocolRequest
				where T : NetProtocolServerPayload {
		/// <summary>
		/// Called before a request is replied to.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		public virtual void PreReply( T reply, int fromWho ) { }
	}



	/// <summary>
	/// Represents a request from the server to a client.
	/// </summary>
	public abstract class NetProtocolRequestServer<T> : NetProtocolRequest
				where T : NetProtocolClientPayload {
		/// <summary>
		/// Called before a request is replied to.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		public virtual void PreReply( T reply, int fromWho ) { }
	}



	/// <summary>
	/// Represents a request from either client or server of its opposite.
	/// </summary>
	public abstract class NetProtocolRequestBidirectional<T> : NetProtocolRequest
				where T : NetProtocolBidirectionalPayload {
		/// <summary>
		/// Called before a request is replied to on the server.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		public virtual void PreReplyOnServer( T reply, int fromWho ) { }

		/// <summary>
		/// Called before a request is replied to on the client.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		public virtual void PreReplyOnClient( T reply, int fromWho ) { }
	}
}
