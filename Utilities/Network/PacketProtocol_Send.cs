using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		public static void QuickSendData<T>( int to_who, int ignore_who, bool server_rebroadcast )
				where T : PacketProtocol, new() {
			T t = new T();
			t.SetDefaults();
			if( Main.netMode == 1 ) {
				t.SetClientDefaults();
			} else if( Main.netMode == 2 ) {
				t.SetServerDefaults();
			}
			t.SendData( to_who, ignore_who, server_rebroadcast );
		}

		public static void QuickSendRequest<T>( int to_who, int ignore_who )
				where T : PacketProtocol, new() {
			T t = new T();
			t.SendRequest( to_who, ignore_who );
		}



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
		
		public void SendData( int to_who, int ignore_who, bool server_rebroadcast ) {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;
			ModPacket packet = mymod.GetPacket();

			packet.Write( name.GetHashCode() );
			packet.Write( false );  // Request
			packet.Write( server_rebroadcast );  // Broadcast
			this.WriteData( packet, this );

			packet.Send( to_who, ignore_who );

			if( mymod.Config.DebugModeNetInfo ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + name + " SendData " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}


		////////////////

		public virtual void WriteData( BinaryWriter writer, PacketProtocol me ) {
			string json_str = JsonConvert.SerializeObject( me );
			var data = Encoding.ASCII.GetBytes( json_str );

			writer.Write( (int)data.Length );
			writer.Write( data );
		}
	}
}
