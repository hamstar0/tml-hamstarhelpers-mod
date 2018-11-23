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

		public override bool CanConvert( Type obj_type ) {
			return obj_type == typeof( SerializableCustomEntity );
		}

		////

		public override object ReadJson( JsonReader reader, Type object_type, object existing_value, JsonSerializer serializer ) {
			IList<CustomEntityComponent> components = new List<CustomEntityComponent>();

			IEnumerable<Type> all_comp_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			IDictionary<string, Type> all_comp_type_map = all_comp_types.ToDictionary( t => t.Name, t => t );

			JObject jo = JObject.Load( reader );

			try {
				string type_name = jo["MyTypeName"].ToObject<String>();

				//int type_id = jo["TypeID"].ToObject<Int32>();
				string player_uid = jo["OwnerPlayerUID"].ToObject<String>();
				CustomEntityCore core = jo["Core"].ToObject<CustomEntityCore>();
				string[] comp_names = jo["ComponentNames"].ToObject<string[]>();
				JToken raw_components = jo["Components"];

				Type[] comp_types = new Type[comp_names.Length];
				int i;

				for( i = 0; i < comp_names.Length; i++ ) {
					if( !all_comp_type_map.ContainsKey( comp_names[i] ) ) {
						return null;
					}
					comp_types[i] = all_comp_type_map[comp_names[i]];
				}

				i = 0;
				foreach( JObject obj in raw_components ) {
					Type comp_type = comp_types[i];
					var comp = (CustomEntityComponent)PacketProtocolData.CreateRaw( comp_type );
					this.ReadIntoComponentFromJson( comp, obj, serializer );
					//var comp = obj.ToObject( comp_type, serializer );

					comp.InternalPostInitialize();

					components.Add( comp );
					i++;
				}

				return new SerializableCustomEntity( type_name, core, components, player_uid );

				//Type ent_type = CustomEntityManager.GetTypeByName( type_name );
				//if( ent_type == null ) {
				//	return null;
				//}
				//return CustomEntity.CreateRaw( ent_type, core, components, player_uid );

				//return (CustomEntity)Activator.CreateInstance( ent_type,
				//	BindingFlags.NonPublic | BindingFlags.Instance,
				//	null,
				//	new object[] { core, components, player_uid },
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
			Type comp_type = comp.GetType();

			foreach( var kv in obj ) {
				string name = kv.Key;
				JToken raw_val = kv.Value;
				object val = null;

				var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				FieldInfo field = comp_type.GetField( name, flags );
				PropertyInfo prop = field != null ? null : comp_type.GetProperty( name, flags );

				if( field != null ) {
					val = raw_val.ToObject( field.FieldType );
					field.SetValue( comp, val );
				} else if( prop != null ) {
					val = raw_val.ToObject( prop.PropertyType );
					prop.SetValue( comp, val );
				}
			}
		}
	}
}
