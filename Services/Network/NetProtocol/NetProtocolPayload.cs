using System;


namespace HamstarHelpers.Services.Network.NetProtocol {
	/// @private
	public interface INetProtocolPayload { }


	
	/// <summary>
	/// Represents broadcast packets (received on server and all clients). Be sure to add a Serializable attribute.
	/// </summary>
	public interface IBroadcastNetProtocolPayload : INetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		void ReceiveBroadcastOnServer( int fromWho );
		/// <summary></summary>
		/// <param name="fromWho"></param>
		void ReceiveBroadcastOnClient( int fromWho );
	}



	/// <summary>
	/// Represents server packets. Be sure to add a Serializable attribute.
	/// </summary>
	public interface IServerNetProtocolPayload : INetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		void ReceiveOnServer( int fromWho );
	}



	/// <summary>
	/// Represents client packets. Be sure to add a Serializable attribute.
	/// </summary>
	public interface IClientNetProtocolPayload : INetProtocolPayload {
		/// <summary></summary>
		void ReceiveOnClient();
	}
}
