using HamstarHelpers.Helpers.Misc;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersData {
		public string ControlPanelNewSince = "1.0.0";
		public bool ModTagsOpened = false;
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
				if( !ModCustomDataFileHelpers.SaveAsJson<ModHelpersData>(this, "data", true, this.Data) ) {
					Helpers.Debug.LogHelpers.Warn( "Could not save Mod Helpers data." );
				}
			} else {
				Helpers.Debug.LogHelpers.Warn( "No Mod Helpers data to save." );
			}

			this.Data = new ModHelpersData();
		}
	}
}
