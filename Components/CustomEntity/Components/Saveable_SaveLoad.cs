using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		private static void SaveAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();

			IDictionary<string, CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<SaveableEntityComponent>()
				.ToDictionary( ent=>ent.GetType().Name, ent=>ent );
			ents = ents.Where(
				kv => kv.Value.GetComponentByType<SaveableEntityComponent>().AsJson == is_json
			).ToDictionary( kv=>kv.Key, kv=>kv.Value );

			if( ents.Count > 0 ) {
				if( is_json ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, CustomEntity.SerializerSettings, ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings, ents );
				}
			}
		}


		private static bool LoadAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();
			bool success;
			IDictionary<string, CustomEntity> ents;

			if( is_json ) {
				ents = DataFileHelpers.LoadJson<Dictionary<string, CustomEntity>>( mymod, file_name, CustomEntity.SerializerSettings, out success );
			} else {
				ents = DataFileHelpers.LoadBinary<Dictionary<string, CustomEntity>>( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings );
				success = ents != null;
			}

			if( success ) {
				foreach( var kv in ents ) {
					if( kv.Value != null ) {
						CustomEntityManager.LoadAs( kv.Key, kv.Value );
					}
				}
			}

			return success;
		}
	}
}
