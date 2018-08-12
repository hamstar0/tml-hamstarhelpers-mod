using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Config;
using System;
using Terraria;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(2, 0, 2, 2); } }
		public static string ConfigFileName { get { return "Mod Helpers Config.json"; } }


		////////////////

		public string VersionSinceUpdate = HamstarHelpersConfigData.ConfigVersion.ToString();

		public bool DebugModeNetInfo = false;
		public bool DebugModeUnhandledExceptionLogging = true;
		public bool DebugModeHighlightEntities = false;
		public bool DebugModeDumpAlsoServer = false;

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
		public bool IsServerPromptingUsersBeforeListingOnBrowser = true;
		public int ServerBrowserCustomPort = -1;

		public int InboxIconPosX = 2;
		public int InboxIconPosY = 80;

		public bool IsServerGaugingAveragePing = true;
		public bool IsCheckingModVersions = true;

		public string PrivilegedUserId = "";



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

			mymod.ConfigJson.SetData( config );

			myplayer.Logic.FinishModSettingsSync();
		}
	}
}
