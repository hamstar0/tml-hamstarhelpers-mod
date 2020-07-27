using System;


namespace HamstarHelpers.Classes.Protocols.Net {
	/// @private
	public interface NetProtocolPayload { }


	
	/// <summary>
	/// Represents broadcast packets (received on server and all clients). Be sure to add a Serializable attribute.
	/// </summary>
	public interface BroadcastNetProtocolPayload : NetProtocolPayload {
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
	public interface ServerNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		void ReceiveOnServer( int fromWho );
	}



	/// <summary>
	/// Represents client packets. Be sure to add a Serializable attribute.
	/// </summary>
	public interface ClientNetProtocolPayload : NetProtocolPayload {
		/// <summary></summary>
		void ReceiveOnClient();
	}
}
