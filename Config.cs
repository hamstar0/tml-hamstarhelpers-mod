using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using System;
using Terraria;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 3, 7); } }
		public static string ConfigFileName { get { return "HamstarHelpers Config.json"; } }


		////////////////

		public string VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

		public bool DebugModeNetInfo = false;
		public bool DebugModeUnhandledExceptionLogging = true;

		public bool DisableControlPanel = false;
		public int ControlPanelIconX = 0;
		public int ControlPanelIconY = 0;

		public bool AddCrimsonLeatherRecipe = true;

		public bool WorldModLockEnable = true;
		public bool WorldModLockMinimumOnly = true;

		public bool ModCallCommandEnabled = true;

		public int ModIssueReportErrorLogMaxLines = 35;



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


		////////////////
		
		internal void LoadFromNetwork( HamstarHelpersMod mymod, HamstarHelpersConfigData config ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			mymod.JsonConfig.SetData( config );

			myplayer.Logic.FinishModSettingsSync();
		}
	}
}
