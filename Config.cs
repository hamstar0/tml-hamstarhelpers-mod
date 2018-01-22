using HamstarHelpers.Utilities.Config;
using System;
using Terraria;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 2, 8); } }
		public static string ConfigFileName { get { return "HamstarHelpers Config.json"; } }


		////////////////

		public string VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

		public bool DisableControlPanel = false;
		public int ControlPanelIconX = 0;
		public int ControlPanelIconY = 0;

		public bool AddCrimsonLeatherRecipe = true;

		public bool WorldModLockEnable = true;
		public bool WorldModLockMinimumOnly = true;

		public bool ModCallCommandEnabled = true;



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


		internal void LoadFromNetwork( HamstarHelpersMod mymod, string json ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			mymod.JsonConfig.DeserializeMe( json );

			myplayer.Logic.FinishModSettingsSync();
		}
	}
}
