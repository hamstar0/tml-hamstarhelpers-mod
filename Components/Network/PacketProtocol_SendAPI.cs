using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		private static void QuickSendToServerBase<T>( bool syncToClients )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Can only send as client." );
			}
			
			T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			t.SetClientDefaults();
			t.OnInitialize();

			t.SendToServer( syncToClients );
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
		/// <param name="toWho">Main.player index of player (client) receiving this data. -1 for all clients.</param>
		/// <param name="ignoreWho">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickSendToClient<T>( int toWho, int ignoreWho )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Can only send as client." );
			}
			
			T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			try {
				t.SetServerDefaults( toWho );
			} catch( NotImplementedException ) {
				t.SetServerDefaults();
			}
			t.OnInitialize();

			t.SendToClient( toWho, ignoreWho );
		}

		////////////////

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from a client.
		/// Requires `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <param name="toWho">Main.player index of player (client) being requested for this data. -1 for all clients.</param>
		/// <param name="ignoreWho">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickRequestToClient<T>( int toWho, int ignoreWho )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToClient( toWho, ignoreWho );
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
			
			T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToServer();
		}
	}
}
