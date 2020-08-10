using System;


namespace HamstarHelpers.Services.Network.NetIO.PayloadTypes {
	/// <summary>
	/// Represents broadcast packets (received on server and then all clients).
	/// </summary>
	[Serializable]
	public abstract class NetProtocolBroadcastPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		/// <returns>`true` permits automatic rebroadcasting to clients. Otherwise, `NetIO.SendToClient(...)` must be called
		/// manually for each active player to complete the "broadcast".</returns>
		public abstract bool ReceiveOnServerBeforeRebroadcast( int fromWho );

		/// <summary></summary>
		public abstract void ReceiveBroadcastOnClient();
	}




	////////////////

	/// @private
	[Serializable]
	public abstract class NetProtocolRequest : NetProtocolPayload {
	}




	////////////////

	/// <summary>
	/// Represents a request from a client to the server.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolRequest<T> : NetProtocolRequest
				where T : NetProtocolBidirectionalPayload {
		/// <summary>
		/// Called before a request is replied to on the client.
		/// </summary>
		/// <param name="reply"></param>
		/// <returns>`true` permits the reply to proceed.</returns>
		public virtual bool PreReplyOnClient( T reply ) {
			return true;
		}

		/// <summary>
		/// Called before a request is replied to on the server.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		/// <returns>`true` permits the reply to proceed.</returns>
		public virtual bool PreReplyOnServer( T reply, int fromWho ) {
			return true;
		}
	}



	/// <summary>
	/// Represents a request for data from a client to the server.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolRequestPayloadFromClient<T> : NetProtocolRequest
				where T : NetProtocolServerPayload {
		/// <summary>
		/// Called before a request is replied to.
		/// </summary>
		/// <param name="reply"></param>
		/// <returns>`true` permits the reply to proceed.</returns>
		public virtual bool PreReplyOnClient( T reply ) {
			return true;
		}
	}



	/// <summary>
	/// Represents a request for data from the server to a client.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolRequestPayloadFromServer<T> : NetProtocolRequest
				where T : NetProtocolClientPayload {
		/// <summary>
		/// Called before a request is replied to.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="fromWho"></param>
		/// <returns>`true` permits the reply to proceed.</returns>
		public virtual bool PreReplyOnServer( T reply, int fromWho ) {
			return true;
		}
	}
}
