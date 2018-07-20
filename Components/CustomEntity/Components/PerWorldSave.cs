using HamstarHelpers.Services.Promises;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	class CustomEntityWorldData {
		public int EntityCount;
		public CustomEntity[] Entities;
	}



	
	public class PerWorldSaveEntityComponent : CustomEntityComponent {
		public bool AsJson;


		////////////////

		internal PerWorldSaveEntityComponent( bool as_json ) {
			this.AsJson = as_json;
		}

		protected override void StaticInitialize() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			Promises.AddCustomPromiseForObject( myworld, () => {
				this.LoadAll();
				return true;
			} );
			Promises.AddCustomPromiseForObject( myworld, () => {
				this.SaveAll();
				return true;
			} );
		}


		////////////////

		internal void LoadAll() {
			
			// Load file
			// Load list of entities
		}


		internal void SaveAll() {
			foreach( var ent in CustomEntityManager.Entities ) {
				//ent.Save();
			}
			// Save each entity that implements this component
		}
	}
}
