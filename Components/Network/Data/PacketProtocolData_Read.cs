﻿using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
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
		private static void ReadStreamIntoContainer( BinaryReader reader, PacketProtocolData fieldContainer ) {
			var mymod = ModHelpersMod.Instance;
			IOrderedEnumerable<FieldInfo> orderedFields = fieldContainer.OrderedFields;
			int i = 0;

			if( mymod.Config.DebugModePacketInfo ) {
				LogHelpers.Log( "  Begun reading packet " + fieldContainer.GetType().Name + " ("+fieldContainer.FieldCount+" fields)" );
			}

			foreach( FieldInfo field in orderedFields ) {
				i++;

				Type fieldType = field.FieldType;
				object fieldData = PacketProtocolData.ReadStreamValue( reader, fieldType );

				if( Main.netMode == 1 ) {
					if( Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreServerAttribute ) ) ) {
						continue;
					}
				} else if( Main.netMode == 2 ) {
					if( Attribute.IsDefined( field, typeof( PacketProtocolWriteIgnoreClientAttribute ) ) ) {
						continue;
					}
				}

				if( mymod.Config.DebugModePacketInfo ) {
					LogHelpers.Log( "  * Reading packet "+fieldContainer.GetType().Name
						+" field ("+i+" of "+fieldContainer.FieldCount+") "+field.Name
						+": "+DotNetHelpers.Stringify(fieldData, 32) );
				}

//LogHelpers.Log( "READ "+ fieldContainer.GetType().Name + " FIELD " + field + " VALUE " + fieldData );
				field.SetValue( fieldContainer, fieldData );
			}
		}


		////////////////

		private static object ReadStreamValue( BinaryReader reader, Type fieldType ) {
			object rawVal;

			switch( Type.GetTypeCode( fieldType ) ) {
			case TypeCode.String:
				rawVal = reader.ReadString();
				break;
			case TypeCode.Single:
				rawVal = reader.ReadSingle();
				break;
			case TypeCode.UInt64:
				rawVal = reader.ReadUInt64();
				break;
			case TypeCode.Int64:
				rawVal = reader.ReadInt64();
				break;
			case TypeCode.UInt32:
				rawVal = reader.ReadUInt32();
				break;
			case TypeCode.Int32:
				rawVal = reader.ReadInt32();
				break;
			case TypeCode.UInt16:
				rawVal = reader.ReadUInt16();
				break;
			case TypeCode.Int16:
				rawVal = reader.ReadInt16();
				break;
			case TypeCode.Double:
				rawVal = reader.ReadDouble();
				break;
			case TypeCode.Char:
				rawVal = reader.ReadChar();
				break;
			case TypeCode.SByte:
				rawVal = reader.ReadSByte();
				break;
			case TypeCode.Byte:
				rawVal = reader.ReadByte();
				break;
			case TypeCode.Boolean:
				rawVal = reader.ReadBoolean();
				break;
			case TypeCode.Decimal:
				rawVal = reader.ReadDecimal();
				break;
			case TypeCode.Object:
				rawVal = PacketProtocolData.ReadStreamObjectValue( reader, fieldType );
				break;
			default:
				rawVal = null;
				break;
			}
			
			return rawVal;
		}


		private static object ReadStreamObjectValue( BinaryReader reader, Type fieldType ) {
			var mymod = ModHelpersMod.Instance;
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
				var data = (PacketProtocol)Activator.CreateInstance( fieldType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

				data.ReadStream( reader );
				data.OnClone();

				return data;

			} else if( ( isEnumerable || typeof( IEnumerable ).IsAssignableFrom( fieldType ) )
					&& ( !isDictionary && !typeof( IDictionary ).IsAssignableFrom( fieldType ) ) ) {
				ushort length = reader.ReadUInt16();
				Type[] innerTypes = fieldType.GetGenericArguments();
				Type innerType;
				
				if( innerTypes.Length == 0 ) {
					innerType = fieldType.GetElementType();
				} else {
					innerType = innerTypes[0];
				}
				
				Array arr = Array.CreateInstance( innerType, length );

				if( innerType.IsSubclassOf( typeof( PacketProtocolData ) ) ) {
					for( int i = 0; i < length; i++ ) {
						var item = (PacketProtocolData)Activator.CreateInstance( fieldType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

						item.ReadStream( reader );
						item.OnClone();

						arr.SetValue( item, i );
					}
				} else {
					for( int i = 0; i < length; i++ ) {
						object item = PacketProtocolData.ReadStreamValue( reader, innerType );
						arr.SetValue( item, i );
					}
				}

				if( fieldType.IsInterface ) {
					switch( dataTypeNameChunks[0] ) {
					case "ISet":
						fieldType = typeof( HashSet<> ).MakeGenericType( innerType );
						break;
					case "IList":
						fieldType = typeof( List<> ).MakeGenericType( innerType );
						break;
					}
				}
				
				if( fieldType.IsArray ) {
					return arr;
				} else {
					try {
						return Activator.CreateInstance( fieldType, new object[] { arr } );
					} catch( Exception e ) {
						throw new HamstarException( "Invalid container type " + fieldType.Name, e );
					}
				}

			} else {
				//if( mymod.Config.DebugModePacketInfo ) {
				//	LogHelpers.Log( "    ReadStreamObjectValue - type: " + fieldType.Name + ", reading: " + reader.BaseStream.Length + " bytes" );
				//}
				string rawJson = reader.ReadString();
				
				if( mymod.Config.DebugModePacketInfo ) {
					LogHelpers.Log( "    - ReadStreamObjectValue - type: "+fieldType.Name+", raw value ("+rawJson.Length+"): \n  "+rawJson );
				}

				var jsonVal = JsonConfig.Deserialize( rawJson, fieldType, XnaContractResolver.DefaultSettings );
				//var jsonVal = JsonConvert.DeserializeObject( rawJson, fieldType );

				return jsonVal;
			}
		}
	}
}
