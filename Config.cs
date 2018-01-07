using HamstarHelpers.Utilities.Config;
using System;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 2, 6); } }
		public static string ConfigFileName { get { return "HamstarHelpers Config.json"; } }


		////////////////

		public string VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

		public bool DisableControlPanel = false;
		public int ControlPanelIconX = 0;
		public int ControlPanelIconY = 0;

		public bool AddCrimsonLeatherRecipe = true;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new HamstarHelpersConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= HamstarHelpersConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
