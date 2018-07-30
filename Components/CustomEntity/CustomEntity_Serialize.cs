using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	internal class CustomEntityContractResolver : DefaultContractResolver {
		protected override IList<JsonProperty> CreateProperties( Type type, MemberSerialization member_serialization ) {
			IList<JsonProperty> properties = base.CreateProperties( type, member_serialization );
			
			properties = properties.Where( (p) => {
				if( p.DeclaringType.Name != "CustomEntityCore" ) { return true; }

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



	
	public partial class CustomEntity : PacketProtocolData {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto,
			ContractResolver = new CustomEntityContractResolver()
		};


		////////////////

		internal static CustomEntity Deserialize( string data ) {
			return JsonConvert.DeserializeObject<CustomEntity>( data, CustomEntity.SerializerSettings );
		}

		internal static string Serialize( CustomEntity ent ) {
			return JsonConvert.SerializeObject( ent, CustomEntity.SerializerSettings );
		}


		////////////////

		public 
	}
}
