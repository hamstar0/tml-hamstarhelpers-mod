using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	class CustomEntitySerializeable {
		public IList<CustomEntityComponent> ComponentsInOrder;


		internal CustomEntitySerializeable( IList<CustomEntityComponent> data ) {
			this.ComponentsInOrder = data;
		}
	}




	public partial class CustomEntity : Entity {
		internal static CustomEntity Deserialize( string data ) {
			var deserialized = JsonConvert.DeserializeObject<CustomEntitySerializeable>( data );

			return (CustomEntity)Activator.CreateInstance( typeof(CustomEntity), deserialized );
		}

		internal static string Serialize( CustomEntity ent ) {
			var serialize = new CustomEntitySerializeable( ent.ComponentsInOrder );

			return JsonConvert.SerializeObject( serialize );
		}
	}
}
