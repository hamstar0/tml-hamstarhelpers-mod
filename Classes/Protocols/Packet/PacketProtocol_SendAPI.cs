using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Components.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		private static void QuickSendToServerBase<T>( bool syncToClients )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 1 ) {
				throw new ModHelpersException( "Can only send as client." );
			}
			
			T t = (T)StreamProtocol.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			t.SetClientDefaults();
			t.OnClone();

			t.SendToServer( syncToClients );
		}


		////////////////


		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to the server. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		protected static void QuickSendToServer<T>()
				where T : PacketProtocol {  //, new()
			PacketProtocol.QuickSendToServerBase<T>( false );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to everyone. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		protected static void QuickSyncToServerAndClients<T>()
				where T : PacketProtocol {  //, new()
			PacketProtocol.QuickSendToServerBase<T>( true );
		}


		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to a client. Requires `SetServerDefaults()`
		/// to be implemented.
		/// </summary>
		/// <param name="toWho">Main.player index of player (client) receiving this data. -1 for all clients.</param>
		/// <param name="ignoreWho">Main.player index of player (client) being ignored. -1 for no client.</param>
		protected static void QuickSendToClient<T>( int toWho, int ignoreWho )
				where T : PacketProtocol {  //, new()
			if( Main.netMode != 2 ) {
				throw new ModHelpersException( "Can only send as client." );
			}
			
			T t = (T)StreamProtocol.CreateInstance( typeof(T) );
			//T t = (T)Activator.CreateInstance( typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			
			t.SetServerDefaults( toWho );
			t.OnClone();

			t.SendToClient( toWho, ignoreWho );
		}
	}
}
