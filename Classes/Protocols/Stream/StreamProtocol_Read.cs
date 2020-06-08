using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;


namespace HamstarHelpers.Classes.Protocols.Stream {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class StreamProtocol {
		private void ReadStreamIntoContainer( BinaryReader reader ) {
			IOrderedEnumerable<FieldInfo> orderedFields = this.OrderedFields;
			int i = 0;

			if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
				var pp = this as PacketProtocol;
				if( pp == null || pp.IsVerbose ) {
					LogHelpers.Log( "  Begun reading packet " + this.GetType().Name + " (" + this.FieldCount + " fields)" );
				}
			}

			foreach( FieldInfo field in orderedFields ) {
				i++;

				Type fieldType = field.FieldType;
				object fieldData = this.ReadStreamValue( reader, fieldType );

				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					if( Attribute.IsDefined( field, typeof( ProtocolWriteIgnoreServerAttribute ) ) ) {
						continue;
					}
				} else if( Main.netMode == NetmodeID.Server ) {
					if( Attribute.IsDefined( field, typeof( ProtocolWriteIgnoreClientAttribute ) ) ) {
						continue;
					}
				}

				if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
					var pp = this as PacketProtocol;
					if( pp == null || pp.IsVerbose ) {
						LogHelpers.Log( "  * Reading packet "+ this.GetType().Name
							+" field ("+i+" of "+ this.FieldCount+") "+field.Name
							+": "+DotNetHelpers.Stringify(fieldData, 32) );
					}
				}

//LogHelpers.Log( "READ "+ fieldContainer.GetType().Name + " FIELD " + field + " VALUE " + fieldData );
				field.SetValue( this, fieldData );
			}
		}


		////////////////

		private object ReadStreamValue( BinaryReader reader, Type fieldType ) {
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
				rawVal = this.ReadStreamObjectValue( reader, fieldType );
				break;
			default:
				rawVal = null;
				break;
			}
			
			return rawVal;
		}


		private object ReadStreamObjectValue( BinaryReader reader, Type fieldType ) {
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
			
			if( fieldType.IsSubclassOf( typeof(StreamProtocol) ) ) {
				var data = (PacketProtocol)StreamProtocol.CreateInstance( fieldType );
				//var data = (PacketProtocol)Activator.CreateInstance( fieldType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

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

				if( innerType.IsSubclassOf( typeof( StreamProtocol ) ) ) {
					for( int i = 0; i < length; i++ ) {
						var item = (StreamProtocol)StreamProtocol.CreateInstance( fieldType );
						//var item = (PacketProtocolData)Activator.CreateInstance( fieldType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );

						item.ReadStream( reader );
						item.OnClone();

						arr.SetValue( item, i );
					}
				} else {
					for( int i = 0; i < length; i++ ) {
						object item = this.ReadStreamValue( reader, innerType );
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
						throw new ModHelpersException( "Invalid container type " + fieldType.Name, e );
					}
				}

			} else {
				//if( mymod.Config.DebugModePacketInfo ) {
				//	LogHelpers.Log( "    ReadStreamObjectValue - type: " + fieldType.Name + ", reading: " + reader.BaseStream.Length + " bytes" );
				//}
				string rawJson = reader.ReadString();
				
				if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
					var pp = this as PacketProtocol;
					if( pp == null || pp.IsVerbose ) {
						LogHelpers.Log( "    - ReadStreamObjectValue - type: " + fieldType.Name + ", raw value (" + rawJson.Length + "): \n  " + rawJson );
					}
				}

				var jsonVal = JsonConvert.DeserializeObject( rawJson, fieldType, XNAContractResolver.DefaultSettings );
				//var jsonVal = JsonConvert.DeserializeObject( rawJson, fieldType );

				return jsonVal;
			}
		}
	}
}
