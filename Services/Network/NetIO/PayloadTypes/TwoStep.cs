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
		/// <param name="fromWho"></param>
		public abstract void ReceiveBroadcastOnClient( int fromWho );
	}




	////////////////

	/// @private
	public abstract class NetProtocolRequestPayload : NetProtocolPayload {
	}




	////////////////

	/// <summary>
	/// Represents a request from a client to the server.
	/// </summary>
	public abstract class NetProtocolRequestClientPayload<T> : NetProtocolRequestPayload
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
	public abstract class NetProtocolRequestServerPayload<T> : NetProtocolRequestPayload
				where T : NetProtocolClientPayload {
		/// <summary>
		/// Called before a request is replied to.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		public virtual void PreReply( T reply, int fromWho ) { }
	}
}
