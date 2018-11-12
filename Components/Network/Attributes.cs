using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Components.Network {
	/// <summary>
	/// Sets a field to be ignored by a protocol.
	/// </summary>
	public class PacketProtocolIgnoreAttribute : Attribute { }
	/// <summary>
	/// Sets a field to be not written by clients in a protocol.
	/// </summary>
	public class PacketProtocolWriteIgnoreClientAttribute : Attribute { }
	/// <summary>
	/// Sets a field to be not written by server in a protocol.
	/// </summary>
	public class PacketProtocolWriteIgnoreServerAttribute : Attribute { }



	[Obsolete( "use PacketProtocolWriteIgnoreServerAttribute", true )]
	public class PacketProtocolReadIgnoreClientAttribute : Attribute { }
	[Obsolete( "use PacketProtocolWriteIgnoreServerAttribute", true )]
	public class PacketProtocolReadIgnoreServerAttribute : Attribute { }
}
