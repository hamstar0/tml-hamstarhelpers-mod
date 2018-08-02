using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public partial class PacketProtocolData {
		private static void WriteStreamIntoContainer( BinaryWriter writer, PacketProtocolData data ) {
			foreach( FieldInfo field in data.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof( PacketProtocolIgnoreAttribute ) ) ) {
					continue;
				}

				object raw_val = field.GetValue( data );
				Type field_type = field.FieldType;

				PacketProtocol.WriteStreamValue( writer, field_type, raw_val );
			}
		}


		////////////////

		private static void WriteStreamValue( BinaryWriter writer, Type field_type, object raw_val ) {
			if( PacketProtocolData.WriteStreamPrimitiveValue(writer, field_type, raw_val) ) {
				return;
			}

			PacketProtocolData.WriteStreamObjectValue( writer, field_type, raw_val );
		}


		private static bool WriteStreamPrimitiveValue( BinaryWriter writer, Type field_type, object raw_val ) {
			bool has_written = true;

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
				has_written = false;
				break;
			}

			return has_written;
		}


		private static void WriteStreamObjectValue( BinaryWriter writer, Type field_type, object raw_val ) {
			if( typeof( PacketProtocolData ).IsAssignableFrom( field_type ) ) {
				PacketProtocol.WriteStreamIntoContainer( writer, (PacketProtocolData)raw_val );
			} else if( typeof( ICollection ).IsAssignableFrom( field_type ) ) {
				var collection = (ICollection)raw_val;
				Type[] inner_types = collection.GetType().GetGenericArguments();
				Type inner_type;

				if( inner_types.Length == 0 ) {
					inner_type = raw_val.GetType().GetElementType();
				} else {
					inner_type = inner_types.Single();
				}

				writer.Write( (ushort)collection.Count );

				if( inner_type.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					foreach( object item in collection ) {
						PacketProtocol.WriteStreamIntoContainer( writer, (PacketProtocolData)item );
					}
				} else {
					foreach( object item in collection ) {
						PacketProtocol.WriteStreamValue( writer, inner_type, item );
					}
				}
			} else {
				string json_enc_val = JsonConvert.SerializeObject( raw_val );
				writer.Write( (String)json_enc_val );
			}
		}
	}
}
