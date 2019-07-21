using HamstarHelpers.Helpers.Misc;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersData {
		public string ControlPanelNewSince = "1.0.0";
	}




	/// @private
	partial class ModHelpersMod : Mod {
		internal ModHelpersData Data = new ModHelpersData();


		////////////////

		private bool LoadModData() {
			var data = ModCustomDataFileHelpers.LoadJson<ModHelpersData>( this, "data" );

			if( data != null ) {
				this.Data = data;
				return true;
			}
			return false;
		}


		private void UnloadModData() {
			if( this.Data != null ) {
				ModCustomDataFileHelpers.SaveAsJson<ModHelpersData>( this, "data", true, this.Data );
			}

			this.Data = new ModHelpersData();
		}
	}
}
