using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.Network.Data {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class PacketProtocolData {
		private static void WriteStreamFromContainer( BinaryWriter writer, PacketProtocolData data ) {
			foreach( FieldInfo field in data.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof( PacketProtocolIgnoreAttribute ) ) ) {
					continue;
				}
				if( Main.netMode == 1 && Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreClientAttribute ) ) ) {
					continue;
				} else if( Main.netMode == 2 && Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreServerAttribute ) ) ) {
					continue;
				}

				object raw_field_val = field.GetValue( data );
//LogHelpers.Log( "WRITE "+ data.GetType().Name+ " FIELD " + field + " VALUE "+(raw_field_val??"null"));
				
				PacketProtocolData.WriteStreamValue( writer, field.FieldType, raw_field_val );
			}
		}


		////////////////
		
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
				writer.Write( (Char)raw_val );
				break;
			case TypeCode.SByte:
				writer.Write( (SByte)raw_val );
				break;
			case TypeCode.Byte:
				writer.Write( (Byte)raw_val );
				break;
			case TypeCode.Boolean:
				writer.Write( (Boolean)raw_val );
				break;
			case TypeCode.Decimal:
				writer.Write( (Decimal)raw_val );
				break;
			case TypeCode.Object:
				PacketProtocolData.WriteStreamObjectValue( writer, field_type, raw_val );
				break;
			}
//LogHelpers.Log( " WriteStreamValue "+Type.GetTypeCode( field_type ).ToString()+" - "+field_type+": "+raw_val );
		}


		private static void WriteStreamObjectValue( BinaryWriter writer, Type field_type, object raw_val ) {
			bool is_enumerable = false, is_dictionary = false;
			string[] data_type_name_chunks = null;

			if( field_type.IsInterface ) {
				data_type_name_chunks = field_type.Name.Split( new char[] { '`' }, 2 );

				switch( data_type_name_chunks[0] ) {
				case "ISet":
				case "IList":
					is_enumerable = true;
					break;
				case "IDictionary":
					is_dictionary = true;
					break;
				}
			}

			if( field_type.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
				((PacketProtocolData)raw_val).WriteStream( writer );

			} else if( ( is_enumerable || typeof( IEnumerable ).IsAssignableFrom( field_type ) )
					&& ( !is_dictionary && !typeof( IDictionary ).IsAssignableFrom( field_type ) ) ) {
				Type[] inner_types = field_type.GetGenericArguments();
				Type inner_type;
				
				if( inner_types.Length == 0 ) {
					inner_type = field_type.GetElementType();
				} else {
					inner_type = inner_types[0];
				}

				IEnumerable<object> collection = ((IEnumerable)raw_val).Cast<object>();
				
				writer.Write( (ushort)collection.Count() );

				if( inner_type.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					foreach( object item in collection ) {
						((PacketProtocolData)item).WriteStream( writer );
					}
				} else {
					foreach( object item in collection ) {
						PacketProtocolData.WriteStreamValue( writer, inner_type, item );
					}
				}

			} else {
				string json_enc_val = JsonConvert.SerializeObject( raw_val );
				writer.Write( (string)json_enc_val );
			}
		}
	}
}
