using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Network {
	public abstract class PacketProtocol {
		protected static readonly object MyLock = new object();


		internal static IDictionary<int, Type> GetProtocols() {
			IDictionary<int, Type> protocols = new Dictionary<int, Type>();
			
			var subclasses = from assembly in AppDomain.CurrentDomain.GetAssemblies()
							 from type in assembly.GetTypes()
							 where type.IsSubclassOf( typeof( PacketProtocol ) ) && !type.IsAbstract
							 select type;

			foreach( Type subclass in subclasses ) {
				try {
					string name = subclass.Name;
					protocols[ name.GetHashCode() ] = subclass;
				} catch( Exception e ) {
					LogHelpers.Log( subclass.Name + " - " + e.Message );
				}
			}

			return protocols;
		}


		internal static void HandlePacket( BinaryReader reader, int player_who ) {
			var mymod = HamstarHelpersMod.Instance;

			int protocol_hash = reader.ReadInt32();
			bool is_request = reader.ReadBoolean();
			bool is_broadcast = reader.ReadBoolean();

			if( mymod.PacketProtocols.ContainsKey( protocol_hash ) ) {
				Type protocol_type = mymod.PacketProtocols[ protocol_hash ];
				PacketProtocol protocol = (PacketProtocol)Activator.CreateInstance( protocol_type );

				if( is_request ) {
					protocol.ReceiveRequest( player_who );
				} else {
					protocol.Receive( reader, player_who );

					if( is_broadcast && Main.netMode == 2 ) {
						protocol.SendData( -1, player_who );
					}
				}
			}
		}


		////////////////

		public static void QuickSendData<T>( int to_who, int ignore_who )
				where T : PacketProtocol, new() {
			T t = new T();
			t.SetDefaults();
			t.SendRequest( to_who, ignore_who );
		}

		public static void QuickSendRequest<T>( int to_who, int ignore_who )
				where T : PacketProtocol, new() {
			T t = new T();
			t.SendRequest( to_who, ignore_who );
		}



		////////////////

		public virtual void SetDefaults() { }
		

		////////////////

		public void SendRequest( int to_who, int ignore_who ) {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();

			packet.Write( name.GetHashCode() );
			packet.Write( true );   // Request
			packet.Write( false );  // Broadcast

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( ">" + name + " SendRequest " + to_who + ", " + ignore_who );
			}
		}
		
		public void SendData( int to_who, int ignore_who ) {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();
			string json_str = (string)JsonConvert.SerializeObject( this );

			packet.Write( name.GetHashCode() );
			packet.Write( false );  // Request
			packet.Write( false );	// Broadcast
			packet.Write( json_str );

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( ">" + name + " SendData " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}

		public void BroadcastData() {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();
			string json_str = (string)JsonConvert.SerializeObject( this );

			packet.Write( name.GetHashCode() );
			packet.Write( false );	// Request
			packet.Write( true );	// Broadcast
			packet.Write( json_str );

			packet.Send( -1, -1 );

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( ">" + name + " BroadcastData: " + json_str );
			}
		}


		////////////////
		
		internal void Receive( BinaryReader reader, int from_who ) {
			var mymod = HamstarHelpersMod.Instance;
			Type my_type = this.GetType();
			string name = my_type.Name;

			string json_str = reader.ReadString();
			var json_obj = JsonConvert.DeserializeObject( json_str, my_type );
			
			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( "<" + name + " Receive: " + json_str );
			}

			Type your_type = json_obj.GetType();

			foreach( FieldInfo mine_field in my_type.GetFields() ) {
				FieldInfo yours_field = your_type.GetField( mine_field.Name );

				if( yours_field == null ) {
					LogHelpers.Log( "Missing " + name + " protocol value for " + mine_field.Name );
					continue;
				}

				object val = yours_field.GetValue( json_obj );

				mine_field.SetValue( this, val );

				if( mymod.Config.DebugModeNetInfo ) {
					LogHelpers.Log( "  " + yours_field.Name + ": " + val );
				}
			}

			if( Main.netMode == 1 ) {
				this.ReceiveOnClient();
			} else {
				this.ReceiveOnServer( from_who );
			}
		}

		internal void ReceiveRequest( int from_who ) {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( "<" + name + " ReceiveRequest..." );
			}

			this.SetDefaults();
			this.SendData( from_who, -1 );

			if( Main.netMode == 1 ) {
				this.ReceiveRequestOnClient();
			} else {
				this.ReceiveRequestOnServer( from_who );
			}
		}

		////////////////

		public virtual void ReceiveOnClient() { }
		public virtual void ReceiveOnServer( int from_who ) { }

		public virtual void ReceiveRequestOnClient() { }
		public virtual void ReceiveRequestOnServer( int from_who ) { }
	}
}
