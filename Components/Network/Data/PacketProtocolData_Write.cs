﻿using HamstarHelpers.Components.Errors;
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
			if( !PacketProtocolData.ValidateConstructor(data.GetType()) ) {
				throw new HamstarException( "Invalid default constructor for "+data.GetType().Name );
			}

			foreach( FieldInfo field in data.OrderedFields ) {
				if( Main.netMode == 1 && Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreClientAttribute ) ) ) {
					continue;
				} else if( Main.netMode == 2 && Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreServerAttribute ) ) ) {
					continue;
				}

				object rawFieldVal = field.GetValue( data );
//LogHelpers.Log( "WRITE "+ data.GetType().Name+ " FIELD " + field + " VALUE "+(rawFieldVal??"null"));
				
				PacketProtocolData.WriteStreamValue( writer, field.FieldType, rawFieldVal );
			}
		}


		////////////////
		
		private static void WriteStreamValue( BinaryWriter writer, Type fieldType, object rawVal ) {
			switch( Type.GetTypeCode( fieldType ) ) {
			case TypeCode.String:
				writer.Write( (String)rawVal );
				break;
			case TypeCode.Single:
				writer.Write( (Single)rawVal );
				break;
			case TypeCode.UInt64:
				writer.Write( (UInt64)rawVal );
				break;
			case TypeCode.Int64:
				writer.Write( (Int64)rawVal );
				break;
			case TypeCode.UInt32:
				writer.Write( (UInt32)rawVal );
				break;
			case TypeCode.Int32:
				writer.Write( (Int32)rawVal );
				break;
			case TypeCode.UInt16:
				writer.Write( (UInt16)rawVal );
				break;
			case TypeCode.Int16:
				writer.Write( (Int16)rawVal );
				break;
			case TypeCode.Double:
				writer.Write( (Double)rawVal );
				break;
			case TypeCode.Char:
				writer.Write( (Char)rawVal );
				break;
			case TypeCode.SByte:
				writer.Write( (SByte)rawVal );
				break;
			case TypeCode.Byte:
				writer.Write( (Byte)rawVal );
				break;
			case TypeCode.Boolean:
				writer.Write( (Boolean)rawVal );
				break;
			case TypeCode.Decimal:
				writer.Write( (Decimal)rawVal );
				break;
			case TypeCode.Object:
				PacketProtocolData.WriteStreamObjectValue( writer, fieldType, rawVal );
				break;
			}
//LogHelpers.Log( " WriteStreamValue "+Type.GetTypeCode( fieldType ).ToString()+" - "+fieldType+": "+rawVal );
		}


		private static void WriteStreamObjectValue( BinaryWriter writer, Type fieldType, object rawVal ) {
			bool isEnumerable = false, isDictionary = false;
			string[] dataTypeNameChunks = null;

			if( fieldType.IsInterface ) {
				dataTypeNameChunks = fieldType.Name.Split( new char[] { '`' }, 2 );

				switch( dataTypeNameChunks[0] ) {
				case "ISet":
				case "IList":
					isEnumerable = true;
					break;
				case "IDictionary":
					isDictionary = true;
					break;
				}
			}

			if( fieldType.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
				((PacketProtocolData)rawVal).WriteStream( writer );

			} else if( ( isEnumerable || typeof( IEnumerable ).IsAssignableFrom( fieldType ) )
					&& ( !isDictionary && !typeof( IDictionary ).IsAssignableFrom( fieldType ) ) ) {
				Type[] innerTypes = fieldType.GetGenericArguments();
				Type innerType;
				
				if( innerTypes.Length == 0 ) {
					innerType = fieldType.GetElementType();
				} else {
					innerType = innerTypes[0];
				}

				IEnumerable<object> collection = ((IEnumerable)rawVal).Cast<object>();
				
				writer.Write( (ushort)collection.Count() );

				if( innerType.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					foreach( object item in collection ) {
						((PacketProtocolData)item).WriteStream( writer );
					}
				} else {
					foreach( object item in collection ) {
						PacketProtocolData.WriteStreamValue( writer, innerType, item );
					}
				}

			} else {
				string jsonEncVal = JsonConvert.SerializeObject( rawVal );
				writer.Write( (string)jsonEncVal );
			}
		}
	}
}
