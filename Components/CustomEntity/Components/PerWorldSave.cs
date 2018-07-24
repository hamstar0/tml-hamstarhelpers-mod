using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PerWorldSaveEntityComponent : CustomEntityComponent {
		public bool AsJson;



		////////////////
		
		public PerWorldSaveEntityComponent( bool as_json ) {
			this.AsJson = as_json;
		}

		////////////////

		protected class MyStaticInitializer : StaticInitializer {
			protected override void StaticInitialize() {
				var mymod = HamstarHelpersMod.Instance;
				var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
				var wld_save_json = new PerWorldSaveEntityComponent( true );
				var wld_save_nojson = new PerWorldSaveEntityComponent( false );

				Promises.AddCustomPromiseForObject( HamstarHelpersWorld.WorldLoad, () => {
					if( !wld_save_json.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (json) failed." );
					}
					if( !wld_save_nojson.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (no json) failed." );
					}
					return true;
				} );

				Promises.AddCustomPromiseForObject( HamstarHelpersWorld.WorldSave, () => {
					wld_save_json.SaveAll();
					wld_save_nojson.SaveAll();
					return true;
				} );
			}
		}


		////////////////

		public string GetFileNameBase() {
			return "world_"+WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		private bool LoadAll() {
			var mymod = HamstarHelpersMod.Instance;
			string file_name = this.GetFileNameBase();
			bool success;
			ISet<CustomEntity> ents;

			if( this.AsJson ) {
				ents = DataFileHelpers.LoadJson<HashSet<CustomEntity>>( mymod, file_name, out success );
			} else {
				ents = DataFileHelpers.LoadBinary<HashSet<CustomEntity>>( mymod, file_name+".dat", false );
				success = ents != null;
			}

			if( success ) {
				foreach( var ent in ents ) {
					CustomEntityManager.Instance.Add( ent );
				}
			}

			return success;
		}


		private void SaveAll() {
			var mymod = HamstarHelpersMod.Instance;
			string file_name = this.GetFileNameBase();

			ISet<CustomEntity> ents = CustomEntityManager.Instance.GetByComponentType<PerWorldSaveEntityComponent>();

			if( ents.Count > 0 ) {
				if( this.AsJson ) {
					DataFileHelpers.SaveAsJson( mymod, file_name, ents );
				} else {
					DataFileHelpers.SaveAsBinary( mymod, file_name + ".dat", false, ents );
				}
			}
		}
	}
}
