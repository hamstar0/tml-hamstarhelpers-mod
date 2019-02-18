using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		[Obsolete( "use QuickRequestToServer(int, int, int)", true )]
		public static void QuickRequestToClient<T>( int toWho, int ignoreWho ) where T : PacketProtocol {  //, new()
			PacketProtocol.QuickRequestToClient<T>( toWho, ignoreWho, 0 );
		}
		
		[Obsolete( "use QuickRequestToServer(int)", true )]
		public static void QuickRequestToServer<T>() where T : PacketProtocol {  //, new()
			PacketProtocol.QuickRequestToServer<T>( 0 );
		}


		////////////////

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from a client.
		/// Requires `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <param name="toWho">Main.player index of player (client) being requested for this data. -1 for all clients.</param>
		/// <param name="ignoreWho">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickRequestToClient<T>( int toWho, int ignoreWho, int retries ) where T : PacketProtocol {
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			T t = (T)PacketProtocolData.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToClient( toWho, ignoreWho, retries );
		}


		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		public static void QuickRequestToServer<T>( int retries ) where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Not a client." );
			}
			
			T t = (T)PacketProtocolData.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

			t.SendRequestToServer( retries );
		}
	}
}
