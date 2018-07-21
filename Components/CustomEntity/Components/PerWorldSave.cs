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

		protected override void StaticInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			Promises.AddCustomPromiseForObject( myworld, () => {
				if( !this.LoadAll() ) {
					LogHelpers.Log( "HamstarHelpersMod.PerWorldSaveEntityComponent.StaticInitialize - Save failed." );
				}
				return true;
			} );
			Promises.AddCustomPromiseForObject( myworld, () => {
				this.SaveAll();
				return true;
			} );
		}


		////////////////

		public string GetFileNameBase() {
			return "world_"+WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		internal bool LoadAll() {
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
				var mngr = CustomEntityManager.Instance;
				mngr.Clear();

				int i = 0;
				foreach( var ent in data.Entities ) {
					mngr[ i++ ] = ent;
				}
			}

			return success;
		}


		internal void SaveAll() {
			var mymod = HamstarHelpersMod.Instance;
			var data = new CustomEntityWorldData(
				CustomEntityManager.Instance.TakeWhile(
					t => t.GetComponentByType<PerWorldSaveEntityComponent>() != null
				).ToArray()
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
