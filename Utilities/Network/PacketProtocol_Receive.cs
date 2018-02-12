using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Utilities.Network {
	public abstract partial class PacketProtocol {
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
	}
}
