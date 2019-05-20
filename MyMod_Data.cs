using HamstarHelpers.Helpers.MiscHelpers;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersData {
		public string ControlPanelNewSince = "1.0.0";
	}




	partial class ModHelpersMod : Mod {
		internal ModHelpersData Data = new ModHelpersData();


		////////////////

		private bool LoadModData() {
			bool success;
			var data = DataFileHelpers.LoadJson<ModHelpersData>( this, "data", out success );

			if( success && data != null ) {
				this.Data = data;
			}

			return success;
		}


		private void UnloadModData() {
			if( this.Data != null ) {
				DataFileHelpers.SaveAsJson<ModHelpersData>( this, "data", this.Data );
			}

			this.Data = new ModHelpersData();
		}
	}
}
