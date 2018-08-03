﻿using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public partial class PacketProtocolData {
		private static void ReadStreamIntoContainer( BinaryReader reader, PacketProtocolData data ) {
			foreach( FieldInfo field in data.OrderedFields ) {
				if( Attribute.IsDefined( field, typeof( PacketProtocolIgnoreAttribute ) ) ) {
					continue;
				}

				Type data_type = field.FieldType;

				if( data_type.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					var field_value = (PacketProtocolData)field.GetValue( data );

					field_value.ReadStream( reader );

				} else {
					var field_data = PacketProtocolData.ReadStreamValue( reader, data_type );
					field.SetValue( data, field_data );
				}
			}
		}


		////////////////

		private static object ReadStreamValue( BinaryReader reader, Type data_type ) {
			bool has_read;
			object value = PacketProtocolData.ReadStreamPrimitiveValue( reader, data_type, out has_read );

			if( has_read ) { return value; }

			return PacketProtocolData.ReadStreamObjectValue( reader, data_type );
		}


		private static object ReadStreamPrimitiveValue( BinaryReader reader, Type data_type, out bool is_read ) {
			is_read = true;

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
				is_read = false;
				return null;
			}
		}


		private static object ReadStreamObjectValue( BinaryReader reader, Type data_type ) {
//LogHelpers.Log( "ReadStreamObjectValue type:"+data_type.Name+" "
//	+(data_type.GetElementType()==null?"?!":data_type.GetElementType().Name)+" "
//	+(data_type.DeclaringType==null?"??":data_type.DeclaringType.Name)+" "
//	+data_type.IsSubclassOf( typeof( PacketProtocolData ) ) + " "
//	+(data_type.GetInterface( "ICollection" )!=null) + " "
//	+(data_type.GetInterface( "IEnumerable" )!=null) );
			if( data_type.IsSubclassOf( typeof(PacketProtocolData) ) ) {
				var item = (PacketProtocolData)Activator.CreateInstance( data_type, true );

				item.ReadStream( reader );
				
				return item;
			} else if( data_type.GetInterface( "IEnumerable" ) != null ) {
				ushort length = reader.ReadUInt16();
				Type[] inner_types = data_type.GetGenericArguments();
				Type inner_type;

				if( inner_types.Length == 0 ) {
					inner_type = data_type.GetElementType();
				} else {
					inner_type = inner_types.Single();
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
				
				if( data_type.IsArray ) {
					return arr;
				} else {
					return Activator.CreateInstance( data_type, arr );
				}
			} else {
				string raw_json = reader.ReadString();

				var json_val = JsonConvert.DeserializeObject( raw_json, data_type );

				return json_val;
			}
		}
	}
}
