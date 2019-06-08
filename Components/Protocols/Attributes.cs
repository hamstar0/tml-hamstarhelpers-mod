using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Components.Network {
	/// <summary>
	/// Sets a field to be ignored by a protocol.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public class ProtocolIgnoreAttribute : Attribute { }
	/// <summary>
	/// Sets a field to be not written by clients in a protocol.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public class ProtocolWriteIgnoreClientAttribute : Attribute { }
	/// <summary>
	/// Sets a field to be not written by server in a protocol.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public class ProtocolWriteIgnoreServerAttribute : Attribute { }
}
