using HamstarHelpers.Helpers.MiscHelpers;


namespace HamstarHelpers {
	class HamstarHelpersData {
		public string ControlPanelNewSince = "1.0.0";
	}




	partial class ModHelpersMod {
		internal HamstarHelpersData Data = new HamstarHelpersData();


		////////////////

		private bool LoadModData() {
			bool success;
			var data = DataFileHelpers.LoadJson<HamstarHelpersData>( this, "data", out success );

			if( success ) {
				this.Data = data;
			}

			return success;
		}


		private void UnloadModData() {
			DataFileHelpers.SaveAsJson<HamstarHelpersData>( this, "data", this.Data );

			this.Data = null;
		}
	}
}
