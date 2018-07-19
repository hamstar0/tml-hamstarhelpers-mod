using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;

namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	public class PerWorldSaveEntityComponentData : CustomEntityComponentData {
		public bool AsJson { get; internal set; }

		////////////////

		internal PerWorldSaveEntityComponentData( bool as_json ) {
			this.AsJson = as_json;
		}
	}



	public class PerWorldSaveEntityComponent : CustomEntityComponent {
		protected override StaticModInitialize() {
			Promises.AddCustomPromise( "ModHelpersWorldLoad", this.LoadAll );
			Promises.AddCustomPromise( "ModHelpersWorldSave", this.SaveAll );
		}
	}
}
