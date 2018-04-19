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
			
			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
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

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
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
				if( Main.netMode == 1 ) {
					this.SendToServer( false );
				} else {
					this.SendToClient( from_who, -1 );
				}
			}
		}


		////////////////

		/// <summary>
		/// Runs when data received on client (class's own fields).
		/// </summary>
		public virtual void ReceiveOnClient() {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Runs when data received on server (class's own fields).
		/// </summary>
		/// <param name="from_who">Main.player index of the player (client) sending us our data.</param>
		public virtual void ReceiveOnServer( int from_who ) {
			throw new NotImplementedException();
		}


		/// <summary>
		/// Runs when a request is received for the client to send data to the server. Expects
		/// `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <returns>True to indicate the request is being handled manually.</returns>
		public virtual bool ReceiveRequestOnClient() {
			return false;
		}
		/// <summary>
		/// Runs when a request is received for the server to send data to the client. Expects
		/// `SetServerDefaults()` to be implemented.
		/// to be implemented to provide this data.
		/// </summary>
		/// <param name="from_who">Main.player index of player (client) sending this request.</param>
		/// <returns>True to indicate the request is being handled manually.</returns>
		public virtual bool ReceiveRequestOnServer( int from_who ) {
			return false;
		}


		////////////////

		/// <summary>
		/// Manually implements reading our protocol's binary data. Defaults to deserializing a
		/// single string of JSON data into a new instance of the current class (no
		/// SetClientDefaults or SetServerDefaults invoked).
		/// </summary>
		/// <param name="reader">Given readable stream of binary data. Protocol must be handled manually.</param>
		/// <returns>A new PacketProtocol instance.</returns>
		public virtual PacketProtocol ReadData( BinaryReader reader ) {
			int num = reader.ReadInt32();
			byte[] data = reader.ReadBytes( num );

			Type my_type = this.GetType();
			string json_str = Encoding.UTF8.GetString( data );

			return (PacketProtocol)JsonConvert.DeserializeObject( json_str, my_type );
		}
	}
}
