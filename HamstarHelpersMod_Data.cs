using HamstarHelpers.MiscHelpers;


namespace HamstarHelpers {
	class HamstarHelpersData {
		internal string ControlPanelNewSince = "1.0.0";
	}



	partial class HamstarHelpersMod {
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


		private void SaveModData() {
			DataFileHelpers.SaveAsJson<HamstarHelpersData>( this, "data", this.Data );
		}
	}
}
