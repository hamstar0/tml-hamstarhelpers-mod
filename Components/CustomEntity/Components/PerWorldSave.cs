using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Components {
	class CustomEntityWorldData {
		public CustomEntity[] Entities;
		
		public CustomEntityWorldData( CustomEntity[] ents ) {
			this.Entities = ents;
		}
	}



	
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
				var wld_save_nojson = new PerWorldSaveEntityComponent( true );

				Promises.AddCustomPromiseForObject( myworld, () => {
					if( !wld_save_json.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (json) failed." );
					}
					if( !wld_save_nojson.LoadAll() ) {
						LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Load (no json) failed." );
					}
					return true;
				} );
				Promises.AddCustomPromiseForObject( myworld, () => {
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
			CustomEntityWorldData data;

			if( this.AsJson ) {
				data = DataFileHelpers.LoadJson<CustomEntityWorldData>( mymod, file_name, out success );
			} else {
				data = DataFileHelpers.LoadBinary<CustomEntityWorldData>( mymod, file_name, false );
				success = data != null;
			}

			if( success ) {
				foreach( var ent in data.Entities ) {
					CustomEntityManager.Instance.Add( ent );
				}
			}

			return success;
		}


		private void SaveAll() {
			var mymod = HamstarHelpersMod.Instance;
			var data = new CustomEntityWorldData(
				CustomEntityManager.Instance.TakeWhile( (t) => {
					var save_comp = t.GetComponentByType<PerWorldSaveEntityComponent>();
					return save_comp != null && save_comp.AsJson == this.AsJson;
				} ).ToArray()
			);
			string file_name = this.GetFileNameBase();
			
			if( this.AsJson ) {
				DataFileHelpers.SaveAsJson<CustomEntityWorldData>( mymod, file_name + ".json", data );
			} else {
				DataFileHelpers.SaveAsBinary<CustomEntityWorldData>( mymod, file_name + ".dat", false, data );
			}
		}
	}
}
