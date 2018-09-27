using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModBrowserUI : ModTagsUI {
		public static void Initialize() {
			new ModBrowserUI();
		}



		////////////////

		protected override string UIName => "UIModBrowser";
		protected override string BaseContextName => "Mod Browser";



		////////////////

		private ModBrowserUI() : base(true) {
			this.InitializeHoverText();
		}

		////////////////

		protected override void InitializeUI() {
			Action<UIState> ui_load = ui => {
				this.RecalculateMenuObjects();
				this.EnableTagButtons();
			};
			Action<UIState> ui_unload = ui => {
				this.ResetMenuObjects();
			};

			MenuUI.AddMenuLoader( this.UIName, "ModHelpers: "+this.BaseContextName+" Load", ui_load, ui_unload );
		}


		////////////////

		public override void OnTagStateChange( UIModTagButton tag_button ) {
			this.FilterMods();
		}
	}
}
