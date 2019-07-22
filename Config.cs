using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Timers;
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
		[Increment(10)]
		[Range( -4096, 4096 )]
		public int ControlPanelIconX = 0;
		/// <summary>
		/// Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.
		/// </summary>
		[Label( "Control Panel Icon Y" )]
		[Tooltip( "Control panel icon's Y coordinate on screen. Negative values align the button from the bottom edge.")]
		[Increment( 10 )]
		[Range( -2160, 2160 )]
		public int ControlPanelIconY = 0;

		/// <summary>
		/// Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.
		/// </summary>
		[Label( "Inbox Icon X" )]
		[Tooltip( "Horizontal X coordinate of in-game inbox icon. Negative values align the button from the right edge.")]
		[Increment( 10 )]
		[Range( -4096, 4096 )]
		[DefaultValue( 2 )]
		public int InboxIconX = 2;
		/// <summary>
		/// Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.
		/// </summary>
		[Label( "Inbox Icon Y" )]
		[Tooltip( "Horizontal Y coordinate of in-game inbox icon. Negative values align the button from the bottom edge.")]
		[Increment( 10 )]
		[Range( -2160, 2160 )]
		[DefaultValue( 80 )]
		public int InboxIconY = 80;

		/// <summary>
		/// Quantity of the latest log entries to pass along with issue reports.
		/// </summary>
		[Label( "Mod Issue Report Error Log Max Lines" )]
		[Tooltip( "Quantity of the latest log entries to pass along with issue reports.")]
		[Increment( 10 )]
		[Range( 0, 1000 )]
		[DefaultValue( 100 )]
		public int ModIssueReportErrorLogMaxLines = 100;

		/// <summary>
		/// Enables mod locking per world (prevents playing a world with missing mods).
		/// </summary>
		[Label( "World Mod Lock Enable" )]
		[Tooltip( "Enables mod locking per world (prevents playing a world with missing mods).")]
		[DefaultValue( true )]
		public bool WorldModLockEnable = true;
		/// <summary>
		/// Sets mod locking to expect only the exact set of mods it was locked with, and no more.
		/// </summary>
		[Label( "World Mod Lock Minimum Only" )]
		[Tooltip( "Sets mod locking to expect only the exact set of mods it was locked with, and no more.")]
		[DefaultValue( true )]
		public bool WorldModLockMinimumOnly = true;


		/// <summary>
		/// Allows calling `Mod.Call(...)` via. chat/console commands. Use at your own risk!
		/// </summary>
		[Header( "Mod Helpers functions settings" )]
		[Label( "Mod Call Command Enabled" )]
		[Tooltip( "Allows calling `Mod.Call(...)` via. chat/console commands. Use at your own risk!" )]
		//[ReloadRequired]
		public bool ModCallCommandEnabled = false;

		//public bool IsServerHiddenFromBrowser = false;
		//public bool IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP = true;
		//public bool IsServerPromptingUsersBeforeListingOnBrowser = true;
		////public int ServerBrowserCustomPort = -1;

		/// <summary>
		/// Duration (in game ticks) to wait in expectation of a reply before a retry attempt of a given packet is made.
		/// </summary>
		[Label( "Packet Request Retry Duration" )]
		[Tooltip("Duration (in game ticks) to wait in expectation of a reply before a retry attempt of a given packet is made.")]
		[Range( 2, 60 * 120 )]
		[DefaultValue( 60 * 4 )]
		public int PacketRequestRetryDuration = 60 * 4; // 5 seconds

		/// <summary>
		/// Server occasionally pings clients to guage their latency.
		/// </summary>
		[Label( "Is Server Gauging Average Ping" )]
		[Tooltip( "Server occasionally pings clients to guage their latency." )]
		[DefaultValue( true )]
		public bool IsServerGaugingAveragePing = true;
		/// <summary>
		/// Duration between latency pings per client.
		/// </summary>
		[Label( "Ping Update Delay" )]
		[Tooltip( "Duration between latency pings per client." )]
		[Range( 2, 60 * 120 )]
		[DefaultValue( 60 * 15 )]
		public int PingUpdateDelay = 60 * 15;   // 15 seconds

		/// <summary>
		/// User ID of a designated privileged (admin) player. Refers to the internal player UID used by Mod Helpers.
		/// </summary>
		[Label( "Privileged User ID (internal UID)" )]
		[Tooltip("User ID of a designated privileged (admin) player. Refers to the internal player UID used by Mod Helpers.")]
		//[ReloadRequired]
		public string PrivilegedUserId = "";


		/// <summary>
		/// Disables mod tags UI for mod browser and mod info.
		/// </summary>
		[Header( "Features settings" )]
		[Label( "Disable mod tags" )]
		[Tooltip( "Disables mod tags UI for mod browser and mod info." )]
		//[ReloadRequired]
		public bool DisableModTags = false;
		/// <summary>
		/// Disables mod recommendations list within mod tags UI.
		/// </summary>
		[Label("Disable mod recommendations")]
		[Tooltip("Disables mod recommendations list within mod tags UI.")]
		public bool DisableModRecommendations = false;
		/// <summary>
		/// Disables mod version updates overlay display in the mod menu.
		/// </summary>
		[Label("Disable mod menu updates overlay")]
		[Tooltip("Disables mod version updates overlay display in the mod menu.")]
		public bool DisableModMenuUpdates = false;
		/// <summary>
		/// Disables main menu support top corner links.
		/// </summary>
		[Label("Disable Mod Helpers sSupport links")]
		[Tooltip("Disables main menu support top corner links.")]
		public bool DisableSupportLinks = false;

		/// <summary>
		/// Disable 'judgmental' mod tags.
		/// </summary>
		[Label("Disable 'judgmental' mod tags")]
		[DefaultValue( true )]
		public bool DisableJudgmentalTags = true;


		/// <summary>
		/// Magi-Tech Scrap items drop from mech bosses.
		/// </summary>
		[Header( "Content settings settings" )]
		[Label("Adds Magi-Tech Scrap mech boss drops")]
		[Tooltip( "Magi-Tech Scrap items drop from mech bosses." )]
		public bool MagiTechScrapMechBossDropsEnabled = false;
		/// <summary>
		/// Coal items can be placed like tiles.
		/// </summary>
		[ReloadRequired]
		[Label("Coal items can be placed like tiles")]
		[DefaultValue( true )]
		public bool CoalAsTile = true;

		/// <summary>
		/// Adds crimson biome alternatve recipe for leather.
		/// </summary>
		[Label("Add crimson biome leather recipe")]
		[Tooltip( "Adds crimson biome alternatve recipe for leather." )]
		[DefaultValue( true )]
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

		/// @private
		public override void OnChanged() {
			string oldVal = this.PrivilegedUserId;
			this.PrivilegedUserId = "";

			Timers.SetTimer( "ModHelpersConfigSyncPrevention", 1, () => {
				this.PrivilegedUserId = oldVal;
				return false;
			} );
		}
	}
}
