using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.CustomEntity {
	internal class CustomEntityConverter : JsonConverter {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.None,
			ContractResolver = new CustomEntityCoreContractResolver(),
			Converters = new List<JsonConverter> { new CustomEntityConverter() }
		};



		////////////////

		public override bool CanWrite => false;

		public override bool CanConvert( Type objType ) {
			return objType == typeof( SerializableCustomEntity );
		}

		////

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			IList<CustomEntityComponent> components = new List<CustomEntityComponent>();

			IEnumerable<Type> allCompTypes = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			IDictionary<string, Type> allCompTypeMap = allCompTypes.ToDictionary( t => t.Name, t => t );

			JObject jo = JObject.Load( reader );

			try {
				string typeName = jo["MyTypeName"].ToObject<String>();

				//int typeId = jo["TypeID"].ToObject<Int32>();
				string playerUid = jo["OwnerPlayerUID"].ToObject<String>();
				CustomEntityCore core = jo["Core"].ToObject<CustomEntityCore>();
				string[] compNames = jo["ComponentNames"].ToObject<string[]>();
				JToken rawComponents = jo["Components"];

				Type[] compTypes = new Type[compNames.Length];
				int i;

				for( i = 0; i < compNames.Length; i++ ) {
					if( !allCompTypeMap.ContainsKey( compNames[i] ) ) {
						return null;
					}
					compTypes[i] = allCompTypeMap[compNames[i]];
				}

				i = 0;
				foreach( JObject obj in rawComponents ) {
					Type compType = compTypes[i];
					var comp = (CustomEntityComponent)PacketProtocolData.CreateRaw( compType );
					this.ReadIntoComponentFromJson( comp, obj, serializer );
					//var comp = obj.ToObject( compType, serializer );

					comp.InternalPostInitialize();

					components.Add( comp );
					i++;
				}

				return new SerializableCustomEntity( typeName, core, components, playerUid );

				//Type entType = CustomEntityManager.GetTypeByName( typeName );
				//if( entType == null ) {
				//	return null;
				//}
				//return CustomEntity.CreateRaw( entType, core, components, playerUid );

				//return (CustomEntity)Activator.CreateInstance( entType,
				//	BindingFlags.NonPublic | BindingFlags.Instance,
				//	null,
				//	new object[] { core, components, playerUid },
				//	null );
			} catch( Exception e ) {
				LogHelpers.Log( "!ModHelpers.CustomEntity.ReadJson - " + e.Message );
				return null;
			}
		}

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			throw new NotImplementedException( "WriteJson not implemented." );
		}


		////////////////

		private void ReadIntoComponentFromJson( CustomEntityComponent comp, JObject obj, JsonSerializer serializer ) {
			Type compType = comp.GetType();

			foreach( var kv in obj ) {
				string name = kv.Key;
				JToken rawVal = kv.Value;
				object val = null;

				var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				FieldInfo field = compType.GetField( name, flags );
				PropertyInfo prop = field != null ? null : compType.GetProperty( name, flags );

				if( field != null ) {
					val = rawVal.ToObject( field.FieldType );
					field.SetValue( comp, val );
				} else if( prop != null ) {
					val = rawVal.ToObject( prop.PropertyType );
					prop.SetValue( comp, val );
				}
			}
		}
	}
}
