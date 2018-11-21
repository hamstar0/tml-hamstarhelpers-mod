using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		private static bool LoadAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();
			bool success = false;
			IList<SerializableCustomEntity> wrapped_ents = null;

			try {
				if( is_json ) {
					wrapped_ents = DataFileHelpers.LoadJson<List<SerializableCustomEntity>>( mymod, file_name, CustomEntity.SerializerSettings, out success );
				} else {
					wrapped_ents = DataFileHelpers.LoadBinary<List<SerializableCustomEntity>>( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings );
					success = wrapped_ents != null;
				}

				if( success ) {
					foreach( SerializableCustomEntity ent in wrapped_ents ) {
						if( ent == null ) { continue; }
						
						CustomEntityManager.AddToWorld( ent.Convert() );
					}
				}
			} catch( Exception e ) {
				LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.LoadAll - " + e.ToString() );
			}

			return success;
		}


		private static void SaveAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();

			IList<SerializableCustomEntity> wrapped_ents = new List<SerializableCustomEntity>();

			foreach( var ent in CustomEntityManager.GetEntitiesByComponent<SaveableEntityComponent>() ) {
				if( ent.GetComponentByType<SaveableEntityComponent>().AsJson != is_json ) {
					continue;
				}
				
				wrapped_ents.Add( new SerializableCustomEntity( ent ) );
			}

			if( wrapped_ents.Count > 0 ) {
				if( is_json ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, CustomEntity.SerializerSettings, wrapped_ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings, wrapped_ents );
				}
			}
		}
	}
}
