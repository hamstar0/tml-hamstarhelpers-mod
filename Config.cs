using HamstarHelpers.Components.Config;
using HamstarHelpers.DebugHelpers;
using System;
using Terraria;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 6, 3); } }
		public static string ConfigFileName { get { return "Mod Helpers Config.json"; } }


		////////////////

		public string VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();
		
		public bool DebugModeNetInfo = false;
		public bool DebugModeUnhandledExceptionLogging = true;

		public bool UseCustomLogging = false;
		public bool UseCustomLoggingPerNetMode = false;
		public bool UseAlsoNormalLogging = false;

		public bool DisableControlPanel = false;
		public bool DisableControlPanelHotkey = false;
		public int ControlPanelIconX = 0;
		public int ControlPanelIconY = 0;

		public bool AddCrimsonLeatherRecipe = true;

		public bool WorldModLockEnable = true;
		public bool WorldModLockMinimumOnly = true;

		public bool ModCallCommandEnabled = true;

		public int ModIssueReportErrorLogMaxLines = 35;

		public bool IsServerHiddenFromBrowser = false;
		public bool IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP = true;
		public bool IsServerPromptingForBrowser = true;
		public int ServerBrowserCustomPort = -1;

		public int InboxIconPosX = 2;
		public int InboxIconPosY = 80;

		public bool IsServerGaugingAveragePing = true;
		public bool IsCheckingModVersions = true;



		////////////////

		public string _OLD_CONFIGS_BELOW_ = "";

		public bool UseCustomModeLogging = false;
		public int ServerBrowserAutoRefreshSeconds = 60 * 10;


		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new HamstarHelpersConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= HamstarHelpersConfigData.ConfigVersion ) {
				return false;
			}
			if( vers_since < new Version( 1, 4, 2, 1 ) ) {
				this.UseCustomLoggingPerNetMode = this.UseCustomModeLogging;
			}
			if( vers_since < new Version( 1, 4, 2, 3 ) ) {
				this.IsServerPromptingForBrowser = true;
			}

			this.VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

			return true;
		}


		////////////////
		
		internal void LoadFromNetwork( HamstarHelpersMod mymod, HamstarHelpersConfigData config ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			mymod.ConfigJson.SetData( config );

			myplayer.Logic.FinishModSettingsSync();
		}
	}
}
