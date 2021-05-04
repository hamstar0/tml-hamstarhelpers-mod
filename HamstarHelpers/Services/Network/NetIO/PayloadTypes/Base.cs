using System;


namespace HamstarHelpers.Services.Network.NetIO.PayloadTypes {
	/// @private
	[Serializable]
	public abstract class NetIOPayload { }




	/// <summary>
	/// Indicates a given payload will produce lots of traffic, and should be logged sparringly.
	/// </summary>
	public class IsNoisyAttribute : Attribute { }
}
