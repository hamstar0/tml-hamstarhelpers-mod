using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.User;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace HamstarHelpers {
	/// <summary>
	/// Defines config settings fields. See `ModConfig` (via. tModLoader).
	/// </summary>
	[Label("Mod Helpers Settings")]
	public class ModHelpersConfig : ModConfig {
		//public static string ConfigFileName => "Mod Helpers Config.json";

		/// @private
		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		/// <summary>
		/// Outputs (to log) debug information relevant to specific Helpers functions (where applicable). Developers only.
		/// </summary>
		[Header( "Debug settings" )]
		[Label("Outputs (to log) debug information relevant to specific Helpers functions (where applicable). Developers only.")]
		public bool DebugModeHelpersInfo = false;
		/// <summary>
		/// Outputs (to log) network message information (sends and receives of PacketProtocol).
		/// </summary>
		[Label("Outputs (to log) network message information (sends and receives of PacketProtocol).")]
		public bool DebugModeNetInfo = false;
		/// <summary>
		/// Catches and logs unhandled exceptions (before crash).
		/// </summary>
		[Label("Catches and logs unhandled exceptions (before crash).")]
		[DefaultValue(true)]
		public bool DebugModeUnhandledExceptionLogging = true;
		/// <summary>
		/// Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user).
		/// </summary>
		[Label("Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user).")]
		public bool DebugModeDumpAlsoServer = false;
		/// <summary>
		/// Silences silent logging.
		/// </summary>
		[Label("Silences silent logging.")]
		public bool DebugModeDisableSilentLogging = false;
		/// <summary>
		/// Logs PacketProtocol payload content.
		/// </summary>
		[Label("Logs PacketProtocol payload content.")]
		public bool DebugModePacketInfo = false;


		/// <summary>
		/// Disables control panel outright.
		/// </summary>
		[Header("Control panel settings")]
		[Label("Disables control panel outright.")]
		public bool DisableControlPanel = false;
		/// <summary>
		/// Control panel icon's X coordinate on screen. Negative values align the button from the right edge.
		/// </summary>
		[Label("Control panel icon's X coordinate on screen. Negative values align the button from the right edge.")]
		public int ControlPanelIconX = 0;
		/// <summary>
		/// Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.
		/// </summary>
		[Label("Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.")]
		public int ControlPanelIconY = 0;

		/// <summary>
		/// Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.
		/// </summary>
		[Label("Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.")]
		public int InboxIconPosX = 2;
		/// <summary>
		/// Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.
		/// </summary>
		[Label("Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.")]
		public int InboxIconPosY = 80;

		/// <summary>
		/// Quantity of the latest log entries to pass along with issue reports.
		/// </summary>
		[Label("Quantity of the latest log entries to pass along with issue reports.")]
		public int ModIssueReportErrorLogMaxLines = 100;

		/// <summary>
		/// Enables mod locking per world (prevents playing a world with missing mods).
		/// </summary>
		[Label("Enables mod locking per world (prevents playing a world with missing mods).")]
		public bool WorldModLockEnable = true;
		/// <summary>
		/// Sets mod locking to expect only the exact set of mods it was locked with, and no more.
		/// </summary>
		[Label("Sets mod locking to expect only the exact set of mods it was locked with, and no more.")]
		public bool WorldModLockMinimumOnly = true;


		[Header( "Mod Helpers functions settings" )]
		[ReloadRequired]
		public bool ModCallCommandEnabled = false;

		//public bool IsServerHiddenFromBrowser = false;
		//public bool IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP = true;
		//public bool IsServerPromptingUsersBeforeListingOnBrowser = true;
		////public int ServerBrowserCustomPort = -1;

		public int PacketRequestRetryDuration = 60 * 4;	// 5 seconds

		public int PingUpdateDelay = 60 * 15;	// 15 seconds
		public bool IsServerGaugingAveragePing = true;

		[ReloadRequired]
		public string PrivilegedUserId = "";


		[Header( "Features settings" )]
		[ReloadRequired]
		public bool DisableModTags = false;
		public bool DisableModRecommendations = false;
		public bool DisableModMenuUpdates = false;
		public bool DisableSupportLinks = false;

		public bool IsCheckingModVersions = true;
		public bool IsCheckingModTags = true;

		public bool DisableJudgmentalTags = true;


		[Header( "Content settings settings" )]
		public bool MagiTechScrapMechBossDropsEnabled = false;
		[ReloadRequired]
		public bool CoalAsTile = true;

		public bool AddCrimsonLeatherRecipe = true;



		////////////////

		/// @private
		public override bool AcceptClientChanges( ModConfig pendingConfig, int whoAmI, ref string message ) {
			if( UserHelpers.HasBasicServerPrivilege( Main.player[whoAmI] ) ) {
				message = "Not authorized.";
				return false;
			}
			return true;
		}
	}
}
