using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to the server. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSendToServer<T>()
				where T : PacketProtocol {   //, new()
			if( Main.netMode != 1 ) {
				throw new Exception( "Can only send as client." );
			}

			//T t = new T();
			T t = (T)Activator.CreateInstance( typeof( T ), true );
			t.SetClientDefaults();

			t.SendToServer( false );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to everyone. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSyncToServerAndClients<T>()
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new Exception( "Can only sync as a client." );
			}

			//T t = new T();
			T t = (T)Activator.CreateInstance( typeof( T ), true );
			t.SetClientDefaults();

			t.SendToServer( true );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to a client. Requires `SetServerDefaults()`
		/// to be implemented.
		/// </summary>
		/// <param name="to_who">Main.player index of player (client) receiving this data. -1 for all clients.</param>
		/// <param name="ignore_who">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickSendToClient<T>( int to_who, int ignore_who )
				where T : PacketProtocol {	//, new()
			if( Main.netMode != 2 ) {
				throw new Exception( "Can only send as client." );
			}

			//T t = new T();
			T t = (T)Activator.CreateInstance( typeof( T ), true );
			t.SetServerDefaults();

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
				throw new Exception( "Not server." );
			}

			//T t = new T();
			T t = (T)Activator.CreateInstance( typeof( T ), true );

			t.SendRequestToClient( to_who, ignore_who );
		}

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		public static void QuickRequestToServer<T>()
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new Exception( "Not a client." );
			}

			//T t = new T();
			T t = (T)Activator.CreateInstance( typeof( T ), true );

			t.SendRequestToServer();
		}
	}
}
