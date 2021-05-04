using System;


namespace HamstarHelpers.Services.Network.SimplePacket {
	/// <summary>
	/// Payload for use with `SimplePacket`.
	/// </summary>
	[Serializable]
	public abstract class SimplePacketPayload {
		/// <summary></summary>
		public abstract void ReceiveOnServer( int fromWho );

		/// <summary></summary>
		public abstract void ReceiveOnClient();
	}




	/// <summary>
	/// Indicates a given payload will produce lots of traffic, and should be logged sparringly.
	/// </summary>
	public class IsNoisyAttribute : Attribute { }
}
