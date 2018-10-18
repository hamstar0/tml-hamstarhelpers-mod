using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Config;
using System;
using Terraria;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Mod Helpers Config.json";


		////////////////

		public string VersionSinceUpdate = new Version(0,0,0,0).ToString();

		public bool DebugModeNetInfo = false;
		public bool DebugModeUnhandledExceptionLogging = true;
		public bool DebugModeDumpAlsoServer = false;
		public bool DebugModeResetCustomEntities = false;
		public bool DebugModeCustomEntityInfo = false;
		public bool DebugModeEnableSilentLogging = false;

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
		public bool IsCheckingModTags = true;

		public bool DisableJudgmentalTags = true;

		public bool DisableModTags = false;
		public bool DisableModRecommendations = false;
		public bool DisableModMenuUpdates = false;
		public bool DisableSupportLinks = false;

		public string PrivilegedUserId = "";



		////////////////

		internal bool UpdateToLatestVersion( ModHelpersMod mymod ) {
			var new_config = new HamstarHelpersConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();
			
			if( vers_since >= mymod.Version ) {
				return false;
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}


		////////////////
		
		internal void LoadFromNetwork( ModHelpersMod mymod, HamstarHelpersConfigData config ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();

			mymod.ConfigJson.SetData( config );

			myplayer.Logic.FinishModSettingsSync();
		}
	}
}
