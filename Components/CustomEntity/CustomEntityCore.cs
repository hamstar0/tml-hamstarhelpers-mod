using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityCore : Entity {
		public string DisplayName = "";

		
		////////////////

		public CustomEntityCore( string name ) {
			this.DisplayName = name;
		}


		public bool ShouldSerialize() {
			return false;
		}
	}
}
