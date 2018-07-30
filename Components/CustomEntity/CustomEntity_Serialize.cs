using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	class CustomEntityContractResolver : DefaultContractResolver {
		protected override IList<JsonProperty> CreateProperties( Type type, MemberSerialization memberSerialization ) {
			IList<JsonProperty> properties = base.CreateProperties( type, memberSerialization );
			
			properties = properties.Where( (p) => {
				switch( p.PropertyName ) {
				case "whoAmI":
				case "lavaWet":
				case "wetCount":
				case "wet":
				case "honeyWet":
				case "oldVelocity":
				case "oldPosition":
				case "velocity":
				case "active":
				case "oldDirection":
				case "BottomRight":
				case "BottomLeft":
				case "Bottom":
				case "TopRight":
				case "TopLeft":
				case "Top":
				case "Right":
				case "Left":
				case "Hitbox":
				case "Size":
				case "Center":
					return false;
				}
				return true;
			} ).ToList();
			
			return properties;
		}
	}




	class CustomEntitySerializeable {
		public IList<CustomEntityComponent> ComponentsInOrder;


		internal CustomEntitySerializeable( IList<CustomEntityComponent> data ) {
			this.ComponentsInOrder = data;
		}
	}




	public partial class CustomEntity : Entity {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new CustomEntityContractResolver()
		};


		////////////////

		internal static CustomEntity Deserialize( string data ) {
			var deserialized = JsonConvert.DeserializeObject<CustomEntitySerializeable>( data, CustomEntity.SerializerSettings );

			return (CustomEntity)Activator.CreateInstance( typeof(CustomEntity) );
		}

		internal static string Serialize( CustomEntity ent ) {
			var serialize = new CustomEntitySerializeable( ent.ComponentsInOrder );

			return JsonConvert.SerializeObject( serialize, CustomEntity.SerializerSettings );
		}
	}
}
