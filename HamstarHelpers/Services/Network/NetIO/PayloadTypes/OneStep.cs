using System;


namespace HamstarHelpers.Services.Network.NetIO.PayloadTypes {
	/// <summary>
	/// Represents packets meant for receipt on server.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolServerPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveOnServer( int fromWho );
	}



	/// <summary>
	/// Represents packets meant for receipt on client.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolClientPayload : NetProtocolPayload {
		/// <summary></summary>
		public abstract void ReceiveOnClient();
	}



	/// <summary>
	/// Represents packets meant for receipt on server or client.
	/// </summary>
	[Serializable]
	public abstract class NetProtocolBidirectionalPayload : NetProtocolPayload {
		/// <summary></summary>
		/// <param name="fromWho"></param>
		public abstract void ReceiveOnServer( int fromWho );

		/// <summary></summary>
		public abstract void ReceiveOnClient();
	}
}
