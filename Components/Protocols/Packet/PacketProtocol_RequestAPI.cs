using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Components.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from a client.
		/// Requires `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <param name="toWho">Main.player index of player (client) being requested for this data. -1 for all clients.</param>
		/// <param name="ignoreWho">Main.player index of player (client) being ignored. -1 for no client.</param>
		/// <param name="retries"></param>
		protected static void QuickRequestToClient<T>( int toWho, int ignoreWho, int retries ) where T : PacketProtocol {
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			T t = (T)StreamProtocol.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToClient( toWho, ignoreWho, retries );
		}


		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		protected static void QuickRequestToServer<T>( int retries ) where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Not a client." );
			}
			
			T t = (T)StreamProtocol.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToServer( retries );
		}
	}
}
