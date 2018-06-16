using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol {
		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to the server. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSendToServer<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != 1 ) {
				throw new Exception( "Can only send as client." );
			}

			T t = new T();
			t.SetClientDefaults();

			t.SendToServer( false );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to everyone. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSyncToServerAndClients<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != 1 ) {
				throw new Exception( "Can only sync as a client." );
			}

			T t = new T();
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
				where T : PacketProtocol, new() {
			if( Main.netMode != 2 ) {
				throw new Exception( "Can only send as client." );
			}

			T t = new T();
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
				where T : PacketProtocol, new() {
			if( Main.netMode != 2 ) {
				throw new Exception( "Not server." );
			}

			T t = new T();
			t.SendRequestToClient( to_who, ignore_who );
		}

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		public static void QuickRequestToServer<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != 1 ) {
				throw new Exception( "Not a client." );
			}

			T t = new T();
			t.SendRequestToServer();
		}



		////////////////

		private void SendRequestToClient( int to_who, int ignore_who ) {
			var mymod = HamstarHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( true );

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToClient " + to_who + ", " + ignore_who );
			}
		}


		private void SendRequestToServer() {
			var mymod = HamstarHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( true, false );

			packet.Send( -1, -1 );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToServer" );
			}
		}


		////////////////

		protected void SendToServer( bool sync_to_clients ) {
			if( Main.netMode != 1 ) {
				throw new Exception("Not a client.");
			}

			var mymod = HamstarHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( false, sync_to_clients );
			
			try {
				this.WriteStream( packet );
			} catch( Exception e ) {
				LogHelpers.Log( "PacketProtocol.SendToServer - " + e.ToString() );
				return;
			}

			packet.Send( -1, -1 );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToServer: " + json_str );
			}
		}


		protected void SendToClient( int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) {
				throw new Exception( "Not server." );
			}

			var mymod = HamstarHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( false );

			try {
				this.WriteStream( packet );
			} catch( Exception e ) {
				LogHelpers.Log( "PacketProtocol.SendToClient - " + e.ToString() );
				return;
			}

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToClient " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}
	}
}
