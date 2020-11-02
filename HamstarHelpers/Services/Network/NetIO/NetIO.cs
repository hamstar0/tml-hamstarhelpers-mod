using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NetSerializer;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.DotNET.Serialization;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		private IDictionary<int, Type> PayloadCodeToType = new Dictionary<int, Type>();
		private IDictionary<Type, int> PayloadTypeToCode = new Dictionary<Type, int>();
		private IDictionary<int, Serializer> PayloadCodeToSerializer = new Dictionary<int, Serializer>();



		////////////////

		void ILoadable.OnModsLoad() {
			IList<Type> payloadTypes = ReflectionHelpers
				.GetAllAvailableSubTypesFromMods( typeof(NetIOPayload) )
				.OrderBy( t => t.Namespace + "." + t.Name )
				.ToList();
			var settings = new Settings {
				CustomTypeSerializers = new ITypeSerializer[] { new HashSetSerializer() }
			};

			int i = 0;
			foreach( Type payloadType in payloadTypes.ToArray() ) {
				if( !payloadType.IsSerializable ) {
					payloadTypes.Remove( payloadType );
					LogHelpers.Warn( "Invalid payload type "+payloadType.Name+" "
						+"(in "+payloadType.Assembly.GetName().Name+")" );
					continue;
				}
				foreach( FieldInfo field in payloadType.GetFields() ) {
					if( !field.FieldType.IsSerializable && !field.IsNotSerialized ) {
						payloadTypes.Remove( payloadType );
						LogHelpers.Warn( "Invalid payload type "+payloadType.Name+"; field "+field.Name+" not serializeable "
							+"(in "+payloadType.Assembly.GetName().Name+")" );
					}
				}

				this.PayloadCodeToType[i] = payloadType;
				this.PayloadTypeToCode[payloadType] = i;
				this.PayloadCodeToSerializer[i] = new Serializer( new Type[] { payloadType }, settings );
				i++;
			}
		}

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
