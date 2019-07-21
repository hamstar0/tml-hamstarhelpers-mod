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
		[Label("Debug Mode - Helpers Info")]
		[Tooltip("Outputs (to log) debug information relevant to specific Helpers functions (where applicable). Developers only.")]
		public bool DebugModeHelpersInfo = false;
		/// <summary>
		/// Outputs (to log) network message information (sends and receives of PacketProtocol).
		/// </summary>
		[Label( "Debug Mode - Net Info" )]
		[Tooltip( "Outputs (to log) network message information (sends and receives of PacketProtocol).")]
		public bool DebugModeNetInfo = false;
		/// <summary>
		/// Catches and logs unhandled exceptions (before crash).
		/// </summary>
		[Label( "Debug Mode - Unhandled Exception Logging" )]
		[Tooltip( "Catches and logs unhandled exceptions (before crash).")]
		[DefaultValue(true)]
		public bool DebugModeUnhandledExceptionLogging = true;
		/// <summary>
		/// Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user).
		/// </summary>
		[Label( "Debug Mode - Also Server" )]
		[Tooltip( "Allows users to invoke 'data dumps' (see DataDump service) on behalf of the server (without being the 'privileged' user).")]
		public bool DebugModeDumpAlsoServer = false;
		/// <summary>
		/// Disables logging of "silenced" exceptions.
		/// </summary>
		[Label( "Debug Mode - Silent Logging" )]
		[Tooltip( "Silences silent logging.")]
		public bool DebugModeDisableSilentLogging = false;
		/// <summary>
		/// Logs PacketProtocol payload content.
		/// </summary>
		[Label( "Debug Mode - Packet Info" )]
		[Tooltip( "Logs PacketProtocol payload content.")]
		public bool DebugModePacketInfo = false;


		/// <summary>
		/// Disables control panel outright.
		/// </summary>
		[Header("Control panel settings")]
		[Label( "Disable Control Panel" )]
		[Tooltip( "Disables control panel outright.")]
		public bool DisableControlPanel = false;
		/// <summary>
		/// Control panel icon's X coordinate on screen. Negative values align the button from the right edge.
		/// </summary>
		[Label( "Control Panel Icon X" )]
		[Tooltip( "Control panel icon's X coordinate on screen. Negative values align the button from the right edge.")]
		public int ControlPanelIconX = 0;
		/// <summary>
		/// Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.
		/// </summary>
		[Label( "Control Panel Icon Y" )]
		[Tooltip( "Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.")]
		public int ControlPanelIconY = 0;

		/// <summary>
		/// Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.
		/// </summary>
		[Label( "Inbox Icon X" )]
		[Tooltip( "Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.")]
		public int InboxIconX = 2;
		/// <summary>
		/// Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.
		/// </summary>
		[Label( "Inbox Icon Y" )]
		[Tooltip( "Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.")]
		public int InboxIconY = 80;

		/// <summary>
		/// Quantity of the latest log entries to pass along with issue reports.
		/// </summary>
		[Label( "Mod Issue Report Error Log Max Lines" )]
		[Tooltip( "Quantity of the latest log entries to pass along with issue reports.")]
		public int ModIssueReportErrorLogMaxLines = 100;

		/// <summary>
		/// Enables mod locking per world (prevents playing a world with missing mods).
		/// </summary>
		[Label( "World Mod Lock Enable" )]
		[Tooltip( "Enables mod locking per world (prevents playing a world with missing mods).")]
		public bool WorldModLockEnable = true;
		/// <summary>
		/// Sets mod locking to expect only the exact set of mods it was locked with, and no more.
		/// </summary>
		[Label( "World Mod Lock Minimum Only" )]
		[Tooltip( "Sets mod locking to expect only the exact set of mods it was locked with, and no more.")]
		public bool WorldModLockMinimumOnly = true;


		[Header( "Mod Helpers functions settings" )]
		[Label( "Mod Call Command Enabled" )]
		[ReloadRequired]
		public bool ModCallCommandEnabled = false;

		//public bool IsServerHiddenFromBrowser = false;
		//public bool IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP = true;
		//public bool IsServerPromptingUsersBeforeListingOnBrowser = true;
		////public int ServerBrowserCustomPort = -1;

		[Label( "Packet Request Retry Duration" )]
		public int PacketRequestRetryDuration = 60 * 4; // 5 seconds

		[Label( "Is Server Gauging Average Ping" )]
		public bool IsServerGaugingAveragePing = true;
		[Label( "Ping Update Delay" )]
		public int PingUpdateDelay = 60 * 15;   // 15 seconds

		//[ReloadRequired]
		[Label( "Privileged User Id (internal UID)" )]
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
