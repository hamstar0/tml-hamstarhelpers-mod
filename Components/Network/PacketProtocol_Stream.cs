using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol {
		private ModPacket GetClientPacket( bool is_request, bool sync_to_all ) {
			if( Main.netMode != 1 ) { throw new Exception("Not a client"); }

			string name = this.GetPacketName();
			var packet = HamstarHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( is_request );
			packet.Write( sync_to_all );

			return packet;
		}


		private ModPacket GetServerPacket( bool is_request ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not a server" ); }

			string name = this.GetPacketName();
			var packet = HamstarHelpersMod.Instance.GetPacket();

			packet.Write( (int)PacketProtocol.GetPacketCode( name ) );
			packet.Write( is_request );

			return packet;
		}
		
		////////////////


		/// <summary>
		/// Implements internal low level data reading for packet receipt.
		/// </summary>
		/// <param name="reader">Binary data reader.</returns>
		protected virtual void ReadStream( BinaryReader reader ) {
//LogHelpers.Log( "RECEIVE PLZ!! packet: " + this.GetType().Name+", field: "+ string.Join(",",this.OrderedFields.Select(f=>f.Name).ToArray()) );
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
					string raw_json = reader.ReadString();
					var json_val = JsonConvert.DeserializeObject( raw_json, field_type );

					field.SetValue( this, json_val );
					break;
				}
			}
		}
		

		/// <summary>
		/// Implements low level data writing for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</returns>
		protected virtual void WriteStream( BinaryWriter writer ) {
			foreach( FieldInfo field in this.OrderedFields ) {
				object raw_val = field.GetValue( this );
				Type field_type = field.FieldType;
				//dynamic dyn_val = Convert.ChangeType( raw_val, val_type );

				switch( Type.GetTypeCode( field_type ) ) {
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
					if( field_type.IsArray ) {
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
					if( field_type.IsArray ) {
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
					string json_enc_val = JsonConvert.SerializeObject( raw_val );

					writer.Write( (String)json_enc_val );
					break;
				}
			}
		}
	}
}
