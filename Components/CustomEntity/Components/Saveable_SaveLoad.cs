using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		private static bool LoadAll( bool isJson ) {
			var mymod = ModHelpersMod.Instance;
			string fileName = SaveableEntityComponent.GetFileNameBase();
			bool success = false;
			IList<SerializableCustomEntity> wrappedEnts = null;

			try {
				if( isJson ) {
					wrappedEnts = DataFileHelpers.LoadJson<List<SerializableCustomEntity>>( mymod, fileName, CustomEntityConverter.SerializerSettings, out success );
				} else {
					wrappedEnts = DataFileHelpers.LoadBinary<List<SerializableCustomEntity>>( mymod, fileName + ".dat", false, CustomEntityConverter.SerializerSettings );
					success = wrappedEnts != null;
				}

				if( success ) {
					foreach( SerializableCustomEntity ent in wrappedEnts ) {
						if( ent == null ) { continue; }
						CustomEntity realEnt = ent.Convert();
						CustomEntityManager.AddToWorld( realEnt );
					}
				}
			} catch( Exception e ) {
				LogHelpers.Log( "!ModHelpers.SaveableEntityComponent.LoadAll - " + e.ToString() );
			}

			return success;
		}


		private static void SaveAll( bool isJson ) {
			var mymod = ModHelpersMod.Instance;
			string fileName = SaveableEntityComponent.GetFileNameBase();

			IList<SerializableCustomEntity> wrappedEnts = new List<SerializableCustomEntity>();

			foreach( var ent in CustomEntityManager.GetEntitiesByComponent<SaveableEntityComponent>() ) {
				if( ent.GetComponentByType<SaveableEntityComponent>().AsJson != isJson ) {
					continue;
				}
				
				wrappedEnts.Add( new SerializableCustomEntity( ent ) );
			}

			if( wrappedEnts.Count > 0 ) {
				if( isJson ) {
					DataFileHelpers.SaveAsJson( mymod, fileName, CustomEntityConverter.SerializerSettings, wrappedEnts );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, fileName + ".dat", false, CustomEntityConverter.SerializerSettings, wrappedEnts );
				}
			}
		}
	}
}
