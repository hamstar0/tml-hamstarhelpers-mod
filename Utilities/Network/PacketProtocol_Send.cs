using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to the server. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSendToServer<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != -1 ) {
				throw new Exception( "Can only send as client." );
			}

			T t = new T();
			t.SetDefaults();
			t.SetClientDefaults();

			t.SendToServer( false );
		}

		/// <summary>
		/// Shorthand to send a default instance of this protocol's data to all other clients. Requires `SetClientDefaults()`
		/// to be implemented.
		/// </summary>
		public static void QuickSyncToOtherClients<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != 1 ) {
				throw new Exception( "Can only sync as a client." );
			}

			T t = new T();
			t.SetDefaults();
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
			t.SetDefaults();
			t.SetServerDefaults();

			t.SendToClient( to_who, ignore_who );
		}

		////////////////

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from the server.
		/// Requires `SetServerDefaults()` to be implemented.
		/// </summary>
		public static void QuickRequestFromServer<T>()
				where T : PacketProtocol, new() {
			if( Main.netMode != 1 ) {
				throw new Exception( "Not a client." );
			}

			T t = new T();
			t.SendRequestOnly( -1, -1 );
		}

		/// <summary>
		/// Shorthand to send a request for a default instance of this protocol's data from a client.
		/// Requires `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <param name="to_who">Main.player index of player (client) being requested for this data. -1 for all clients.</param>
		/// <param name="ignore_who">Main.player index of player (client) being ignored. -1 for no client.</param>
		public static void QuickRequestFromClient<T>( int to_who, int ignore_who )
				where T : PacketProtocol, new() {
			if( Main.netMode != 2 ) {
				throw new Exception( "Not server." );
			}

			T t = new T();
			t.SendRequestOnly( to_who, ignore_who );
		}



		////////////////

		private void SendRequestOnly( int to_who, int ignore_who ) {
			if( Main.netMode == 0 ) {
				throw new Exception( "Cannot send packets in single player." );
			}

			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();

			packet.Write( PacketProtocol.GetPacketCode( name ) );
			packet.Write( true );   // Request
			packet.Write( false );  // Broadcast

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + name + " SendRequest " + to_who + ", " + ignore_who );
			}
		}
		
		protected void SendToServer( bool sync_to_clients ) {
			if( Main.netMode != 1 ) {
				throw new Exception("Not a client.");
			}

			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();

			packet.Write( PacketProtocol.GetPacketCode( name ) );
			packet.Write( false );  // Request
			packet.Write( sync_to_clients );  // Broadcast
			this.WriteData( packet );

			packet.Send( -1, -1 );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + name + " SendDataToServer: " + json_str );
			}
		}

		protected void SendToClient( int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) {
				throw new Exception( "Not server." );
			}

			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();

			packet.Write( PacketProtocol.GetPacketCode( name ) );
			packet.Write( false );  // Request
			this.WriteData( packet );

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + name + " SendDataToClient " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}


		////////////////

		/// <summary>
		/// Manually implements writing our protocol's binary data. Defaults to serializing a
		/// single string of JSON data to the given binary stream.
		/// </summary>
		/// <param name="writer">Given writable stream of binary data. Protocol must be handled manually.</param>
		public virtual void WriteData( BinaryWriter writer ) {
			string json_str = JsonConvert.SerializeObject( this );
			var data = Encoding.ASCII.GetBytes( json_str );

			writer.Write( (int)data.Length );
			writer.Write( data );
		}
	}
}
