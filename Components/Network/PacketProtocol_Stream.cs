using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
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
			PacketProtocol.ReadStreamIntoContainer( reader, this );
		}

		private static void ReadStreamIntoContainer( BinaryReader reader, PacketProtocolData data ) {
			foreach( FieldInfo field in data.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof(JsonIgnoreAttribute) ) ) {
					continue;
				}

				var data_type = field.FieldType;

				if( typeof( PacketProtocolData ).IsAssignableFrom( data_type ) ) {
					PacketProtocol.ReadStreamIntoContainer( reader, (PacketProtocolData)field.GetValue( data ) );
				} else {
					var field_data = PacketProtocol.ReadStreamValue( reader, data_type );
					field.SetValue( data, field_data );
				}
			}
		}

		private static object ReadStreamValue( BinaryReader reader, Type data_type ) {
			switch( Type.GetTypeCode( data_type ) ) {
			case TypeCode.String:
				return reader.ReadString();
			case TypeCode.Single:
				return reader.ReadSingle();
			case TypeCode.UInt64:
				return reader.ReadUInt64();
			case TypeCode.Int64:
				return reader.ReadInt64();
			case TypeCode.UInt32:
				return reader.ReadUInt32();
			case TypeCode.Int32:
				return reader.ReadInt32();
			case TypeCode.UInt16:
				return reader.ReadUInt16();
			case TypeCode.Int16:
				return reader.ReadInt16();
			case TypeCode.Double:
				return reader.ReadDouble();
			case TypeCode.Char:
				if( data_type.IsArray ) {
					int count = reader.ReadInt32();
					return reader.ReadChars( count );
				} else {
					return reader.ReadChar();
				}
			case TypeCode.SByte:
				return reader.ReadSByte();
			case TypeCode.Byte:
				if( data_type.IsArray ) {
					int count = reader.ReadInt32();
					return reader.ReadBytes( count );
				} else {
					return reader.ReadByte();
				}
			case TypeCode.Boolean:
				return reader.ReadBoolean();
			case TypeCode.Decimal:
				return reader.ReadDecimal();
			default:
				if( typeof( ICollection ).IsAssignableFrom( data_type ) ) {
					Type inner_type = data_type.GetGenericArguments().Single();
					ushort length = reader.ReadUInt16();

					Array arr = Array.CreateInstance( inner_type, length );

					for( int i = 0; i < length; i++ ) {
						object item = PacketProtocol.ReadStreamValue( reader, inner_type );
						arr.SetValue( item, i );
					}

					return Activator.CreateInstance( data_type, arr );
				} else {
					string raw_json = reader.ReadString();
					var json_val = JsonConvert.DeserializeObject( raw_json, data_type );
					return json_val;
				}
			}
		}


		////////////////

		/// <summary>
		/// Implements low level stream output for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</returns>
		protected virtual void WriteStream( BinaryWriter writer ) {
			PacketProtocol.WriteStreamIntoContainer( writer, this );
		}

		private static void WriteStreamIntoContainer( BinaryWriter writer, PacketProtocolData data ) {
			foreach( FieldInfo field in data.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof( JsonIgnoreAttribute ) ) ) {
					continue;
				}

				object raw_val = field.GetValue( data );
				Type field_type = field.FieldType;

				PacketProtocol.WriteStreamValue( writer, field_type, raw_val );
			}
		}

		private static void WriteStreamValue( BinaryWriter writer, Type field_type, object raw_val ) {
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
				if( typeof( PacketProtocolData ).IsAssignableFrom( field_type ) ) {
					PacketProtocol.WriteStreamIntoContainer( writer, (PacketProtocolData)raw_val );
				} if( typeof( ICollection ).IsAssignableFrom( field_type ) ) {
					var collection = (ICollection)raw_val;
					Type inner_type = collection.GetType().GetGenericArguments().Single();

					writer.Write( (ushort)collection.Count );

					foreach( object item in collection ) {
						PacketProtocol.WriteStreamValue( writer, inner_type, item );
					}
				} else {
					string json_enc_val = JsonConvert.SerializeObject( raw_val );
					writer.Write( (String)json_enc_val );
				}
				
				break;
			}
		}
	}
}
