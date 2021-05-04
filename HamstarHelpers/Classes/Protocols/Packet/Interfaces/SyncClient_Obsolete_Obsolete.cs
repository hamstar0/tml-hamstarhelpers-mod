using HamstarHelpers.Classes.Errors;
using System;
using Terraria;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// @private
	[Obsolete( "use `PacketProtocolSyncBetweenClients` or `PacketProtocolRequestToServer`", true )]
	public abstract partial class PacketProtocolSyncClient : PacketProtocol {
		/// @private
		[Obsolete( "use `SyncFromClientsToMe`", true)]
		protected static void SyncToMe<T>( int retries ) where T : PacketProtocolSyncClient {
			PacketProtocol.QuickRequestToServer<T>( retries );
		}

		/// @private
		[Obsolete( "use `SyncFromServerToClients`", true )]
		protected static void SyncFromServer<T>( int toWho, int ignoreWho ) where T : PacketProtocolSyncClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}
	}
}
