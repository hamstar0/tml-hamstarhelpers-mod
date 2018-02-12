using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Terraria;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
		internal void Receive( BinaryReader reader, int from_who ) {
			var mymod = HamstarHelpersMod.Instance;
			Type my_type = this.GetType();
			string name = my_type.Name;

			PacketProtocol data_obj = this.ReadData( reader );
			//Type your_type = data_obj.GetType();
			
			if( mymod.Config.DebugModeNetInfo ) {
				string json_str = JsonConvert.SerializeObject( data_obj );
				LogHelpers.Log( "<" + name + " Receive: " + json_str );
			}

			foreach( FieldInfo mine_field in my_type.GetFields() ) {
				//FieldInfo yours_field = your_type.GetField( mine_field.Name );
				FieldInfo yours_field = my_type.GetField( mine_field.Name );

				if( yours_field == null ) {
					LogHelpers.Log( "Missing " + name + " protocol value for " + mine_field.Name );
					continue;
				}

				object val = yours_field.GetValue( data_obj );

				mine_field.SetValue( this, val );

				//if( mymod.Config.DebugModeNetInfo ) {
				//	LogHelpers.Log( "  " + yours_field.Name + ": " + val );
				//}
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
			if( Main.netMode == 1 ) {
				this.SetClientDefaults();
			} else if( Main.netMode == 2 ) {
				this.SetServerDefaults();
			}

			bool skip_send = false;

			if( Main.netMode == 1 ) {
				skip_send = this.ReceiveRequestOnClient();
			} else {
				skip_send = this.ReceiveRequestOnServer( from_who );
			}

			if( !skip_send ) {
				this.SendData( from_who, -1, false );
			}
		}


		////////////////

		public virtual void ReceiveOnClient() {
			throw new NotImplementedException();
		}
		public virtual void ReceiveOnServer( int from_who ) {
			throw new NotImplementedException();
		}


		public virtual bool ReceiveRequestOnClient() {
			return false;
		}
		public virtual bool ReceiveRequestOnServer( int from_who ) {
			return false;
		}


		////////////////

		public virtual PacketProtocol ReadData( BinaryReader reader ) {
			int num = reader.ReadInt32();
			byte[] data = reader.ReadBytes( num );

			Type my_type = this.GetType();
			string json_str = Encoding.UTF8.GetString( data );

			return (PacketProtocol)JsonConvert.DeserializeObject( json_str, my_type );
		}
	}
}
