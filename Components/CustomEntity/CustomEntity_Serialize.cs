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
	internal class SerializableCustomEntity : CustomEntity {
		public string MyTypeName;


		////////////////

		protected SerializableCustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }
		
		internal SerializableCustomEntity( CustomEntity ent )
				: base( new PacketProtocolDataConstructorLock( typeof(CustomEntity) ) ) {
			this.MyTypeName = ent.GetType().Name;
			this.Core = ent.Core;
			this.Components = ent.Components;
			this.OwnerPlayerUID = ent.OwnerPlayerUID;
		}

		internal SerializableCustomEntity( string type_name, CustomEntityCore core, IList<CustomEntityComponent> components, string player_uid )
				: base( new PacketProtocolDataConstructorLock( typeof(CustomEntity) ) ) {
			this.MyTypeName = type_name;
			this.Core = core;
			this.Components = components;
			this.OwnerPlayerUID = player_uid;
		}

		////////////////

		protected override IList<CustomEntityComponent> CreateComponentsTemplate() {
			if( !this.IsInitialized ) { throw new NotImplementedException( "SerializableCustomEntity components not initialized." ); }
			return this.Components.ToList();
			//throw new NotImplementedException( "SerializableCustomEntity does not supply component templates." );
		}

		protected override CustomEntityCore CreateCoreTemplate() {
			if( !this.IsInitialized ) { throw new NotImplementedException( "SerializableCustomEntity core not initialized." ); }
			return new CustomEntityCore( this.Core );
			//throw new NotImplementedException( "SerializableCustomEntity does not supply core templates." );
		}


		////////////////

		internal CustomEntity Convert() {
			Type ent_type = CustomEntityManager.GetEntityType( this.MyTypeName );

			if( ent_type == null ) {
				throw new HamstarException( this.MyTypeName + " does not exist." );
			}
			if( !ent_type.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new HamstarException( ent_type.Name + " is not a valid CustomEntity." );
			}
			
			if( string.IsNullOrEmpty(this.OwnerPlayerUID) ) {
				return CustomEntity.CreateRaw( ent_type, this.Core, this.Components );
			} else {
				return CustomEntity.CreateRaw( ent_type, this.Core, this.Components, this.OwnerPlayerUID );
			}
			//var args = this.OwnerPlayerUID == "" ?
			//	new object[] { this.Core, this.Components } :
			//	new object[] { this.OwnerPlayerUID, this.Core, this.Components };
			//return (CustomEntity)Activator.CreateInstance( ent_type,
			//	BindingFlags.NonPublic | BindingFlags.Instance,
			//	null,
			//	args,
			//	null );
		}
	}




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



	public abstract partial class CustomEntity : PacketProtocolData {
	}
}
