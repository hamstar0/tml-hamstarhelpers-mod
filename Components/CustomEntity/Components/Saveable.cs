using HamstarHelpers.Components.CustomEntity.Templates;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator LoadAllValidator;

		internal readonly static object LoadAllDataKey = new object();


		////////////////

		static SaveableEntityComponent() {
			SaveableEntityComponent.LoadAllDataKey = new object();
			SaveableEntityComponent.MyValidatorKey = new object();
			SaveableEntityComponent.LoadAllValidator = new PromiseValidator( SaveableEntityComponent.MyValidatorKey );
		}


		////////////////

		public static string GetFileNameBase() {
			return "world_" + WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		private static bool LoadAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();
			bool success;
			ISet<CustomEntity> ents;

			if( is_json ) {
				ents = DataFileHelpers.LoadJson<HashSet<CustomEntity>>( mymod, file_name, CustomEntity.SerializerSettings, out success );
			} else {
				ents = DataFileHelpers.LoadBinary<HashSet<CustomEntity>>( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings );
				success = ents != null;
			}

			if( success ) {
				foreach( var ent in ents ) {
					if( ent != null && CustomEntityTemplateManager.GetID( ent.Components ) == -1 ) {
						CustomEntityTemplateManager.Add( new CustomEntityTemplate( ent ) );
					}
				}
				foreach( var ent in ents ) {
					if( ent != null ) {
						CustomEntityManager.AddEntity( ent );
					}
				}
			}

			return success;
		}


		private static void SaveAll( bool is_json ) {
			var mymod = ModHelpersMod.Instance;
			string file_name = SaveableEntityComponent.GetFileNameBase();

			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<SaveableEntityComponent>();
			ents = new HashSet<CustomEntity>(
				ents.Where(
					ent => ent.GetComponentByType<SaveableEntityComponent>().AsJson == is_json
//ent => {
//	LogHelpers.Log( "saving ent: " + ent.ToString() + " behav:" + ent.GetComponentByName( "TrainBehaviorEntityComponent" )?.ToString() );
//	return ent.GetComponentByType<SaveableEntityComponent>().AsJson == is_json;
//}
				)
			);

			if( ents.Count > 0 ) {
				if( is_json ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, CustomEntity.SerializerSettings, ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, CustomEntity.SerializerSettings, ents );
				}
			}
		}



		////////////////

		public bool AsJson;



		////////////////

		private SaveableEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) { }

		public SaveableEntityComponent( bool as_json ) {
			this.AsJson = as_json;

			this.ConfirmLoad();
		}
	}
}
