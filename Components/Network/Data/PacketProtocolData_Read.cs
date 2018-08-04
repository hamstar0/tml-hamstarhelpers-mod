﻿using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public partial class PacketProtocolData {
		private static void ReadStreamIntoContainer( BinaryReader reader, PacketProtocolData field_container ) {
			foreach( FieldInfo field in field_container.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof( PacketProtocolIgnoreAttribute ) ) ) {
					continue;
				}
				
				Type field_type = field.FieldType;
				
//LogHelpers.Log( "READ "+ field_container.GetType().Name + " FIELD " + field );
				object field_data = PacketProtocolData.ReadStreamValue( reader, field_type );

				field.SetValue( field_container, field_data );
			}
		}


		////////////////

		private static object ReadStreamValue( BinaryReader reader, Type field_type ) {
			object raw_val;

			switch( Type.GetTypeCode( field_type ) ) {
			case TypeCode.String:
				raw_val = reader.ReadString();
				break;
			case TypeCode.Single:
				raw_val = reader.ReadSingle();
				break;
			case TypeCode.UInt64:
				raw_val = reader.ReadUInt64();
				break;
			case TypeCode.Int64:
				raw_val = reader.ReadInt64();
				break;
			case TypeCode.UInt32:
				raw_val = reader.ReadUInt32();
				break;
			case TypeCode.Int32:
				raw_val = reader.ReadInt32();
				break;
			case TypeCode.UInt16:
				raw_val = reader.ReadUInt16();
				break;
			case TypeCode.Int16:
				raw_val = reader.ReadInt16();
				break;
			case TypeCode.Double:
				raw_val = reader.ReadDouble();
				break;
			case TypeCode.Char:
				raw_val = reader.ReadChar();
				break;
			case TypeCode.SByte:
				raw_val = reader.ReadSByte();
				break;
			case TypeCode.Byte:
				raw_val = reader.ReadByte();
				break;
			case TypeCode.Boolean:
				raw_val = reader.ReadBoolean();
				break;
			case TypeCode.Decimal:
				raw_val = reader.ReadDecimal();
				break;
			case TypeCode.Object:
				raw_val = PacketProtocolData.ReadStreamObjectValue( reader, field_type );
				break;
			default:
				raw_val = null;
				break;
			}
			
//LogHelpers.Log( " ReadStreamValue "+Type.GetTypeCode( field_type )+" - "+field_type+": "+raw_val );
			return raw_val;
		}


		private static object ReadStreamObjectValue( BinaryReader reader, Type field_type ) {
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

			if( field_type.IsSubclassOf( typeof(PacketProtocolData) ) ) {
				var item = (PacketProtocolData)Activator.CreateInstance( field_type, true );

				item.ReadStream( reader );
				
				return item;

			} else if( ( is_enumerable || typeof( IEnumerable ).IsAssignableFrom( field_type ) )
					&& ( !is_dictionary && !typeof( IDictionary ).IsAssignableFrom( field_type ) ) ) {
				ushort length = reader.ReadUInt16();
				Type[] inner_types = field_type.GetGenericArguments();
				Type inner_type;
				
				if( inner_types.Length == 0 ) {
					inner_type = field_type.GetElementType();
				} else {
					inner_type = inner_types[0];
				}
				
				Array arr = Array.CreateInstance( inner_type, length );

				if( inner_type.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					for( int i = 0; i < length; i++ ) {
						var item = (PacketProtocolData)Activator.CreateInstance( inner_type, true );

						item.ReadStream( reader );
						arr.SetValue( item, i );
					}
				} else {
					for( int i = 0; i < length; i++ ) {
						object item = PacketProtocolData.ReadStreamValue( reader, inner_type );
						arr.SetValue( item, i );
					}
				}

				if( field_type.IsInterface ) {
					switch( data_type_name_chunks[0] ) {
					case "ISet":
						field_type = typeof( HashSet<> ).MakeGenericType( inner_type );
						break;
					case "IList":
						field_type = typeof( List<> ).MakeGenericType( inner_type );
						break;
					}
				}
				
//LogHelpers.Log( "  1 field_type: "+field_type+ " (IsArray? " + field_type.IsArray+"), arr: "+arr+", dict? "+typeof(IDictionary).IsAssignableFrom(field_type) );
				if( field_type.IsArray ) {
					return arr;
				} else {
					return Activator.CreateInstance( field_type, new object[] { arr } );
				}

			} else {
				string raw_json = reader.ReadString();

				var json_val = JsonConvert.DeserializeObject( raw_json, field_type );
//LogHelpers.Log( "  2 field_type: "+field_type+ " , raw_json: " + raw_json+ ", json_val: "+JsonConvert.SerializeObject(json_val) );
				
				return json_val;
			}
		}
	}
}
