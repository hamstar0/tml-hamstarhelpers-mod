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
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;


namespace HamstarHelpers.Classes.Protocols.Stream {
	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class StreamProtocol {
		private void WriteStreamFromContainer( BinaryWriter writer ) {
			IOrderedEnumerable<FieldInfo> orderedFields = this.OrderedFields;
			int i = 0;

			if( !StreamProtocol.ValidateConstructor( this.GetType()) ) {
				throw new ModHelpersException( "Invalid default constructor for "+ this.GetType().Name );
			}

			if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
				var pp = this as PacketProtocol;
				if( pp == null || pp.IsVerbose ) {
					LogLibraries.Log( "  Begun writing packet " + this.GetType().Name + " (" + this.FieldCount + " fields)" );
				}
			}

			foreach( FieldInfo field in orderedFields ) {
				i++;

				if( Main.netMode == NetmodeID.MultiplayerClient && Attribute.IsDefined( field, typeof( ProtocolWriteIgnoreClientAttribute ) ) ) {
					continue;
				} else if( Main.netMode == NetmodeID.Server && Attribute.IsDefined( field, typeof( ProtocolWriteIgnoreServerAttribute ) ) ) {
					continue;
				}

				object rawFieldVal = field.GetValue( this );
				//LogHelpers.Log( "WRITE "+ data.GetType().Name+ " FIELD " + field + " VALUE "+(rawFieldVal??"null"));

				if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
					var pp = this as PacketProtocol;
					if( pp == null || pp.IsVerbose ) {
						LogLibraries.Log( "  * Writing packet " + this.GetType().Name
							+ " field (" + i + " of " + this.FieldCount + ") "
							+ field.Name + ": " + DotNetLibraries.Stringify( rawFieldVal, 32 ) );
					}
				}

				this.WriteStreamValue( writer, field.FieldType, rawFieldVal );
			}
		}


		////////////////
		
		private void WriteStreamValue( BinaryWriter writer, Type fieldType, object rawVal ) {
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
				this.WriteStreamObjectValue( writer, fieldType, rawVal );
				break;
			}
//LogHelpers.Log( " WriteStreamValue "+Type.GetTypeCode( fieldType ).ToString()+" - "+fieldType+": "+rawVal );
		}


		private void WriteStreamObjectValue( BinaryWriter writer, Type fieldType, object rawVal ) {
			bool isEnumerable = false;
			bool isDictionary = false;

			if( fieldType.IsInterface ) {
				string[] dataTypeNameChunks = fieldType.Name.Split( new char[] { '`' }, 2 );

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

			if( fieldType.IsSubclassOf( typeof( StreamProtocol ) ) ) {
				((StreamProtocol)rawVal).WriteStream( writer );

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

				if( innerType.IsSubclassOf( typeof( StreamProtocol ) ) ) {
					foreach( object item in collection ) {
						((StreamProtocol)item).WriteStream( writer );
					}
				} else {
					foreach( object item in collection ) {
						this.WriteStreamValue( writer, innerType, item );
					}
				}

			} else {
				string jsonEncVal = JsonConvert.SerializeObject( rawVal );
				writer.Write( (string)jsonEncVal );

				if( ModHelpersConfig.Instance.DebugModePacketInfo ) {
					var pp = this as PacketProtocol;
					if( pp == null || pp.IsVerbose ) {
						LogLibraries.Log( "    - WriteStreamObjectValue - type: " + fieldType.Name + ", raw value (" + jsonEncVal.Length + "): \n  " + jsonEncVal );
					}
				}
			}
		}
	}
}
