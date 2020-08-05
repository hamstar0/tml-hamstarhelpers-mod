using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.UI.LayerDisable {
	/// <summary>
	/// Allows remotely disabling interface layers (via. ModifyInterfaceLayers).
	/// </summary>
	public class LayerDisable : ILoadable {
		/// <summary></summary>
		public const string InterfaceLogic = "Vanilla: Interface Logic";
		/// <summary></summary>
		public const string EmoteBubbles = "Vanilla: Emote Bubbles";
		/// <summary></summary>
		public const string SmartCursorTargets = "Vanilla: Smart Cursor Targets";
		/// <summary></summary>
		public const string LaserRuler = "Vanilla: Laser Ruler";
		/// <summary></summary>
		public const string Ruler = "Vanilla: Ruler";
		/// <summary></summary>
		public const string GamepadLockOn = "Vanilla: Gamepad Lock On";
		/// <summary></summary>
		public const string TileGridOption = "Vanilla: Tile Grid Option";
		/// <summary></summary>
		public const string TownNPCHouseBanners = "Vanilla: Town NPC House Banners";
		/// <summary></summary>
		public const string HideUIToggle = "Vanilla: Hide UI Toggle";
		/// <summary></summary>
		public const string WireSelection = "Vanilla: Wire Selection";
		/// <summary></summary>
		public const string CaptureManagerCheck = "Vanilla: Capture Manager Check";
		/// <summary></summary>
		public const string IngameOptions = "Vanilla: Ingame Options";
		/// <summary></summary>
		public const string FancyUI = "Vanilla: Fancy UI";
		/// <summary></summary>
		public const string AchievementCompletePopups = "Vanilla: Achievement Complete Popups";
		/// <summary></summary>
		public const string EntityHealthBars = "Vanilla: Entity Health Bars";
		/// <summary></summary>
		public const string InvasionProgressBars = "Vanilla: Invasion Progress Bars";
		/// <summary></summary>
		public const string MapOrMinimap = "Vanilla: Map / Minimap";
		/// <summary></summary>
		public const string DiagnoseNet = "Vanilla: Diagnose Net";
		/// <summary></summary>
		public const string DiagnoseVideo = "Vanilla: Diagnose Video";
		/// <summary></summary>
		public const string SignTileBubble = "Vanilla: Sign Tile Bubble";
		/// <summary></summary>
		public const string MPPlayerNames = "Vanilla: MP Player Names";
		/// <summary></summary>
		public const string HairWindow = "Vanilla: Hair Window";
		/// <summary></summary>
		public const string DresserWindow = "Vanilla: Dresser Window";
		/// <summary></summary>
		public const string NPCOrSignDialog = "Vanilla: NPC / Sign Dialog";
		/// <summary></summary>
		public const string InterfaceLogic2 = "Vanilla: Interface Logic 2";
		/// <summary></summary>
		public const string ResourceBars = "Vanilla: Resource Bars";
		/// <summary></summary>
		public const string InterfaceLogic3 = "Vanilla: Interface Logic 3";
		/// <summary></summary>
		public const string Inventory = "Vanilla: Inventory";
		/// <summary></summary>
		public const string InfoAccessoriesBar = "Vanilla: Info Accessories Bar";
		/// <summary></summary>
		public const string SettingsButton = "Vanilla: Settings Button";
		/// <summary></summary>
		public const string Hotbar = "Vanilla: Hotbar";
		/// <summary></summary>
		public const string BuilderAccessoriesBar = "Vanilla: Builder Accessories Bar";
		/// <summary></summary>
		public const string RadialHotbars = "Vanilla: Radial Hotbars";
		/// <summary></summary>
		public const string MouseText = "Vanilla: Mouse Text";
		/// <summary></summary>
		public const string PlayerChat = "Vanilla: Player Chat";
		/// <summary></summary>
		public const string DeathText = "Vanilla: Death Text";
		/// <summary></summary>
		public const string Cursor = "Vanilla: Cursor";
		/// <summary></summary>
		public const string DebugStuff = "Vanilla: Debug Stuff";
		/// <summary></summary>
		public const string MouseItemOrNPCHead = "Vanilla: Mouse Item / NPC Head";
		/// <summary></summary>
		public const string MouseOver = "Vanilla: Mouse Over";
		/// <summary></summary>
		public const string InteractItemIcon = "Vanilla: Interact Item Icon";
		/// <summary></summary>
		public const string InterfaceLogic4 = "Vanilla: Interface Logic 4";


		////////////////

		/// @private
		public static LayerDisable Instance => ModContent.GetInstance<LayerDisable>();



		////////////////

		/// <summary></summary>
		public ISet<string> DisabledLayers { get; } = new HashSet<string>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
