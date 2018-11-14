using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		private static void QuickSendToServerBase<T>( bool sync_to_clients )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Can only send as client." );
			}

			T t = PacketProtocolData.Create<T>();
			t.SetClientDefaults();

			t.SendToServer( sync_to_clients );
		}


		////////////////
		
		
		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to the server. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSendToServer<T>()
				where T : PacketProtocol {  //, new()
			PacketProtocol.QuickSendToServerBase<T>( false );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to everyone. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSyncToServerAndClients<T>()
				where T : PacketProtocol {  //, new()
			PacketProtocol.QuickSendToServerBase<T>( true );
		}


		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to a client. Requires `SetServerDefaults()`
		/// to be implemented.
		/// </summary>
		/// <param name="to_who">Main.player index of player (client) receiving this data. -1 for all clients.</param>
		/// <param name="ignore_who">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickSendToClient<T>( int to_who, int ignore_who )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Can only send as client." );
			}

			T t = PacketProtocolData.Create<T>();
			try {
				t.SetServerDefaults( to_who );
			} catch( NotImplementedException ) {
				t.SetServerDefaults();
			}

			t.SendToClient( to_who, ignore_who );
		}

		////////////////

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from a client.
		/// Requires `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <param name="to_who">Main.player index of player (client) being requested for this data. -1 for all clients.</param>
		/// <param name="ignore_who">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickRequestToClient<T>( int to_who, int ignore_who )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			T t = PacketProtocolData.Create<T>();

			t.SendRequestToClient( to_who, ignore_who );
		}


		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		public static void QuickRequestToServer<T>()
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Not a client." );
			}

			T t = PacketProtocolData.Create<T>();

			t.SendRequestToServer();
		}
	}
}
