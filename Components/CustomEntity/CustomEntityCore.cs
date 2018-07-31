using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityCore : Entity {
		public string DisplayName = "";

		
		////////////////

		public CustomEntityCore( string name, int width, int height ) {
			this.DisplayName = name;
			this.width = width;
			this.height = height;
		}
		
		internal CustomEntityCore Clone() {
			return (CustomEntityCore)this.MemberwiseClone();
		}
	}
}
