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

			try {
				this.ReadStream( reader );
			} catch( Exception e ) {
				LogHelpers.Log( "PacketProtocol.Receive - "+e.ToString() );
				return;
			}
			
			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( "<" + name + " Receive: " + json_str );
			}

			foreach( FieldInfo my_field in my_type.GetFields() ) {
				FieldInfo your_field = my_type.GetField( my_field.Name );

				if( your_field == null ) {
					LogHelpers.Log( "Missing " + name + " protocol value for " + my_field.Name );
					continue;
				}

				object val = your_field.GetValue( this );

				my_field.SetValue( this, val );
			}
			
			if( Main.netMode == 1 ) {
				try {
#pragma warning disable 612, 618
					this.ReceiveOnClient();
#pragma warning restore 612, 618
				} catch( NotImplementedException ) {
					this.ReceiveWithClient();
				}
			} else {
				try {
#pragma warning disable 612, 618
					this.ReceiveOnServer( from_who );
#pragma warning restore 612, 618
				} catch( NotImplementedException ) {
					this.ReceiveWithServer( from_who );
				}
			}
		}


		internal void ReceiveRequest( int from_who ) {
			var mymod = HamstarHelpersMod.Instance;
			string name = this.GetType().Name;

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( "<" + name + " ReceiveRequest..." );
			}

#pragma warning disable 612, 618
			this.SetDefaults();
#pragma warning restore 612, 618
			if( Main.netMode == 1 ) {
				this.SetClientDefaults();
			} else if( Main.netMode == 2 ) {
				this.SetServerDefaults();
			}

			bool skip_send = false;

			if( Main.netMode == 1 ) {
				try {
#pragma warning disable 612, 618
					skip_send = this.ReceiveRequestOnClient();
#pragma warning restore 612, 618
				} catch( NotImplementedException ) {
					skip_send = this.ReceiveRequestWithClient();
				}
			} else {
				try {
#pragma warning disable 612, 618
					skip_send = this.ReceiveRequestOnServer( from_who );
#pragma warning restore 612, 618
				} catch( NotImplementedException ) {
					skip_send = this.ReceiveRequestWithServer( from_who );
				}
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
		protected virtual void ReceiveWithClient() {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Runs when data received on server (class's own fields).
		/// </summary>
		/// <param name="from_who">Main.player index of the player (client) sending us our data.</param>
		protected virtual void ReceiveWithServer( int from_who ) {
			throw new NotImplementedException();
		}


		/// <summary>
		/// Runs when a request is received for the client to send data to the server. Expects
		/// `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithClient() {
			return false;
		}
		/// <summary>
		/// Runs when a request is received for the server to send data to the client. Expects
		/// `SetServerDefaults()` to be implemented.
		/// </summary>
		/// <param name="from_who">Main.player index of player (client) sending this request.</param>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithServer( int from_who ) {
			return false;
		}


		////////////////

		/// <summary>
		/// Implements internal low level data reading for packet receipt.
		/// </summary>
		/// <param name="reader">Binary data reader.</returns>
		protected virtual void ReadStream( BinaryReader reader ) {
			foreach( FieldInfo field in this.OrderedFields ) {
				Type field_type = field.FieldType;

				switch( Type.GetTypeCode( field_type ) ) {
				case TypeCode.String:
					field.SetValue( this, reader.ReadString() );
					break;
				case TypeCode.Single:
					field.SetValue( this, reader.ReadSingle() );
					break;
				case TypeCode.UInt64:
					field.SetValue( this, reader.ReadUInt64() );
					break;
				case TypeCode.Int64:
					field.SetValue( this, reader.ReadInt64() );
					break;
				case TypeCode.UInt32:
					field.SetValue( this, reader.ReadUInt32() );
					break;
				case TypeCode.Int32:
					field.SetValue( this, reader.ReadInt32() );
					break;
				case TypeCode.UInt16:
					field.SetValue( this, reader.ReadUInt16() );
					break;
				case TypeCode.Int16:
					field.SetValue( this, reader.ReadInt16() );
					break;
				case TypeCode.Double:
					field.SetValue( this, reader.ReadDouble() );
					break;
				case TypeCode.Char:
					if( field_type.IsArray ) {
						int count = reader.ReadInt32();
						field.SetValue( this, reader.ReadChars(count) );
					} else {
						field.SetValue( this, reader.ReadChar() );
					}
					break;
				case TypeCode.SByte:
					field.SetValue( this, reader.ReadSByte() );
					break;
				case TypeCode.Byte:
					if( field_type.IsArray ) {
						int count = reader.ReadInt32();
						field.SetValue( this, reader.ReadBytes( count ) );
					} else {
						field.SetValue( this, reader.ReadByte() );
					}
					break;
				case TypeCode.Boolean:
					field.SetValue( this, reader.ReadBoolean() );
					break;
				case TypeCode.Decimal:
					field.SetValue( this, reader.ReadDecimal() );
					break;
				default:
					var json_val = JsonConvert.DeserializeObject( reader.ReadString(), field_type );
					field.SetValue( this, json_val );
					break;
				}
			}
		}
	}
}
