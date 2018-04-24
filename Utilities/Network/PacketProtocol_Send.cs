using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
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
#pragma warning disable 612, 618
			t.SetDefaults();
#pragma warning restore 612, 618
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
#pragma warning disable 612, 618
			t.SetDefaults();
#pragma warning restore 612, 618
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
#pragma warning disable 612, 618
			t.SetDefaults();
#pragma warning restore 612, 618
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
			
			try {
				this.WriteStream( packet );
			} catch( Exception e ) {
				LogHelpers.Log( "PacketProtocol.SendToServer - " + e.ToString() );
				return;
			}

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

			try {
				this.WriteStream( packet );
			} catch( Exception e ) {
				LogHelpers.Log( "PacketProtocol.SendToServer - " + e.ToString() );
				return;
			}

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + name + " SendDataToClient " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}


		////////////////

		/// <summary>
		/// Implements low level data writing for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</returns>
		protected virtual void WriteStream( BinaryWriter writer ) {
			foreach( FieldInfo field in this.OrderedFields ) {
				object raw_val = field.GetValue( this );
				Type val_type = field.FieldType;
				//dynamic dyn_val = Convert.ChangeType( raw_val, val_type );

				switch( Type.GetTypeCode( val_type ) ) {
				case TypeCode.String:
					writer.Write( (String)raw_val );
					break;
				case TypeCode.Single:
					writer.Write( (Single)raw_val );
					break;
				case TypeCode.UInt64:
					writer.Write( (UInt64)raw_val );
					break;
				case TypeCode.Int64:
					writer.Write( (Int64)raw_val );
					break;
				case TypeCode.UInt32:
					writer.Write( (UInt32)raw_val );
					break;
				case TypeCode.Int32:
					writer.Write( (Int32)raw_val );
					break;
				case TypeCode.UInt16:
					writer.Write( (UInt16)raw_val );
					break;
				case TypeCode.Int16:
					writer.Write( (Int16)raw_val );
					break;
				case TypeCode.Double:
					writer.Write( (Double)raw_val );
					break;
				case TypeCode.Char:
					if( val_type.IsArray ) {
						var val = (Char[])raw_val;
						writer.Write( (Int32)val.Length );
						writer.Write( val );
					} else {
						writer.Write( (Char)raw_val );
					}
					break;
				case TypeCode.SByte:
					writer.Write( (SByte)raw_val );
					break;
				case TypeCode.Byte:
					if( val_type.IsArray ) {
						var val = (Byte[])raw_val;
						writer.Write( (Int32)val.Length );
						writer.Write( val );
					} else {
						writer.Write( (Byte)raw_val );
					}
					break;
				case TypeCode.Boolean:
					writer.Write( (Boolean)raw_val );
					break;
				case TypeCode.Decimal:
					writer.Write( (Decimal)raw_val );
					break;
				default:
					writer.Write( (String)JsonConvert.SerializeObject( raw_val ) );
					break;
				}
			}
		}
	}
}
