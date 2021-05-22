using HamstarHelpers.Libraries.Misc;
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
			var data = ModCustomDataFileLibraries.LoadJson<ModHelpersData>( this, "data" );

			if( data != null ) {
				this.Data = data;
				return true;
			}

			return false;
		}


		private void UnloadModData() {
			//this.SaveModData();
		}


		////////////////

		internal void SaveModData() {
			if( this.Data != null ) {
				if( !ModCustomDataFileLibraries.SaveAsJson<ModHelpersData>(this, "data", true, this.Data) ) {
					Libraries.Debug.LogLibraries.Warn( "Could not save Mod Helpers data." );
				}
			} else {
				Libraries.Debug.LogLibraries.Warn( "No Mod Helpers data to save." );
			}

			this.Data = new ModHelpersData();
		}
	}
}
